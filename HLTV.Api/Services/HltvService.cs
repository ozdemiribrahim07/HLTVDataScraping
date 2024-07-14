using HLTV.Api.Models;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using Org.BouncyCastle.Utilities.IO;
using System.Net;
using System.Xml.Serialization;
using OpenQA.Selenium.Support.UI;

namespace HLTV.Api.Services
{
    public class HltvService
    {
        private readonly string _url = "https://www.hltv.org/matches";

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
                    throw new Exception("No nodes found. Please check the XPath expression and the HTML structure of the target page.");
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

        public List<Match> GetUpcomingMatches()
        {
            //Selenium işlemleri 
            var options = new ChromeOptions();


            using var driver = new ChromeDriver(options);
            driver.Navigate().GoToUrl(_url);

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
            wait.Until(drv => drv.FindElement(By.XPath("//div[contains(@class, 'upcomingMatches')]")));

            var pageSource = driver.PageSource;
            driver.Quit();

            // HtmlAgilityPack ile sayfa kaynağını yükle
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(pageSource);

            var matchNodes = htmlDoc.DocumentNode.SelectNodes("//div[contains(@class, 'upcomingMatch')]");

            var matches = new List<Match>();

            if (matchNodes != null)
            {
                foreach (var node in matchNodes)
                {
                    var matchTeamsNode = node.SelectSingleNode(".//div[@class='matchTeams text-ellipsis']");


                    if (matchTeamsNode is null)
                    {
                        continue;
                    }

                    
                    var team1Name = matchTeamsNode.SelectSingleNode(".//div[@class='matchTeam team1']//div[@class='matchTeamName text-ellipsis']")?.InnerText.Trim();

                    var team2Name = matchTeamsNode.SelectSingleNode(".//div[@class='matchTeam team2']//div[@class='matchTeamName text-ellipsis']")?.InnerText.Trim();


                    var team1Logo = matchTeamsNode.SelectSingleNode(".//div[@class='matchTeam team1']//img")?.GetAttributeValue("src", "");

                    var team2Logo = matchTeamsNode.SelectSingleNode(".//div[@class='matchTeam team2']//img")?.GetAttributeValue("src", "");

                    var timeString = node.SelectSingleNode(".//div[@class='matchTime']").GetAttributeValue("data-unix", "");

                    var matchFormat = node.SelectSingleNode(".//div[@class='matchMeta']")?.InnerText.Trim();

                    var eventString = node.SelectSingleNode(".//div[@class='matchEvent']//div[@class='matchEventName gtSmartphone-only']")?.InnerText.Trim();

                    matches.Add(new Match
                    {
                        Team1 = team1Name,
                        Logo1 = team1Logo,
                        Team2 = team2Name,
                        Logo2 = team2Logo,
                        MatchTime = DateTimeOffset.FromUnixTimeMilliseconds(long.Parse(timeString)).UtcDateTime,
                        Event = eventString,
                        MatchFormat = matchFormat
                    });
                }
            }

            var distinctMatches = matches
            .GroupBy(m => new { m.Team1, m.Team2, m.MatchFormat })
            .Select(g => g.First())
            .ToList();

            return distinctMatches;
           

        }
    }


}
