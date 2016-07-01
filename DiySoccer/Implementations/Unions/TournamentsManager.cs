using System.Linq;
using Interfaces.Events.DataAccess;
using Interfaces.Games.DataAccess;
using Interfaces.Teams.DataAccess;
using Interfaces.Unions.BuisnessLogic;
using Interfaces.Unions.BuisnessLogic.Models.Tournaments;

namespace Implementations.Unions
{
    public class TournamentsManager : ITournamentsManager
    {
        private readonly ITeamsRepository _teamsRepository;
        private readonly IGamesRepository _gamesRepository;
        private readonly IEventsRepository _eventsRepository;

        private readonly TournamentsMapper _tournamentsMapper;

        public TournamentsManager(ITeamsRepository teamsRepository, IGamesRepository gamesRepository, IEventsRepository eventsRepository, TournamentsMapper tournamentsMapper)
        {
            _teamsRepository = teamsRepository;
            _gamesRepository = gamesRepository;
            _eventsRepository = eventsRepository;
            _tournamentsMapper = tournamentsMapper;
        }

        public TournamentInfoViewModel GetTournamentInfo(string leagueId)
        {
            var teams = _teamsRepository.GetByLeague(leagueId).ToList();
            var games = _gamesRepository.GetByLeague(leagueId).ToList();
            var events = _eventsRepository.GetByLeague(leagueId).ToList();

            return _tournamentsMapper.MapTournametInfo(events, teams, games);
        }
    }
}
