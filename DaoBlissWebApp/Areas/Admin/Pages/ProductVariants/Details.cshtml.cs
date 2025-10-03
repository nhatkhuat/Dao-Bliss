using DaoBlissWebApp.Common.Entities;
using DaoBlissWebApp.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DaoBlissWebApp.Areas.Admin.Pages.ProductVariants
{
	[Authorize(Roles = "Admin")]
	public class DetailsModel : PageModel
    {
        private readonly DaoBlissWebApp.Data.ApplicationDbContext _context;

        public DetailsModel(DaoBlissWebApp.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public ProductVariant ProductVariant { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productvariant = await _context.Variants.FirstOrDefaultAsync(m => m.Id == id);
            if (productvariant == null)
            {
                return NotFound();
            }
            else
            {
                ProductVariant = productvariant;
            }
            return Page();
        }
    }
}
