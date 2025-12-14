using Microsoft.AspNetCore.Mvc;

namespace AeroDroxUAV.Controllers
{
    // Apply anti-caching
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public class AboutController : Controller
    {
        // GET: /About/Index
        public IActionResult Index()
        {
            ViewData["Title"] = "About Us";
            // Example data to pass to the view
            ViewBag.YearFounded = 2020; 
            ViewBag.Mission = "To provide cutting-edge, reliable, and sustainable UAV solutions for industrial and recreational use.";
            return View();
        }
    }
}