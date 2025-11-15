using System.Collections.Generic;
using Fall2025_Project3_jppierre.Models;

namespace Fall2025_Project3_jppierre.ViewModels
{
    public class ActorDetailsViewModel
    {
        public Actor Actor { get; set; } = default!;
        // List of movie titles for the details page
        public List<string> MovieTitles { get; set; } = new();

        // AI results: pair of tweet and sentiment
        public List<(string Tweet, string Sentiment)> TweetsWithSentiment { get; set; } = new();

        // Overall sentiment computed from the tweets
        public string OverallSentiment { get; set; } = string.Empty;
    }
}