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
                    Properties.Settings.Default.Token = value;
                    break;
                case SettingKeys.UserInfo:
                    Properties.Settings.Default.UserInfo = value;
                    break;
                case SettingKeys.SelectedHosts:
                    Properties.Settings.Default.SelectedHosts = value;
                    break;
                case SettingKeys.LaunchStartUp:
                    Properties.Settings.Default.LaunchStartUp = bool.Parse(value);
                    break;
            }
            Properties.Settings.Default.Save();
        }

        public static string Get(SettingKeys key)
        {
            switch (key)
            {
                case SettingKeys.Token:
                    return Properties.Settings.Default.Token;
                case SettingKeys.UserInfo:
                    return Properties.Settings.Default.UserInfo;
                case SettingKeys.SelectedHosts:
                    return Properties.Settings.Default.SelectedHosts;
                case SettingKeys.LaunchStartUp:
                    return Properties.Settings.Default.LaunchStartUp.ToString();
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
