using System.Collections.Generic;
using System.Linq;
using Interfaces.Events.BuisnessLogic.Models;
using Interfaces.Events.DataAccess.Model;
using Interfaces.Games.DataAccess.Model;

namespace Implementations.Events
{
    public class EventsMapper
    {
        public EventGameDb Map(EventGameVewModel eventGameModel)
        {
            return new EventGameDb
            {
                GuestTeamId = eventGameModel.GuestTeamId,
                HomeTeamId = eventGameModel.HomeTeamId,
                Id = eventGameModel.Id
            };
        }

        public EventGameVewModel Map(EventGameDb eventGameEntity, IList<GameDb> games)
        {
            var game = games.FirstOrDefault(x => x.HomeTeam.Id == eventGameEntity.HomeTeamId && x.GuestTeam.Id == eventGameEntity.GuestTeamId);

            return new EventGameVewModel
            {
                Id = eventGameEntity.Id,
                GuestTeamId = eventGameEntity.GuestTeamId,
                GuestTeamScore = game == null ? 0 : game.GuestTeam.Score,
                HomeTeamId = eventGameEntity.HomeTeamId,
                HomeTeamScore = game == null ? 0 : game.HomeTeam.Score,
                GameId = game != null 
                    ? game.EntityId
                    : null
            };
        }

        public EventVewModel Map(EventDb entity, IList<GameDb> games)
        {
            var eventGames = entity.Games.Select(x => Map(x, games));

            return new EventVewModel
            {
                Id = entity.EntityId,
                Minor = entity.Minor,
                Group = entity.Group,
                Name = entity.Name,
                StartDate = entity.StartDate,
                EndDate = entity.EndDate,
                Games = eventGames
            };
        }
    }
}
