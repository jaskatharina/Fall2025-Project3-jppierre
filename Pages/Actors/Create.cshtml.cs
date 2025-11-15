using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Fall2025_Project3_jppierre.Data;
using Fall2025_Project3_jppierre.Models;

namespace Fall2025_Project3_jppierre.Pages.Actors
{
    public class CreateModel : PageModel
    {
        private readonly Fall2025_Project3_jppierre.Data.ApplicationDbContext _context;

        public CreateModel(Fall2025_Project3_jppierre.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Actor Actor { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(IFormFile? PhotoFile)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Handle photo upload
            if (PhotoFile != null && PhotoFile.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await PhotoFile.CopyToAsync(memoryStream);
                    Actor.Photo = memoryStream.ToArray();
                }
            }

            _context.Actor.Add(Actor);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
