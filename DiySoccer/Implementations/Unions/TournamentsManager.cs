using System.Linq;
using Interfaces.Events.DataAccess;
using Interfaces.Games.DataAccess;
using Interfaces.Leagues.DataAccess;
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
        private readonly ILeaguesRepository _leaguesRepository;

        private readonly TournamentsMapper _tournamentsMapper;

        public TournamentsManager(ITeamsRepository teamsRepository, IGamesRepository gamesRepository, IEventsRepository eventsRepository, TournamentsMapper tournamentsMapper, ILeaguesRepository leaguesRepository)
        {
            _teamsRepository = teamsRepository;
            _gamesRepository = gamesRepository;
            _eventsRepository = eventsRepository;
            _tournamentsMapper = tournamentsMapper;
            _leaguesRepository = leaguesRepository;
        }

        public TournamentInfoViewModel GetTournamentInfo(string leagueId)
        {
            var league = _leaguesRepository.Get(leagueId);
            var teams = _teamsRepository.GetByLeague(leagueId).ToList();
            var games = _gamesRepository.GetByLeague(leagueId).ToList();
            var events = _eventsRepository.GetByLeague(leagueId).ToList();

            return _tournamentsMapper.MapTournametInfo(league, events, teams, games);
        }
    }
}
