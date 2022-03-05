using DNSLabWinApp.Utilities;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DNSLabWinApp.Windows
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();

            LoadStartUpCheckBoxStatus();
        }

        private void LoadStartUpCheckBoxStatus()
        {
            chkLaunchStartUp.IsChecked = bool.Parse(SettingsUtility.Get(SettingKeys.LaunchStartUp));
        }

        private void chkRunInStartUp_Checked(object sender, RoutedEventArgs e)
        {
            SettingsUtility.Set(SettingKeys.LaunchStartUp, "true");

            RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            Assembly curAssembly = Assembly.GetExecutingAssembly();
            key.SetValue(curAssembly.GetName().Name, curAssembly.Location);
        }

        private void chkRunInStartUp_Unchecked(object sender, RoutedEventArgs e)
        {
            SettingsUtility.Set(SettingKeys.LaunchStartUp, "false");

            RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            Assembly curAssembly = Assembly.GetExecutingAssembly();
            key.DeleteValue(curAssembly.GetName().Name, false);
        }
    }
}
