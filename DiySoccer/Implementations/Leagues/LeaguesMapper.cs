using System;
using System.Collections.Generic;
using System.Linq;
using Implementations.Core;
using Implementations.Events;
using Interfaces.Core;
using Interfaces.Events.DataAccess.Model;
using Interfaces.Games.DataAccess.Model;
using Interfaces.Leagues.BuisnessLogic.Model;
using Interfaces.Leagues.DataAccess.Model;
using Interfaces.Teams.DataAccess;
using Interfaces.Users.DataAccess;

namespace Implementations.Leagues
{
    public class LeaguesMapper
    {
        private readonly ScoreCalculation _scoreCalculation;
        private readonly UserStatisticCalculation _userStatisticCalculation;
        private readonly EventsMapper _eventsMapper;

        public LeaguesMapper(ScoreCalculation scoreCalculation, UserStatisticCalculation userStatisticCalculation, EventsMapper eventsMapper)
        {
            _scoreCalculation = scoreCalculation;
            _userStatisticCalculation = userStatisticCalculation;
            _eventsMapper = eventsMapper;
        }

        public LeaguesViewModel MapLeagues(IList<LeagueDb> entities)
        {
            var leagues = entities
                .Where(x => x.Type != LeagueType.Tournament)
                .Select(Map);
            var tournaments = entities
                .Where(x => x.Type == LeagueType.Tournament)
                .Select(Map);

            return new LeaguesViewModel
            {
                Leagues = leagues,
                Tournaments = tournaments
            };
        }

        public LeagueViewModel Map(LeagueDb league)
        {
            return new LeagueViewModel
            {
                Id = league.EntityId,
                Name = league.Name,
                MediaId = league.MediaId,
                Description = league.SubName,
                Type = league.Type
            };
        }

        public LeagueUnsecureViewModel MapUnsecure(LeagueDb league, Dictionary<string, string> users)
        {
            return new LeagueUnsecureViewModel
            {
                Id = league.EntityId,
                Name = league.Name,
                SubName = league.SubName,
                Description = league.Description,
                Information = league.Information,
                VkGroup = league.VkSecurityGroup,
                MediaId = league.MediaId,
                Type = league.Type,
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


        private LeagueInfoGameToPlayViewModel Map(EventDb entity, IList<GameDb> games, IList<TeamDb> teams)
        {
            if (games == null || !entity.StartDate.HasValue)
                return null;

            var dictionary = teams.ToDictionary(x => x.EntityId, x => x);
            var teamValues = entity
                .Games
                .Where(x => !games.Any(game => game.HomeTeam.Id == x.HomeTeamId && game.GuestTeam.Id == x.GuestTeamId))
                .Where(x => dictionary.ContainsKey(x.HomeTeamId) && !dictionary[x.HomeTeamId].Hidden &&
                            dictionary.ContainsKey(x.GuestTeamId) && !dictionary[x.GuestTeamId].Hidden)
                .Select(x => dictionary[x.HomeTeamId].Name + " - " + dictionary[x.GuestTeamId].Name);
            
            return new LeagueInfoGameToPlayViewModel
            {
                Name = entity.Name,
                Date = entity.StartDate.Value.ToShortDateString(),
                Teams = teamValues,
                Important = entity.StartDate < DateTime.UtcNow.Date.AddDays(-14)
            };
        }

        public LeagueInfoViewModel MapInfo(LeagueDb league, List<TeamDb> teams, IList<GameDb> games, IList<EventDb> events, Dictionary<string, UserDb> users)
        {
            var teamsStatistic = teams
                .Where(x => !x.Hidden)
                .Select(x => new LeagueInfoTeamViewModel
                {
                    Id = x.EntityId,
                    Name = x.Name,
                    MediaId = x.MediaId,
                    Games = _scoreCalculation.GameCount(games, x.EntityId),
                    Points = _scoreCalculation.Points(games, x.EntityId),
                    Scores = _scoreCalculation.GoalsCount(games, x.EntityId),
                    Missed = _scoreCalculation.MissedCount(games, x.EntityId)
                })
                .OrderByDescending(x => x.Points)
                .ThenByDescending(x => x.Scores)
                .ThenByDescending(x => x.Missed)
                .ToList();

            var best = _userStatisticCalculation.GetBestStatistic(games)
                .OrderByDescending(x => x.Value)
                .FirstOrDefault();
            var bestPlayerTeam = best != null
                ? teams.FirstOrDefault(x => x.MemberIds.Contains(best.Id))
                : null;
            var bestPlayer = best == null || users[best.Id] == null
                ? null
                : new LeagueInfoPlayerViewModel
                {
                    Id = best.Id,
                    MediaId = bestPlayerTeam != null ? bestPlayerTeam.MediaId : null,
                    Name = users[best.Id].Name
                };

            var goals = _userStatisticCalculation.GetGoalsStatistic(games)
                .OrderByDescending(x => x.Value)
                .FirstOrDefault();
            var goalsPlayerTeam = goals != null
                ? teams.FirstOrDefault(x => x.MemberIds.Contains(goals.Id))
                : null;
            var bestGoalPlayer = goals == null || users[goals.Id] == null
                ? null
                : new LeagueInfoPlayerViewModel
                {
                    Id = goals.Id,
                    MediaId = bestPlayerTeam != null ? goalsPlayerTeam.MediaId : null,
                    Name = users[goals.Id].Name
                };

            var helps = _userStatisticCalculation.GetHelpsStatistic(games)
                .OrderByDescending(x => x.Value)
                .FirstOrDefault();
            var helpsPlayerTeam = goals != null
                ? teams.FirstOrDefault(x => x.MemberIds.Contains(helps.Id))
                : null;
            var bestHelpPlayer = helps == null || users[goals.Id] == null
                ? null
                : new LeagueInfoPlayerViewModel
                {
                    Id = helps.Id,
                    MediaId = bestPlayerTeam != null ? helpsPlayerTeam.MediaId : null,
                    Name = users[helps.Id].Name
                };

            var futureEvents = events
                .Where(x => x.StartDate > DateTime.UtcNow)
                .OrderBy(x => x.StartDate)
                .Take(1)
                .Select(x => MapEvent(x, teams))
                .ToList();

            var gamesToPlay = events
                .Where(x => x.StartDate < DateTime.UtcNow.Date)
                .OrderBy(x => x.StartDate)
                .Select(x => Map(x, games, teams))
                .Where(x => x != null && x.Teams.Any())
                .ToList();

            return new LeagueInfoViewModel
            {
                Name = league.Name,
                SubName = league.SubName,
                Description = league.Description,
                MediaId = league.MediaId,
                BestPlayer = bestPlayer,
                BestGoalPlayer = bestGoalPlayer,
                BestHelpPlayer = bestHelpPlayer,
                Teams = teamsStatistic,
                Events = futureEvents,
                GamesToPlay = gamesToPlay
            };
        }

        private LeagueInfoEventViewModel MapEvent(EventDb eventDb, IEnumerable<TeamDb> teams)
        {
            var dictionary = teams.ToDictionary(x => x.EntityId, x => x);
            var teamValues = eventDb
                .Games
                .Where(x => dictionary.ContainsKey(x.HomeTeamId) && !dictionary[x.HomeTeamId].Hidden &&
                    dictionary.ContainsKey(x.GuestTeamId) && !dictionary[x.GuestTeamId].Hidden)
                .Select(x => dictionary[x.HomeTeamId].Name + " - " + dictionary[x.GuestTeamId].Name);

            return new LeagueInfoEventViewModel
            {
                Name = eventDb.Name,
                Date = eventDb.StartDate,
                Teams = teamValues
            };
        }
    }
}
