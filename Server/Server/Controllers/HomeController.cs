﻿using Microsoft.AspNetCore.Mvc;
using Server.Models;
using System.Diagnostics;

namespace Server.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> logger;

		public HomeController(ILogger<HomeController> logger) {
			this.logger = logger;
		}

		public IActionResult Index() {
			return View();
		}

		public IActionResult AccessDenied(string? returnUrl) {
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error() {
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}