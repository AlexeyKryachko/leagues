using System;
using System.Collections.Generic;
using System.Linq;
using Implementations.Core;
using Interfaces.Events.DataAccess.Model;
using Interfaces.Games.DataAccess.Model;
using Interfaces.Leagues.DataAccess.Model;
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

        public TournamentInfoViewModel MapTournametInfo(LeagueDb league, IList<EventDb> events, IList<TeamDb> teams, IList<GameDb> games)
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
                Name = league.Name,
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

            return eventEntity.Games.All(x => !string.IsNullOrEmpty(x.GuestTeamId) && !string.IsNullOrEmpty(x.HomeTeamId))
                ? MapTournamentGroupGames(eventEntity, teamsDictionary, games)
                : MapTournamentPlayoffGames(eventEntity, teams, games);
        }

        private TournamentInfoGroupViewModel MapTournamentGroupGames(EventDb eventEntity, IDictionary<string, TeamDb> teams, IList<GameDb> games)
        {
            var eventGames = games.Where(x => x.EventId == eventEntity.EntityId).ToList();
            if (!eventGames.Any())
                return null;
            
            return new TournamentInfoGroupViewModel
            {
                Name = eventEntity.Name,
                PlayOffGames = eventGames.Select(x => MapPlayoffGame(x, teams))
            };
        }

        private TournamentInfoPlayOffGameViewModel MapPlayoffGame(GameDb game, IDictionary<string, TeamDb> teams)
        {
            var home = teams[game.HomeTeam.Id];
            var guest = teams[game.GuestTeam.Id];

            return new TournamentInfoPlayOffGameViewModel()
            {
                HomeTeamMediaId = home.MediaId,
                HomeTeamScore = game.HomeTeam.Score,
                HomeTeamName = home.Name,
                GuestTeamMediaId = guest.MediaId,
                GuestTeamScore = game.GuestTeam.Score,
                GuestTeamName = guest.Name
            };
        }

        public TournamentInfoGroupViewModel MapTournamentPlayoffGames(EventDb eventEntity, IList<TeamDb> teams, IList<GameDb> games)
        {
            var teamIdsInGroup = eventEntity.Games.Select(x => x.HomeTeamId);
            var teamsInGroup = teams.Where(x => teamIdsInGroup.Contains(x.EntityId));

            var eventGames = games.Where(x => x.EventId == eventEntity.EntityId).ToList();
            var teamResults = teamsInGroup
                .Select(team => new TournamentInfoGroupGameViewModel
                {
                    Name = team.Name,
                    Points = _scoreCalculation.Points(eventGames, team.EntityId),
                    Wins = _scoreCalculation.WinsCount(eventGames, team.EntityId),
                    Drafts = _scoreCalculation.DraftCount(eventGames, team.EntityId),
                    Loses = _scoreCalculation.LosesCount(eventGames, team.EntityId),
                })
                .OrderByDescending(x => x.Points);

            return new TournamentInfoGroupViewModel
            {
                Name = eventEntity.Name,
                GroupGames = teamResults
            };
        }
    }
}
