using System;
using System.Collections.Generic;
using System.Linq;
using Implementations.GameApproval.DataAccess;
using Interfaces.Events.DataAccess;
using Interfaces.GameApproval.BuisnessLogic;
using Interfaces.GameApproval.BuisnessLogic.Model;
using Interfaces.GameApproval.DataAccess;
using Interfaces.GameApproval.DataAccess.Model;
using Interfaces.Games.BuisnessLogic.Models;
using Interfaces.Games.DataAccess;
using Interfaces.Teams.DataAccess;
using Interfaces.Users.DataAccess;

namespace Implementations.GameApproval.BuisnessLogic
{
    public class GameApprovalManager : IGameApprovalManager
    {
        private readonly IEventsRepository _eventsRepository;
        private readonly ITeamsRepository _teamsRepository;
        private readonly IPlayersRepository _playersRepository;
        private readonly IGamesRepository _gamesRepository;
        private readonly IGameApprovalRepository _gameApprovalRepository;

        public GameApprovalManager(
            IEventsRepository eventsRepository, 
            ITeamsRepository teamsRepository, 
            IPlayersRepository playersRepository, 
            IGamesRepository gamesRepository,
            IGameApprovalRepository gameApprovalRepository)
        {
            _eventsRepository = eventsRepository;
            _teamsRepository = teamsRepository;
            _playersRepository = playersRepository;
            _gamesRepository = gamesRepository;
            _gameApprovalRepository = gameApprovalRepository;
        }

        public ApprovalsViewModel GetApprovals(string leagueId)
        {
            var approvalsDb = _gameApprovalRepository
                .GetByLeagueId(leagueId)
                .ToList();

            var events = _eventsRepository.GetByLeague(leagueId).ToList();
            var teams = _teamsRepository.GetByLeague(leagueId).ToList();
            var players = _playersRepository
                .Find(leagueId, null, 0, Int32.MaxValue)
                .ToDictionary(x => x.EntityId, y => y.Name);
            
            var model = new ApprovalsViewModel();

            foreach (var approvalDb in approvalsDb)
            {
                var approvalEvent = events.FirstOrDefault(x => x.EntityId == approvalDb.EventId);
                if (approvalEvent == null)
                    continue;

                var game = approvalEvent.Games.FirstOrDefault(x => x.HomeTeamId == approvalDb.HomeTeam.Id && x.GuestTeamId == approvalDb.GuestTeam.Id);
                if (game == null)
                    continue;

                var homeTeam = teams.FirstOrDefault(x => x.EntityId == game.HomeTeamId);
                if (homeTeam == null)
                    continue;

                var guestTeam = teams.FirstOrDefault(x => x.EntityId == game.GuestTeamId);
                if (guestTeam == null)
                    continue;

                var approval = new ApprovalViewModel
                {
                    Id = approvalDb.EntityId,
                    EventTitle = approvalEvent.Name,
                    HomeTeamTitle = homeTeam.Name,
                    HomeTeamScore = approvalDb.HomeTeam.Score,
                    GuestPlayers = approvalDb
                        .GuestTeam
                        .Members
                        .Select(x => new PlayerApprovalViewModel
                        {
                            Name = players[x.Id],
                            Score = x.Score,
                            Help = x.Help,
                            Best = x.Id == approvalDb.GuestTeam.BestMemberId
                        }),
                    GuestTeamTitle = guestTeam.Name,
                    GuestTeamScore = approvalDb.HomeTeam.Score,
                    HomePlayers = approvalDb
                        .HomeTeam
                        .Members
                        .Select(x => new PlayerApprovalViewModel
                        {
                            Name = players[x.Id],
                            Score = x.Score,
                            Help = x.Help,
                            Best = x.Id == approvalDb.HomeTeam.BestMemberId
                        })
                };

                model.Approvals.Add(approval);
            }

            return model;
        }

        public void Create(string leagueId, GameExternalViewModel model)
        {
            if (model.Events == null || !model.Events.Any())
                return;

            var entities = MapToEntity(model).ToList();
            if (!entities.Any())
                return;

            _gameApprovalRepository.AddRange(leagueId, entities);
        }

        public void Approve(string leagueId, string id)
        {
            var approveDb = _gameApprovalRepository.Get(id);
            if (approveDb == null)
                return;

            var existed = _gamesRepository.GetByTeams(leagueId, approveDb.HomeTeam.Id, approveDb.GuestTeam.Id);
            if (existed == null)
            {
                var game = MapToGame(approveDb);

                _gamesRepository.Create(leagueId, game);
            }

            Delete(id);
        }

        private GameVewModel MapToGame(GameApprovalDb approveDb)
        {
            return new GameVewModel
            {
                EventId = approveDb.EventId,
                HomeTeam = new GameTeamViewModel
                {
                    Id = approveDb.HomeTeam.Id,
                    BestId = approveDb.HomeTeam.BestMemberId,
                    Score = approveDb.HomeTeam.Members.Sum(x => x.Score),
                    Members = approveDb
                        .HomeTeam
                        .Members
                        .Select(x => new GameMemberViewModel
                        {
                            Id = x.Id,
                            Score = x.Score,
                            Help = x.Help
                        })
                },
                GuestTeam = new GameTeamViewModel
                {
                    Id = approveDb.GuestTeam.Id,
                    BestId = approveDb.GuestTeam.BestMemberId,
                    Score = approveDb.GuestTeam.Members.Sum(x => x.Score),
                    Members = approveDb
                        .GuestTeam
                        .Members
                        .Select(x => new GameMemberViewModel
                        {
                            Id = x.Id,
                            Score = x.Score,
                            Help = x.Help
                        })
                }
            };
        }

        public void Delete(string id)
        {
            new GameApprovalRepository().Delete(id);
        }

        private IEnumerable<GameApprovalDb> MapToEntity(GameExternalViewModel model)
        {
            foreach (var eventModel in model.Events)
            {
                if (!eventModel.Selected)
                    continue;
                
                var activeGames = eventModel.Games.Where(x => x.Selected).ToList();
                foreach (var activeGame in activeGames)
                {
                    if (activeGame.HomeTeam.Members == null 
                        || !activeGame.HomeTeam.Members.Any() 
                        || activeGame.GuestTeam.Members == null 
                        || !activeGame.GuestTeam.Members.Any())
                        continue;

                    var entity = new GameApprovalDb
                    {
                        EventId = eventModel.Id,
                        HomeTeam = new TeamGameApprovalDb
                        {
                            Id = activeGame.HomeTeam.Id,
                            BestMemberId = activeGame.HomeTeam.BestId,
                            Score = activeGame.HomeTeam.Members.Sum(x => x.Score),
                            Members = activeGame
                                .HomeTeam
                                .Members
                                .Select(x => new MemberGameApprovalDb
                                {
                                    Id = x.Id,
                                    Score = x.Score,
                                    Help = x.Help
                                })
                        },
                        GuestTeam = new TeamGameApprovalDb
                        {
                            Id = activeGame.GuestTeam.Id,
                            BestMemberId = activeGame.GuestTeam.BestId,
                            Score = activeGame.GuestTeam.Members.Sum(x => x.Score),
                            Members = activeGame
                                .GuestTeam
                                .Members
                                .Select(x => new MemberGameApprovalDb
                                {
                                    Id = x.Id,
                                    Score = x.Score,
                                    Help = x.Help
                                })
                        }


                    };

                    yield return entity;
                }
            }
        }
    }
}
