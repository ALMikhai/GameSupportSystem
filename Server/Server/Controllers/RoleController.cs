using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Server.Models;

namespace Server.Controllers
{
	public class RoleController : Controller
	{
		private RoleManager<IdentityRole> roleManager;
		private UserManager<Models.Operator.Account> userManager;

		public RoleController(RoleManager<IdentityRole> roleManager, UserManager<Models.Operator.Account> userManager) {
			this.roleManager = roleManager;
			this.userManager = userManager;
		}

		public IActionResult Index() {
			return View(roleManager.Roles);
		}

		public IActionResult Create() => View();

		[HttpPost]
		public async Task<IActionResult> Create(string name) {
			if (!string.IsNullOrEmpty(name)) {
				var result = await roleManager.CreateAsync(new IdentityRole(name));
				if (result.Succeeded) {
					return RedirectToAction("Index");
				} else {
					foreach (var error in result.Errors) {
						ModelState.AddModelError(string.Empty, error.Description);
					}
				}
			}
			return View(name);
		}

		[HttpPost]
		public async Task<IActionResult> Delete(string id) {
			var role = await roleManager.FindByIdAsync(id);
			if (role != null) {
				var result = await roleManager.DeleteAsync(role);
			}
			return RedirectToAction("Index");
		}

		public IActionResult UserList() => View(userManager.Users.ToList());

		public async Task<IActionResult> Edit(string userId) {
			var user = await userManager.FindByIdAsync(userId);
			if (user != null) {
				var userRoles = await userManager.GetRolesAsync(user);
				var allRoles = roleManager.Roles.ToList();
				var model = new ChangeRoleModel {
					UserId = user.Id,
					UserEmail = user.Email,
					UserRoles = userRoles,
					AllRoles = allRoles
				};
				return View(model);
			}
			return NotFound();
		}

		[HttpPost]
		public async Task<IActionResult> Edit(string userId, List<string> roles) {
			var user = await userManager.FindByIdAsync(userId);
			if (user != null) {
				var userRoles = await userManager.GetRolesAsync(user);
				var addedRoles = roles.Except(userRoles);
				var removedRoles = userRoles.Except(roles);
				await userManager.AddToRolesAsync(user, addedRoles);
				await userManager.RemoveFromRolesAsync(user, removedRoles);
				return RedirectToAction("UserList");
			}
			return NotFound();
		}
	}
}
