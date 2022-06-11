using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Server.Models;

namespace Server.Controllers
{
	/// <summary>
	/// Controller that determines the functionality of working with roles.
	/// </summary>
	[Authorize(Roles = "Operator")]
	public class RoleController : Controller
	{
		private RoleManager<IdentityRole> roleManager;
		private UserManager<Models.Operator.Account> userManager;

		/// <summary>
		/// Controller that determines the functionality of working with roles.
		/// </summary>
		/// <param name="roleManager">Role manager.</param>
		/// <param name="userManager">Operator manager.</param>
		public RoleController(RoleManager<IdentityRole> roleManager, UserManager<Models.Operator.Account> userManager) {
			this.roleManager = roleManager;
			this.userManager = userManager;
		}

		/// <summary>
		/// Page listing existing roles.
		/// </summary>
		/// <returns>Index page.</returns>
		public IActionResult Index() {
			return View(roleManager.Roles);
		}

		/// <summary>
		/// Create role page with empty model.
		/// </summary>
		/// <returns>Create page.</returns>
		public IActionResult Create() => View();

		/// <summary>
		/// Create role request with fill model.
		/// </summary>
		/// <param name="name">Role name.</param>
		/// <returns>If the create was successful, redirect to the index page, else back to the create page.</returns>
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

		/// <summary>
		/// Delete role request.
		/// </summary>
		/// <param name="id">Id of delete role.</param>
		/// <returns>Redirect to the index page.</returns>
		[HttpPost]
		public async Task<IActionResult> Delete(string id) {
			var role = await roleManager.FindByIdAsync(id);
			if (role != null) {
				var result = await roleManager.DeleteAsync(role);
			}
			return RedirectToAction("Index");
		}

		/// <summary>
		/// Page listing existing users.
		/// </summary>
		/// <returns></returns>
		public IActionResult UserList() => View(userManager.Users.ToList());

		/// <summary>
		/// Page for edit user roles.
		/// </summary>
		/// <param name="userId">User id.</param>
		/// <returns>If user exist return edit page, else NotFound page.</returns>
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

		/// <summary>
		/// Edit role user roles request.
		/// </summary>
		/// <param name="userId">User id.</param>
		/// <param name="roles">New roles.</param>
		/// <returns>If user exist redirect to users page, else NotFound page.</returns>
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
