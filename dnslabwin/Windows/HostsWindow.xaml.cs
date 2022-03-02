using dnslabwin.DTOs;
using dnslabwin.Repository;
using dnslabwin.Utilities;
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
using System.Windows.Shapes;

namespace dnslabwin.Windows
{
    /// <summary>
    /// Interaction logic for HostsWindow.xaml
    /// </summary>
    public partial class HostsWindow : Window
    {
        public HostsWindow()
        {
            InitializeComponent();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            HostListView.ItemsSource = await LoadHostsAsync();
            HostListView.Visibility = Visibility.Visible;
        }

        private async Task<List<HostNamesAndCheckedDTO>> LoadHostsAsync()
        {
            var hostsSummary = await new DNSRepository().GetOwnHostsSummary();

            IEnumerable<Guid> selectedHosts = new List<Guid>();
            string strSelectedHosts = SettingsUtility.Get(SettingKeys.SelectedHosts);
            if (!String.IsNullOrEmpty(strSelectedHosts))
                selectedHosts = JsonSerializer.Deserialize<IEnumerable<Guid>>(strSelectedHosts)!.ToList();

            selectedHosts = selectedHosts.Where(x => hostsSummary.Select(h => h.Id).Contains(x));
            if (selectedHosts.Count() == 0)
                SettingsUtility.Set(SettingKeys.SelectedHosts, String.Empty);
            else
                SettingsUtility.Set(SettingKeys.SelectedHosts, JsonSerializer.Serialize(selectedHosts));

            if (hostsSummary != null)
            {
                return hostsSummary.Select(x => new HostNamesAndCheckedDTO
                {
                    Id = x.Id,
                    IsChecked = selectedHosts.Any(s => s == x.Id),
                    HostName = x.Address
                }).ToList();
            }
            return null;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (!((IEnumerable<HostNamesAndCheckedDTO>)HostListView.ItemsSource).Any(x => x.IsChecked == true))
            {
                SettingsUtility.Set(SettingKeys.SelectedHosts, String.Empty);
                this.Close();
            }

            var selectedHostsId = ((IEnumerable<HostNamesAndCheckedDTO>)HostListView.ItemsSource)
                                        .Where(x => x.IsChecked == true)
                                        .Select(x => x.Id);
            SettingsUtility.Set(SettingKeys.SelectedHosts, JsonSerializer.Serialize(selectedHostsId));

            this.Close();
        }
    }
}
