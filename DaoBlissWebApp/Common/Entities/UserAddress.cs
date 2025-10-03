using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DaoBlissWebApp.Common.Entities
{
	public class UserAddress
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public string UserId { get; set; }

		[Required, MaxLength(200)]
		public string FullName { get; set; }

		[Required, MaxLength(20)]
		public string PhoneNumber { get; set; }

		[Required, MaxLength(500)]
		public string Address{ get; set; }

		[Required, MaxLength(100)]
		public string City { get; set; }

		[Required, MaxLength(100)]
		public string District { get; set; }

		[Required, MaxLength(100)]
		public string Ward { get; set; }

		[MaxLength(10)]
		public string PostalCode { get; set; }

		public DateTime CreatedAt { get; set; } = DateTime.Now;

		// Navigation Properties
		[ForeignKey("UserId")]
		public ApplicationUser User { get; set; }
	}
}
