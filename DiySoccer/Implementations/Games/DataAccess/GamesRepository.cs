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

            Add(entity);
        }

        public IEnumerable<GameDb> GetByLeague(string leagueId)
        {
            return Collection.AsQueryable().Where(x => x.LeagueId == leagueId);
        }
    }
}
