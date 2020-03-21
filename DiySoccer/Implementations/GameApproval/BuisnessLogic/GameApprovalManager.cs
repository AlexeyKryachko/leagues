using System.Collections.Generic;
using System.Linq;
using Implementations.GameApproval.DataAccess;
using Interfaces.GameApproval.BuisnessLogic;
using Interfaces.GameApproval.DataAccess;
using Interfaces.GameApproval.DataAccess.Model;
using Interfaces.Games.BuisnessLogic.Models;

namespace Implementations.GameApproval.BuisnessLogic
{
    public class GameApprovalManager : IGameApprovalManager
    {
        public void Create(string leagueId, GameExternalViewModel model)
        {
            if (model.Events == null || !model.Events.Any())
                return;

            var entities = MapToEntity(model).ToList();
            if (!entities.Any())
                return;

            new GameApprovalRepository().AddRange(leagueId, entities);
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
