using Google.Protobuf.WellKnownTypes;
using HLTV.Api.Contexts;
using HLTV.Api.Models;
using HLTV.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Crmf;
using RestSharp;

namespace HLTV.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RankingsController : ControllerBase
    {
        private readonly HltvService _hltvService;
        private readonly HltvContext _hltvContext;

        public RankingsController(HltvService hltvService, HltvContext hltvContext)
        {
            _hltvService = hltvService;
            _hltvContext = hltvContext;
        }

        [HttpPost]
        public async Task<IActionResult> ScrapeAndSave()
        {
            var url = "https://www.hltv.org/ranking/teams/2024/july/8";
            var teamRankings = await _hltvService.ScrapeRanking(url);

            _hltvContext.Rankings.AddRange(teamRankings);
            await _hltvContext.SaveChangesAsync();

            return Ok(teamRankings);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ranking>>> GetAllRankings()
        {
            return await _hltvContext.Rankings.ToListAsync();
        }


        [HttpGet]
        public IEnumerable<Match> GetMatches()
        {
            return _hltvService.GetUpcomingMatches();
        }



       

            


       
   
    }

}