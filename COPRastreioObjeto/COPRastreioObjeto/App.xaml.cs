using Business;
using COPRastreioObjeto.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using Windows.UI.ApplicationSettings;
using Callisto.Controls;
using Windows.UI;
using Entities;
using System.Diagnostics.Tracing;
using Entities.LOG;

// The Grid App template is documented at http://go.microsoft.com/fwlink/?LinkId=234226

namespace COPRastreioObjeto
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        private Color _background = Color.FromArgb(255, 0, 77, 96);

        /// <summary>
        /// Initializes the singleton Application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used when the application is launched to open a specific file, to display
        /// search results, and so forth.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override async void OnLaunched(LaunchActivatedEventArgs args)
        {
            //progress bar atualizar inicial.
            try
            {
                if (args.PreviousExecutionState != ApplicationExecutionState.Running)
                {
                    bool loadState = (args.PreviousExecutionState == ApplicationExecutionState.Terminated);
                    ExtendedSplash extendedSplash = new ExtendedSplash(args.SplashScreen, loadState);
                    Window.Current.Content = extendedSplash;
                }

                Window.Current.Activate();

                //Verificar onde atualizar pela primeira vez.
                Preferencias preferencias = await Core.GetSPreferencias();//Verfica se existe preferencia registra, caso nao cria uma padrão.
                var sucesso = await Core.AtualizacaoAutomatica(preferencias.TempoAtualizacao);//Core.AtualizarObjetosDisco();

                if (sucesso != null && sucesso.Objetos != null)
                {

                }

                var sucessoHistorico = await Core.GravarObjetoHistorico();

                if (sucessoHistorico)
                {

                }
            }
            catch (Exception ex)
            {
                //Entities.LOG.MetroEventSource.Log.Error(ex.Message + "#" + ex.InnerException + "APP.cs" + "#" + "OnLaunched");                
            }

            // Register handler for CommandsRequested events from the settings pane 
            SettingsPane.GetForCurrentView().CommandsRequested += OnCommandsRequested;
            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active

            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();
                //Associate the frame with a SuspensionManager key                                
                SuspensionManager.RegisterFrame(rootFrame, "AppFrame");

                if (args.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    // Restore the saved session state only when appropriate
                    try
                    {
                        await SuspensionManager.RestoreAsync();
                    }
                    catch (SuspensionManagerException)
                    {
                        //Something went wrong restoring state.
                        //Assume there is no state and continue
                    }
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }
            if (rootFrame.Content == null)
            {
                // First time execution, initialize the logger
                try
                {
                    EventListener verboseListener = new StorageFileEventListener("LOG_MyListenerVerbose");
                    EventListener informationListener = new StorageFileEventListener("LOG_MyListenerInformation");

                    verboseListener.EnableEvents(MetroEventSource.Log, EventLevel.Verbose);
                    informationListener.EnableEvents(MetroEventSource.Log, EventLevel.Informational);
                }
                catch (Exception ex)
                {

                }

                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                if (!rootFrame.Navigate(typeof(GroupedItemsPage), "AllGroups"))
                {
                    throw new Exception("Failed to create initial page");
                }
            }
            // Ensure the current window is active
            Window.Current.Activate();
        }

        /// <summary>
        /// criar telas de configuracao
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void OnCommandsRequested(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs args)
        {
            // Add a Preferences command 
            var preferences = new SettingsCommand("preferences", "Preferências", (handler) =>
            {
                var settings = new SettingsFlyout();
                settings.Content = new PreferencesUserControl();
                settings.HeaderBrush = new SolidColorBrush(_background);
                settings.Background = new SolidColorBrush(_background);
                settings.HeaderText = "Preferências";
                settings.IsOpen = true;
            });
            args.Request.ApplicationCommands.Add(preferences);

            // Add an politica command     
            var politica = new SettingsCommand("politica", "Política de Privacidade", (handler) =>
            {
                var settings = new SettingsFlyout();
                settings.Content = new PoliticaPrivacidade();
                settings.HeaderBrush = new SolidColorBrush(_background);
                settings.Background = new SolidColorBrush(_background);
                settings.HeaderText = "Política de Privacidade";
                settings.IsOpen = true;
            });
            args.Request.ApplicationCommands.Add(politica);

            // Add an About command     
            var about = new SettingsCommand("about", "Sobre", (handler) =>
            {
                var settings = new SettingsFlyout();
                settings.Content = new AboutUserControl();
                settings.HeaderBrush = new SolidColorBrush(_background);
                settings.Background = new SolidColorBrush(_background);
                settings.HeaderText = "Sobre";
                settings.IsOpen = true;
            });
            args.Request.ApplicationCommands.Add(about);
        }



        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private async void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            await SuspensionManager.SaveAsync();
            deferral.Complete();
        }
    }
}
