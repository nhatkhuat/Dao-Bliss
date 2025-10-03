using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DaoBlissWebApp.Common.Entities
{
	public class Article
	{
		[Key]
		public int Id { get; set; }

		[Required, MaxLength(300)]
		public string Title { get; set; }
		public string? Slug { get; set; }

		[MaxLength(500)]
		public string Summary { get; set; }

		[MaxLength(500)]
		public string ImageUrl { get; set; }

		[Required]
		[Column(TypeName = "nvarchar(max)")]
		public string Content { get; set; }

		[MaxLength(50)]
		public string Status { get; set; } = "Draft";
		public DateTime CreatedAt { get; set; } = DateTime.Now;
		public DateTime UpdatedAt { get; set; } = DateTime.Now;
	}
}
