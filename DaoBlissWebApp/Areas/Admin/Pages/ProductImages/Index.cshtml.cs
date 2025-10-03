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

namespace DaoBlissWebApp.Areas.Admin.Pages.ProductImages
{
	[Authorize(Roles = "Admin")]
	public class IndexModel : PageModel
    {
        private readonly DaoBlissWebApp.Data.ApplicationDbContext _context;

        public IndexModel(DaoBlissWebApp.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<ProductImage> ProductImage { get;set; } = default!;

        public async Task OnGetAsync()
        {
            ProductImage = await _context.ProductImages
                .Include(p => p.Product).ToListAsync();
        }
    }
}
