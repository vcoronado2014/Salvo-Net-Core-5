﻿using Microsoft.AspNetCore.Authorization;
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
    [Route("api/games")]
    [ApiController]
    [Authorize]
    public class GamesController : ControllerBase
    {
        private IGameRepository _repository;
        private IPlayerRepository _playerRepository;
        private IGamePlayerRepository _gamePlayerRepository;


        public GamesController(IGameRepository repository, IPlayerRepository playerRepository, IGamePlayerRepository gamePlayerRepository)
        {
            _repository = repository;
            _playerRepository = playerRepository;
            _gamePlayerRepository = gamePlayerRepository;

        }

        // GET: api/Games
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Get()
        {
            try
            {
                GameListDTO gameList = new GameListDTO
                {
                    Email = User.FindFirst("Player") != null ? User.FindFirst("Player").Value : "Guest",
                    Games = _repository.GetAllGamesWithPlayers()
                                   .Select(g => new GameDTO
                                   {
                                       Id = g.Id,
                                       CreationDate = g.CreationDate,
                                       GamePlayers = g.GamePlayers.Select(gp => new GamePlayerDTO
                                       {
                                           Id = gp.Id,
                                           JoinDate = gp.JoinDate,
                                           Player = new PlayerDTO
                                           {
                                               Id = gp.Player.Id,
                                               Email = gp.Player.Email
                                           },
                                           Point = gp.GetScore() != null ? (double?)gp.GetScore().Point : null
                                       }).ToList()
                                   }).ToList()
                };

                return Ok(gameList);


            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET: api/Games/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Games
        [HttpPost]
        public IActionResult Post()
        {
            try
            {
                string email = User.FindFirst("Player") != null ? User.FindFirst("Player").Value : "Guest";
                Player player = _playerRepository.FindByEmail(email);
                GamePlayer gamePlayer = new GamePlayer
                {
                    Game = new Game
                    {
                        CreationDate = DateTime.Now
                    },
                    PlayerId = player.Id,
                    JoinDate = DateTime.Now
                };
                _gamePlayerRepository.Save(gamePlayer);
                return StatusCode(201, gamePlayer.Id);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        // PUT: api/Games/5
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
