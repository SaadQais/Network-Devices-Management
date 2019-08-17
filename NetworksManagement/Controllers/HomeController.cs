using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetworksManagement.Core;
using NetworksManagement.Data.Models;

namespace NetworksManagement.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IDevicesRepository _devicesRepository;
       
        public HomeController(IDevicesRepository devicesRepository)
        {
            _devicesRepository = devicesRepository;
        }

        public async Task<IActionResult> Index()
        {
            var devices = await _devicesRepository.GetAllMonitoring().ToListAsync();

            ViewBag.Current = "Home";
            return View(devices);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
