using DaoBlissWebApp.Interfaces.Repositories;
using DaoBlissWebApp.Interfaces.Services;

namespace DaoBlissWebApp.Services
{
	public class CouponService : ICouponService
	{
		private readonly ICouponRepository _couponRepository;

		public CouponService(ICouponRepository couponRepository)
		{
			_couponRepository = couponRepository;
		}

		public async Task<decimal> ApplyCouponAsync(string code, decimal subTotal)
		{
			var coupon = await _couponRepository.GetCouponByCodeAsync(code);
			if (coupon == null || !coupon.IsActive || DateTime.Now < coupon.StartDate || DateTime.Now > coupon.EndDate)
			{
				return 0;
			}

			decimal discount = 0;
			if (coupon.DiscountType == "Percentage")
			{
				discount = subTotal * (coupon.DiscountValue / 100);
				if (coupon.MaxDiscountAmount.HasValue && discount > coupon.MaxDiscountAmount.Value)
				{
					discount = coupon.MaxDiscountAmount.Value;
				}
			}
			else if (coupon.DiscountType == "FixedAmount")
			{
				discount = coupon.DiscountValue;
			}

			return discount;
		}
	}
}
