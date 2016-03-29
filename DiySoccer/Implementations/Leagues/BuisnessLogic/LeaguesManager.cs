using System.Collections.Generic;
using System.Linq;
using Interfaces.Leagues.BuisnessLogic;
using Interfaces.Leagues.BuisnessLogic.Model;
using Interfaces.Leagues.DataAccess;

namespace Implementations.Leagues.BuisnessLogic
{
    public class LeaguesManager : ILeaguesManager
    {
        private readonly ILeaguesRepository _leaguesRepository;

        public LeaguesManager(ILeaguesRepository leaguesRepository)
        {
            _leaguesRepository = leaguesRepository;
        }

        public IEnumerable<LeagueViewModel> GetAll()
        {
            return _leaguesRepository.GetAll().Select(x => new LeagueViewModel
            {
                Id = x.EntityId,
                Name = x.Name,
                Description = x.Description
            });
        }

        public LeagueViewModel Get(string leagueId)
        {
            var league = _leaguesRepository.Get(leagueId);
            return league == null
                ? null
                : new LeagueViewModel
                {
                    Id = league.EntityId,
                    Name = league.Name,
                    Description = league.Description
                };
        }

        public void Create(LeagueViewModel model)
        {
            _leaguesRepository.Create(model.Name, model.Description);
        }

        public void Update(LeagueViewModel model)
        {
            _leaguesRepository.Update(model.Id, model.Name, model.Description);
        }

        public void Delete(string leagueId)
        {
            throw new System.NotImplementedException();
        }
    }
}
