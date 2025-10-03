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
	public class IndexModel : PageModel
    {
        private readonly DaoBlissWebApp.Data.ApplicationDbContext _context;

        public IndexModel(DaoBlissWebApp.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<ProductVariant> ProductVariant { get;set; } = default!;

        public async Task OnGetAsync()
        {
            ProductVariant = await _context.Variants
                .Include(p => p.Product)
                .Include(p => p.Size).ToListAsync();
        }
    }
}
