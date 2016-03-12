using System.Linq;
using Interfaces.Games.BuisnessLogic;
using Interfaces.Games.BuisnessLogic.Models;
using Interfaces.Games.DataAccess;
using Interfaces.Games.DataAccess.Model;
using Interfaces.Users.DataAccess;

namespace Implementations.Games.BuisnessLogic
{
    public class GamesManager : IGamesManager
    {
        private readonly IGamesRepository _gamesRepository;
        private readonly IUsersRepository _usersRepository;

        public GamesManager(IGamesRepository gamesRepository, IUsersRepository usersRepository)
        {
            _gamesRepository = gamesRepository;
            _usersRepository = usersRepository;
        }

        public GameVewModel Get(string leagueId, string gameId)
        {
            var game = _gamesRepository.Get(gameId);
            var userIds = game.HomeTeam.Members.Select(x => x.Id).Concat(game.GuestTeam.Members.Select(x => x.Id));
            var users = _usersRepository.GetRange(userIds).ToDictionary(x => x.EntityId, y => y.Name);

            return new GameVewModel
            {
                Id = game.EntityId,
                HomeTeam = new GameTeamViewModel
                {
                    Id = game.HomeTeam.Id,
                    Score = game.HomeTeam.Score,
                    Members = game.HomeTeam.Members.Select(x => new GameMemberViewModel
                    {
                        Id = x.Id,
                        Name = users[x.Id],
                        Score = x.Score,
                        Help = x.Help
                    })
                },
                GuestTeam = new GameTeamViewModel
                {
                    Id = game.GuestTeam.Id,
                    Score = game.GuestTeam.Score,
                    Members = game.GuestTeam.Members.Select(x => new GameMemberViewModel
                    {
                        Id = x.Id,
                        Name = users[x.Id],
                        Score = x.Score,
                        Help = x.Help
                    })
                }
            };
        }

        public void Create(string leagueId, GameVewModel model)
        {
            _gamesRepository.Create(leagueId, model);
        }

        public void Update(string leagueId, string gameId, GameVewModel model)
        {
            var guestScore = model.CustomScores
                ? model.GuestTeam.Score
                : model.GuestTeam.Members.Sum(x => x.Score);
            var guestMemberScores = model.GuestTeam.Members.Select(x => new GameMemberDb
            {
                Id = x.Id,
                Help = x.Help,
                Score = x.Score
            });

            var homeScore = model.CustomScores
                ? model.HomeTeam.Score
                : model.HomeTeam.Members.Sum(x => x.Score);
            var homeMemberScores = model.HomeTeam.Members.Select(x => new GameMemberDb
            {
                Id = x.Id,
                Help = x.Help,
                Score = x.Score
            });

            _gamesRepository.Update(leagueId, gameId, guestScore, guestMemberScores, homeScore, homeMemberScores);
        }

        public void Delete(string gameId)
        {
            _gamesRepository.Delete(gameId);
        }
    }
}
