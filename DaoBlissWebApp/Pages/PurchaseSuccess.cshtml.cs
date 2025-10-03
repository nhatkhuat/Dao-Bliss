using DaoBlissWebApp.Common.Data.Migrations;
using DaoBlissWebApp.Common.Entities;
using DaoBlissWebApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DaoBlissWebApp.Pages
{
	public class PurchaseSuccessModel : PageModel
	{
		private readonly ApplicationDbContext _context;
		private readonly IConfiguration _configuration;

		public PurchaseSuccessModel(ApplicationDbContext context, IConfiguration configuration)
		{
			_context = context;
			_configuration = configuration;
		}

		public string QrDataUrl { get; set; }
		public async Task<IActionResult> OnGetAsync(string orderNumber)
		{
			Order order = await _context.Orders.FirstOrDefaultAsync(o => o.OrderNumber.Equals(orderNumber));
			if (order == null)
			{
				return Redirect("/NotFound");
			}
			string qrDataUrl = order.PaymentMethod.Equals("COD")
				? null
				: await GenerateVietQRCode(order.OrderNumber, order.Total);
			QrDataUrl = qrDataUrl;
			return Page();
		}
		private async Task<string> GenerateVietQRCode(string orderNumber, decimal total)
		{
			using var httpClient = new HttpClient();

			var clientId = _configuration["VietQR:ClientId"];
			var apiKey = _configuration["VietQR:ApiKey"];

			httpClient.DefaultRequestHeaders.Add("x-client-id", clientId);
			httpClient.DefaultRequestHeaders.Add("x-api-key", apiKey);

			var requestBody = new
			{
				accountNo = "0972968271",
				accountName = "DAOBLISS",
				acqId = 970422,
				amount = (int)total,
				addInfo = $"Thanh toán ðõn hàng {orderNumber}",
				format = "text",
				template = "compact"
			};

			var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
			var response = await httpClient.PostAsync("https://api.vietqr.io/v2/generate", content);

			if (response.IsSuccessStatusCode)
			{
				var jsonResponse = await response.Content.ReadAsStringAsync();
				var vietQrResponse = JsonSerializer.Deserialize<VietQrResponse>(jsonResponse);
				if (vietQrResponse.code.Equals("00"))
					return vietQrResponse.data.qrDataURL;
			}

			throw new Exception("Failed to generate VietQR code");
		}

		public class VietQrResponse
		{
			public string code { get; set; }
			public string desc { get; set; }
			public VietQrData data { get; set; }
		}

		public class VietQrData
		{

			public int acpId { get; set; }
			public string accountName { get; set; }
			public string qrCode { get; set; }
			public string qrDataURL { get; set; }
		}
	}
}
