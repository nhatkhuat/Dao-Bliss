using DaoBlissWebApp.Common.Entities;
using DaoBlissWebApp.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace DaoBlissWebApp.Areas.Admin.Pages.Dashboard
{
	[Authorize(Roles = "Admin")]
	public class IndexModel : PageModel
    {
		private readonly ApplicationDbContext _context;

		public IndexModel(ApplicationDbContext context)
		{
			_context = context;
		}

		public int TotalOrders { get; set; }
		public int PendingOrders { get; set; }
		public int ShippingOrders { get; set; }
		public int CompletedOrders { get; set; }
		public int CancelledOrders { get; set; }
		public string TotalOrdersChange { get; set; }
		public string PendingOrdersChange { get; set; }
		public string ShippingOrdersChange { get; set; }
		public string CompletedOrdersChange { get; set; }
		public string CancelledOrdersChange { get; set; }
		public List<Order> LatestOrders { get; set; }

		public async Task OnGetAsync()
		{
			var now = DateTime.Now;
			var startOfToday = now.Date;
			var startOfThisMonth = new DateTime(now.Year, now.Month, 1);
			var startOfLastMonth = startOfThisMonth.AddMonths(-1);
			var startOfThisWeek = now.Date.AddDays(-(int)now.DayOfWeek);
			var startOfLastWeek = startOfThisWeek.AddDays(-7);

			// Tổng đơn hàng
			TotalOrders = await _context.Orders.CountAsync();
			var lastMonthTotal = await _context.Orders
				.Where(o => o.CreatedAt >= startOfLastMonth && o.CreatedAt < startOfThisMonth)
				.CountAsync();
			TotalOrdersChange = CalculatePercentageChange(TotalOrders, lastMonthTotal);

			// Đang chờ xử lý
			PendingOrders = await _context.Orders
				.Where(o => o.Status == "Pending")
				.CountAsync();
			PendingOrdersChange = PendingOrders > 0 ? $"+{PendingOrders}" : "0";

			// Đang vận chuyển
			ShippingOrders = await _context.Orders
				.Where(o => o.Status == "Shipping")
				.CountAsync();
			var lastWeekShipping = await _context.Orders
				.Where(o => o.Status == "Shipping" && o.CreatedAt >= startOfLastWeek && o.CreatedAt < startOfThisWeek)
				.CountAsync();
			ShippingOrdersChange = CalculatePercentageChange(ShippingOrders, lastWeekShipping);

			// Đã hoàn thành
			CompletedOrders = await _context.Orders
				.Where(o => o.Status == "Completed")
				.CountAsync();
			var lastMonthCompleted = await _context.Orders
				.Where(o => o.Status == "Completed" && o.CreatedAt >= startOfLastMonth && o.CreatedAt < startOfThisMonth)
				.CountAsync();
			CompletedOrdersChange = CalculatePercentageChange(CompletedOrders, lastMonthCompleted);

			// Đã hủy
			CancelledOrders = await _context.Orders
				.Where(o => o.Status == "Cancelled")
				.CountAsync();
			var lastMonthCancelled = await _context.Orders
				.Where(o => o.Status == "Cancelled" && o.CreatedAt >= startOfLastMonth && o.CreatedAt < startOfThisMonth)
				.CountAsync();
			CancelledOrdersChange = CalculatePercentageChange(CancelledOrders, lastMonthCancelled);

			// 5 đơn hàng mới nhất
			LatestOrders = await _context.Orders
				.OrderByDescending(o => o.CreatedAt)
				.Take(5)
				.ToListAsync();
		}

		private string CalculatePercentageChange(int current, int previous)
		{
			if (previous == 0) return current > 0 ? "+100%" : "0%";
			var change = ((double)(current - previous) / previous) * 100;
			return change >= 0 ? $"+{change:F0}%" : $"{change:F0}%";
		}
	}
}
