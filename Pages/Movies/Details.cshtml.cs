using Fall2025_Project3_jppierre.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fall2025_Project3_jppierre.Models;
using Fall2025_Project3_jppierre.Services;

namespace Fall2025_Project3_jppierre.Pages.Movies
{
    public class DetailsModel : PageModel
    {
        private readonly Fall2025_Project3_jppierre.Data.ApplicationDbContext _context;
        private readonly IAiService _aiService;

        public DetailsModel(Fall2025_Project3_jppierre.Data.ApplicationDbContext context, IAiService aiService)
        {
            _context = context;
            _aiService = aiService;
        }

        public Movie Movie { get; set; } = default!;
        public IList<Actor> ActorsInMovie { get; set; } = new List<Actor>();
        public IList<string> Reviews { get; set; } = new List<string>();
        public IList<string> Sentiments { get; set; } = new List<string>();
        public double AverageSentiment { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie
                .Include(m => m.ActorMovies)
                .ThenInclude(am => am.Actor)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (movie is not null)
            {
                Movie = movie;

                // Get actors in this movie
                ActorsInMovie = movie.ActorMovies?.Select(am => am.Actor).ToList() ?? new List<Actor>();

                // Generate 10 AI reviews
                Reviews = await _aiService.GenerateReviewsAsync(movie.Title ?? "this movie", 3);

                // Analyze sentiment for each review
                Sentiments = await _aiService.AnalyzeSentimentAsync(Reviews);

                // Calculate average sentiment (Positive=1, Neutral=0, Negative=-1)
                if (Sentiments.Any())
                {
                    var sentimentValues = Sentiments.Select(s => s switch
                    {
                        "Positive" => 1.0,
                        "Negative" => -1.0,
                        _ => 0.0
                    });
                    AverageSentiment = sentimentValues.Average();
                }

                return Page();
            }

            return NotFound();
        }
    }
}
