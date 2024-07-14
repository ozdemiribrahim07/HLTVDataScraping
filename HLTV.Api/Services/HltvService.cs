using HLTV.Api.Models;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OpenQA.Selenium.Chrome;
using Org.BouncyCastle.Utilities.IO;
using System.Net;
using System.Xml.Serialization;

namespace HLTV.Api.Services
{
    public class HltvService
    {
        public async Task<List<Ranking>> ScrapeRanking(string url)
        {

            var options = new ChromeOptions();

            using (var driver = new ChromeDriver(options))
            {
                driver.Navigate().GoToUrl("https://www.hltv.org/ranking/teams/2024/july/8");

                System.Threading.Thread.Sleep(5000); 

                var html = driver.PageSource;

                var htmlDoc = new HtmlAgilityPack.HtmlDocument();
                htmlDoc.LoadHtml(html);

                var nodes = htmlDoc.DocumentNode.SelectNodes("//div[contains(@class, 'ranked-team')]");

                if (nodes == null)
                {
                    throw new Exception("Xpath Bulunamadı.");
                }

                var teamRankings = new List<Ranking>();

                foreach (var node in nodes)
                {
                    var rankNode = node.SelectSingleNode(".//div[@class='ranking-header']/span[@class='position']");
                    var teamNameNode = node.SelectSingleNode(".//div[@class='teamLine sectionTeamPlayers ']/span[@class='name']");
                    var pointsNode = node.SelectSingleNode(".//div[@class='teamLine sectionTeamPlayers ']/span[@class='points']");
                    var logoNode = node.SelectSingleNode("//span[@class='team-logo']/img[@class='night-only']/@src");

                    if (rankNode != null && teamNameNode != null && pointsNode != null)
                    {
                        var ranking = new Ranking
                        {
                            Rank = int.Parse(rankNode.InnerText.Trim('#')),
                            TeamName = teamNameNode.InnerText.Trim(),
                            Points = pointsNode.InnerText.Trim(),
                            TeamLogo = logoNode.GetAttributeValue("src", string.Empty)
                        };

                        teamRankings.Add(ranking);
                    }
                }

                return teamRankings;
            }


        }

    }
    
}
