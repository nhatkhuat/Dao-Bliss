using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DaoBlissWebApp.Common.Entities
{
	public class CustomerRank
	{
		[Key]
		public int Id { get; set; }

		[Required, MaxLength(50)]
		public string Name { get; set; }

		[MaxLength(200)]
		public string Description { get; set; }

		[Column(TypeName = "decimal(18,2)")]
		public decimal MinSpentAmount { get; set; }
		public int SortOrder { get; set; }
		public bool IsActive { get; set; } = true;
		public DateTime CreatedAt { get; set; } = DateTime.Now;

		// Navigation Properties
		public ICollection<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();
		public ICollection<Coupon> Coupons { get; set; } = new List<Coupon>();
	}
}
