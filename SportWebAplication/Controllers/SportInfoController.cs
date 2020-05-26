using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RestSharp;
using SportWebAplication.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Amazon.DynamoDBv2.Model;

namespace SportWebAplication.Controllers
{
    [ApiController]
    [Route("api/sportinfo")]
    public class SportInfoController : ControllerBase
    {
        

        private readonly ILogger<SportInfoController> _logger;

        public SportInfoController(ILogger<SportInfoController> logger)
        {
            _logger = logger;
        }
        PlayersContext db;
        List<TopScorers>  topscorer = new List<TopScorers>(20);
        [HttpGet("{id}")]
        public async Task<ActionResult<Player>> Get(int id)
        {
            using var client = new HttpClient();

            client.BaseAddress = new Uri("https://api-football-v1.p.rapidapi.com");
            client.DefaultRequestHeaders.Add("x-rapidapi-host", "api-football-v1.p.rapidapi.com");
            client.DefaultRequestHeaders.Add("x-rapidapi-key", "eb0316cd3bmsh0b06c0649153a48p13b7a4jsne4adcbfaeeaa");
            client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

            var url = "v2/topscorers/4";
            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var resp = await response.Content.ReadAsStringAsync();

            
            Player players = JsonConvert.DeserializeObject<Player>(resp);

            for(int k = 0; k < 20; k++)
            {
                topscorer.Add(players.api.topscorers[k]);
            }
            return new ObjectResult(topscorer[id]);
        }

        [HttpPost]
        public async Task<ActionResult<Player>> Post(TopScorers player)
        {
            if (player == null)
            {
                return BadRequest();
            }

            db.Players.Add(player);
            await db.SaveChangesAsync();
            return Ok(player);
        }

        [HttpPut]
        public async Task<ActionResult<Player>> Put(TopScorers player)
        {
            if (player == null)
            {
                return BadRequest();
            }
            if (!db.Players.Any(x => x.player_id == player.player_id))
            {
                return NotFound();
            }

            db.Update(player);
            await db.SaveChangesAsync();
            return Ok(player);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Player>> Delete(int id)
        {
            TopScorers player = db.Players.FirstOrDefault(x => x.player_id == id);
            if (player == null)
            {
                return NotFound();
            }
            db.Players.Remove(player);
            await db.SaveChangesAsync();
            return Ok(player);
        }
    }
}
