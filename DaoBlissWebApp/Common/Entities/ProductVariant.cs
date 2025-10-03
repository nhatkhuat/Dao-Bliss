using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DaoBlissWebApp.Common.Entities
{
	public class ProductVariant
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public int ProductId { get; set; }

		[Required]
		public int SizeId { get; set; }

		[Required]
		[Column(TypeName = "decimal(18,2)")]
		public decimal Price { get; set; }

		[Column(TypeName = "decimal(18,2)")]
		public decimal? CompareAtPrice { get; set; }

		public int Stock { get; set; } = 0;

		[MaxLength(50)]
		public string Sku { get; set; }

		[Column(TypeName = "decimal(10,2)")]
		public decimal? Weight { get; set; }

		public bool IsActive { get; set; } = true;

		// Navigation Properties
		[ForeignKey("ProductId")]
		public Product? Product { get; set; }

		[ForeignKey("SizeId")]
		public Size? Size { get; set; }

		public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
	}
}
