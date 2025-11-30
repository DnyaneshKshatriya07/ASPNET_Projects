using AeroDroxUAV.Models;
using AeroDroxUAV.Services; 
using Microsoft.AspNetCore.Mvc;

public class AccountController : Controller
{
    private readonly IUserService _userService; 

    public AccountController(IUserService userService) 
    {
        _userService = userService;
    }

    private void SetNoCacheHeaders()
    {
        // Fix: Use indexer property to set/replace headers, resolving ASP0019 warning.
        Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
        Response.Headers["Pragma"] = "no-cache";
        Response.Headers["Expires"] = "0";
    }

    [HttpGet]
    public IActionResult Login()
    {
        SetNoCacheHeaders();
        if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Username")))
        {
            return RedirectToAction("Index", "Drone");
        }
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(string Username, string Password)
    {
        SetNoCacheHeaders();
        var user = await _userService.AuthenticateAsync(Username, Password);

        if(user != null)
        {
            HttpContext.Session.SetString("Username", user.Username);
            HttpContext.Session.SetString("Role", user.Role);
            return RedirectToAction("Index", "Drone");
        }
        else
        {
            ViewBag.Error = "Invalid username or password";
            return View();
        }
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }
    
    // ===================================
    // NEW: REGISTER ACTIONS
    // ===================================
    
    [HttpGet]
    public IActionResult Register()
    {
        SetNoCacheHeaders();
        
        // Pass a flag to the view to conditionally show role selection options
        ViewBag.IsAdminLoggedIn = HttpContext.Session.GetString("Role") == "Admin";
        
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(string Username, string Password, string Role)
    {
        SetNoCacheHeaders();
        
        string finalRole = "User";

        // Security Logic: Only allow creating an Admin if the current session belongs to an Admin
        if (Role == "Admin")
        {
            if (HttpContext.Session.GetString("Role") != "Admin")
            {
                ViewBag.Error = "You do not have permission to create an Admin account.";
                ViewBag.IsAdminLoggedIn = false;
                return View("Register");
            }
            finalRole = "Admin";
        }

        var success = await _userService.CreateUserAsync(Username, Password, finalRole);

        if (success)
        {
            if (finalRole == "Admin")
            {
                 // Admin created by another admin: Stay logged in and redirect
                return RedirectToAction("Index", "Drone");
            }
            // Standard User created: Redirect to Login page
            return RedirectToAction("Login");
        }
        else
        {
            ViewBag.Error = "Username already exists. Please choose a different name.";
            ViewBag.IsAdminLoggedIn = HttpContext.Session.GetString("Role") == "Admin";
            return View("Register");
        }
    }
}