using Microsoft.AspNetCore.Mvc;
using Server.Models;
using System.Diagnostics;

namespace Server.Controllers
{
	/// <summary>
	/// Controller contain functionality for main page.
	/// Also contain functionality for error and access denied pages.
	/// </summary>
	public class HomeController : Controller
	{
		/// <summary>
		/// Main page.
		/// </summary>
		/// <returns>Index page.</returns>
		public IActionResult Index() {
			return View();
		}

		/// <summary>
		/// Access denied page.
		/// Used if the user does not have enough rights to use any functionality.
		/// </summary>
		/// <param name="returnUrl">Url to go to when access is allowed.</param>
		/// <returns>AccessDenied page.</returns>
		public IActionResult AccessDenied(string? returnUrl) {
			return View();
		}

		/// <summary>
		/// Error page. Used in case of unhandled exceptions.
		/// </summary>
		/// <returns>Error page.</returns>
		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error() {
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}