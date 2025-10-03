using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DaoBlissWebApp.Common.Entities
{
	public class Order
	{
		[Key]
		public int Id { get; set; }

		public string? UserId { get; set; }

		[Required, MaxLength(50)]
		public string OrderNumber { get; set; }

		[Required, MaxLength(50)]
		public string Status { get; set; } = "Pending";

		[Required, MaxLength(50)]
		public string PaymentStatus { get; set; } = "Pending";

		[MaxLength(50)]
		public string PaymentMethod { get; set; }

		// Shipping Information
		[Required, MaxLength(200)]
		public string ShippingFullName { get; set; }

		[Required, MaxLength(20)]
		public string ShippingPhoneNumber { get; set; }

		[Required, MaxLength(500)]
		public string ShippingAddress { get; set; }

		[Required, MaxLength(100)]
		public string ShippingCity { get; set; }

		[Required, MaxLength(100)]
		public string ShippingDistrict { get; set; }

		[Required, MaxLength(100)]
		public string ShippingWard { get; set; }

		[Required, MaxLength(200)]
		public string ShippingEmail { get; set; }

		// Financial Information
		[Required]
		[Column(TypeName = "decimal(18,2)")]
		public decimal SubTotal { get; set; }

		[Column(TypeName = "decimal(18,2)")]
		public decimal ShippingFee { get; set; } = 0;

		[Column(TypeName = "decimal(18,2)")]
		public decimal Discount { get; set; } = 0;

		[Required]
		[Column(TypeName = "decimal(18,2)")]
		public decimal Total { get; set; }

		[MaxLength(1000)]
		public string? Notes { get; set; }

		public DateTime CreatedAt { get; set; } = DateTime.Now;
		public DateTime UpdatedAt { get; set; } = DateTime.Now;

		// Navigation Properties
		[ForeignKey("UserId")]
		public ApplicationUser? User { get; set; }
		public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
	}
}
