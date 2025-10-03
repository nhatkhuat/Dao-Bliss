using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DaoBlissWebApp.Common.Entities
{
	public class ProductReview
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public int ProductId { get; set; }

		[Required]
		public string UserId { get; set; }

		[Required]
		[Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
		public int Rating { get; set; }

		[Column(TypeName = "nvarchar(max)")]
		public string Comment { get; set; }
		public bool IsApproved { get; set; } = false;
		public DateTime CreatedAt { get; set; } = DateTime.Now;

		[ForeignKey("ProductId")]
		public Product? Product { get; set; }

		[ForeignKey("UserId")]
		public ApplicationUser? User { get; set; }
	}
}
