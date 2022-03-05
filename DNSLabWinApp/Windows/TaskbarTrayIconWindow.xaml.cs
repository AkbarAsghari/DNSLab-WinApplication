using dnslabwin;
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
    /// Interaction logic for TaskbarTrayIconWindow.xaml
    /// </summary>
    public partial class TaskbarTrayIconWindow : Window
    {
        private System.Windows.Forms.NotifyIcon notifyIcon = null;

        private readonly MainWindow mainWindow;
        public TaskbarTrayIconWindow(MainWindow main)
        {
            mainWindow = main;
            InitializeComponent();

            notifyIcon = new System.Windows.Forms.NotifyIcon();
            notifyIcon.Icon = new System.Drawing.Icon(AppDomain.CurrentDomain.BaseDirectory + "/Assets/Images/icon.ico");
            notifyIcon.DoubleClick += new EventHandler(notifyIcon_DoubleClick);

            // Initialize contextMenu1
            var contextMenu = new System.Windows.Forms.ContextMenu();

            var refreshMenuItem = new System.Windows.Forms.MenuItem();
            refreshMenuItem.Text = "&Refresh Now";
            refreshMenuItem.Click += MenuRefresh_Click;

            var exitMenuItem = new System.Windows.Forms.MenuItem();
            exitMenuItem.Text = "E&xit";
            exitMenuItem.Click += MenuExit_Click;



            contextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[]
            {
                refreshMenuItem,
                exitMenuItem,
            });

            notifyIcon.Text = "DNSLab Dynamic DNS";
            notifyIcon.ContextMenu = contextMenu;
        }

        private void MenuRefresh_Click(object sender, EventArgs e)
        {
            mainWindow.bntRefreshNow_Click(null, null);
        }

        private void MenuExit_Click(object sender, EventArgs e)
        {
            notifyIcon.Visible = false;
            Application.Current.Shutdown();
        }

        private void notifyIcon_DoubleClick(object sender, EventArgs e)
        {
            mainWindow.Show();
            mainWindow.Focus();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            notifyIcon.Visible = true;
        }
    }
}
