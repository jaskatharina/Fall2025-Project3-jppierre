using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Fall2025_Project3_jppierre.Data;
using ActorMovieModel = Fall2025_Project3_jppierre.Models.ActorMovie;

namespace Fall2025_Project3_jppierre.Pages.ActorMovies
{
    public class EditModel : PageModel
    {
        private readonly Fall2025_Project3_jppierre.Data.ApplicationDbContext _context;

        public EditModel(Fall2025_Project3_jppierre.Data.ApplicationDbContext context)
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

            var actormovie =  await _context.ActorMovie.FirstOrDefaultAsync(m => m.Id == id);
            if (actormovie == null)
            {
                return NotFound();
            }
            ActorMovie = actormovie;
            ViewData["ActorId"] = new SelectList(_context.Actor, "Id", "Name", ActorMovie.ActorId);
            ViewData["MovieId"] = new SelectList(_context.Movie, "Id", "Title", ActorMovie.MovieId);
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                ViewData["ActorId"] = new SelectList(_context.Actor, "Id", "Name", ActorMovie.ActorId);
                ViewData["MovieId"] = new SelectList(_context.Movie, "Id", "Title", ActorMovie.MovieId);
                return Page();
            }

            _context.Attach(ActorMovie).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ActorMovieExists(ActorMovie.Id))
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

        private bool ActorMovieExists(int id)
        {
            return _context.ActorMovie.Any(e => e.Id == id);
        }
    }
}
