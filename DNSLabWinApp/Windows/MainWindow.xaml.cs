using DNSLabWinApp.DTOs.User;
using DNSLabWinApp.Extensions;
using dnslabwin.Repository;
using DNSLabWinApp.Utilities;
using dnslabwin.Windows;
using Newtonsoft.Json;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using DNSLabWinApp.Repository;
using DNSLabWinApp.Enums;
using AutoUpdaterDotNET;
using DNSLabWinApp.Windows;

namespace dnslabwin
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private TimeSpan RemainTime;
        private IPRepository _IPRepository;
        private DNSRepository _DNSRepository;
        private readonly TaskbarTrayIconWindow NotifyIcon;
        public MainWindow()
        {
            NotifyIcon = new TaskbarTrayIconWindow(this);

            if (bool.Parse(SettingsUtility.Get(SettingKeys.LaunchStartUp)))
            {
                NotifyIcon.Show();
                this.Hide();
            }

            InitilizeDataAsync().GetAwaiter();


            RemainTime = new TimeSpan(0, 5, 0);

            DispatcherTimer checkForUpdateHostsIPTimer = new DispatcherTimer();
            checkForUpdateHostsIPTimer.Interval = RemainTime;
            checkForUpdateHostsIPTimer.Tick += CheckForUpdateHostsIPTimer_Tick;
            checkForUpdateHostsIPTimer.Start();

            DispatcherTimer updateRemainTimer = new DispatcherTimer();
            updateRemainTimer.Interval = TimeSpan.FromSeconds(1);
            updateRemainTimer.Tick += UpdateRemainTimer_Tick;
            updateRemainTimer.Start();
        }

        private void UpdateRemainTimer_Tick(object sender, EventArgs e)
        {
            RemainTime = RemainTime.Add(new TimeSpan(0, 0, -1));

            txbNextChangeTime.Text = $"{RemainTime.Minutes} min {RemainTime.Seconds} sec";
        }

        private async void CheckForUpdateHostsIPTimer_Tick(object sender, EventArgs e)
        {
            RemainTime = new TimeSpan(0, 5, 0);
            await UpdateIPAddress();
            await UpdateDNSIPAddress();
        }

        private async Task UpdateIPAddress()
        {
            bntRefreshNow.IsEnabled = false;

            var ip = await _IPRepository.GetIP();
            txblockStatusBar.Text = $"{DateTime.Now.ToString("hh:mm tt")}: Remote IP Found: { ip.iPv4 }";
            txbIPAddress.Text = ip.iPv4;
            imgIPInfo.SetAlert(AlertEnum.Success);

            bntRefreshNow.IsEnabled = true;
        }

        private async Task UpdateDNSIPAddress()
        {
            btnEditHost.IsEnabled = false;

            IEnumerable<Guid> selectedHosts = new List<Guid>();
            string strSelectedHosts = SettingsUtility.Get(SettingKeys.SelectedHosts);
            if (!String.IsNullOrEmpty(strSelectedHosts))
                selectedHosts = JsonConvert.DeserializeObject<IEnumerable<Guid>>(strSelectedHosts).ToList();

            if (selectedHosts.Count() == 0)
            {
                imgUpdateInfo.SetAlert(AlertEnum.Warning);
                txbUpdateMessage.Text = $"{selectedHosts.Count()} Hosts selected";
            }
            else
            {
                if (await _DNSRepository.UpdateDNSIPAddress(selectedHosts))
                {
                    imgUpdateInfo.SetAlert(AlertEnum.Success);
                    txbUpdateMessage.Text = $"{selectedHosts.Count()} Hosts selected for dynamic update";
                }
                else
                {
                    imgUpdateInfo.SetAlert(AlertEnum.Danger);
                    txbUpdateMessage.Text = $"{selectedHosts.Count()} Hosts does't update";
                }
            }

            btnEditHost.IsEnabled = true;
        }

        private void LoadAccountInfo()
        {
            try
            {
                string userInfoJson = SettingsUtility.Get(SettingKeys.UserInfo);
                if (!String.IsNullOrEmpty(userInfoJson))
                {
                    var userInfo = JsonConvert.DeserializeObject<UserInfo>(userInfoJson);
                    if (userInfo != null)
                    {
                        txbUserName.Text = userInfo.Username;
                        txbEmail.Text = userInfo.Email;
                        imgAccountInfo.SetAlert(AlertEnum.Success);
                    }
                }
                else
                {
                    imgAccountInfo.SetAlert(AlertEnum.Warning);
                }
            }
            catch
            {
                imgAccountInfo.SetAlert(AlertEnum.Danger);
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

        private async void btnEditHost_Click(object sender, RoutedEventArgs e)
        {
            ((Button)sender).IsEnabled = false;

            new HostsWindow().ShowDialog();

            await UpdateDNSIPAddress();

            ((Button)sender).IsEnabled = true;
        }


        public async void bntRefreshNow_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button)
                ((Button)sender).IsEnabled = false;

            await UpdateIPAddress();
            await UpdateDNSIPAddress();

            if (sender is Button)
                ((Button)sender).IsEnabled = true;
        }

        public async Task InitilizeDataAsync()
        {
            _IPRepository = new IPRepository();
            _DNSRepository = new DNSRepository();

            if (String.IsNullOrEmpty(SettingsUtility.Get(SettingKeys.Token)))
            {
                this.Hide();
                new AuthWindow(this).Show();
            }
            else
            {
                InitializeComponent();
                LoadAccountInfo();
                await UpdateDNSIPAddress();
            }
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            AutoUpdater.Start("http://api.dnslab.ir/updates/win/check");

            await UpdateIPAddress();
            await UpdateDNSIPAddress();


        }

        private void SettingsMenu_Click(object sender, RoutedEventArgs e)
        {
            new SettingsWindow().ShowDialog();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            NotifyIcon.Show();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            NotifyIcon.Show();
            e.Cancel = true;
            this.Hide();
        }

        public void BringToForeground()
        {
            if (this.WindowState == WindowState.Minimized || this.Visibility == Visibility.Hidden)
            {
                this.Show();
                this.WindowState = WindowState.Normal;
            }

            // According to some sources these steps gurantee that an app will be brought to foreground.
            this.Activate();
            this.Topmost = true;
            this.Topmost = false;
            this.Focus();
        }
    }
}
