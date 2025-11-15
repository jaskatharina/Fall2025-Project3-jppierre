using Fall2025_Project3_jppierre.Data;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Fall2025_Project3_jppierre.Models;

namespace Fall2025_Project3_jppierre.ViewModels
{
    public class MovieDetailsViewModel
    {
        public Movie Movie { get; set; } = default!;
        // List of actor names for the details page
        public List<string> ActorNames { get; set; } = new();

        // AI results: pair of review and sentiment
        public List<(string Review, string Sentiment)> ReviewsWithSentiment { get; set; } = new();

        // Overall sentiment computed from the reviews (e.g., "Positive", "Neutral", "Negative")
        public string OverallSentiment { get; set; } = string.Empty;
    }
}   