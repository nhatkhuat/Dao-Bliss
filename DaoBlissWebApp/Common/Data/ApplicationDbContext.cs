
using DaoBlissWebApp.Interfaces;
using DaoBlissWebApp.Common.Entities;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace DaoBlissWebApp.Data
{
	public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{
		}
		public DbSet<CustomerRank> CustomerRanks { get; set; }
		public DbSet<Category> Categories { get; set; }
		public DbSet<Size> Sizes { get; set; }
		public DbSet<Product> Products { get; set; }
		public DbSet<ProductVariant> Variants { get; set; }
		public DbSet<ProductImage> ProductImages { get; set; }
		public DbSet<UserAddress> UserAddresses { get; set; }
		public DbSet<Order> Orders { get; set; }
		public DbSet<OrderItem> OrderItems { get; set; }
		public DbSet<ProductReview> ProductReviews { get; set; }
		public DbSet<Article> Articles { get; set; }
		public DbSet<Coupon> Coupons { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder builder)
		{
			base.OnConfiguring(builder);
		}


		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			// Performance indexes
			modelBuilder.Entity<Product>()
				.HasIndex(p => new { p.CategoryId, p.IsActive });

			modelBuilder.Entity<Order>()
				.HasIndex(o => new { o.UserId, o.Status });

			// Delete behaviors (only where needed)
			modelBuilder.Entity<Order>()
				.HasOne(o => o.User)
				.WithMany(u => u.Orders)
				.HasForeignKey(o => o.UserId)
				.OnDelete(DeleteBehavior.SetNull);

			modelBuilder.Entity<OrderItem>()
				.HasOne(oi => oi.ProductVariant)
				.WithMany(pv => pv.OrderItems)
				.HasForeignKey(oi => oi.ProductVariantId)
				.OnDelete(DeleteBehavior.Restrict);

			modelBuilder.Entity<ProductReview>()
				.HasOne(pr => pr.User)
				.WithMany(u => u.Reviews)
				.HasForeignKey(pr => pr.UserId)
				.OnDelete(DeleteBehavior.Restrict);

			foreach (var entityType in modelBuilder.Model.GetEntityTypes())
			{
				var tableName = entityType.GetTableName();
				if (tableName.StartsWith("AspNet"))
				{
					entityType.SetTableName(tableName.Substring(6));
				}
			}
		}

		public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
		{
			// Auto update UpdatedAt timestamp
			var entries = ChangeTracker
					.Entries()
					.Where(e => e.Entity is Product &&
				   (e.State == EntityState.Added || e.State == EntityState.Modified));

			foreach (var entityEntry in entries)
			{
				if (entityEntry.State == EntityState.Added)
				{
					((Product)entityEntry.Entity).CreatedAt = DateTime.Now;
					((Product)entityEntry.Entity).UpdatedAt = DateTime.Now;
				}
				else if (entityEntry.State == EntityState.Modified)
				{
					entityEntry.Property("CreatedAt").IsModified = false; // không sửa CreatedAt
					((Product)entityEntry.Entity).UpdatedAt = DateTime.Now;
				}
			}

			return base.SaveChangesAsync(cancellationToken);
		}
	}
}
