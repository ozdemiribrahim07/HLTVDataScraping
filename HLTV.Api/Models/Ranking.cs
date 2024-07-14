using System.Xml.Serialization;

namespace HLTV.Api.Models
{
    public class Ranking
    {
        public int Id { get; set; }
        public int Rank { get; set; }
        public string TeamName { get; set; }
        public string Points { get; set; }
        public string TeamLogo { get; set; }
    }
}