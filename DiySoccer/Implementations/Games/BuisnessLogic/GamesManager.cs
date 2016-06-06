﻿using System.Collections.Generic;
using System.Linq;
using Interfaces.Games.BuisnessLogic;
using Interfaces.Games.BuisnessLogic.Models;
using Interfaces.Games.DataAccess;
using Interfaces.Games.DataAccess.Model;
using Interfaces.Teams.DataAccess;
using Interfaces.Users.DataAccess;

namespace Implementations.Games.BuisnessLogic
{
    public class GamesManager : IGamesManager
    {
        private readonly IGamesRepository _gamesRepository;
        private readonly IPlayersRepository _playersRepository;
        private readonly ITeamsRepository _teamsRepository;

        private readonly GamesMapper _gameMapper;

        public GamesManager(IGamesRepository gamesRepository, IPlayersRepository playersRepository, GamesMapper gameMapper, ITeamsRepository teamsRepository)
        {
            _gamesRepository = gamesRepository;
            _playersRepository = playersRepository;
            _gameMapper = gameMapper;
            _teamsRepository = teamsRepository;
        }

        public GameVewModel Get(string leagueId, string gameId)
        {
            var game = _gamesRepository.Get(gameId);
            var userIds = game.HomeTeam.Members.Select(x => x.Id).Concat(game.GuestTeam.Members.Select(x => x.Id));
            var users = _playersRepository.GetRange(userIds).ToDictionary(x => x.EntityId, y => y.Name);

            return new GameVewModel
            {
                Id = game.EntityId,
                CustomScores = game.CustomScores,
                HomeTeam = new GameTeamViewModel
                {
                    Id = game.HomeTeam.Id,
                    Score = game.HomeTeam.Score,
                    BestId = game.HomeTeam.BestMemberId,
                    Members = game.HomeTeam.Members.Select(x => new GameMemberViewModel
                    {
                        Id = x.Id,
                        Name = users[x.Id],
                        Score = x.Score,
                        Help = x.Help
                    })
                },
                GuestTeam = new GameTeamViewModel
                {
                    Id = game.GuestTeam.Id,
                    Score = game.GuestTeam.Score,
                    BestId = game.GuestTeam.BestMemberId,
                    Members = game.GuestTeam.Members.Select(x => new GameMemberViewModel
                    {
                        Id = x.Id,
                        Name = users[x.Id],
                        Score = x.Score,
                        Help = x.Help
                    })
                }
            };
        }

        public GameInfoViewModel GetInfo(string leagueId, string gameId)
        {
            var game = _gamesRepository.Get(gameId);
            var teamIds = new List<string>() { game.HomeTeam.Id, game.GuestTeam.Id };
            var teams = _teamsRepository.GetRange(teamIds);
            var userIds = game
                .HomeTeam.Members
                    .Where(x => x.Help != 0 || x.Score != 0)
                    .Select(x => x.Id)
                .Concat(game
                .GuestTeam.Members
                    .Where(x => x.Help != 0 || x.Score != 0)
                    .Select(x => x.Id))
                .ToList();

            if (!string.IsNullOrEmpty(game.HomeTeam.BestMemberId))
                userIds.Add(game.HomeTeam.BestMemberId);
            if (!string.IsNullOrEmpty(game.GuestTeam.BestMemberId))
                userIds.Add(game.GuestTeam.BestMemberId);

            var users = _playersRepository
                .GetRange(userIds.Distinct())
                .ToDictionary(x => x.EntityId, y => y.Name);

            return _gameMapper.MapGameInfo(game, teams, users);
        }

        public void Create(string leagueId, GameVewModel model)
        {
            _gamesRepository.Create(leagueId, model);
        }

        public void Update(string leagueId, string gameId, GameVewModel model)
        {
            var guestMemberScores = model.GuestTeam.Members.Select(x => new GameMemberDb
            {
                Id = x.Id,
                Help = x.Help,
                Score = x.Score
            });
            
            var homeMemberScores = model.HomeTeam.Members.Select(x => new GameMemberDb
            {
                Id = x.Id,
                Help = x.Help,
                Score = x.Score
            });

            _gamesRepository.Update(leagueId, gameId, model.CustomScores, model.GuestTeam.Score, model.GuestTeam.BestId, guestMemberScores,
                model.HomeTeam.Score, model.HomeTeam.BestId, homeMemberScores);
        }

        public void Delete(string gameId)
        {
            _gamesRepository.Delete(gameId);
        }
    }
}
