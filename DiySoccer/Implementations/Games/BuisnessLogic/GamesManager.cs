using System.Collections.Generic;
using System.Linq;
using Interfaces.Core;
using Interfaces.Events.DataAccess;
using Interfaces.Games.BuisnessLogic;
using Interfaces.Games.BuisnessLogic.Models;
using Interfaces.Games.DataAccess;
using Interfaces.Teams.DataAccess;
using Interfaces.Users.DataAccess;

namespace Implementations.Games.BuisnessLogic
{
    public class GamesManager : IGamesManager
    {
        private readonly IGamesRepository _gamesRepository;
        private readonly IPlayersRepository _playersRepository;
        private readonly ITeamsRepository _teamsRepository;
        private readonly IEventsRepository _eventsRepository;

        private readonly GamesMapper _gameMapper;

        public GamesManager(IGamesRepository gamesRepository, IPlayersRepository playersRepository, GamesMapper gameMapper, ITeamsRepository teamsRepository, IEventsRepository eventsRepository)
        {
            _gamesRepository = gamesRepository;
            _playersRepository = playersRepository;
            _gameMapper = gameMapper;
            _teamsRepository = teamsRepository;
            _eventsRepository = eventsRepository;
        }

        public GameVewModel Get(string leagueId, string gameId)
        {
            var game = _gamesRepository.Get(gameId);
            var userIds = game.HomeTeam.Members.Select(x => x.Id).Concat(game.GuestTeam.Members.Select(x => x.Id));
            var users = _playersRepository.GetRange(userIds).ToDictionary(x => x.EntityId, y => y.Name);
            var events = _eventsRepository.GetByLeague(leagueId);

            return new GameVewModel
            {
                Id = game.EntityId,
                CustomScores = game.CustomScores,
                EventId = game.EventId,
                Events = events.Select(x => new IdNameViewModel
                {
                   Id = x.EntityId,
                   Name = x.Name
                }),
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

        public GameExternalViewModel GetExternal(string leagueId)
        {
            var eventsDb = _eventsRepository.GetByLeague(leagueId).ToList();
            var teamsDb = _teamsRepository.GetByLeague(leagueId).ToList();
            var playersDb = _playersRepository
                .Find(leagueId, null, 0, int.MaxValue)
                .ToDictionary(x => x.EntityId, y => y);

            var eventsViewModel = new List<EventsGameExternalViewModel>();

            foreach (var eventDb in eventsDb)
            {
                var eventViewModel = new EventsGameExternalViewModel
                {
                    Id = eventDb.EntityId,
                    Title = eventDb.Name
                };

                foreach (var gameDb in eventDb.Games)
                {
                    var homeTeam = teamsDb.FirstOrDefault(x => x.EntityId == gameDb.HomeTeamId);
                    if (homeTeam == null)
                        continue;

                    var guestTeam = teamsDb.FirstOrDefault(x => x.EntityId == gameDb.GuestTeamId);
                    if (guestTeam == null)
                        continue;
                    
                    var gameViewModel = new EventGameExternalViewModel
                    {
                        Title = homeTeam.Name + " - " + guestTeam.Name,
                        Value = homeTeam.EntityId + "-" + guestTeam.EntityId,
                        HomeTeamTitle = homeTeam.Name,
                        HomeTeam = _gameMapper.MapTeamInfo(homeTeam, playersDb),
                        GuestTeamTitle = guestTeam.Name,
                        GuestTeam = _gameMapper.MapTeamInfo(guestTeam, playersDb),
                    };

                    eventViewModel.Games.Add(gameViewModel);
                }

                eventsViewModel.Add(eventViewModel);
            }

            return new GameExternalViewModel
            {
                Events = eventsViewModel
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
            _gamesRepository.Update(leagueId, gameId, model);
        }

        public void Delete(string gameId)
        {
            _gamesRepository.Delete(gameId);
        }
    }
}
