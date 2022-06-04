using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Server.Models.Operator;

namespace Server.Controllers
{
	public class OperatorController : Controller
	{
		private readonly UserManager<Account> userManager;
		private readonly SignInManager<Account> signInManager;

		public OperatorController(UserManager<Account> userManager, SignInManager<Account> signInManager) {
			this.userManager = userManager;
			this.signInManager = signInManager;
		}

		[HttpGet]
		public IActionResult Register() {
			return View();
		}

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

		[HttpGet]
		public IActionResult Login() {
			return View(new LoginModel());
		}

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

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Logout() {
			await signInManager.SignOutAsync();
			return RedirectToAction("Index", "Home");
		}
	}
}
