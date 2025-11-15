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
    public class IndexModel : PageModel
    {
        private readonly Fall2025_Project3_jppierre.Data.ApplicationDbContext _context;

        public IndexModel(Fall2025_Project3_jppierre.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<ActorMovieModel> ActorMovies { get;set; } = default!;

        public async Task OnGetAsync()
        {
            ActorMovies = await _context.ActorMovie
                .Include(a => a.Actor)
                .Include(a => a.Movie).ToListAsync();
        }
    }
}
