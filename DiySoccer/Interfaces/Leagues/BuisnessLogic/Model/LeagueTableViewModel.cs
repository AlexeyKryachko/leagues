using System.Collections.Generic;
using System.Linq;
using Interfaces.Teams.BuisnessLogic.Models;
using Newtonsoft.Json;

namespace Interfaces.Leagues.BuisnessLogic.Model
{
    public class LeagueTableViewModel
    {
        [JsonProperty("teamStats")]
        public IEnumerable<TeamStatisticsViewModel> TeamStatistics { get; set; }

        public LeagueTableViewModel()
        {
            TeamStatistics = Enumerable.Empty<TeamStatisticsViewModel>();
            
        }
    }
}
