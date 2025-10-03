using DaoBlissWebApp.Common.Entities;
using DaoBlissWebApp.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DaoBlissWebApp.Areas.Admin.Pages.Orders
{
	[Authorize(Roles = "Admin")]
	public class IndexModel : PageModel
	{
		private readonly ApplicationDbContext _context;

		public IndexModel(ApplicationDbContext context)
		{
			_context = context;
		}

		public IList<Order> Orders { get; set; } = default!;

		public async Task OnGetAsync()
		{
			Orders = await _context.Orders
			.Include(o => o.User)
				.OrderBy(o => o.Status.Equals("Pending") ? 0 :
				  o.Status.Equals("Processing") ? 1 :
				  o.Status.Equals("Shipped") ? 2 :
				  o.Status.Equals("Completed") ? 3 :
				  o.Status.Equals("Cancelled") ? 4 :
				  o.Status.Equals("Returned") ? 5 : 6)
			.ToListAsync();
		}
	}
}
