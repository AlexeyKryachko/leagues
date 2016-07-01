using System.Collections.Generic;
using System.Linq;
using Implementations.Core;
using Implementations.Teams;
using Interfaces.Core;
using Interfaces.Events.DataAccess;
using Interfaces.Games.DataAccess;
using Interfaces.Leagues.BuisnessLogic;
using Interfaces.Leagues.BuisnessLogic.Model;
using Interfaces.Leagues.DataAccess;
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
        private readonly IEventsRepository _eventsRepository;

        private readonly TeamsMapper _teamMapper;
        private readonly LeaguesMapper _leaguesMapper;
        private readonly UserStatisticCalculation _userStatisticCalculation;

        public LeaguesManager(ILeaguesRepository leaguesRepository, ITeamsRepository teamsRepository, IGamesRepository gamesRepository, IPlayersRepository playersRepository, IUsersRepository usersRepository, TeamsMapper teamMapper, UserStatisticCalculation userStatisticCalculation, LeaguesMapper leaguesMapper, IEventsRepository eventsRepository)
        {
            _leaguesRepository = leaguesRepository;
            _teamsRepository = teamsRepository;
            _gamesRepository = gamesRepository;
            _playersRepository = playersRepository;
            _usersRepository = usersRepository;
            _teamMapper = teamMapper;
            _userStatisticCalculation = userStatisticCalculation;
            _leaguesMapper = leaguesMapper;
            _eventsRepository = eventsRepository;
        }

        public LeagueInfoViewModel GetInfo(string leagueId)
        {
            var league = _leaguesRepository.Get(leagueId);
            var teams = _teamsRepository.GetByLeague(leagueId).ToList();
            var games = _gamesRepository.GetByLeague(leagueId).ToList();
            var events = _eventsRepository.GetByLeague(leagueId).ToList();

            var userIds = teams.SelectMany(x => x.MemberIds);
            var users = _playersRepository.GetRange(userIds)
                .ToDictionary(x => x.EntityId, x => x);

            return _leaguesMapper.MapInfo(league, teams, games, events, users);
        }

        public LeaguesViewModel GetLeagues()
        {
            var leagueEntities = _leaguesRepository
                .GetAll()
                .ToList();

            return _leaguesMapper.MapLeagues(leagueEntities);
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

            return _leaguesMapper.MapUnsecure(league, users);
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
                .Select(x => _teamMapper.MapTeamStatistic(x, games));
            
            var bestPlayers = _userStatisticCalculation.GetBestStatistic(games) 
                .OrderByDescending(x => x.Value)
                .Take(7)
                .ToList();

            var bestForwards = _userStatisticCalculation.GetGoalsStatistic(games)
                .OrderByDescending(x => x.Value)
                .Take(7)
                .ToList();

            var bestHelpers = _userStatisticCalculation.GetHelpsStatistic(games)
                .OrderByDescending(x => x.Value)
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
                    Number = x.Value
                }),
                BestForwards = bestForwards.Select(x => new LeagueMemberStatisticViewModel
                {
                    Id = x.Id,
                    Name = users.ContainsKey(x.Id)
                        ? users[x.Id]
                        : null,
                    Number = x.Value
                }),
                BestHelpers = bestHelpers.Select(x => new LeagueMemberStatisticViewModel
                {
                    Id = x.Id,
                    Name = users.ContainsKey(x.Id)
                        ? users[x.Id]
                        : null,
                    Number = x.Value
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
