using DNSLab.DTOs.User;
using dnslabwin.Extensions;
using dnslabwin.Repository;
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
using System.Windows.Threading;

namespace dnslabwin
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private TimeSpan RemainTime;
        private readonly IPRepository _IPRepository;
        private readonly DNSRepository _DNSRepository;
        public MainWindow()
        {
            InitilizeData();

            _IPRepository = new IPRepository();
            _DNSRepository = new DNSRepository();

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

        private void UpdateRemainTimer_Tick(object? sender, EventArgs e)
        {
            RemainTime = RemainTime.Add(new TimeSpan(0, 0, -1));

            txbNextChangeTime.Text = $"{RemainTime.Minutes} min {RemainTime.Seconds} sec";
        }

        private async void CheckForUpdateHostsIPTimer_Tick(object? sender, EventArgs e)
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
            imgIPInfo.SetAlert(Enums.AlertEnum.Success);

            bntRefreshNow.IsEnabled = true;
        }

        private async Task UpdateDNSIPAddress()
        {
            btnEditHost.IsEnabled = false;

            IEnumerable<Guid> selectedHosts = new List<Guid>();
            string strSelectedHosts = SettingsUtility.Get(SettingKeys.SelectedHosts);
            if (!String.IsNullOrEmpty(strSelectedHosts))
                selectedHosts = JsonSerializer.Deserialize<IEnumerable<Guid>>(strSelectedHosts)!.ToList();

            if (selectedHosts.Count() == 0)
            {
                imgUpdateInfo.SetAlert(Enums.AlertEnum.Warning);
                txbUpdateMessage.Text = $"{selectedHosts.Count()} Hosts selected";
            }
            else
            {
                if (await _DNSRepository.UpdateDNSIPAddress(selectedHosts))
                {
                    imgUpdateInfo.SetAlert(Enums.AlertEnum.Success);
                    txbUpdateMessage.Text = $"{selectedHosts.Count()} Hosts selected for dynamic update";
                }
                else
                {
                    imgUpdateInfo.SetAlert(Enums.AlertEnum.Danger);
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

        private async void btnEditHost_Click(object sender, RoutedEventArgs e)
        {
            ((Button)sender).IsEnabled = false;

            new HostsWindow().ShowDialog();

            await UpdateDNSIPAddress();

            ((Button)sender).IsEnabled = true;
        }


        private async void bntRefreshNow_Click(object sender, RoutedEventArgs e)
        {
            ((Button)sender).IsEnabled = false;
            await UpdateIPAddress();
            await UpdateDNSIPAddress();
            ((Button)sender).IsEnabled = true;
        }

        public void InitilizeData()
        {
            if (String.IsNullOrEmpty(SettingsUtility.Get(SettingKeys.Token)))
            {
                this.Hide();
                new AuthWindow(this).Show();
            }
            else
            {
                InitializeComponent();
                LoadAccountInfo();
            }
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await UpdateIPAddress();
            await UpdateDNSIPAddress();
        }
    }
}
