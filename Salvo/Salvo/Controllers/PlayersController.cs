using Microsoft.AspNetCore.Mvc;
using Salvo.Models;
using Salvo.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Salvo.Controllers
{
    [Route("api/players")]
    [ApiController]
    public class PlayersController : ControllerBase
    {
        private IPlayerRepository _repository;

        public PlayersController(IPlayerRepository repository)
        {
            _repository = repository;
        }

        // GET: api/Players
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Players/5
        [HttpGet("{id}", Name = "GetPlayer")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Players
        [HttpPost]
        public IActionResult Post([FromBody] PlayerDTO player)
        {
            try
            {
                if (String.IsNullOrEmpty(player.Email) || String.IsNullOrEmpty(player.Password))
                    return StatusCode(403, "datos inválidos");

                Player dbPlayer = _repository.FindByEmail(player.Email);
                if (dbPlayer != null)
                    return StatusCode(403, "email en uso");

                Player newPlayer = new Player
                {
                    Email = player.Email,
                    Password = player.Password
                };

                _repository.Save(newPlayer);
                return StatusCode(201, newPlayer);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }


        }

        // PUT: api/Players/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }

}
