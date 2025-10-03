using DaoBlissWebApp.Common.Entities;
using DaoBlissWebApp.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace DaoBlissWebApp.Areas.Identity.Pages.Account.Manage
{
	[Authorize]
	public class OrdersModel : PageModel
	{
		private readonly ApplicationDbContext _context;
		private readonly UserManager<ApplicationUser> _userManager;

		public OrdersModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
		{
			_context = context;
			_userManager = userManager;
		}

		public List<Order> Orders { get; set; } = new List<Order>();

		public async Task OnGetAsync()
		{
			var user = await _userManager.GetUserAsync(User);
			if (user != null)
			{
				Orders = await _context.Orders
					.Include(o => o.Items)
					.Where(o => o.UserId == user.Id)
					.OrderByDescending(o => o.CreatedAt)
					.ToListAsync();
			}
		}
	}
}
