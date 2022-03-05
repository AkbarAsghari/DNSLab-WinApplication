using DNSLabWinApp.Utilities;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
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
            //chkRunInStartUp.IsChecked = bool.Parse(SettingsUtility.Get(SettingKeys.LunchStartUp));
        }

        private void chkRunInStartUp_Checked(object sender, RoutedEventArgs e)
        {
           SettingsUtility.Set(SettingKeys.LunchStartUp,"true");

            var path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location); ;
            RegistryKey key = Registry.CurrentUser.OpenSubKey(path, true);
            key.SetValue("DNSLab", path);
        }

        private void chkRunInStartUp_Unchecked(object sender, RoutedEventArgs e)
        {
            SettingsUtility.Set(SettingKeys.LunchStartUp, "false");

            var path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location); 
            RegistryKey key = Registry.CurrentUser.OpenSubKey(path, true);
            key.DeleteValue("DNSLab", false);
        }
    }
}
