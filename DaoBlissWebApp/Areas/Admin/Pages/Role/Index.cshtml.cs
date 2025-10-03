
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace DaoBlissWebApp.Areas.Admin.Pages.Role
{
<<<<<<< HEAD
	//[Authorize(Roles = "Admin")]
=======
	[Authorize(Roles = "Admin")]
>>>>>>> Nhat
	public class IndexModel : RolePageModel
	{
		public IndexModel(RoleManager<IdentityRole> roleManager) : base(roleManager)
		{
		}

		public List<IdentityRole> Roles { get; set; } // Renamed property to avoid ambiguity

		public async Task OnGet()
		{
			Roles = await _roleManager.Roles.OrderBy(r => r.Name).ToListAsync(); // Updated to use the renamed property
		}

		public void onPost() => RedirectToPage();
	}
}
