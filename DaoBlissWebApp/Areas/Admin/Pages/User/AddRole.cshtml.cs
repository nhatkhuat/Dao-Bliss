// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using DaoBlissWebApp.Common.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace DaoBlissWebApp.Areas.Admin.Pages.User
{
<<<<<<< HEAD
	//[Authorize(Roles = "Admin")]
=======
	[Authorize(Roles = "Admin")]
>>>>>>> Nhat
	public class AddRoleModel : PageModel
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly RoleManager<IdentityRole> _roleManager;

		public AddRoleModel(
			UserManager<ApplicationUser> userManager,
			RoleManager<IdentityRole> roleManager,
			SignInManager<ApplicationUser> signInManager)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_roleManager = roleManager;
		}

		/// <summary>
		///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
		///     directly from your code. This API may change or be removed in future releases.
		/// </summary>

		/// <summary>
		///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
		///     directly from your code. This API may change or be removed in future releases.
		/// </summary>
		[TempData]
		public string StatusMessage { get; set; }

		/// <summary>
		///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
		///     directly from your code. This API may change or be removed in future releases.
		/// </summary>

		public ApplicationUser User { get; set; }

		[BindProperty]
		[Display(Name = "User's roles")]
		public string[] RoleNames { get; set; }
		public SelectList AllRoles { get; set; }

		public async Task<IActionResult> OnGetAsync(string id)
		{

			if (string.IsNullOrEmpty(id))
			{
				return NotFound("User ID cannot be null or empty.");
			}
			User = await _userManager.FindByIdAsync(id);
			if (User == null)
			{
				return NotFound($"Unable to load user with ID '{id}'.");
			}

			RoleNames = (await _userManager.GetRolesAsync(User)).ToArray<string>();

			List<string> roleNames = await _roleManager.Roles
<<<<<<< HEAD
=======
				.Where(role => (!"Admin".Equals(role.Name)))
>>>>>>> Nhat
				.Select(role => role.Name)
				.ToListAsync();

			AllRoles = new SelectList(roleNames);

			return Page();
		}

		public async Task<IActionResult> OnPostAsync(string id)
		{
			if (!ModelState.IsValid)
			{
				return Page();
			}

			if (string.IsNullOrEmpty(id))
			{
				return NotFound("User ID cannot be null or empty.");
			}

			User = await _userManager.FindByIdAsync(id);

			if (User == null)
			{
				return NotFound($"Unable to load user with ID '{id}'.");
			}

			var OldRoleNames = (await _userManager.GetRolesAsync(User)).ToArray<string>();

			var deleteRoles = OldRoleNames.Where(r => !RoleNames.Contains(r));
			var addRoles = RoleNames.Where(r => !OldRoleNames.Contains(r));

List<string> roleNames = await _roleManager.Roles
				.Select(role => role.Name)
				.ToListAsync();

			AllRoles = new SelectList(roleNames);

			var resultDelete = await _userManager.RemoveFromRolesAsync(User, deleteRoles);
			if (!resultDelete.Succeeded)
			{
				foreach (var error in resultDelete.Errors)
				{
					ModelState.AddModelError(string.Empty, error.Description);
				}
				return Page();
			}

			var resultAdd = await _userManager.AddToRolesAsync(User, addRoles);
			if (!resultDelete.Succeeded)
			{
				foreach (var error in resultDelete.Errors)
				{
					ModelState.AddModelError(string.Empty, error.Description);
				}
				return Page();
			}
			StatusMessage = $"{User.UserName}'s role has been set.";

			return RedirectToPage("./Index");
		}
	}
}
