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
    public class DetailsModel : PageModel
    {
        private readonly DaoBlissWebApp.Data.ApplicationDbContext _context;

        public DetailsModel(DaoBlissWebApp.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public ProductReview ProductReview { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productreview = await _context.ProductReviews.FirstOrDefaultAsync(m => m.Id == id);
            if (productreview == null)
            {
                return NotFound();
            }
            else
            {
                ProductReview = productreview;
            }
            return Page();
        }
    }
}
