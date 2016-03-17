﻿using System.Collections.Generic;

namespace Interfaces.Core.DataAccess
{
    public interface IBaseRepository<T> where T: IBaseEntity
    {
        void Add(string leagueId, T entity);

        void AddRange(string leagueId, IEnumerable<T> entities);

        T Get(string id);

        IEnumerable<T> GetRange(IEnumerable<string> ids);

        void Delete(string id);
    }
}
