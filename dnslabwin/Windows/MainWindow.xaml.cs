using DNSLab.DTOs.User;
using dnslabwin.Extensions;
using dnslabwin.Utilities;
using dnslabwin.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace dnslabwin
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoadAccountInfo()
        {
            try
            {
                string userInfoJson = SettingsUtility.Get(SettingKeys.UserInfo);
                if (!String.IsNullOrEmpty(userInfoJson))
                {
                    var userInfo = JsonSerializer.Deserialize<UserInfo>(userInfoJson);
                    txbUserName.Text = userInfo!.Username;
                    txbEmail.Text = userInfo!.Email;
                    imgAccountInfo.SetAlert(Enums.AlertEnum.Success);
                }
                else
                {
                    imgAccountInfo.SetAlert(Enums.AlertEnum.Warning);
                }
            }
            catch
            {
                imgAccountInfo.SetAlert(Enums.AlertEnum.Danger);
            }
        }

        private void ExitMenu_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnEditAccount_Click(object sender, RoutedEventArgs e)
        {
            ((Button)sender).IsEnabled = false;
            new AuthWindow(this).ShowDialog();
            ((Button)sender).IsEnabled = true;
        }

        private void btnEditHost_Click(object sender, RoutedEventArgs e)
        {
            ((Button)sender).IsEnabled = false;

            new HostsWindow().ShowDialog();

            imgUpdateInfo.SetAlert(Enums.AlertEnum.Warning);
            ((Button)sender).IsEnabled = true;
        }

        private void bntRefreshNow_Click(object sender, RoutedEventArgs e)
        {
            ((Button)sender).IsEnabled = false;

            imgIPInfo.SetAlert(Enums.AlertEnum.Success);
            ((Button)sender).IsEnabled = true;
        }

        public void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(SettingsUtility.Get(SettingKeys.Token)))
            {
                this.Hide();
                new AuthWindow(this).Show();
            }
            else
            {
                LoadAccountInfo();
                txblockStatusBar.Text = $"{DateTime.Now.ToString("hh:mm tt")}: Remote IP Found: { "84.241.47.110" }";
            }
        }
    }
}
