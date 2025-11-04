using System.Data;

namespace Fall2025_Project3_jppierre.Models
{
    public class Actor
    {
        public string? Name { get; set; }
        public string? Gender { get; set; }
        public int? Age { get; set; }
        public string? IMDBLink { get; set; }
        public byte[]? Photo { get; set; }   

        public string[]? Movies { get; set; }

        public DataTable? AITweets { get; set; }

        public Actor()
        {
            AITweets = new DataTable();
            AITweets.Columns.Add("Tweets", typeof(string));
            AITweets.Columns.Add("Sentiment", typeof(string));
        }

    }
}
