using System.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fall2025_Project3_jppierre.Models
{
    public class Actor
    {
        [Key]
        public int Id { get; set; }

        public string? Name { get; set; }
        public string? Gender { get; set; }
        public int? Age { get; set; }
        public string? IMDBLink { get; set; }
        public byte[]? Photo { get; set; }

        // navigation to join table (many-to-many via ActorMovie)
        public ICollection<ActorMovie>? ActorMovies { get; set; }

        // Not persisted by EF — used at runtime to populate the details page
        [NotMapped]
        public DataTable? AITweets { get; set; }

        public Actor()
        {
            AITweets = new DataTable();
            AITweets.Columns.Add("Tweets", typeof(string));
            AITweets.Columns.Add("Sentiment", typeof(string));
        }
    }
}
