using DaoBlissWebApp.Common.Entities;
using DaoBlissWebApp.Data;
using DaoBlissWebApp.Interfaces.Repositories;
using DaoBlissWebApp.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace DaoBlissWebApp.Areas.Identity.Pages.Account.Manage
{
	[Authorize]
	public class OrderDetailModel : PageModel
	{
		private readonly ApplicationDbContext _context;
		private readonly UserManager<ApplicationUser> _userManager;

		private readonly IOrderService _orderService;
		public OrderDetailModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IOrderService orderService)
		{
			_context = context;
			_userManager = userManager;
			_orderService = orderService;
		}

		public Order? Order { get; set; }
		[BindProperty(SupportsGet = true)]
		public string OrderId { get; set; }
		public async Task<IActionResult> OnGetAsync()
		{
			var user = await _userManager.GetUserAsync(User);

			Order = await _orderService.GetOrderByNumberAsync(OrderId);

			if (Order == null)
				return NotFound();

			return Page();
		}
		public async Task<IActionResult> OnPostCancelOrderAsync(string orderId)
		{
			var user = await _userManager.GetUserAsync(User);
			if (user == null)
				return Unauthorized();

			var order = await _orderService.GetOrderByNumberAsync(orderId);

			if (order == null)
				return NotFound();

			// Cập nhật trạng thái đơn
			order.Status = Common.Enums.OrderStatus.Cancelled.ToString();

			await _context.SaveChangesAsync();

			return new JsonResult(new { success = true, message = "Đơn hàng đã được hủy thành công" });
		}
	}
}
