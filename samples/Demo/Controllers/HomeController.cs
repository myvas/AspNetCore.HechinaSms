using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AspNetCore.HechinaSmsService.Sample.Models;
using AspNetCore.HechinaSmsService;

namespace AspNetCore.HechinaSmsService.Sample.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ISmsSender _smsSender;

        public HomeController(
            ILoggerFactory loggerFactory,
            ISmsSender smsSender)
        {
            _logger = loggerFactory?.CreateLogger<HomeController>() ?? throw new ArgumentNullException(nameof(loggerFactory));
            _smsSender = smsSender ?? throw new ArgumentNullException(nameof(smsSender));
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendSms(SendSmsViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return Ok(false);
            }

            var result = await _smsSender.SendSmsAsync(vm.Mobile, vm.Content);

            return Ok(result);
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }
    }
}