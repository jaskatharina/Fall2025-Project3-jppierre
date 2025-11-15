using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Fall2025_Project3_jppierre.Data;
using ActorMovieModel = Fall2025_Project3_jppierre.Models.ActorMovie;

namespace Fall2025_Project3_jppierre.Pages.ActorMovies
{
    public class DeleteModel : PageModel
    {
        private readonly Fall2025_Project3_jppierre.Data.ApplicationDbContext _context;

        public DeleteModel(Fall2025_Project3_jppierre.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public ActorMovieModel ActorMovie { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var actormovie = await _context.ActorMovie.FirstOrDefaultAsync(m => m.Id == id);

            if (actormovie is not null)
            {
                ActorMovie = actormovie;

                return Page();
            }

            return NotFound();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var actormovie = await _context.ActorMovie.FindAsync(id);
            if (actormovie != null)
            {
                ActorMovie = actormovie;
                _context.ActorMovie.Remove(ActorMovie);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
