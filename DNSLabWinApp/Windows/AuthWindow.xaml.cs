using DNSLabWinApp.DTOs.User;
using DNSLabWinApp.Repository;
using DNSLabWinApp.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace dnslabwin.Windows
{
    /// <summary>
    /// Interaction logic for AuthWindow.xaml
    /// </summary>
    public partial class AuthWindow : Window
    {
        private readonly MainWindow _Main;
        public AuthWindow(MainWindow mainWindow)
        {
            InitializeComponent();
            _Main = mainWindow;
        }

        private void link_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                UseShellExecute = true,
                FileName = e.Uri.AbsoluteUri
            });
            e.Handled = true;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(SettingsUtility.Get(SettingKeys.Token)))
                Application.Current.Shutdown();
        }

        private async void btnSignIn_Click(object sender, RoutedEventArgs e)
        {
            btnSignIn.IsEnabled = false;

            var repo = new AccountRepository();


            var token = await repo.Login(new AuthenticateDTO
            {
                Username = txtUserName.Text,
                Password = txtPassword.Password
            });

            if (!String.IsNullOrEmpty(token))
            {
                SettingsUtility.Set(SettingKeys.Token, token);
                SettingsUtility.Set(SettingKeys.SelectedHosts, String.Empty);

                repo = new AccountRepository();

                var userInfo = await repo.Get();
                if (userInfo != null)
                {
                    SettingsUtility.Set(SettingKeys.UserInfo, JsonConvert.SerializeObject(userInfo));
                    this.Close();
                    _Main.Show();
                    _Main.InitilizeDataAsync();
                }
            }

            btnSignIn.IsEnabled = true;
        }
    }
}
