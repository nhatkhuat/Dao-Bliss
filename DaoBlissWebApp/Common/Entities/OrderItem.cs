using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DaoBlissWebApp.Common.Entities
{
	public class OrderItem
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public int OrderId { get; set; }

		[Required]
		public int ProductVariantId { get; set; }

		[Required, MaxLength(200)]
		public string Name { get; set; }

		[Required, MaxLength(50)]
		public string Size { get; set; }

		[Required]
		[Column(TypeName = "decimal(18,2)")]
		public decimal Price { get; set; }

		[Required]
		public int Quantity { get; set; }

		[Required]
		[Column(TypeName = "decimal(18,2)")]
		public decimal Total { get; set; }

		// Navigation Properties
		[ForeignKey("OrderId")]
		public Order? Order { get; set; }

		[ForeignKey("ProductVariantId")]
		public ProductVariant? ProductVariant { get; set; }
	}
}
