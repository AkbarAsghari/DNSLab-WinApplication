using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnslabwin.Utilities
{
    public static class SettingsUtility
    {
        public static void Set(SettingKeys key, string value)
        {
            SettingsProperty prop;

            if (Properties.Settings.Default.Properties[key.ToString()] != null)
            {
                prop = Properties.Settings.Default.Properties[key.ToString()];
            }
            else
            {
                prop = new SettingsProperty(key.ToString());
                prop.PropertyType = typeof(string);
                Properties.Settings.Default.Properties.Add(prop);
                Properties.Settings.Default.Save();
            }
            Properties.Settings.Default.Properties[key.ToString()].DefaultValue = value;
            Properties.Settings.Default.Save();
        }

        public static string Get(SettingKeys key)
        {
            if (Properties.Settings.Default.Properties[key.ToString()] != null)
                return Properties.Settings.Default.Properties[key.ToString()].DefaultValue.ToString();

            return String.Empty;
        }
    }

    public enum SettingKeys
    {
        Token = 0,
    }
}
