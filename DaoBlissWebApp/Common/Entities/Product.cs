using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DaoBlissWebApp.Common.Entities
{
	public class Product
	{
		[Key]
		public int Id { get; set; }

		[Required, MaxLength(200)]
		public string Name { get; set; }

		[Column(TypeName = "nvarchar(max)")]
		public string Description { get; set; }

		public string? Slug { get; set; }

		[Required]
		public int CategoryId { get; set; }

		public bool IsActive { get; set; } = true;
		public DateTime CreatedAt { get; set; } = DateTime.Now;
		public DateTime UpdatedAt { get; set; } = DateTime.Now;

		// Navigation Properties
		[ForeignKey("CategoryId")]
		public Category? Category { get; set; }
		public ICollection<ProductVariant> Variants { get; set; } = new List<ProductVariant>();
		public ICollection<ProductImage> Images { get; set; } = new List<ProductImage>();
		public ICollection<ProductReview> Reviews { get; set; } = new List<ProductReview>();
	}
}
