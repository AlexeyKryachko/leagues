using Interfaces.Settings.BuisnessLogic;

namespace Interfaces.Authenticate.BuisnessLogic
{
    public interface IAuthenticateManager
    {
        SettingsViewModel GetSettings();

        bool IsEditor(string leagueId);

        bool IsAdmin();
    }
}
