using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Fall2025_Project3_jppierre.Data;
using ActorMovieModel = Fall2025_Project3_jppierre.Models.ActorMovie;

namespace Fall2025_Project3_jppierre.Pages.ActorMovies
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
            ViewData["ActorId"] = new SelectList(_context.Actor, "Id", "Name");
            ViewData["MovieId"] = new SelectList(_context.Movie, "Id", "Title");
            return Page();
        }

        [BindProperty]
        public ActorMovieModel ActorMovie { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                ViewData["ActorId"] = new SelectList(_context.Actor, "Id", "Name", ActorMovie.ActorId);
                ViewData["MovieId"] = new SelectList(_context.Movie, "Id", "Title", ActorMovie.MovieId);
                return Page();
            }

            _context.ActorMovie.Add(ActorMovie);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
