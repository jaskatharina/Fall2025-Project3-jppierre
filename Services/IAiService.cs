using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fall2025_Project3_jppierre.Services
{
    public interface IAiService
    {
        // Single API call to generate `count` reviews (movie)
        Task<IList<string>> GenerateReviewsAsync(string movieTitle, int count);

        // Single API call to generate `count` tweets (actor)
        Task<IList<string>> GenerateTweetsAsync(string actorName, int count);

        // Analyze sentiment for a list of texts with VaderSharp2 and return labels ("Positive"/"Neutral"/"Negative")
        Task<IList<string>> AnalyzeSentimentAsync(IList<string> texts);
    }
}