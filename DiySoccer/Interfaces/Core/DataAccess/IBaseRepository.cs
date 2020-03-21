using System.Collections.Generic;

namespace Interfaces.Core.DataAccess
{
    public interface IBaseRepository<T> where T: IBaseEntity
    {
        T Create(string leagueId);

        void Add(string leagueId, T entity);

        void AddRange(string leagueId, IEnumerable<T> entities);

        T Get(string id);

        IEnumerable<T> GetRange(IEnumerable<string> ids);
        IEnumerable<T> GetByLeagueId(string leagueId);

        void Delete(string id);

        void DeleteByLeagueId(string leagueId);
    }
}
