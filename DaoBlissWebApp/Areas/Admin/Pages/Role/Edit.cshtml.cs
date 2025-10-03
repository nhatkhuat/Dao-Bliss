
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Shared;
using System.ComponentModel.DataAnnotations;

namespace DaoBlissWebApp.Areas.Admin.Pages.Role
{
	[Authorize(Roles = "Admin")]
	public class EditModel : RolePageModel
	{
		public EditModel(RoleManager<IdentityRole> roleManager) : base(roleManager)
		{
		}

		[BindProperty]
		public InputModel Input { get; set; }
		[BindProperty]
		public IdentityRole Role { get; set; }
		public class InputModel
		{
			[Display(Name = "RoleName")]
			[Required(ErrorMessage = "Not null")]
			[StringLength(256, MinimumLength = 3, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.")]
			public string Name { get; set; }
		}

		public async Task<IActionResult> OnGetAsync(string roleid)
		{
			if (string.IsNullOrEmpty(roleid))
			{
				return NotFound("Role not found");
			}
			var role = await _roleManager.FindByIdAsync(roleid);

			if (role != null)
			{
				Input = new InputModel
				{
					Name = role.Name
				};
				return Page();
			}

			return NotFound("Role not found");
		}
		public async Task<IActionResult> OnPostAsync(string roleid)
		{
			if (roleid == null) return NotFound("Role not found");
			var role = await _roleManager.FindByIdAsync(roleid);
			if(role == null ) return NotFound("Role not found");
			if (!ModelState.IsValid)
			{
				return Page();
			}


			role.Name = Input.Name;
			var result = await _roleManager.UpdateAsync(role);

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
