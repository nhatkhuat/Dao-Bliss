using System.ComponentModel.DataAnnotations;

namespace DaoBlissWebApp.Common.Enums
{
	public enum PaymentStatus
	{
		[Display(Name = "Chờ xử lý")]
		Pending,
		[Display(Name = "Đã thanh toán")]
		Paid,
		[Display(Name = "Đã hoàn tiền")]
		Refunded
	}
}
