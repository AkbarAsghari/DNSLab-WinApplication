using DNSLabWinApp.Properties;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNSLabWinApp.Utilities
{
    public static class SettingsUtility
    {
        public static void Set(SettingKeys key, string value)
        {
            switch (key)
            {
                case SettingKeys.Token:
                    Settings.Default.Token = value;
                    break;
                case SettingKeys.UserInfo:
                    Settings.Default.UserInfo = value;
                    break;
                case SettingKeys.SelectedHosts:
                    Settings.Default.SelectedHosts = value;
                    break;
                case SettingKeys.LaunchStartUp:
                    Settings.Default.LaunchStartUp = bool.Parse(value);
                    break;
            }
            Properties.Settings.Default.Save();
        }

        public static string Get(SettingKeys key)
        {
            if (Settings.Default.UpgradeRequired)
            {
                Settings.Default.Upgrade();
                Settings.Default.UpgradeRequired = false;
                Settings.Default.Save();
            }

            switch (key)
            {
                case SettingKeys.Token:
                    return Settings.Default.Token;
                case SettingKeys.UserInfo:
                    return Settings.Default.UserInfo;
                case SettingKeys.SelectedHosts:
                    return Settings.Default.SelectedHosts;
                case SettingKeys.LaunchStartUp:
                    return Settings.Default.LaunchStartUp.ToString();
            }
            return String.Empty;
        }
    }

    public enum SettingKeys
    {
        Token = 0,
        UserInfo = 1,
        SelectedHosts = 2,
        LaunchStartUp = 3,
    }
}
