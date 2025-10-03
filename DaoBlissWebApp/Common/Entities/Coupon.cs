using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DaoBlissWebApp.Common.Entities
{
	public class Coupon
	{
		[Key]
		public int Id { get; set; }

		[Required, MaxLength(50)]
		public string Code { get; set; }

		[Required, MaxLength(200)]
		public string Name { get; set; }

		[MaxLength(500)]
		public string Description { get; set; }

		[Required, MaxLength(20)]
		public string DiscountType { get; set; } // Percentage, FixedAmount

		[Required]
		[Column(TypeName = "decimal(18,2)")]
		public decimal DiscountValue { get; set; }

		[Column(TypeName = "decimal(18,2)")]
		public decimal? MinOrderAmount { get; set; }

		[Column(TypeName = "decimal(18,2)")]
		public decimal? MaxDiscountAmount { get; set; }

		public int? UsageLimit { get; set; }
		public int UsedCount { get; set; } = 0;

		public int? MinRankId { get; set; } // Rank requirement

		[Required]
		public DateTime StartDate { get; set; }

		[Required]
		public DateTime EndDate { get; set; }

		public bool IsActive { get; set; } = true;
		public DateTime CreatedAt { get; set; } = DateTime.Now;

		[ForeignKey("MinRankId")]
		public CustomerRank? MinRank { get; set; }
	}
}
