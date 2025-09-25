using GameStore.PL.Models;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace GameStore.PL.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        
        public IActionResult Error()
        {
            var vm = new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };

            // التقاط الاستثناء من الـ pipeline
            var feature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            var env = HttpContext.RequestServices.GetRequiredService<IWebHostEnvironment>();

            if (feature != null && env.IsDevelopment())
            {
                vm.Path = feature.Path;
                vm.ExceptionMessage = feature.Error.Message;
                vm.StackTrace = feature.Error.StackTrace;
            }

            return View(vm);
        }

        // صفحة أكواد الحالة (404, 403, 500 ...)
        [Route("Home/StatusCode")]
        public IActionResult StatusCodePage(int code)
        {
            var vm = new ErrorViewModel
            {
                StatusCode = code,
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };
            return View("StatusCode", vm);
        }
    }
}
