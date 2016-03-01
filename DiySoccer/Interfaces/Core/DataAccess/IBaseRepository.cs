using System.Collections.Generic;

namespace Interfaces.Core.DataAccess
{
    public interface IBaseRepository<T> where T: IBaseEntity
    {
        void Add(T entity);

        void AddRange(IEnumerable<T> entities);

        T Get(string id);

        IEnumerable<T> GetRange(IEnumerable<string> ids);
    }
}
