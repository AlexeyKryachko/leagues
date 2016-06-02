using System.Collections.Generic;
using System.Linq;
using Interfaces.Core;
using Interfaces.Games.DataAccess;
using Interfaces.Games.DataAccess.Model;
using Interfaces.Leagues.BuisnessLogic.Model;
using Interfaces.Leagues.DataAccess;
using Interfaces.Teams.BuisnessLogic;
using Interfaces.Teams.BuisnessLogic.Models;
using Interfaces.Teams.DataAccess;
using Interfaces.Users.DataAccess;

namespace Implementations.Teams.BuisnessLogic
{
    public class TeamsManager : ITeamsManager
    {
        private readonly ILeaguesRepository _leaguesRepository;
        private readonly IGamesRepository _gamesRepository;
        private readonly ITeamsRepository _teamsRepository;
        private readonly IPlayersRepository _playersRepository;

        private readonly ScoreCalculation _scoreCalculation = new ScoreCalculation();

        public TeamsManager(ITeamsRepository teamsRepository, IPlayersRepository playersRepository, IGamesRepository gamesRepository, ILeaguesRepository leaguesRepository)
        {
            _teamsRepository = teamsRepository;
            _playersRepository = playersRepository;
            _gamesRepository = gamesRepository;
            _leaguesRepository = leaguesRepository;
        }

        public void Create(string leagueId, TeamViewModel model)
        {
            var exitedIds = model.Members
                .Where(x => !string.IsNullOrEmpty(x.Id))
                .Select(x => x.Id);
            var userEntities = model.Members
                .Where(x => string.IsNullOrEmpty(x.Id))
                .Select(x => new UserDb {Name = x.Name})
                .ToList();
            _playersRepository.AddRange(leagueId, userEntities);

            var userIds = userEntities.Select(x => x.EntityId);
            var memberIds = exitedIds.Concat(userIds);

            var entity = new TeamDb
            {
                LeagueId = leagueId,
                Name = model.Name,
                Hidden = model.Hidden,
                MemberIds = memberIds,
                MediaId = model.Media,
                Description = model.Description
            };
            _teamsRepository.Add(leagueId, entity);
        }

        public TeamViewModel Get(string leagueId, string teamId)
        {
            var team = _teamsRepository.Get(teamId);
            if (team == null)
                return null;

            var members = _playersRepository.GetRange(team.MemberIds);
            return new TeamViewModel
            {
                Id = team.EntityId,
                Name = team.Name,
                Hidden = team.Hidden,
                Members = members.Select(x => new IdNameViewModel
                {
                    Id = x.EntityId,
                    Name = x.Name
                }),
                Media = team.MediaId,
                Description = team.Description
            };
        }

        public TeamInfoViewModel GetInfo(string leagueId, string teamId)
        {
            var league = _leaguesRepository.Get(leagueId);
            var games = _gamesRepository.GetByTeam(leagueId, teamId);
            var teamIds = games.Select(x => x.GuestTeam.Id).Concat(games.Select(x => x.HomeTeam.Id)).ToList();
            teamIds.Add(teamId);
            teamIds = teamIds.Distinct().ToList();

            var teams = _teamsRepository
                .GetRange(teamIds)
                .ToDictionary(x => x.EntityId, y => y);

            var userIds = teams[teamId].MemberIds;
            var users = _playersRepository.GetRange(userIds);

            var stats = new List<TeamInfoStatisticViewModel>();
            var currentStatisticLeague = new TeamInfoStatisticViewModel(GetStatistic(teams[teamId], games), teams[teamId]);
            stats.Add(currentStatisticLeague);

            return new TeamInfoViewModel
            {
                Id = teamId,
                Name = teams[teamId].Name,
                MediaId = teams[teamId].MediaId,
                Description = teams[teamId].Description,
                Games = games.Select(x => new TeamInfoGameViewModel
                {
                    Id = x.EntityId,
                    OpponentName = x.GuestTeam.Id == teamId
                        ? teams[x.HomeTeam.Id].Name
                        : teams[x.GuestTeam.Id].Name,
                    Goals = x.GuestTeam.Id == teamId
                        ? x.HomeTeam.Score + ":" + x.GuestTeam.Score + "*"
                        : x.GuestTeam.Score + ":" + x.HomeTeam.Score + "*"
                }),
                Players = users.Select(y => new TeamInfoMemberViewModel
                {
                    Id = y.EntityId,
                    Name = y.Name
                }),
                Statistics = stats
            };
        }

        public void Update(string leagueId, string teamId, TeamViewModel model)
        {
            var exitedIds = model.Members
                .Where(x => !string.IsNullOrEmpty(x.Id))
                .Select(x => x.Id);
            var userEntities = model.Members
                .Where(x => string.IsNullOrEmpty(x.Id))
                .Select(x => new UserDb { Name = x.Name })
                .ToList();
            _playersRepository.AddRange(leagueId, userEntities);

            var userIds = userEntities.Select(x => x.EntityId);
            var memberIds = exitedIds.Concat(userIds);

            var entity = new TeamDb
            {
                EntityId = teamId,
                LeagueId = leagueId,
                Name = model.Name,
                Hidden = model.Hidden,
                MemberIds = memberIds,
                MediaId = model.Media,
                Description = model.Description
            };
            _teamsRepository.Update(leagueId, entity);
        }

        public IEnumerable<TeamViewModel> GetByLeague(string id)
        {
            var teams = _teamsRepository.GetByLeague(id).ToList();
            var userIds = teams.SelectMany(x => x.MemberIds).Distinct();
            var users = _playersRepository.GetRange(userIds).ToDictionary(x => x.EntityId, y => y);

            return teams.Select(x => new TeamViewModel
            {
                Id = x.EntityId,
                Name = x.Name,
                Hidden = x.Hidden,
                Members = x.MemberIds.Select(y => new IdNameViewModel
                {
                    Id = y,
                    Name = users[y].Name
                })
            });
        }

        public TeamStatisticViewModel GetStatistic(TeamDb team, IEnumerable<GameDb> games)
        {
            var model = new TeamStatisticViewModel
            {
                Id = team.EntityId,
                Name = team.Name,
                MediaId = team.MediaId
            };

            model.GamesCount = games.Count(x => x.GuestTeam.Id == team.EntityId || x.HomeTeam.Id == team.EntityId);

            model.Wins = games.Count(x =>
                (x.GuestTeam.Id == team.EntityId && x.GuestTeam.Score > x.HomeTeam.Score) ||
                (x.HomeTeam.Id == team.EntityId && x.HomeTeam.Score > x.GuestTeam.Score));

            model.Loses = games.Count(x =>
                (x.GuestTeam.Id == team.EntityId && x.GuestTeam.Score < x.HomeTeam.Score) ||
                (x.HomeTeam.Id == team.EntityId && x.HomeTeam.Score < x.GuestTeam.Score));

            model.Draws = games.Count(x =>
                (x.GuestTeam.Id == team.EntityId && x.GuestTeam.Score == x.HomeTeam.Score) ||
                (x.HomeTeam.Id == team.EntityId && x.HomeTeam.Score == x.GuestTeam.Score));

            model.Scores = games
                .Where(x => x.GuestTeam.Id == team.EntityId)
                .Sum(x => x.GuestTeam.Score);
            model.Scores += games
                .Where(x => x.HomeTeam.Id == team.EntityId)
                .Sum(x => x.HomeTeam.Score);

            model.Missed = games
                .Where(x => x.GuestTeam.Id == team.EntityId)
                .Sum(x => x.HomeTeam.Score);
            model.Missed += games
                .Where(x => x.HomeTeam.Id == team.EntityId)
                .Sum(x => x.GuestTeam.Score);

            model.Points = _scoreCalculation.Default(model.Wins, model.Loses, model.Draws);

            return model;
        }
    }
}
