
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace DaoBlissWebApp.Areas.Admin.Pages.Role
{
	[Authorize(Roles = "Admin")]
	public class CreateModel : RolePageModel
	{
		public CreateModel(RoleManager<IdentityRole> roleManager) : base(roleManager)
		{
		}

		[BindProperty]
		public InputModel Input { get; set; }

		public class InputModel
		{
			[Display(Name = "RoleName")]
			[Required(ErrorMessage = "Not null")]
			[StringLength(256, MinimumLength = 3, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.")]
			public string Name { get; set; }
		}

		public void OnGet()
		{
		}
		public async Task<IActionResult> OnPostAsync()
		{
			if (!ModelState.IsValid)
			{
				return Page();
			}

			var newRole = new IdentityRole(Input.Name);
			var result = await _roleManager.CreateAsync(newRole);
			if (result.Succeeded)
			{
				StatusMessage = $"Role '{Input.Name}' created successfully.";
				return RedirectToPage("./Index");
			}
			else
			{
				result.Errors.ToList().ForEach(error =>
						ModelState.AddModelError(string.Empty, error.Description)
				);
			}
			return Page();
		}
	}
}
