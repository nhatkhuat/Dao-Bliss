using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DaoBlissWebApp.Common.Entities
{
	public class ApplicationUser : IdentityUser
	{

		[StringLength(50)]
		public string? FirstName { get; set; }

		[StringLength(50)]
		public string? LastName { get; set; }

		public bool? Gender { get; set; }

		public DateOnly? DateOfBirth { get; set; }

		[Required]
		public bool IsActive { get; set; } = true;

		public int? RankId { get; set; }

		[Column(TypeName = "decimal(18,2)")]
		public decimal TotalSpent { get; set; } = 0;
		public DateTime CreatedAt { get; set; } = DateTime.Now;
		
		[ForeignKey("RankId")]
		public virtual CustomerRank? Rank { get; set; }
		public ICollection<UserAddress> Addresses { get; set; } = new List<UserAddress>();
		public ICollection<Order> Orders { get; set; } = new List<Order>();
		public ICollection<ProductReview> Reviews { get; set; } = new List<ProductReview>();
		public ICollection<Article> Articles { get; set; } = new List<Article>();
	}
}
