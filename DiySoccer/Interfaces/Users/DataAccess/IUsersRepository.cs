﻿using System.Collections.Generic;
using Interfaces.Core.DataAccess;

namespace Interfaces.Users.DataAccess
{
    public interface IUsersRepository : IBaseRepository<UserDb>
    {
        IEnumerable<UserDb> Find(string leagueId, string query, int page, int pageSize);
    }
}
