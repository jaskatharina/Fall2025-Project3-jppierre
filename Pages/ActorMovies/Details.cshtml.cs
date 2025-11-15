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
    public class DetailsModel : PageModel
    {
        private readonly Fall2025_Project3_jppierre.Data.ApplicationDbContext _context;

        public DetailsModel(Fall2025_Project3_jppierre.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public ActorMovieModel ActorMovie { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var actormovie = await _context.ActorMovie
                .Include(am => am.Actor)
                .Include(am => am.Movie)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (actormovie is not null)
            {
                ActorMovie = actormovie;

                return Page();
            }

            return NotFound();
        }
    }
}
