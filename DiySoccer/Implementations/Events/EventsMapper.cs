using System.Collections.Generic;
using System.Linq;
using Interfaces.Events.BuisnessLogic.Models;
using Interfaces.Events.DataAccess.Model;
using Interfaces.Teams.DataAccess;

namespace Implementations.Events
{
    public class EventsMapper
    {
        public EventGameDb Map(EventGameVewModel eventGameModel)
        {
            return new EventGameDb
            {
                GuestTeamId = eventGameModel.GuestTeamId,
                HomeTeamId = eventGameModel.HomeTeamId
            };
        }

        public EventGameVewModel Map(EventGameDb eventGameEntity, Dictionary<string, TeamDb> teams)
        {
            return new EventGameVewModel
            {
                GuestTeamId = eventGameEntity.GuestTeamId,
                GuestTeamName = teams[eventGameEntity.GuestTeamId].Name,
                HomeTeamId = eventGameEntity.HomeTeamId,
                HomeTeamName = teams[eventGameEntity.HomeTeamId].Name
            };
        }

        public EventVewModel Map(EventDb entity, Dictionary<string, TeamDb> teams)
        {
            var games = entity.Games.Select(x => Map(x, teams));

            return new EventVewModel
            {
                Id = entity.EntityId,
                Name = entity.Name,
                StartDate = entity.StartDate,
                EndDate = entity.EndDate,
                Games = games
            };
        }
    }
}
