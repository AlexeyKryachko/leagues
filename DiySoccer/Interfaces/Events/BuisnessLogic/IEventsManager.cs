using System.Collections.Generic;
using Interfaces.Events.BuisnessLogic.Models;

namespace Interfaces.Events.BuisnessLogic
{
    public interface IEventsManager
    {
        IEnumerable<EventVewModel> GetRange(string leagueId);

        EventVewModel Create(string leagueId);

        EventGameVewModel CreateEventGame(string leagueId, string eventId);

        EventVewModel Update(string leagueId, EventVewModel model);

        void Delete(string eventId);

        void DeleteEventGame(string eventId, int eventGameId);
    }
}
