using System.Collections.Generic;
using System.Linq;
using Implementations.Core.DataAccess;
using Interfaces.Games.BuisnessLogic.Models;
using Interfaces.Games.DataAccess;
using Interfaces.Games.DataAccess.Model;
using MongoDB.Driver;

namespace Implementations.Games.DataAccess
{
    public class GamesRepository : BaseRepository<GameDb>, IGamesRepository
    {
        protected override string CollectionName => "games";

        public IEnumerable<GameDb> GetByLeague(string leagueId)
        {
            return Collection.AsQueryable().Where(x => x.LeagueId == leagueId);
        }

        public IEnumerable<GameDb> GetByTeam(string leagueId, string teamId)
        {
            return Collection.AsQueryable().Where(x => x.GuestTeam.Id == teamId || x.HomeTeam.Id == teamId);
        }

        public void Create(string leagueId, GameVewModel model)
        {
            var entity = new GameDb
            {
                LeagueId = leagueId,
                HomeTeam = new GameTeamDb
                {
                    Id = model.HomeTeam.Id,
                    Score = model.CustomScores 
                        ? model.HomeTeam.Score
                        : model.HomeTeam.Members.Sum(x => x.Score),
                    Members = model.HomeTeam.Members.Select(x => new GameMemberDb
                    {
                        Id = x.Id,
                        Score = x.Score,
                        Help = x.Help
                    })
                },
                GuestTeam = new GameTeamDb
                {
                    Id = model.GuestTeam.Id,
                    Score = model.CustomScores
                        ? model.GuestTeam.Score
                        : model.GuestTeam.Members.Sum(x => x.Score),
                    Members = model.GuestTeam.Members.Select(x => new GameMemberDb
                    {
                        Id = x.Id,
                        Score = x.Score,
                        Help = x.Help
                    })
                },
            };

            Add(leagueId, entity);
        }

        public void Update(string leagueId, string id, int guestScore, IEnumerable<GameMemberDb> guestMembersScores, int homeScore, IEnumerable<GameMemberDb> homeMembersScores)
        {
            var filter = Builders<GameDb>.Filter.Eq(x => x.LeagueId, leagueId) & Builders<GameDb>.Filter.Eq(x => x.EntityId, id);
            var update = Builders<GameDb>.Update
                .Set(x => x.GuestTeam.Score, guestScore)
                .Set(x => x.GuestTeam.Members, guestMembersScores)
                .Set(x => x.HomeTeam.Score, homeScore)
                .Set(x => x.HomeTeam.Members, homeMembersScores);

            Collection.UpdateOne(filter, update);
        }
    }
}
