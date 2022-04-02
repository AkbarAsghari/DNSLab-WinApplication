using dnslabwin;
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
        /// <summary>The event mutex name.</summary>
        private const string UniqueEventName = "DNSLabEvent";

        /// <summary>The unique mutex name.</summary>
        private const string UniqueMutexName = "DNSLab";

        /// <summary>The event wait handle.</summary>
        private EventWaitHandle eventWaitHandle;

        /// <summary>The mutex.</summary>
        private Mutex mutex;

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            bool isOwned;
            this.mutex = new Mutex(true, "DNSLab", out isOwned);
            this.eventWaitHandle = new EventWaitHandle(false, EventResetMode.AutoReset, UniqueEventName);

            // So, R# would not give a warning that this variable is not used.
            GC.KeepAlive(this.mutex);

            if (isOwned)
            {
                StartupUri = new Uri("Windows/MainWindow.xaml", UriKind.Relative);

                // Spawn a thread which will be waiting for our event
                var thread = new Thread(
                    () =>
                    {
                        while (this.eventWaitHandle.WaitOne())
                        {
                            Current.Dispatcher.BeginInvoke(
                                (Action)(() => ((MainWindow)Current.MainWindow).BringToForeground()));
                        }
                    });

                // It is important mark it as background otherwise it will prevent app from exiting.
                thread.IsBackground = true;

                thread.Start();
                return;
            }

            // Notify other instance so it could bring itself to foreground.
            this.eventWaitHandle.Set();

            // Terminate this instance.
            this.Shutdown();
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
