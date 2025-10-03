using DaoBlissWebApp.Common.Entities;
using DaoBlissWebApp.Data;
using DaoBlissWebApp.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DaoBlissWebApp.Repositories
{
	public class CouponRepository : ICouponRepository
	{
		private readonly ApplicationDbContext _context;

		public CouponRepository(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<Coupon?> GetCouponByIdAsync(int id)
		{
			return await _context.Coupons.FindAsync(id);
		}

		public async Task<Coupon?> GetCouponByCodeAsync(string code)
		{
			return await _context.Coupons.FirstOrDefaultAsync(c => c.Code == code && c.IsActive);
		}

		public async Task<List<Coupon>> GetAllActiveCouponsAsync()
		{
			return await _context.Coupons.Where(c => c.IsActive).ToListAsync();
		}

		public async Task AddCouponAsync(Coupon coupon)
		{
			_context.Coupons.Add(coupon);
			await _context.SaveChangesAsync();
		}

		public async Task UpdateCouponAsync(Coupon coupon)
		{
			_context.Coupons.Update(coupon);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteCouponAsync(int id)
		{
			var coupon = await _context.Coupons.FindAsync(id);
			if (coupon != null)
			{
				_context.Coupons.Remove(coupon);
				await _context.SaveChangesAsync();
			}
		}
	}
}
