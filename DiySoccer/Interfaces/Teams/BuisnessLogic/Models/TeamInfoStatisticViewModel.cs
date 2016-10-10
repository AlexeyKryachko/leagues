using Interfaces.Leagues.BuisnessLogic.Model;
using Interfaces.Teams.DataAccess;
using Newtonsoft.Json;

namespace Interfaces.Teams.BuisnessLogic.Models
{
    public class TeamInfoStatisticViewModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string LeagueName { get; set; }

        [JsonProperty("gamesCount")]
        public int GamesCount { get; set; }

        [JsonProperty("wins")]
        public int Wins { get; set; }

        [JsonProperty("loses")]
        public int Loses { get; set; }

        [JsonProperty("draw")]
        public int Draws { get; set; }

        [JsonProperty("scores")]
        public int Scores { get; set; }

        [JsonProperty("missed")]
        public int Missed { get; set; }

        [JsonProperty("points")]
        public int Points { get; set; }

        public TeamInfoStatisticViewModel(TeamStatisticsViewModel model, TeamDb team)
        {
            this.LeagueName = team.Name;

            this.Id = model.Id;
            this.GamesCount = model.GamesCount;
            this.Wins = model.Wins;
            this.Loses = model.Loses;
            this.Draws = model.Draws;
            this.Scores = model.Scores;
            this.Missed = model.Missed;
            this.Points = model.Points;
        }
    }
}
