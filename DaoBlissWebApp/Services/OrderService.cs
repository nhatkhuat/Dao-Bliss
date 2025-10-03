

using DaoBlissWebApp.Interfaces.Services;
using DaoBlissWebApp.Interfaces.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Text;
using DaoBlissWebApp.Common.Entities;

namespace DaoBlissWebApp.Services.OrderServices
{
	public class OrderService : IOrderService
	{
		private readonly IOrderRepository _orderRepository;
		private readonly IProductService _productService;
		private readonly IEmailSender _emailSender;
		private readonly UserManager<ApplicationUser> _userManager;

		public OrderService(IOrderRepository orderRepository, IProductService productService, IEmailSender emailSender, UserManager<ApplicationUser> userManager)
		{
			_orderRepository = orderRepository;
			_productService = productService;
			_emailSender = emailSender;
			_userManager = userManager;
		}

		public async Task CreateOrderAsync(Order order, List<OrderItem> items)
		{
			// Validate stock
			foreach (var item in items)
			{
				var variant = await _productService.GetProductVariantByIdAsync(item.ProductVariantId);
				if (variant == null || variant.Stock < item.Quantity)
				{
					throw new InvalidOperationException($"Insufficient stock for variant {item.ProductVariantId}");
				}
			}

			// Update stock
			foreach (var item in items)
			{
				await _productService.UpdateVariantStockAsync(item.ProductVariantId, -item.Quantity);
			}

			// Add order and items
			await _orderRepository.AddOrderAsync(order);
			foreach (var item in items)
			{
				item.OrderId = order.Id;
				await _orderRepository.AddOrderItemAsync(item);
			}

			await _orderRepository.SaveChangesAsync();

			// Send emails
			_ = Task.Run(async () =>
			{
				try
				{
					await _emailSender.SendEmailAsync(
						"daobliss.official@gmail.com",
						"Daobliss có đơn đặt hàng mới",
						"Có đơn đặt hàng mới, hãy vào website để liên hệ xác nhận đơn hàng với khách.");

					var body = BuildOrderEmailBody(order);
					await _emailSender.SendEmailAsync(
						order.ShippingEmail,
						$"Đặt hàng thành công #{order.OrderNumber}",
						body);
				}
				catch (Exception ex)
				{
					Console.WriteLine($"[Email Error] {ex.Message}");
				}
			});
		}

		public async Task<Order?> GetOrderByNumberAsync(string orderNumber)
		{
			return await _orderRepository.GetOrderByNumberAsync(orderNumber);
		}

		private string BuildOrderEmailBody(Order order)
		{
			var sb = new StringBuilder();

			sb.Append($@"
    <div style='font-family: Arial, sans-serif; line-height:1.5;'>
        <h2 style='color:#2c3e50;'>Daobliss - Đặt hàng thành công #{order.OrderNumber}</h2>
        <p>Thân gửi anh/chị {order.ShippingFullName},</p>
        <p>Cảm ơn anh/chị đã mua hàng tại <strong>Daobliss</strong>!</p>
        <p><strong>Tình trạng: </strong>Chờ xác nhận</p>
        <p><strong>Mã đơn hàng:</strong> {order.OrderNumber}</p>

<<<<<<< HEAD
			//foreach (var item in details)
			//{
			//	sb.Append($@"
			//             <tr>
			//                 <td>{item.Name}</td>
			//                 <td>{item.UnitPrice.ToString("C0", culture)}</td>
			//                 <td>{item.Quantity}</td>
			//                 <td>{item.TotalPrice.ToString("C0", culture)}</td>
			//             </tr>");
			//}
=======
        <h3>Thông tin sản phẩm:</h3>
        <table style='width:100%; border-collapse: collapse;'>
            <thead>
                <tr style='background:#f2f2f2;'>
                    <th style='border:1px solid #ddd; padding:8px;'>Tên sản phẩm</th>
                    <th style='border:1px solid #ddd; padding:8px;'>Số lượng</th>
                    <th style='border:1px solid #ddd; padding:8px;'>Đơn giá</th>
                    <th style='border:1px solid #ddd; padding:8px;'>Thành tiền</th>
                </tr>
            </thead>
            <tbody>");
>>>>>>> Nhat

			foreach (var item in order.Items)
			{
				sb.Append($@"
            <tr>
                <td style='border:1px solid #ddd; padding:8px;'>{item.Name} ({item.Size}{item.ProductVariant?.Size?.Unit})</td>
                <td style='border:1px solid #ddd; padding:8px;'>{item.Quantity}</td>
                <td style='border:1px solid #ddd; padding:8px;'>{item.Price:N0}đ</td>
                <td style='border:1px solid #ddd; padding:8px;'>{item.Total:N0}đ</td>
            </tr>");
			}

			sb.Append($@"
            </tbody>
        </table>

        <p><strong>Tổng giá trị sản phẩm:</strong> {order.SubTotal:N0}đ</p>
        <p><strong>Phí giao hàng:</strong> +{order.ShippingFee:N0}đ</p>
        <p><strong>Khuyến mại:</strong> -{order.Discount:N0}đ</p>
        <p><strong>Tổng thanh toán:</strong> {order.Total:N0}đ</p>
        <p><strong>Hình thức thanh toán:</strong> {order.PaymentMethod}</p>

        <h3>Thông tin nhận hàng:</h3>
        <p>
            Họ và tên: {order.ShippingFullName}<br/>
            Số điện thoại: {order.ShippingPhoneNumber}<br/>
            Địa chỉ: {order.ShippingAddress}, {order.ShippingWard}, {order.ShippingDistrict}, {order.ShippingCity}
        </p>
        <p><strong>Ghi chú:</strong> {order.Notes}</p>
    </div>");

			return sb.ToString();
		}
	}
}
