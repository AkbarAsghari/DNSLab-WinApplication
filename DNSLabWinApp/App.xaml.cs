using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace DNSLabWinApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        Mutex myMutex;
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            myMutex = new Mutex(true, "DNSLab", out bool aIsNewInstance);
            if (!aIsNewInstance)
                App.Current.Shutdown();
        }

        void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            // Process unhandled exception

            string exceptionMessage = e.Exception.Message.ToLower();
            if (exceptionMessage.Contains("an error occurred while sending the request"))
                MessageBox.Show("unHandler Exception ---> Please check your network");

            // Prevent default unhandled exception processing
            e.Handled = true;

            Application.Current.Shutdown();
        }
    }
}
