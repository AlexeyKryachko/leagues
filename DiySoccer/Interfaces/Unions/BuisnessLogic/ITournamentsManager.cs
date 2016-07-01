using Interfaces.Unions.BuisnessLogic.Models.Tournaments;

namespace Interfaces.Unions.BuisnessLogic
{
    public interface ITournamentsManager
    {
        TournamentInfoViewModel GetTournamentInfo(string tournamentId);
    }
}
