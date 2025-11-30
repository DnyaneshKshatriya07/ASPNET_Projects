using AeroDroxUAV.Models;
using AeroDroxUAV.Services; 
using Microsoft.AspNetCore.Mvc;

namespace AeroDroxUAV.Controllers
{
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public class DroneController : Controller
    {
        private readonly IDroneService _droneService; 

        public DroneController(IDroneService droneService) 
        {
            _droneService = droneService;
        }

        // Security Helpers
        private bool IsAdmin() => HttpContext.Session.GetString("Role") == "Admin";
        private bool IsLoggedIn() => !string.IsNullOrEmpty(HttpContext.Session.GetString("Username"));


        // View all drones for everyone
        public async Task<IActionResult> Index()
        {
            if (!IsLoggedIn()) return RedirectToAction("Login", "Account");

            var drones = await _droneService.GetAllDronesAsync();
            ViewBag.Role = HttpContext.Session.GetString("Role");
            return View(drones);
        }

        // ========== ADMIN CRUD ONLY ==========

        public IActionResult Create()
        {
            if(!IsLoggedIn() || !IsAdmin()) return Unauthorized();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Drone drone)
        {
            if(!IsLoggedIn() || !IsAdmin()) return Unauthorized();

            if(ModelState.IsValid)
            {
                await _droneService.CreateDroneAsync(drone);
                return RedirectToAction("Index");
            }
            return View(drone);
        }

        public async Task<IActionResult> Edit(int id)
        {
            if(!IsLoggedIn() || !IsAdmin()) return Unauthorized();

            var drone = await _droneService.GetDroneByIdAsync(id);
            if(drone == null) return NotFound();
            return View(drone);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Drone drone)
        {
            if(!IsLoggedIn() || !IsAdmin()) return Unauthorized();

            if(ModelState.IsValid)
            {
                await _droneService.UpdateDroneAsync(drone);
                return RedirectToAction("Index");
            }
            return View(drone);
        }

        public async Task<IActionResult> Delete(int id)
        {
            if(!IsLoggedIn() || !IsAdmin()) return Unauthorized();

            var drone = await _droneService.GetDroneByIdAsync(id);
            if(drone == null) return NotFound();
            return View(drone);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if(!IsLoggedIn() || !IsAdmin()) return Unauthorized();

            await _droneService.DeleteDroneAsync(id);
            
            return RedirectToAction("Index");
        }
    }
}