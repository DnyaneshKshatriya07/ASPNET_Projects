using AeroDroxUAV.Models;
using AeroDroxUAV.Services;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace AeroDroxUAV.Controllers
{
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public class DroneServicesController : Controller
    {
        private readonly IDroneServicesService _droneServicesService;
        private readonly IWebHostEnvironment _webHostEnvironment; // Added for file paths

        public DroneServicesController(IDroneServicesService droneServicesService, IWebHostEnvironment webHostEnvironment)
        {
            _droneServicesService = droneServicesService;
            _webHostEnvironment = webHostEnvironment;
        }

        // Security Helpers
        private bool IsAdmin() => HttpContext.Session.GetString("Role") == "Admin";
        private bool IsLoggedIn() => !string.IsNullOrEmpty(HttpContext.Session.GetString("Username"));

        public async Task<IActionResult> Index()
        {
            var droneServicesList = await _droneServicesService.GetAllDroneServicesAsync();
            ViewBag.Role = HttpContext.Session.GetString("Role") ?? "";
            ViewBag.IsLoggedIn = IsLoggedIn();
            return View(droneServicesList);
        }

        public async Task<IActionResult> Details(int id)
        {
            var droneService = await _droneServicesService.GetDroneServicesByIdAsync(id);
            if (droneService == null) return NotFound();

            ViewBag.Role = HttpContext.Session.GetString("Role") ?? "";
            ViewBag.IsLoggedIn = IsLoggedIn();
            return View(droneService);
        }

        // ========== ADMIN CRUD ONLY ==========

        public IActionResult Create()
        {
            if (!IsLoggedIn() || !IsAdmin()) return Unauthorized();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(DroneServices droneServices, IFormFile imageFile)
        {
            if (!IsLoggedIn() || !IsAdmin()) return Unauthorized();

            if (ModelState.IsValid)
            {
                // Handle Image Upload
                if (imageFile != null && imageFile.Length > 0)
                {
                    droneServices.ImageUrl = await SaveImage(imageFile);
                }

                await _droneServicesService.CreateDroneServicesAsync(droneServices);
                return RedirectToAction("Index");
            }
            return View(droneServices);
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (!IsLoggedIn() || !IsAdmin()) return Unauthorized();

            var droneServices = await _droneServicesService.GetDroneServicesByIdAsync(id);
            if (droneServices == null) return NotFound();
            return View(droneServices);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(DroneServices droneServices, IFormFile? imageFile)
        {
            if (!IsLoggedIn() || !IsAdmin()) return Unauthorized();

            if (ModelState.IsValid)
            {
                // If a new image is uploaded, replace the old one
                if (imageFile != null && imageFile.Length > 0)
                {
                    droneServices.ImageUrl = await SaveImage(imageFile);
                }

                await _droneServicesService.UpdateDroneServicesAsync(droneServices);
                return RedirectToAction("Index");
            }
            return View(droneServices);
        }

        // Helper Method to Save Image
        private async Task<string> SaveImage(IFormFile imageFile)
        {
            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images/services");
            
            // Ensure directory exists
            if (!Directory.Exists(uploadsFolder)) 
                Directory.CreateDirectory(uploadsFolder);

            string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(imageFile.FileName);
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }

            return "/images/services/" + uniqueFileName;
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (!IsLoggedIn() || !IsAdmin()) return Unauthorized();

            var droneServices = await _droneServicesService.GetDroneServicesByIdAsync(id);
            if (droneServices == null) return NotFound();
            return View(droneServices);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!IsLoggedIn() || !IsAdmin()) return Unauthorized();

            await _droneServicesService.DeleteDroneServicesAsync(id);
            return RedirectToAction("Index");
        }
    }
}