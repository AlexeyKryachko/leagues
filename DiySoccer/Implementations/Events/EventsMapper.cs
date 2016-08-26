using System.Linq;
using Interfaces.Events.BuisnessLogic.Models;
using Interfaces.Events.DataAccess.Model;

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

        public EventGameVewModel Map(EventGameDb eventGameEntity)
        {
            return new EventGameVewModel
            {
                Id = eventGameEntity.Id,
                GuestTeamId = eventGameEntity.GuestTeamId,
                HomeTeamId = eventGameEntity.HomeTeamId,
            };
        }

        public EventVewModel Map(EventDb entity)
        {
            var games = entity.Games.Select(Map);

            return new EventVewModel
            {
                Id = entity.EntityId,
                Minor = entity.Minor,
                Group = entity.Group,
                Name = entity.Name,
                StartDate = entity.StartDate,
                EndDate = entity.EndDate,
                Games = games
            };
        }
    }
}
