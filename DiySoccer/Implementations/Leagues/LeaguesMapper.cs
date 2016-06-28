using System;
using System.Collections.Generic;
using System.Linq;
using Implementations.Core;
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

        public LeaguesMapper(ScoreCalculation scoreCalculation, UserStatisticCalculation userStatisticCalculation)
        {
            _scoreCalculation = scoreCalculation;
            _userStatisticCalculation = userStatisticCalculation;
        }

        public LeagueInfoViewModel MapInfo(LeagueDb league, IEnumerable<TeamDb> teams, IEnumerable<GameDb> games, IEnumerable<EventDb> events, Dictionary<string, UserDb> users)
        {
            var teamsStatistic = teams
                .Where(x => !x.Hidden)
                .Select(x => new LeagueInfoTeamViewModel
                {
                    Name = x.Name,
                    Games = _scoreCalculation.GameCount(games, x.EntityId),
                    Points = _scoreCalculation.Points(games, x.EntityId)
                });

            var best = _userStatisticCalculation.GetBestStatistic(games)
                .OrderByDescending(x => x.Value)
                .FirstOrDefault();
            var bestPlayer = best == null || users[best.Id] == null
                ? null
                : new IdNameViewModel
                {
                    Id = best.Id,
                    Name = users[best.Id].Name
                };

            var goals = _userStatisticCalculation.GetGoalsStatistic(games)
                .OrderByDescending(x => x.Value)
                .FirstOrDefault();
            var bestGoalPlayer = goals == null || users[goals.Id] == null
                ? null
                : new IdNameViewModel
                {
                    Id = goals.Id,
                    Name = users[goals.Id].Name
                };

            var helps = _userStatisticCalculation.GetHelpsStatistic(games)
                .OrderByDescending(x => x.Value)
                .FirstOrDefault();
            var bestHelpPlayer = helps == null || users[goals.Id] == null
                ? null
                : new IdNameViewModel
                {
                    Id = helps.Id,
                    Name = users[helps.Id].Name
                };

            var futureEvents = events
                .Where(x => x.StartDate > DateTime.UtcNow)
                .OrderBy(x => x.StartDate)
                .Take(3)
                .Select(x => new LeagueInfoEventViewModel
                {
                    Name = x.Name,
                    Date = x.StartDate
                });

            var pastEvents = events
                .Where(x => x.StartDate <= DateTime.UtcNow)
                .OrderByDescending(x => x.StartDate)
                .Take(3)
                .Select(x => new LeagueInfoEventViewModel
                {
                    Name = x.Name,
                    Date = x.StartDate
                });

            return new LeagueInfoViewModel
            {
                Name = league.Name,
                Description = league.Description,
                MediaId = league.MediaId,
                BestPlayer = bestPlayer,
                BestGoalPlayer = bestGoalPlayer,
                BestHelpPlayer = bestHelpPlayer,
                Teams = teamsStatistic,
                FutureEvents = futureEvents,
                PastEvents = pastEvents
            };
        }
    }
}
