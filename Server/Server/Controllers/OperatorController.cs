using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Server.Models.Operator;

namespace Server.Controllers
{
	/// <summary>
	/// Controller that determines the functionality of working with operators.
	/// </summary>
	public class OperatorController : Controller
	{
		private readonly UserManager<Account> userManager;
		private readonly SignInManager<Account> signInManager;

		/// <summary>
		/// Controller that determines the functionality of working with operators.
		/// </summary>
		/// <param name="userManager">Manager for to create new accounts.</param>
		/// <param name="signInManager">Manager for sing in operations.</param>
		public OperatorController(UserManager<Account> userManager, SignInManager<Account> signInManager) {
			this.userManager = userManager;
			this.signInManager = signInManager;
		}

		/// <summary>
		/// Registeration page with empty model.
		/// </summary>
		/// <returns>Register page.</returns>
		[HttpGet]
		public IActionResult Register() {
			return View();
		}

		/// <summary>
		/// Registeration request with fill model.
		/// </summary>
		/// <param name="model">Fill registration model.</param>
		/// <returns>If the registration was successful, redirect to the home page, else back to the registration page.</returns>
		[HttpPost]
		public async Task<IActionResult> Register(RegisterModel model) {
			if (ModelState.IsValid) {
				var user = new Account { Email = model.Email, UserName = model.Email, Name = model.Name };
				var result = await userManager.CreateAsync(user, model.Password);
				if (result.Succeeded) {
					await signInManager.SignInAsync(user, false);
					return RedirectToAction("Index", "Home");
				} else {
					foreach (var error in result.Errors) {
						ModelState.AddModelError(string.Empty, error.Description);
					}
				}
			}
			return View(model);
		}

		/// <summary>
		/// Login page with empty model.
		/// </summary>
		/// <returns>Login page.</returns>
		[HttpGet]
		public IActionResult Login() {
			return View(new LoginModel());
		}

		/// <summary>
		/// Login request with fill model.
		/// </summary>
		/// <param name="model">Fill login model.</param>
		/// <returns>If the login was successful, redirect to the home page, else back to the login page.</returns>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Login(LoginModel model) {
			if (ModelState.IsValid) {
				var result =
					await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
				if (result.Succeeded) {
					return RedirectToAction("Index", "Home");
				} else {
					ModelState.AddModelError("", "Wrong email and/or password");
				}
			}
			return View(model);
		}

		/// <summary>
		/// Logout request.
		/// </summary>
		/// <returns>Redirect to the home page.</returns>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Logout() {
			await signInManager.SignOutAsync();
			return RedirectToAction("Index", "Home");
		}
	}
}
