using System.Collections.Generic;
using Interfaces.Core.DataAccess;
using Interfaces.Events.BuisnessLogic.Models;
using Interfaces.Events.DataAccess.Model;

namespace Interfaces.Events.DataAccess
{
    public interface IEventsRepository : IBaseRepository<EventDb>
    {
        IEnumerable<EventDb> GetByLeague(string leagueId);
        
        void Update(string leagueId, EventVewModel model);

        void Update(EventDb entity);
    }
}
