using DaoBlissWebApp.Common.Entities;
using DaoBlissWebApp.Common.Helpers;
using DaoBlissWebApp.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json;

namespace DaoBlissWebApp.Pages
{
	public class CartModel : PageModel
	{
		private readonly ApplicationDbContext _context;
		private readonly IHttpClientFactory _httpClientFactory;
		private readonly IEmailSender _emailSender;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly IUserStore<ApplicationUser> _userStore;
		private readonly IUserEmailStore<ApplicationUser> _emailStore;
		public CartModel(ApplicationDbContext context, IHttpClientFactory httpClientFactory,
			IOptions<GhnSettings> options,
			IEmailSender emailSender,
			SignInManager<ApplicationUser> signInManager,
			UserManager<ApplicationUser> userManager,
			IUserStore<ApplicationUser> userStore
			)
		{
			_context = context;
			_httpClientFactory = httpClientFactory;
			_emailSender = emailSender;
			_userManager = userManager;
			_userStore = userStore;
			_emailStore = GetEmailStore();
		}

		[BindProperty]
		[Required(ErrorMessage = "Vui lòng nhập họ và tên")]
		[StringLength(50, ErrorMessage = "Họ tên không được vượt quá 50 ký tự")]
		public string ShippingFullName { get; set; }

		[BindProperty]
		[Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
		[Phone(ErrorMessage = "Sai định dạng số điện thoại")]
		public string ShippingPhoneNumber { get; set; }

		[BindProperty]
		[Required(ErrorMessage = "Vui lòng nhập email")]
		[EmailAddress(ErrorMessage = "Email không hợp lệ")]
		public string ShippingEmail { get; set; }

		[BindProperty]
		[Required(ErrorMessage = "Vui lòng chọn tỉnh/thành phố")]
		public string ShippingCity { get; set; }

		[BindProperty]
		[Required(ErrorMessage = "Vui lòng chọn quận/huyện")]
		public string ShippingDistrict { get; set; }

		[BindProperty]
		[Required(ErrorMessage = "Vui lòng chọn phường/xã")]
		public string ShippingWard { get; set; }

		[BindProperty]
		[Required(ErrorMessage = "Vui lòng nhập địa chỉ cụ thể")]
		public string ShippingAddress { get; set; }

		[BindProperty]
		[Required(ErrorMessage = "Vui lòng chọn phương thức thanh toán")]
		public string PaymentMethod { get; set; }

		[BindProperty]
		[StringLength(50, ErrorMessage = "Mã khuyến mãi không được vượt quá 50 ký tự")]
		public string? PromoCode { get; set; }

		[BindProperty]
		public string? Note { get; set; }

		[BindProperty]
		public decimal ShippingFee { get; set; } = 0M;

		[BindProperty]
		public decimal Discount { get; set; } = 0M;


		[BindProperty]
		public List<CartItemDto> Cart { get; set; } = new List<CartItemDto>();
		public class CartItemDto
		{
			public int ProductId { get; set; }
			public int VariantId { get; set; }
			public int Quantity { get; set; }
		}

		public class ShippingRequest
		{
			public int DistrictId { get; set; }
			public string WardCode { get; set; }
			public string ProvinceName { get; set; }
			public string DistrictName { get; set; }
			public string WardName { get; set; }
			public List<CartItemDto> Cart { get; set; }
		}
		public class VariantRequest
		{
			public List<int> VariantIds { get; set; }
		}

		public class VariantResponse
		{
			public int VariantId { get; set; }
			public int ProductId { get; set; }
			public string ProductName { get; set; }
			public string VariantName { get; set; }
			public decimal Price { get; set; }
			public string ImageUrl { get; set; }
			public decimal? Weight { get; set; }
		}

		public async Task<IActionResult> OnGetAsync()
		{
			if (User.Identity?.IsAuthenticated == true)
			{
				var user = await _context.Users
					.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name || u.Email == User.Identity.Name);
				if (user != null)
				{
					ShippingEmail = user.Email;
					ShippingFullName = user.FirstName;
					ShippingPhoneNumber = user.PhoneNumber;
				}
				else
				{
					Console.WriteLine($"Không tìm thấy người dùng với UserName hoặc Email: {User.Identity.Name}");
				}
			}

			return Page();
		}
		public async Task<IActionResult> OnPostGetVariantsAsync([FromBody] VariantRequest request)
		{
			if (request?.VariantIds == null || !request.VariantIds.Any())
				return new JsonResult(new { error = "No variant IDs provided" });

			var variants = await _context.Variants
				.Where(v => request.VariantIds.Contains(v.Id) && v.IsActive)
				.Include(v => v.Product)
				.Select(v => new VariantResponse
				{
					VariantId = v.Id,
					ProductId = v.ProductId,
					ProductName = v.Product.Name,
					VariantName = v.Size.Name + v.Size.Unit,
					Price = v.Price,
					ImageUrl = v.Product.Images.FirstOrDefault().ImageUrl ?? "https://readdy.ai/api/search-image?query=serum%20vitamin%20c%20skincare%20bottle%20with%20dropper%20on%20clean%20white%20background%2C%20minimalist%20product%20photography%2C%20bright%20lighting&width=80&height=80&seq=1&orientation=squarish",
					Weight = v.Weight,
				})
				.ToListAsync();

			return new JsonResult(new { variants });
		}
		public async Task<IActionResult> OnPostCalculateShippingAsync([FromBody] ShippingRequest request)
		{
			if (request?.Cart == null || !request.Cart.Any())
				return new JsonResult(new { error = "Cart is empty" });

			// 1. Lấy variants từ DB
			var variantIds = request.Cart.Select(c => c.VariantId).ToList();
			var variants = await _context.Variants
				.Where(v => variantIds.Contains(v.Id) && v.IsActive)
				.Include(v => v.Product) // lấy tên sp để gửi GHN
				.ToListAsync();

			// 2. Tạo danh sách items gửi GHN (Hàng nhẹ)
			var items = new List<object>();
			int totalWeight = 0;

			foreach (var item in request.Cart)
			{
				var variant = variants.FirstOrDefault(v => v.Id == item.VariantId);
				if (variant != null)
				{
					int weight = (int)(variant.Weight * item.Quantity);
					totalWeight += weight;

					items.Add(new
					{
						name = variant.Product.Name,
						quantity = item.Quantity,
						length = 20,
						width = 15,
						height = 10,
						weight = weight
					});
				}
			}

			// 3. Body request GHN
			var requestBody = new
			{
				service_type_id = 2, // hoặc 5 tùy theo gói bạn chọn
				from_district_id = 1808, // quận shop
				from_ward_code = "1B1908",
				to_district_id = request.DistrictId,
				to_ward_code = request.WardCode,
				length = 20,
				width = 30,
				height = 20,
				weight = totalWeight,
				insurance_value = 0,
				coupon = (string)null,
				items = items
			};

			var client = _httpClientFactory.CreateClient("GHN");
			var json = JsonSerializer.Serialize(requestBody);
			var content = new StringContent(json, Encoding.UTF8, "application/json");

			var response = await client.PostAsync("v2/shipping-order/fee", content);
			var result = await response.Content.ReadAsStringAsync();

			decimal shippingFee = 0;
			using (var doc = JsonDocument.Parse(result))
			{
				var root = doc.RootElement;
				if (root.GetProperty("code").GetInt32() == 200)
				{
					shippingFee = root.GetProperty("data").GetProperty("total").GetDecimal();
				}
				else
				{
					return new JsonResult(new { error = root.GetProperty("message").GetString() });
				}
			}

			return new JsonResult(new { shippingFee });
		}

		public async Task<IActionResult> OnPostApplyPromoAsync([FromBody] string promoCode)
		{
			decimal discount = 0;
			if (promoCode == "ChaoMungBanMoi")
			{
				discount = 20000; // Example discount
			}
			else
			{
				return new JsonResult(new { error = "Mã khuyến mãi không hợp lệ" });
			}

			return new JsonResult(new { discount });
		}
		public async Task<IActionResult> OnGetProvincesAsync()
		{
			var client = _httpClientFactory.CreateClient("GHN");
			var response = await client.GetAsync("master-data/province");
			var result = await response.Content.ReadAsStringAsync();
			return Content(result, "application/json");
		}

		public async Task<IActionResult> OnPostDistrictsAsync([FromQuery] int province_id)
		{
			var client = _httpClientFactory.CreateClient("GHN");
			var json = JsonSerializer.Serialize(new { province_id = province_id });
			var content = new StringContent(json, Encoding.UTF8, "application/json");
			var response = await client.PostAsync("master-data/district", content);
			var result = await response.Content.ReadAsStringAsync();
			return Content(result, "application/json");
		}
		public async Task<IActionResult> OnPostWardsAsync([FromQuery] int district_id)
		{
			var client = _httpClientFactory.CreateClient("GHN");
			var json = JsonSerializer.Serialize(new { district_id = district_id });
			var content = new StringContent(json, Encoding.UTF8, "application/json");

			var response = await client.PostAsync($"master-data/ward?district_id={district_id}", content);
			var result = await response.Content.ReadAsStringAsync();

			return Content(result, "application/json");
		}

		public async Task<IActionResult> OnPostCreateOrderAsync()
		{
			if (!ModelState.IsValid)
			{
				var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
				return new JsonResult(new { error = string.Join("; ", errors) });
			}

			if (Cart == null || !Cart.Any())
				return new JsonResult(new { error = "Giỏ hàng trống" });

			await using var transaction = await _context.Database.BeginTransactionAsync();
			try
			{
				var variantIds = Cart.Select(c => c.VariantId).ToList();
				var variants = await _context.Variants
					.Where(v => variantIds.Contains(v.Id) && v.IsActive)
					.Include(v => v.Product)
					.Include(v => v.Size)
					.ToListAsync();

				if (variants.Count != Cart.Count)
					return new JsonResult(new { error = "Một số sản phẩm không tồn tại hoặc không hoạt động" });

				decimal subTotal = 0;
				var orderItems = new List<OrderItem>();
				foreach (var cartItem in Cart)
				{
					var variant = variants.First(v => v.Id == cartItem.VariantId);
					if (variant.Stock < cartItem.Quantity)
						return new JsonResult(new { error = $"Sản phẩm {variant.Product.Name} không đủ hàng tồn kho" });

					decimal itemTotal = variant.Price * cartItem.Quantity;
					subTotal += itemTotal;

					orderItems.Add(new OrderItem
					{
						ProductVariantId = variant.Id,
						Name = variant.Product.Name,
						Size = variant.Size.Name,
						Price = variant.Price,
						Quantity = cartItem.Quantity,
						Total = itemTotal
					});

					variant.Stock -= cartItem.Quantity;
				}

				string userId = null;
				if (User.Identity?.IsAuthenticated == true)
				{
					var user = await _context.Users
						.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name || u.Email == User.Identity.Name);
					if (user != null)
					{
						userId = user.Id;
					}
					else
					{
						Console.WriteLine($"Không tìm thấy người dùng với UserName hoặc Email: {User.Identity.Name}");
					}
				}
				else
				{
					// Create user if doesn't exist
					await CreateUserIfNotExists(ShippingEmail, ShippingFullName, ShippingPhoneNumber);
					var user = await _userManager.FindByEmailAsync(ShippingEmail);
					if (user != null)
					{
						userId = user.Id;
					}
				}

				decimal shippingFee = ShippingFee;
				decimal discount = Discount;

				var order = new Order
				{
					UserId = userId,
					OrderNumber = DateTime.Now.ToString("yyyyMMddHHmmss") + new Random().Next(1000, 9999),
					Status = "Pending",
					PaymentStatus = "Pending",
					PaymentMethod = PaymentMethod,
					ShippingFullName = ShippingFullName,
					ShippingPhoneNumber = ShippingPhoneNumber,
					ShippingAddress = ShippingAddress,
					ShippingCity = ShippingCity,
					ShippingDistrict = ShippingDistrict,
					ShippingWard = ShippingWard,
					SubTotal = subTotal,
					ShippingFee = shippingFee,
					Discount = discount,
					Total = subTotal + shippingFee - discount,
					Notes = Note,
					ShippingEmail = ShippingEmail,
					Items = orderItems
				};

				_context.Orders.Add(order);
				await _context.SaveChangesAsync();

				// Commit transaction
				await transaction.CommitAsync();

				_ = Task.Run(async () =>
				{
					try
					{
						await _emailSender.SendEmailAsync(
					"daobliss.official@gmail.com",
					"Daobliss có đơn đặt hàng mới", "Có đơn đặt hàng mới, hãy vào website để liên hệ xác nhận đơn hàng với khách.");

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
				return new JsonResult(new { success = true, orderNumber = order.OrderNumber });
			}
			catch (Exception ex)
			{
				await transaction.RollbackAsync();
				return new JsonResult(new
				{
					error = "Tạo đơn hàng thất bại: " + ex.Message
				});
			}
		}
		private async Task CreateUserIfNotExists(string email, string firstName, string phoneNumber)
		{
			var existingUser = await _userManager.FindByEmailAsync(email);
			if (existingUser == null)
			{
				var user = CreateUser();
				user.FirstName = firstName;
				user.PhoneNumber = phoneNumber;

				await _userStore.SetUserNameAsync(user, email, CancellationToken.None);
				await _emailStore.SetEmailAsync(user, email, CancellationToken.None);

				// Generate a random password
				var password = Guid.NewGuid().ToString("n").Substring(0, 12);
				var result = await _userManager.CreateAsync(user, password);

			}
		}
		private ApplicationUser CreateUser()
		{
			try
			{
				return Activator.CreateInstance<ApplicationUser>();
			}
			catch
			{
				throw new InvalidOperationException($"Can't create an instance of '{nameof(ApplicationUser)}'. " +
					$"Ensure that '{nameof(ApplicationUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
					$"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
			}
		}

		private IUserEmailStore<ApplicationUser> GetEmailStore()
		{
			if (!_userManager.SupportsUserEmail)
			{
				throw new NotSupportedException("The default UI requires a user store with email support.");
			}
			return (IUserEmailStore<ApplicationUser>)_userStore;
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
