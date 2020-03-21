using System.Collections.Generic;
using System.Linq;
using Interfaces.Games.BuisnessLogic.Models;
using Interfaces.Games.DataAccess.Model;
using Interfaces.Teams.DataAccess;
using Interfaces.Users.DataAccess;

namespace Implementations.Games
{
    public class GamesMapper
    {
        public GameTeamViewModel MapTeamInfo(TeamDb team, Dictionary<string, UserDb> users)
        {
            return new GameTeamViewModel
            {
                Id = team.EntityId,
                Members = team
                    .MemberIds
                    .Where(users.ContainsKey)
                    .Select(x => new GameMemberViewModel
                    {
                        Id = x,
                        Name = users[x].Name,
                    })
            };
        }

        public GameInfoViewModel MapGameInfo(GameDb game, IEnumerable<TeamDb> teams, Dictionary<string, string> users)
        {
            var result = new GameInfoViewModel();

            var homeTeam = teams.FirstOrDefault(x => x.EntityId == game.HomeTeam.Id);
            if (homeTeam != null)
            {
                result.HomeTeamName = homeTeam.Name;
                result.HomeTeamMediaId = homeTeam.MediaId;
                result.HomeTeamScore = game.HomeTeam.Score;
                if (!string.IsNullOrEmpty(game.HomeTeam.BestMemberId))
                    result.HomeTeamBestPlayer = users[game.HomeTeam.BestMemberId];
                result.HomeTeamScores = game
                    .HomeTeam.Members
                    .Where(x => x.Help != 0 || x.Score != 0)
                    .Select(x => MapGameInfoMember(x, users));
            }

            var guestTeam = teams.FirstOrDefault(x => x.EntityId == game.GuestTeam.Id);
            if (guestTeam != null)
            {
                result.GuestTeamName = guestTeam.Name;
                result.GuestTeamMediaId = guestTeam.MediaId;
                result.GuestTeamScore = game.GuestTeam.Score;
                if (!string.IsNullOrEmpty(game.GuestTeam.BestMemberId))
                    result.GuestTeamBestPlayer = users[game.GuestTeam.BestMemberId];
                result.GuestTeamScores = game
                    .GuestTeam.Members
                    .Where(x => x.Help != 0 || x.Score != 0)
                    .Select(x => MapGameInfoMember(x, users));
            }

            return result;
        }

        private GameInfoMemberScoreViewModel MapGameInfoMember(GameMemberDb gameMember, Dictionary<string, string> users)
        {
            return new GameInfoMemberScoreViewModel
            {
                Name = users[gameMember.Id],
                Help = gameMember.Help,
                Score = gameMember.Score
            };
        }
    }
}
