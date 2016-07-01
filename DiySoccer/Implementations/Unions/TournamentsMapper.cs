using System;
using System.Collections.Generic;
using System.Linq;
using Implementations.Core;
using Interfaces.Events.DataAccess.Model;
using Interfaces.Games.DataAccess.Model;
using Interfaces.Teams.DataAccess;
using Interfaces.Unions.BuisnessLogic.Models.Tournaments;

namespace Implementations.Unions
{
    public class TournamentsMapper
    {
        private class TournamentsDateComparator : IComparable<TournamentsDateComparator>
        {
            public int Year { get; private set; }
            public int Month { get; private set; }
            public int Day { get; private set; }
            public int Hour { get; private set; }
            public int Minute { get; private set; }

            public TournamentsDateComparator(DateTime dateTime)
            {
                Year = dateTime.Year;
                Month = dateTime.Month;
                Day = dateTime.Day;
                Hour = dateTime.Hour;
                Minute = dateTime.Minute;
            }

            public int CompareTo(TournamentsDateComparator other)
            {
                if (Year == other.Year)
                {
                    return 0;
                }

                return 1;
            }
        }

        private readonly ScoreCalculation _scoreCalculation;

        public TournamentsMapper(ScoreCalculation scoreCalculation)
        {
            _scoreCalculation = scoreCalculation;
        }

        public TournamentInfoViewModel MapTournametInfo(IList<EventDb> events, IList<TeamDb> teams, IList<GameDb> games)
        {
            var grouppedEvents = events
                .Where(x => x.StartDate.HasValue)
                .GroupBy(x => x.StartDate.Value.AddSeconds(-x.StartDate.Value.Second).AddMilliseconds(-x.StartDate.Value.Millisecond))
                .OrderByDescending(x => x.Key);

            var groups = grouppedEvents
                .Select(x => MapTournamentGroups(x, teams, games))
                .Where(x => x != null)
                .ToList();

            return new TournamentInfoViewModel
            {
                Events = groups
            };
        }

        private List<TournamentInfoGroupViewModel> MapTournamentGroups(IGrouping<DateTime, EventDb> events, IList<TeamDb> teams, IList<GameDb> games)
        {
            return events
                .Select(eventDb => MapTournamentGroup(eventDb, teams, games))
                .Where(x => x != null)
                .ToList();
        }

        private TournamentInfoGroupViewModel MapTournamentGroup(EventDb eventEntity, IList<TeamDb> teams, IList<GameDb> games)
        {
            var teamsDictionary = teams.ToDictionary(x => x.EntityId, x => x);

            return eventEntity.Games.Count() == 1
                ? MapTournamentGroupGame(eventEntity, teamsDictionary, games)
                : MapTournamentGroupGames(eventEntity, teams, games);
        }

        private TournamentInfoGroupViewModel MapTournamentGroupGame(EventDb eventEntity, IDictionary<string, TeamDb> teams, IList<GameDb> games)
        {
            var game = games.FirstOrDefault(x => x.EventId == eventEntity.EntityId);
            if (game == null)
                return null;

            if (teams[game.HomeTeam.Id] == null || teams[game.GuestTeam.Id] == null)
                return null;

            return new TournamentInfoGroupViewModel
            {
                Name = eventEntity.Name,
                Teams = new List<TournamentInfoGameViewModel>
                {
                    new TournamentInfoGameViewModel
                    {
                        Name = teams[game.HomeTeam.Id].Name,
                        Value = game.HomeTeam.Score
                    },
                    new TournamentInfoGameViewModel
                    {
                        Name = teams[game.GuestTeam.Id].Name,
                        Value = game.GuestTeam.Score
                    }
                }
            };
        }

        public TournamentInfoGroupViewModel MapTournamentGroupGames(EventDb eventEntity, IList<TeamDb> teams, IList<GameDb> games)
        {
            var teamIdsInGroup = eventEntity.Games.Select(x => x.HomeTeamId);
            var teamsInGroup = teams.Where(x => teamIdsInGroup.Contains(x.EntityId));

            var eventGames = games.Where(x => x.EventId == eventEntity.EntityId).ToList();
            var teamResults = teamsInGroup
                .Select(team => new TournamentInfoGameViewModel
                {
                    Name = team.Name,
                    Value = _scoreCalculation.Points(eventGames, team.EntityId)
                })
                .OrderByDescending(x => x.Value);

            return new TournamentInfoGroupViewModel
            {
                Name = eventEntity.Name,
                Teams = teamResults
            };
        }
    }
}
