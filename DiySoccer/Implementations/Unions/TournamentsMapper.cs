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
        private readonly ScoreCalculation _scoreCalculation;

        public TournamentsMapper(ScoreCalculation scoreCalculation)
        {
            _scoreCalculation = scoreCalculation;
        }

        public TournamentInfoViewModel MapTournametInfo(LeagueDb league, IList<EventDb> events, IList<TeamDb> teams, IList<GameDb> games)
        {
            var groups = events
                .OrderByDescending(x => x.StartDate)
                .Select(x => MapTournamentGroup(x, teams, games))
                .Where(x => x != null)
                .ToList();

            return new TournamentInfoViewModel
            {
                Name = league.Name,
                SubName = league.SubName,
                Description = league.Description,
                MediaId = league.MediaId,
				Information = league.Information,
                Events = groups
            };
        }

        private TournamentInfoGroupViewModel MapTournamentGroup(EventDb eventEntity, IList<TeamDb> teams, IList<GameDb> games)
        {
            var teamsDictionary = teams.ToDictionary(x => x.EntityId, x => x);

            if (eventEntity.Games.All(x => !string.IsNullOrEmpty(x.GuestTeamId) && !string.IsNullOrEmpty(x.HomeTeamId)))
            {
                return MapTournamentPlayoffGames(eventEntity, teamsDictionary, games);
            }
            else
            {
                var model = MapTournamentGroupGames(eventEntity, teams, games);

                model.Games = games
                    .Where(x => x.EventId == eventEntity.EntityId)
                    .Select(x => MapPlayoffGame(x, teamsDictionary));

                return model;
            }
        }

        private TournamentInfoGroupViewModel MapTournamentPlayoffGames(EventDb eventEntity, IDictionary<string, TeamDb> teams, IList<GameDb> games)
        {
            var eventGames = games.Where(x => x.EventId == eventEntity.EntityId).ToList();
            if (!eventGames.Any())
                return null;
            
            return new TournamentInfoGroupViewModel
            {
                Name = eventEntity.Name,
                Minor = eventEntity.Minor,
                PlayOffGames = eventGames
                    .Select(x => MapPlayoffGame(x, teams))
            };
        }

        private TournamentInfoPlayOffGameViewModel MapPlayoffGame(GameDb game, IDictionary<string, TeamDb> teams)
        {
            return new TournamentInfoPlayOffGameViewModel
            {
                GameId = game.EntityId,
                GuestTeamMediaId = teams[game.GuestTeam.Id].MediaId,
                GuestTeamName = teams[game.GuestTeam.Id].Name,
                GuestTeamScore = game.GuestTeam.Score,
                HomeTeamMediaId = teams[game.HomeTeam.Id].MediaId,
                HomeTeamName = teams[game.HomeTeam.Id].Name,
                HomeTeamScore = game.HomeTeam.Score
            };
        }

        public TournamentInfoGroupViewModel MapTournamentGroupGames(EventDb eventEntity, IList<TeamDb> teams, IList<GameDb> games)
        {
            var teamIdsInGroup = eventEntity.Games.Select(x => x.HomeTeamId);
            var teamsInGroup = teams.Where(x => teamIdsInGroup.Contains(x.EntityId));

            var eventGames = games.Where(x => x.EventId == eventEntity.EntityId).ToList();
            var teamResults = teamsInGroup
                .Select(team => new TournamentInfoGroupGameViewModel
                {
                    Name = team.Name,
                    MediaId = team.MediaId,
                    Points = _scoreCalculation.Points(eventGames, team.EntityId),
                    Wins = _scoreCalculation.WinsCount(eventGames, team.EntityId),
                    Drafts = _scoreCalculation.DraftCount(eventGames, team.EntityId),
                    Loses = _scoreCalculation.LosesCount(eventGames, team.EntityId),
                    Goals = _scoreCalculation.GoalsCount(eventGames, team.EntityId),
                    Missed = _scoreCalculation.MissedCount(eventGames, team.EntityId)
                })
                .OrderByDescending(x => x.Points)
                .ThenByDescending(x => x.Goals)
                .ThenByDescending(x => x.Missed);

            return new TournamentInfoGroupViewModel
            {
                Name = eventEntity.Name,
                GroupGames = teamResults
            };
        }
    }
}
