using System.Data;
using System.ComponentModel.DataAnnotations.Schema;


namespace Fall2025_Project3_jppierre.Models
{
    public class Movie
    {
        public string? Title { get; set; }
        public string? IMDBLink { get; set; }
        public string? Genre { get; set; }
        public int? ReleaseYear { get; set; }
        public byte[]? Poster { get; set; }
        public string[]? Actors { get; set; }
        public DataTable? AIReviews { get; set; }
        public Movie()
        {
            AIReviews = new DataTable();
            AIReviews.Columns.Add("Review", typeof(string));
            AIReviews.Columns.Add("Sentiment", typeof(string));
        }
        public string? overallSentiment { get; set; }

    }
}
