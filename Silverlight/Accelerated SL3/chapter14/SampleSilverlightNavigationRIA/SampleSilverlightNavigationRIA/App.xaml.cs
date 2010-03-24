using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace SampleSilverlightNavigationRIA
{
    public partial class App : Application
    {

        public App()
        {
            this.Startup += this.Application_Startup;
            this.UnhandledException += this.Application_UnhandledException;
            this.ExecutionStateChanged += new EventHandler(App_ExecutionStateChanged);
            InitializeComponent();
        }

        void App_ExecutionStateChanged(object sender, EventArgs e)
        {
            if (App.Current.ExecutionState == ExecutionStates.DetachedUpdatesAvailable)
            {
                MessageBox.Show("An updated version of the application is available. Now the application will be closed. Please relaunch application to get the latest version.");
                //close application and relaunch automatically
            }            
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            this.RootVisual = new MainPage();
        }

        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            // If the app is running outside of the debugger then report the exception using
            // the browser's exception mechanism. On IE this will display it a yellow alert 
            // icon in the status bar and Firefox will display a script error.
            if (!System.Diagnostics.Debugger.IsAttached)
            {

                // NOTE: This will allow the application to continue running after an exception has been thrown
                // but not handled. 
                // For production applications this error handling should be replaced with something that will 
                // report the error to the website and stop the application.
                e.Handled = true;
                ChildWindow ErrorWin = new ErrorWindow(e.ExceptionObject);
                ErrorWin.Show();
            }
        }
    }
}
