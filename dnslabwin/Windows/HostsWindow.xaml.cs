using dnslabwin.DTOs;
using dnslabwin.Repository;
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

            if (hostsSummary != null)
            {
                return hostsSummary.Select(x => new HostNamesAndCheckedDTO
                {
                    HostName = x.Address
                }).ToList();
            }
            return null;
        }
    }
}
