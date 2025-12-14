// File: ViewComponents/ServicesNavigationViewComponent.cs
using AeroDroxUAV.Services;
using Microsoft.AspNetCore.Mvc;

namespace AeroDroxUAV.ViewComponents
{
    // The name is inferred from the class name (ServicesNavigationViewComponent -> 'ServicesNavigation')
    public class ServicesNavigationViewComponent : ViewComponent
    {
        private readonly IDroneServicesService _droneServicesService;

        // Inject the Service Layer dependency
        public ServicesNavigationViewComponent(IDroneServicesService droneServicesService)
        {
            _droneServicesService = droneServicesService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            // 1. Fetch the data
            var services = await _droneServicesService.GetAllDroneServicesAsync();
            
            // 2. Pass the data to the View Component's view.
            return View(services);
        }
    }
}