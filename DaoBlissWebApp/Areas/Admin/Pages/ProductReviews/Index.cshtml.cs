using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DaoBlissWebApp.Common.Entities;
using DaoBlissWebApp.Data;

namespace DaoBlissWebApp.Areas.Admin.Pages.ProductReviews
{
    public class IndexModel : PageModel
    {
        private readonly DaoBlissWebApp.Data.ApplicationDbContext _context;

        public IndexModel(DaoBlissWebApp.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<ProductReview> ProductReview { get;set; } = default!;

        public async Task OnGetAsync()
        {
            ProductReview = await _context.ProductReviews
                .Include(p => p.Product)
                .Include(p => p.User).ToListAsync();
        }
    }
}
