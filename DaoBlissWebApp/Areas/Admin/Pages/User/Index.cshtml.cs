
using DaoBlissWebApp.Common.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace DaoBlissWebApp.Areas.Admin.Pages.User
{
<<<<<<< HEAD
	//[Authorize(Roles = "Admin")]
=======
	[Authorize(Roles = "Admin")]
>>>>>>> Nhat
	public class IndexModel : PageModel
	{
		private readonly UserManager<ApplicationUser> _userManager;
		public IndexModel(UserManager<ApplicationUser> userManager)
		{
			_userManager = userManager;
		}
		[TempData]
		public string StatusMessage { get; set; }

		public class UserAndRole : ApplicationUser
		{
			public string RoleNames { get; set; }
		}

		public List<UserAndRole> Users { get; set; }

		public const int ITEMS_PER_PAGE = 10;

		[BindProperty(SupportsGet = true,Name = "pageNumber")]
		public int currentPage { get; set; }
		public int countPages { get; set; }
		public int totalUsers { get; set; }
		public async Task OnGet()
		{
			//Users = await _userManager.Users.OrderBy(r => r.UserName).ToListAsync();
			var qr = _userManager.Users.OrderBy(r => r.UserName);
			 totalUsers = await qr.CountAsync();	

			countPages = (int)Math.Ceiling((double)totalUsers / ITEMS_PER_PAGE);

			if(currentPage < 1)
			{
				currentPage = 1;
			}
			else if(currentPage > countPages)
			{
				currentPage = countPages;
			}

			var paging = qr.Skip((currentPage - 1) * ITEMS_PER_PAGE)
				.Take(ITEMS_PER_PAGE)
				.Select(u => new UserAndRole()
				{
					Id = u.Id,
					UserName = u.UserName,
				});

			Users = await paging.ToListAsync();

			foreach (var user in Users)
			{
				var roles = await _userManager.GetRolesAsync(user);
				user.RoleNames = string.Join(",", roles);
			}


		}
		public void onPost() => RedirectToPage();
	}
}
