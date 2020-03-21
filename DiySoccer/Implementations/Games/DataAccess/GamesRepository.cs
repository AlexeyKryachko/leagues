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

        public GameDb GetByTeams(string leagueId, string homeTeamId, string guestTeamId)
        {
            return Collection.AsQueryable().FirstOrDefault(x => x.GuestTeam.Id == guestTeamId || x.HomeTeam.Id == homeTeamId);
        }

        public IEnumerable<GameDb> GetByExceptTeams(string leagueId, IEnumerable<string> teamIds)
        {
            if (teamIds == null)
                teamIds = Enumerable.Empty<string>();

            return Collection.AsQueryable().Where(x => x.LeagueId == leagueId &&
                !teamIds.Contains(x.HomeTeam.Id) &&
                !teamIds.Contains(x.GuestTeam.Id));
        }

        public void Create(string leagueId, GameVewModel model)
        {
            var entity = new GameDb
            {
                LeagueId = leagueId,
                CustomScores = model.CustomScores,
                EventId = model.EventId,
                HomeTeam = new GameTeamDb
                {
                    Id = model.HomeTeam.Id,
                    Score = model.CustomScores 
                        ? model.HomeTeam.Score
                        : model.HomeTeam.Members.Sum(x => x.Score),
                    BestMemberId = model.HomeTeam.BestId,
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
                    BestMemberId = model.GuestTeam.BestId,
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

        public void Update(string leagueId, string id, GameVewModel model)
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

            var filter = Builders<GameDb>.Filter.Eq(x => x.LeagueId, leagueId) & Builders<GameDb>.Filter.Eq(x => x.EntityId, id);

            var guestScore = model.CustomScores
                ? model.GuestTeam.Score
                : guestMemberScores.Sum(x => x.Score);

            var homeScore = model.CustomScores
                ? model.HomeTeam.Score
                : homeMemberScores.Sum(x => x.Score);

            var update = Builders<GameDb>.Update
                .Set(x => x.GuestTeam.Score, guestScore)
                .Set(x => x.GuestTeam.BestMemberId, model.GuestTeam.BestId)
                .Set(x => x.GuestTeam.Members, guestMemberScores)
                .Set(x => x.HomeTeam.Score, homeScore)
                .Set(x => x.HomeTeam.BestMemberId, model.HomeTeam.BestId)
                .Set(x => x.HomeTeam.Members, homeMemberScores)
                .Set(x => x.CustomScores, model.CustomScores)
                .Set(x => x.EventId, model.EventId);

            Collection.UpdateOne(filter, update);
        }
    }
}
