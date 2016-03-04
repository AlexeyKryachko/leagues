using System.Linq;
using Implementations.Core.DataAccess;
using Interfaces.Games.BuisnessLogic.Models;
using Interfaces.Games.DataAccess;
using Interfaces.Games.DataAccess.Model;

namespace Implementations.Games.DataAccess
{
    public class GamesRepository : BaseRepository<GameDb>, IGamesRepository
    {
        protected override string CollectionName => "games";
        
        public void Create(GameVewModel model)
        {
            var entity = new GameDb
            {
                LeagueId = model.LeagueId,
                HomeTeam = new GameTeamDb
                {
                    Id = model.HomeTeam.Id,
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
                    Members = model.GuestTeam.Members.Select(x => new GameMemberDb
                    {
                        Id = x.Id,
                        Score = x.Score,
                        Help = x.Help
                    })
                },
            };

            Collection.InsertOne(entity);
        }
    }
}
