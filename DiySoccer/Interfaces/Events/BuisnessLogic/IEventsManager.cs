using System.Collections.Generic;
using Interfaces.Events.BuisnessLogic.Models;

namespace Interfaces.Events.BuisnessLogic
{
    public interface IEventsManager
    {
        IEnumerable<EventVewModel> GetRange(string leagueId);

        void Create(string leagueId, EventVewModel model);

        void Update(string leagueId, EventVewModel model);

        void Delete(string eventId);
    }
}
