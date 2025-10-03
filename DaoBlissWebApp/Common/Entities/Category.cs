using System.ComponentModel.DataAnnotations;

namespace DaoBlissWebApp.Common.Entities
{
	public class Category
	{
		[Key]
		public int CategoryId { get; set; }

		[Required, StringLength(255)]
		public string CategoryName { get; set; }

		[MaxLength(255)]
		public string Description { get; set; }
		public bool IsActive { get; set; } = true;

		public virtual ICollection<Product>? Products { get; set; } = new List<Product>();
	}
}
