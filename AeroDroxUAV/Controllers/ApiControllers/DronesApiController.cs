using AeroDroxUAV.Models;
using AeroDroxUAV.Services; // Dependency on Service Layer
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AeroDroxUAV.Controllers
{
    [Route("api/[controller]")]
    [ApiController] 
    public class DronesApiController : ControllerBase 
    {
        private readonly IDroneService _droneService; 

        public DronesApiController(IDroneService droneService)
        {
            _droneService = droneService;
        }

        private bool IsLoggedIn() => !string.IsNullOrEmpty(HttpContext.Session.GetString("Username"));
        private bool IsAdmin() => HttpContext.Session.GetString("Role") == "Admin";

        // GET: api/DronesApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Drone>>> GetDrones()
        {
            // if (!IsLoggedIn()) { return Unauthorized(); }
            
            var drones = await _droneService.GetAllDronesAsync(); 
            return Ok(drones);
        }

        // GET: api/DronesApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Drone>> GetDrone(int id)
        {
            // if (!IsLoggedIn()) { return Unauthorized(); }

            var drone = await _droneService.GetDroneByIdAsync(id);

            if (drone == null)
            {
                return NotFound();
            }

            return Ok(drone);
        }

        // POST: api/DronesApi
        [HttpPost]
        public async Task<ActionResult<Drone>> PostDrone(Drone drone)
        {
            // if (!IsLoggedIn() || !IsAdmin()) { return Forbid(); }
            
            await _droneService.CreateDroneAsync(drone);

            return CreatedAtAction(nameof(GetDrone), new { id = drone.Id }, drone);
        }

        // PUT: api/DronesApi/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDrone(int id, Drone drone)
        {
            if (id != drone.Id)
            {
                return BadRequest();
            }

            // if (!IsLoggedIn() || !IsAdmin()) { return Forbid(); }
            
            // This existence check now uses the non-tracked entity, preventing the error.
            var existingDrone = await _droneService.GetDroneByIdAsync(id);
            if (existingDrone == null)
            {
                return NotFound();
            }

            try
            {
                // This call to UpdateDroneAsync now succeeds.
                await _droneService.UpdateDroneAsync(drone);
            }
            catch (DbUpdateConcurrencyException)
            {
                // Simple existence check after a potential concurrency error
                if (await _droneService.GetDroneByIdAsync(id) == null)
                {
                    return NotFound();
                }
                throw; 
            }

            return NoContent();
        }

        // DELETE: api/DronesApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDrone(int id)
        {
            // if (!IsLoggedIn() || !IsAdmin())
            // {
            //     return Forbid();
            // }

            await _droneService.DeleteDroneAsync(id);
            
            return NoContent();
        }
    }
}