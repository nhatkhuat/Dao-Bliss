using DaoBlissWebApp.Common.Entities;
using DaoBlissWebApp.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace DaoBlissWebApp.Areas.Admin.Pages.Products
{
	[Authorize(Roles = "Admin")]
	public class EditModel : PageModel
	{
		private readonly DaoBlissWebApp.Data.ApplicationDbContext _context;

		public EditModel(DaoBlissWebApp.Data.ApplicationDbContext context)
		{
			_context = context;
		}

		[BindProperty]
		public Product Product { get; set; } = default!;

		[BindProperty]
		public ProductDescription DescriptionModel { get; set; }

		public async Task<IActionResult> OnGetAsync(int? id)
		{
			Product = await _context.Products.FirstOrDefaultAsync(m => m.Id == id);

			if (Product == null)
				return NotFound();

			// Parse JSON sang object
			if (!string.IsNullOrEmpty(Product.Description))
			{
				try
				{
					DescriptionModel = JsonSerializer.Deserialize<ProductDescription>(Product.Description);
				}
				catch
				{
					DescriptionModel = new ProductDescription();
				}
			}
			else
			{
				DescriptionModel = new ProductDescription();
			}

			ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName");
			return Page();
		}

		public async Task<IActionResult> OnPostAsync()
		{
			ModelState.Remove("Product.Description");

			if (!ModelState.IsValid)
			{
				return Page();
			}
			// Serialize lại JSON từ form
			Product.Description = JsonSerializer.Serialize(DescriptionModel);

			_context.Attach(Product).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!ProductExists(Product.Id))
				{
					return NotFound();
				}
				else
				{
					throw;
				}
			}

			return RedirectToPage("./Index");
		}

		private bool ProductExists(int id)
		{
			return _context.Products.Any(e => e.Id == id);
		}
		
	}
	
}
