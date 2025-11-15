using System.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;


namespace Fall2025_Project3_jppierre.Models
{
    public class Movie
    {
        [Key]
        public int Id { get; set; }

        public string? Title { get; set; }
        public string? IMDBLink { get; set; }
        public string? Genre { get; set; }
        public int? ReleaseYear { get; set; }
        public byte[]? Poster { get; set; }

        // navigation to join table (many-to-many via ActorMovie)
        public ICollection<ActorMovie>? ActorMovies { get; set; }

        // Not persisted by EF — used at runtime to populate the details page
        [NotMapped]
        public DataTable? AIReviews { get; set; }

        // Not persisted: computed from AI reviews at runtime
        [NotMapped]
        public string? overallSentiment { get; set; }

        public Movie()
        {
            AIReviews = new DataTable();
            AIReviews.Columns.Add("Review", typeof(string));
            AIReviews.Columns.Add("Sentiment", typeof(string));
        }
    }
}
