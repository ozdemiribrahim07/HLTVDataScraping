namespace HLTV.Api.Models
{
    public class Match
    {
        public string Team1 { get; set; }
        public string Team2 { get; set; }
        public DateTime MatchTime { get; set; }
        public string Event { get; set; }

        public string Logo1 { get; set; }
        public string Logo2 { get; set; }

        public string MatchFormat { get; set; }
    }
}
