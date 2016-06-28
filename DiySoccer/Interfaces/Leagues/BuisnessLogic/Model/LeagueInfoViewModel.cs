using System.Collections.Generic;
using System.Linq;
using Interfaces.Core;

namespace Interfaces.Leagues.BuisnessLogic.Model
{
    public class LeagueInfoViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string MediaId { get; set; }
        public IEnumerable<LeagueInfoTeamViewModel> Teams { get; set; }
        public IEnumerable<LeagueInfoEventViewModel> FutureEvents { get; set; }
        public IEnumerable<LeagueInfoEventViewModel> PastEvents { get; set; }
        public IdNameViewModel BestPlayer { get; set; }
        public IdNameViewModel BestGoalPlayer { get; set; }
        public IdNameViewModel BestHelpPlayer { get; set; }
        public IEnumerable<IdNameViewModel> News { get; set; }

        public LeagueInfoViewModel()
        {
            Teams = Enumerable.Empty<LeagueInfoTeamViewModel>();
            FutureEvents = Enumerable.Empty<LeagueInfoEventViewModel>();
            PastEvents = Enumerable.Empty<LeagueInfoEventViewModel>();
            News = Enumerable.Empty<IdNameViewModel>();
        }
    }
}
