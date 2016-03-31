using Newtonsoft.Json;

namespace Interfaces.Settings.BuisnessLogic
{
    public class SettingsViewModel
    {
        [JsonProperty("permissions")]
        public PermissionsViewModel Permissions { get; set; }

        public SettingsViewModel()
        {
            Permissions = new PermissionsViewModel();
        }
    }
}
