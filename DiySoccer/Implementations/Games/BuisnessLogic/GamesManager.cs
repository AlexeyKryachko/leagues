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
        private readonly IPlayersRepository _playersRepository;

        public GamesManager(IGamesRepository gamesRepository, IPlayersRepository playersRepository)
        {
            _gamesRepository = gamesRepository;
            _playersRepository = playersRepository;
        }

        public GameVewModel Get(string leagueId, string gameId)
        {
            var game = _gamesRepository.Get(gameId);
            var userIds = game.HomeTeam.Members.Select(x => x.Id).Concat(game.GuestTeam.Members.Select(x => x.Id));
            var users = _playersRepository.GetRange(userIds).ToDictionary(x => x.EntityId, y => y.Name);

            return new GameVewModel
            {
                Id = game.EntityId,
                CustomScores = game.CustomScores,
                HomeTeam = new GameTeamViewModel
                {
                    Id = game.HomeTeam.Id,
                    Score = game.HomeTeam.Score,
                    BestId = game.HomeTeam.BestMemberId,
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
                    BestId = game.GuestTeam.BestMemberId,
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
            var guestMemberScores = model.GuestTeam.Members.Select(x => new GameMemberDb
            {
                Id = x.Id,
                Help = x.Help,
                Score = x.Score
            });
            
            var homeMemberScores = model.HomeTeam.Members.Select(x => new GameMemberDb
            {
                Id = x.Id,
                Help = x.Help,
                Score = x.Score
            });

            _gamesRepository.Update(leagueId, gameId, model.CustomScores, model.GuestTeam.Score, model.GuestTeam.BestId, guestMemberScores,
                model.HomeTeam.Score, model.HomeTeam.BestId, homeMemberScores);
        }

        public void Delete(string gameId)
        {
            _gamesRepository.Delete(gameId);
        }
    }
}
