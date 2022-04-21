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
        private readonly RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
        private Assembly curAssembly = Assembly.GetExecutingAssembly();
        public SettingsWindow()
        {
            InitializeComponent();
            UpdateSettingsWithCurrentStartupStatus();
            LoadStartUpCheckBoxStatus();
            LoadLanguageCheckBoxStatus();
        }

        private void LoadLanguageCheckBoxStatus()
        {
            cmbLanguage.SelectedValue = SettingsUtility.Get(SettingKeys.SelectedCulture);
        }

        private void UpdateSettingsWithCurrentStartupStatus()
        {
            if (key.GetValue(curAssembly.GetName().Name) != null)
                SettingsUtility.Set(SettingKeys.LaunchStartUp, "true");
        }

        private void LoadStartUpCheckBoxStatus()
        {
            chkLaunchStartUp.IsChecked = bool.Parse(SettingsUtility.Get(SettingKeys.LaunchStartUp));
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (chkLaunchStartUp.IsChecked == true)
            {
                SettingsUtility.Set(SettingKeys.LaunchStartUp, "true");
                key.SetValue(curAssembly.GetName().Name, curAssembly.Location);
            }
            else
            {
                SettingsUtility.Set(SettingKeys.LaunchStartUp, "false");
                key.DeleteValue(curAssembly.GetName().Name, false);
            }

            if (cmbLanguage.SelectedIndex != -1)
            {
                if (cmbLanguage.SelectedValue.ToString() != SettingsUtility.Get(SettingKeys.SelectedCulture))
                {
                    SettingsUtility.Set(SettingKeys.SelectedCulture, cmbLanguage.SelectedValue.ToString());
                    App.SelectCulture(cmbLanguage.SelectedValue.ToString());
                    MessageBox.Show(FindResource("LanguageAfterChangeMessage").ToString());
                }
            }

            this.Close();
        }
    }
}
