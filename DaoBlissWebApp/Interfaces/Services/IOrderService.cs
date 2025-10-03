using DaoBlissWebApp.Common.Entities;


namespace DaoBlissWebApp.Interfaces.Services
{
	public interface IOrderService
	{
		Task CreateOrderAsync(Order order, List<OrderItem> items);
		Task<Order?> GetOrderByNumberAsync(string orderNumber);
	}
}
