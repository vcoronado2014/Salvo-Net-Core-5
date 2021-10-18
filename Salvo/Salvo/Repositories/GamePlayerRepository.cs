﻿using Microsoft.EntityFrameworkCore;
using Salvo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Salvo.Repositories
{
    public class GamePlayerRepository : RepositoryBase<GamePlayer>, IGamePlayerRepository
    {

        public GamePlayerRepository(SalvoContext repositoryContext)
            : base(repositoryContext)
        {
        }

        public GamePlayer GetGamePlayerView(int idGamePlayer)
        {
            return FindAll(source => source.Include(gamePlayer => gamePlayer.Ships)
                                               .ThenInclude(ship => ship.Locations)
                                            .Include(gamePlayer => gamePlayer.Salvos)
                                                .ThenInclude(salvo => salvo.Locations)
                                            .Include(gamePlayer => gamePlayer.Game)
                                                .ThenInclude(game => game.GamePlayers)
                                                    .ThenInclude(gp => gp.Player)
                                            .Include(gamePlayer => gamePlayer.Game)
                                                .ThenInclude(game => game.GamePlayers)
                                                    .ThenInclude(gp => gp.Salvos)
                                                        .ThenInclude(salvo => salvo.Locations)
                                            .Include(gamePlayer => gamePlayer.Game)
                                                .ThenInclude(game => game.GamePlayers)
                                                    .ThenInclude(gp => gp.Ships)
                                                        .ThenInclude(ship => ship.Locations)

                                            )
                .Where(gamePlayer => gamePlayer.Id == idGamePlayer)
                .OrderBy(game => game.JoinDate)
                .FirstOrDefault();

        }
        public void Save(GamePlayer gamePlayer)
        {
            if (gamePlayer.Id == 0)
                Create(gamePlayer);
            else
                Update(gamePlayer);
            SaveChanges();
        }

        public GamePlayer FindById(int id)
        {
            return FindByCondition(gp => gp.Id == id)
                    .Include(gp => gp.Game)
                        .ThenInclude(game => game.GamePlayers)
                            .ThenInclude(gp => gp.Salvos)
                                .ThenInclude(salvo => salvo.Locations)
                    .Include(gp => gp.Game)
                        .ThenInclude(game => game.GamePlayers)
                            .ThenInclude(gp => gp.Ships)
                                .ThenInclude(ship => ship.Locations)
                    .Include(gp => gp.Player)
                    .Include(gp => gp.Ships)
                    .Include(gp => gp.Salvos)
                .FirstOrDefault();


        }


    }

}
