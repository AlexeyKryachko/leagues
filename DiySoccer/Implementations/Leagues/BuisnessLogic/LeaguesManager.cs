using System.Collections.Generic;
using System.Linq;
using Interfaces.Core;
using Interfaces.Games.DataAccess;
using Interfaces.Leagues.BuisnessLogic;
using Interfaces.Leagues.BuisnessLogic.Model;
using Interfaces.Leagues.DataAccess;
using Interfaces.Teams.BuisnessLogic;
using Interfaces.Teams.BuisnessLogic.Models;
using Interfaces.Teams.DataAccess;
using Interfaces.Users.DataAccess;

namespace Implementations.Leagues.BuisnessLogic
{
    public class LeaguesManager : ILeaguesManager
    {
        private readonly ILeaguesRepository _leaguesRepository;
        private readonly ITeamsRepository _teamsRepository;
        private readonly IGamesRepository _gamesRepository;
        private readonly IPlayersRepository _playersRepository;
        private readonly IUsersRepository _usersRepository;

        private readonly ITeamsManager _teamsManager;

        public LeaguesManager(ILeaguesRepository leaguesRepository, ITeamsRepository teamsRepository, IGamesRepository gamesRepository, IPlayersRepository playersRepository, IUsersRepository usersRepository, ITeamsManager teamsManager)
        {
            _leaguesRepository = leaguesRepository;
            _teamsRepository = teamsRepository;
            _gamesRepository = gamesRepository;
            _playersRepository = playersRepository;
            _usersRepository = usersRepository;
            _teamsManager = teamsManager;
        }

        public IEnumerable<LeagueViewModel> GetAll()
        {
            return _leaguesRepository.GetAll().Select(x => new LeagueViewModel
            {
                Id = x.EntityId,
                Name = x.Name,
                Description = x.Description
            });
        }

        public IEnumerable<LeagueUnsecureViewModel> GetAllUnsecure()
        {
            var leagues = _leaguesRepository.GetAll().ToList();
            var userIds = leagues.SelectMany(x => x.Admins).ToList();
            var users = _usersRepository.GetRange(userIds).ToDictionary(x => x.Id, y => y.UserName);

            return leagues.Select(x => new LeagueUnsecureViewModel
            {
                Id = x.EntityId,
                Name = x.Name,
                Description = x.Description,
                VkGroup = x.VkSecurityGroup,
                Admins = x.Admins
                    .Select(y => users.ContainsKey(y) 
                        ? new IdNameViewModel
                        {
                            Id = y,
                            Name = users[y]
                        }  
                        : null)
                    .Where(y => y != null)
            });
        }

        public LeagueUnsecureViewModel GetUnsecure(string leagueId)
        {
            var league = _leaguesRepository.Get(leagueId);
            if (league == null)
                return null;

            var userIds = league.Admins;
            var users = _usersRepository.GetRange(userIds).ToDictionary(x => x.Id, y => y.UserName);

            return new LeagueUnsecureViewModel
                {
                    Id = league.EntityId,
                    Name = league.Name,
                    Description = league.Description,
                    VkGroup = league.VkSecurityGroup,
                    Admins = league.Admins
                        .Select(y => users.ContainsKey(y)
                            ? new IdNameViewModel
                            {
                                Id = y,
                                Name = users[y]
                            }
                            : null)
                        .Where(y => y != null)
            };
        }

        public void Create(LeagueUnsecureViewModel model)
        {
            _leaguesRepository.Create(model);
        }

        public void Update(LeagueUnsecureViewModel model)
        {
            _leaguesRepository.Update(model);
        }

        public LeagueStatisticViewModel GetStatisticByLeague(string leagueId)
        {
            var teams = _teamsRepository.GetByLeague(leagueId).ToList();
            var games = _gamesRepository.GetByLeague(leagueId).ToList();

            var teamsStatistic = teams
                .Where(x => !x.Hidden)
                .Select(x => _teamsManager.GetStatistic(x, games));
            
            var bestPlayers = Enumerable.Concat(
                games.Where(x => !string.IsNullOrEmpty(x.HomeTeam.BestMemberId)).Select(x => x.HomeTeam.BestMemberId),
                games.Where(x => !string.IsNullOrEmpty(x.GuestTeam.BestMemberId)).Select(x => x.GuestTeam.BestMemberId))
                .GroupBy(x => x)
                .Select(x => new
                {
                    Id = x.Key,
                    Count = x.Count()
                })
                .OrderByDescending(x => x.Count)
                .Take(7)
                .ToList();

            var bestForwards = Enumerable.Concat(
                games.SelectMany(x => x.HomeTeam.Members),
                games.SelectMany(x => x.GuestTeam.Members))
                .GroupBy(x => x.Id)
                .Select(x => new
                {
                    Id = x.Key,
                    Goals = x.Sum(y => y.Score)
                })
                .OrderByDescending(x => x.Goals)
                .Take(7)
                .ToList();

            var bestHelpers = Enumerable.Concat(
                games.SelectMany(x => x.HomeTeam.Members),
                games.SelectMany(x => x.GuestTeam.Members))
                .GroupBy(x => x.Id)
                .Select(x => new
                {
                    Id = x.Key,
                    Helps = x.Sum(y => y.Help)
                })
                .OrderByDescending(x => x.Helps)
                .Take(7)
                .ToList();

            var userIds = new List<string>();
            userIds.AddRange(bestPlayers.Select(x => x.Id));
            userIds.AddRange(bestForwards.Select(x => x.Id));
            userIds.AddRange(bestHelpers.Select(x => x.Id));
            userIds = userIds.Distinct().ToList();
            var users = _playersRepository
                .GetRange(userIds)
                .ToDictionary(x => x.EntityId, 
                    y => teams.Exists(x => x.MemberIds.Contains(y.EntityId)) 
                        ? y.Name + " (" + teams.Find(x => x.MemberIds.Contains(y.EntityId)).Name + ")"
                        : y.Name);

            return new LeagueStatisticViewModel
            {
                BestPlayers = bestPlayers.Select(x => new LeagueMemberStatisticViewModel
                {
                    Id = x.Id,
                    Name = users.ContainsKey(x.Id) 
                        ? users[x.Id]
                        : null,
                    Number = x.Count
                }),
                BestForwards = bestForwards.Select(x => new LeagueMemberStatisticViewModel
                {
                    Id = x.Id,
                    Name = users.ContainsKey(x.Id)
                        ? users[x.Id]
                        : null,
                    Number = x.Goals
                }),
                BestHelpers = bestHelpers.Select(x => new LeagueMemberStatisticViewModel
                {
                    Id = x.Id,
                    Name = users.ContainsKey(x.Id)
                        ? users[x.Id]
                        : null,
                    Number = x.Helps
                }),
                TeamStatistics = teamsStatistic
                    .OrderByDescending(x => x.Points)
                    .ThenByDescending(x => x.Scores)
                    .ThenByDescending(x => x.Missed)
            };
        }

        public void Delete(string leagueId)
        {
            throw new System.NotImplementedException();
        }
    }
}
