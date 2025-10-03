using DaoBlissWebApp.Common.Entities;
using DaoBlissWebApp.Data;
using DaoBlissWebApp.Interfaces.Repositories;
using DaoBlissWebApp.Interfaces.Services;
using MailKit.Search;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DaoBlissWebApp.Areas.Admin.Pages.Orders
{
	[Authorize(Roles = "Admin")]
	public class EditModel : PageModel
	{
		private readonly ApplicationDbContext _context;
		private readonly IOrderService _orderService;

		public EditModel(ApplicationDbContext context, IOrderService orderService)
		{
			_context = context;
			_orderService = orderService;
		}

		[BindProperty]
		public Order Order { get; set; } = default!;

		public async Task<IActionResult> OnGetAsync(string? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			Order = await _orderService.GetOrderByNumberAsync(id);
			if (Order == null)
			{
				return NotFound();
			}
			return Page();
		}

		public async Task<IActionResult> OnPostAsync()
		{
			if (!ModelState.IsValid)
			{
				return Page();
			}

			_context.Attach(Order).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
				TempData["SuccessMessage"] = "Đã chỉnh sửa đơn hàng thành công!";
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!OrderExists(Order.Id))
				{
					return NotFound();
				}
				else
				{
					throw;
				}
			}

			return RedirectToPage("./Index");
		}

		private bool OrderExists(int id)
		{
			return _context.Orders.Any(e => e.Id == id);
		}
	}
}
