using DaoBlissWebApp.Common.Entities;


namespace DaoBlissWebApp.Interfaces.Repositories
{
	public interface IOrderRepository
	{
		Task<Order?> GetOrderByIdAsync(int id);
		Task<Order?> GetOrderByNumberAsync(string orderNumber);
		Task<List<Order>> GetAllOrdersAsync();
		Task<List<Order>> GetOrdersByUserIdAsync(string userId);
		Task AddOrderAsync(Order order);
		Task UpdateOrderAsync(Order order);
		Task DeleteOrderAsync(int id);
		Task AddOrderItemAsync(OrderItem orderItem);
		Task UpdateOrderItemAsync(OrderItem orderItem);
		Task DeleteOrderItemAsync(int id);
		Task SaveChangesAsync();
	}
}
