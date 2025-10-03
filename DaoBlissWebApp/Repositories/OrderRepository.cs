
using DaoBlissWebApp.Interfaces.Repositories;
using DaoBlissWebApp.Common.Entities;

using DaoBlissWebApp.Data;
using Microsoft.EntityFrameworkCore;

namespace DaoBlissWebApp.Repositories
{
	public class OrderRepository : IOrderRepository
	{
		private readonly ApplicationDbContext _context;

		public OrderRepository(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<Order?> GetOrderByIdAsync(int id)
		{
			return await _context.Orders
				.Include(o => o.Items).ThenInclude(i => i.ProductVariant).ThenInclude(pv => pv.Product).ThenInclude(p => p.Images)
				.Include(o => o.Items).ThenInclude(i => i.ProductVariant).ThenInclude(pv => pv.Size)
				.FirstOrDefaultAsync(o => o.Id == id);
		}

		public async Task<Order?> GetOrderByNumberAsync(string orderNumber)
		{
			return await _context.Orders
				.Include(o => o.Items).ThenInclude(i => i.ProductVariant).ThenInclude(pv => pv.Product).ThenInclude(p => p.Images)
				.Include(o => o.Items).ThenInclude(i => i.ProductVariant).ThenInclude(pv => pv.Size)
				.FirstOrDefaultAsync(o => o.OrderNumber == orderNumber);
		}

		public async Task<List<Order>> GetAllOrdersAsync()
		{
			return await _context.Orders
				.Include(o => o.Items)
				.ToListAsync();
		}

		public async Task<List<Order>> GetOrdersByUserIdAsync(string userId)
		{
			return await _context.Orders
				.Where(o => o.UserId == userId)
				.Include(o => o.Items)
				.ToListAsync();
		}

		public async Task AddOrderAsync(Order order)
		{
			_context.Orders.Add(order);
			await _context.SaveChangesAsync();
		}

		public async Task UpdateOrderAsync(Order order)
		{
			_context.Orders.Update(order);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteOrderAsync(int id)
		{
			var order = await _context.Orders.FindAsync(id);
			if (order != null)
			{
				_context.Orders.Remove(order);
				await _context.SaveChangesAsync();
			}
		}

		public async Task AddOrderItemAsync(OrderItem orderItem)
		{
			_context.OrderItems.Add(orderItem);
			await _context.SaveChangesAsync();
		}

		public async Task UpdateOrderItemAsync(OrderItem orderItem)
		{
			_context.OrderItems.Update(orderItem);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteOrderItemAsync(int id)
		{
			var item = await _context.OrderItems.FindAsync(id);
			if (item != null)
			{
				_context.OrderItems.Remove(item);
				await _context.SaveChangesAsync();
			}
		}

		public async Task SaveChangesAsync()
		{
			await _context.SaveChangesAsync();
		}
	}
}
