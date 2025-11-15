using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Fall2025_Project3_jppierre.Data;
using Fall2025_Project3_jppierre.Models;
using Fall2025_Project3_jppierre.Services;

namespace Fall2025_Project3_jppierre.Pages.Actors
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

        public Actor Actor { get; set; } = default!;
        public IList<Movie> MoviesWithActor { get; set; } = new List<Movie>();
        public IList<string> Tweets { get; set; } = new List<string>();
        public IList<string> Sentiments { get; set; } = new List<string>();
        public double AverageSentiment { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var actor = await _context.Actor
                .Include(a => a.ActorMovies)
                .ThenInclude(am => am.Movie)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (actor is not null)
            {
                Actor = actor;

                // Get movies with this actor
                MoviesWithActor = actor.ActorMovies?.Select(am => am.Movie).ToList() ?? new List<Movie>();

                // Generate 5 AI tweets
                Tweets = await _aiService.GenerateTweetsAsync(actor.Name ?? "this actor", 5);

                // Analyze sentiment for each tweet
                Sentiments = await _aiService.AnalyzeSentimentAsync(Tweets);

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
