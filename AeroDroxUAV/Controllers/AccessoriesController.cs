using AeroDroxUAV.Models;
using AeroDroxUAV.Services; 
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace AeroDroxUAV.Controllers
{
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public class AccessoriesController : Controller
    {
        private readonly IAccessoriesService _accessoriesService; 
        private readonly IWebHostEnvironment _environment;

        public AccessoriesController(IAccessoriesService accessoriesService, IWebHostEnvironment environment) 
        {
            _accessoriesService = accessoriesService;
            _environment = environment;
        }

        // Security Helpers
        private bool IsAdmin() => HttpContext.Session.GetString("Role") == "Admin";
        private bool IsLoggedIn() => !string.IsNullOrEmpty(HttpContext.Session.GetString("Username"));

        // View all accessories - NO LOGIN REQUIRED
        public async Task<IActionResult> Index()
        {
            var accessories = await _accessoriesService.GetAllAccessoriesAsync();
            ViewBag.Role = HttpContext.Session.GetString("Role") ?? "";
            ViewBag.IsLoggedIn = IsLoggedIn();
            return View(accessories);
        }

        // Accessory Details - NO LOGIN REQUIRED
        public async Task<IActionResult> Details(int id)
        {
            var accessory = await _accessoriesService.GetAccessoriesByIdAsync(id);
            if (accessory == null) return NotFound();
            ViewBag.IsLoggedIn = IsLoggedIn();
            ViewBag.Role = HttpContext.Session.GetString("Role") ?? "";
            return View(accessory);
        }

        // ========== ADMIN CRUD ONLY ==========
        public IActionResult Create()
        {
            if(!IsLoggedIn() || !IsAdmin()) return Unauthorized();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Accessories accessory)
        {
            if(!IsLoggedIn() || !IsAdmin()) return Unauthorized();

            if(ModelState.IsValid)
            {
                // Handle image upload
                if (accessory.ImageFile != null && accessory.ImageFile.Length > 0)
                {
                    var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", "accessories");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(accessory.ImageFile.FileName);
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await accessory.ImageFile.CopyToAsync(fileStream);
                    }

                    accessory.ImageUrl = $"/uploads/accessories/{uniqueFileName}";
                }
                else
                {
                    accessory.ImageUrl = "/images/default-accessory.jpg";
                }

                accessory.CreatedAt = DateTime.Now;
                
                // Validate discount price
                if (accessory.DiscountPrice.HasValue && accessory.DiscountPrice.Value >= accessory.Price)
                {
                    ModelState.AddModelError("DiscountPrice", "Discount price must be less than original price");
                    return View(accessory);
                }

                await _accessoriesService.CreateAccessoriesAsync(accessory);
                return RedirectToAction("Index");
            }
            
            return View(accessory);
        }

        public async Task<IActionResult> Edit(int id)
        {
            if(!IsLoggedIn() || !IsAdmin()) return Unauthorized();

            var accessory = await _accessoriesService.GetAccessoriesByIdAsync(id);
            if(accessory == null) return NotFound();
            return View(accessory);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Accessories accessory)
        {
            if(!IsLoggedIn() || !IsAdmin()) return Unauthorized();

            if(ModelState.IsValid)
            {
                var existingAccessory = await _accessoriesService.GetAccessoriesByIdAsync(accessory.Id);
                if (existingAccessory == null) return NotFound();

                // Handle image upload
                if (accessory.ImageFile != null && accessory.ImageFile.Length > 0)
                {
                    // Delete old image if exists
                    if (!string.IsNullOrEmpty(existingAccessory.ImageUrl) && 
                        existingAccessory.ImageUrl != "/images/default-accessory.jpg")
                    {
                        var oldImagePath = Path.Combine(_environment.WebRootPath, existingAccessory.ImageUrl.TrimStart('/'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    // Save new image
                    var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", "accessories");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(accessory.ImageFile.FileName);
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await accessory.ImageFile.CopyToAsync(fileStream);
                    }

                    accessory.ImageUrl = $"/uploads/accessories/{uniqueFileName}";
                }
                else
                {
                    accessory.ImageUrl = existingAccessory.ImageUrl;
                }

                // Validate discount price
                if (accessory.DiscountPrice.HasValue && accessory.DiscountPrice.Value >= accessory.Price)
                {
                    ModelState.AddModelError("DiscountPrice", "Discount price must be less than original price");
                    return View(accessory);
                }

                await _accessoriesService.UpdateAccessoriesAsync(accessory);
                return RedirectToAction("Index");
            }
            return View(accessory);
        }

        public async Task<IActionResult> Delete(int id)
        {
            if(!IsLoggedIn() || !IsAdmin()) return Unauthorized();

            var accessory = await _accessoriesService.GetAccessoriesByIdAsync(id);
            if(accessory == null) return NotFound();
            return View(accessory);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if(!IsLoggedIn() || !IsAdmin()) return Unauthorized();

            var accessory = await _accessoriesService.GetAccessoriesByIdAsync(id);
            if (accessory != null)
            {
                // Delete image file if exists and not default
                if (!string.IsNullOrEmpty(accessory.ImageUrl) && 
                    accessory.ImageUrl != "/images/default-accessory.jpg")
                {
                    var imagePath = Path.Combine(_environment.WebRootPath, accessory.ImageUrl.TrimStart('/'));
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }
            }

            await _accessoriesService.DeleteAccessoriesAsync(id);
            return RedirectToAction("Index");
        }
    }
}