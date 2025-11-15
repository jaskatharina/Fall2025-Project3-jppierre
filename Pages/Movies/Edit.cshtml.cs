using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Fall2025_Project3_jppierre.Data;
using Fall2025_Project3_jppierre.Models;


namespace Fall2025_Project3_jppierre.Pages.Movies
{
    public class EditModel : PageModel
    {
        private readonly Fall2025_Project3_jppierre.Data.ApplicationDbContext _context;

        public EditModel(Fall2025_Project3_jppierre.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Movie Movie { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie =  await _context.Movie.FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }
            Movie = movie;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(IFormFile? PosterFile)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Handle new poster upload if provided
            if (PosterFile != null && PosterFile.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await PosterFile.CopyToAsync(memoryStream);
                    Movie.Poster = memoryStream.ToArray();
                }
            }
            else
            {
                // Keep the existing poster if no new file uploaded
                var existingMovie = await _context.Movie.AsNoTracking().FirstOrDefaultAsync(m => m.Id == Movie.Id);
                if (existingMovie != null)
                {
                    Movie.Poster = existingMovie.Poster;
                }
            }

            _context.Attach(Movie).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieExists(Movie.Id))
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

        private bool MovieExists(int id)
        {
            return _context.Movie.Any(e => e.Id == id);
        }
    }
}
