using Interfaces.Settings.BuisnessLogic;

namespace Interfaces.Authenticate.BuisnessLogic
{
    public interface IAuthenticateManager
    {
        SettingsViewModel GetSettings();

        bool IsMember(string leagueId);

        bool IsEditor(string leagueId);

        bool IsAdmin();
    }
}
