using AeroDroxUAV.Models;
using AeroDroxUAV.Services; // NEW: Dependency on Service Layer
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AeroDroxUAV.Controllers
{
    // The routing and API attributes remain the same
    [Route("api/[controller]")]
    [ApiController] 
    public class DronesApiController : ControllerBase 
    {
        private readonly IDroneService _droneService; // CHANGED: Use Service Interface

        // NEW: Constructor now injects the service
        public DronesApiController(IDroneService droneService)
        {
            _droneService = droneService;
        }

        // Authorization Helpers (Remain in Controller as the application boundary)
        private bool IsLoggedIn() => !string.IsNullOrEmpty(HttpContext.Session.GetString("Username"));
        private bool IsAdmin() => HttpContext.Session.GetString("Role") == "Admin";

        // GET: api/DronesApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Drone>>> GetDrones()
        {
            if (!IsLoggedIn())
            {
                return Unauthorized(); // Returns 401
            }
            
            // DELEGATION: Calls the Service layer
            var drones = await _droneService.GetAllDronesAsync(); 
            return Ok(drones); // Explicitly return 200 OK with data
        }

        // GET: api/DronesApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Drone>> GetDrone(int id)
        {
            if (!IsLoggedIn())
            {
                return Unauthorized();
            }

            // DELEGATION: Calls the Service layer
            var drone = await _droneService.GetDroneByIdAsync(id);

            if (drone == null)
            {
                return NotFound(); // Returns 404
            }

            return Ok(drone); // Returns 200 OK with single JSON object
        }

        // POST: api/DronesApi
        [HttpPost]
        public async Task<ActionResult<Drone>> PostDrone(Drone drone)
        {
            if (!IsLoggedIn() || !IsAdmin())
            {
                return Forbid(); // Returns 403 (Logged in but wrong role)
            }
            
            // DELEGATION: Calls the Service layer
            await _droneService.CreateDroneAsync(drone);

            // Returns 201 CreatedAtAction with the created resource and its URL
            return CreatedAtAction(nameof(GetDrone), new { id = drone.Id }, drone);
        }

        // PUT: api/DronesApi/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDrone(int id, Drone drone)
        {
            if (id != drone.Id)
            {
                return BadRequest(); // Returns 400
            }

            if (!IsLoggedIn() || !IsAdmin())
            {
                return Forbid();
            }
            
            // DELEGATION: Check if drone exists before updating (business logic in service)
            var existingDrone = await _droneService.GetDroneByIdAsync(id);
            if (existingDrone == null)
            {
                return NotFound();
            }

            try
            {
                // DELEGATION: Calls the Service layer
                await _droneService.UpdateDroneAsync(drone);
            }
            catch (DbUpdateConcurrencyException)
            {
                // Handle concurrency issues if required, but simpler to check existence post-facto
                return NotFound();
            }

            return NoContent(); // Returns 204 success, no content
        }

        // DELETE: api/DronesApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDrone(int id)
        {
            if (!IsLoggedIn() || !IsAdmin())
            {
                return Forbid();
            }

            // DELEGATION: Calls the Service layer
            await _droneService.DeleteDroneAsync(id);
            
            // Service handles NotFound implicitly. If no exception, assume success.
            // A common pattern is checking service result, but we return 204 regardless of if it existed.
            return NoContent(); // Returns 204 success
        }
    }
}