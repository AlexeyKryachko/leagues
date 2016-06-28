using System.Collections.Generic;
using System.Linq;
using Interfaces.Core;
using Interfaces.Games.DataAccess.Model;

namespace Implementations.Core
{
    public class UserStatisticCalculation
    {
        public IEnumerable<UserStatistic> GetBestStatistic(IEnumerable<GameDb> games)
        {
            return Enumerable.Concat(
                games.Where(x => !string.IsNullOrEmpty(x.HomeTeam.BestMemberId)).Select(x => x.HomeTeam.BestMemberId),
                games.Where(x => !string.IsNullOrEmpty(x.GuestTeam.BestMemberId)).Select(x => x.GuestTeam.BestMemberId))
                .GroupBy(x => x)
                .Select(x => new UserStatistic
                {
                    Id = x.Key,
                    Value = x.Count()
                });
        }

        public IEnumerable<UserStatistic> GetGoalsStatistic(IEnumerable<GameDb> games)
        {
            return Enumerable.Concat(
                games.SelectMany(x => x.HomeTeam.Members),
                games.SelectMany(x => x.GuestTeam.Members))
                .GroupBy(x => x.Id)
                .Select(x => new UserStatistic
                {
                    Id = x.Key,
                    Value = x.Sum(y => y.Score)
                });
        }

        public IEnumerable<UserStatistic> GetHelpsStatistic(IEnumerable<GameDb> games)
        {
            return Enumerable.Concat(
                games.SelectMany(x => x.HomeTeam.Members),
                games.SelectMany(x => x.GuestTeam.Members))
                .GroupBy(x => x.Id)
                .Select(x => new UserStatistic
                {
                    Id = x.Key,
                    Value = x.Sum(y => y.Help)
                });
        }
    }
}
