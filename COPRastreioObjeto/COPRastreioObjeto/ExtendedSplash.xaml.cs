using COPRastreioObjeto.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace COPRastreioObjeto
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ExtendedSplash : Page
    {
        private SplashScreen splash; // Variable to hold the splash screen object.
        internal Rect splashImageRect; // Rect to store splash screen image coordinates.
        internal bool dismissed = false; // Variable to track splash screen dismissal status.

        public ExtendedSplash(SplashScreen splashscreen, bool loadState)
        {
            this.InitializeComponent();
            //var ver = Windows.ApplicationModel.Package.Current.Id.Version;
            //this.txtVersion.Text = ver.Major.ToString() + "." + ver.Minor.ToString() + "." + ver.Build.ToString() + "." + ver.Revision.ToString();
            try
            {
                Window.Current.SizeChanged += new WindowSizeChangedEventHandler(ExtendedSplash_OnResize);

                splash = splashscreen;

                if (splash != null)
                {
                    // Register an event handler to be executed when the splash screen has been dismissed.
                    splash.Dismissed += new TypedEventHandler<SplashScreen, Object>(DismissedEventHandler);

                    // Retrieve the window coordinates of the splash screen image.
                    splashImageRect = splash.ImageLocation;
                    PositionImage();
                }

                // Create a Frame to act as the navigation context 
                //rootFrame = new Frame();

                // Restore the saved session state if necessary
                RestoreStateAsync(loadState);
            }
            catch (Exception)
            {

            }
        }

        async void RestoreStateAsync(bool loadState)
        {
            try
            {
                if (loadState)
                    await SuspensionManager.RestoreAsync();
            }
            catch (Exception)
            {
            }

            // Normally you should start the time consuming task asynchronously here and 
            // dismiss the extended splash screen in the completed handler of that task
            // This sample dismisses extended splash screen  in the handler for "Learn More" button for demonstration
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        void ExtendedSplash_OnResize(Object sender, WindowSizeChangedEventArgs e)
        {
            // Safely update the extended splash screen image coordinates. This function will be fired in response to snapping, unsnapping, rotation, etc...
            if (splash != null)
            {
                // Update the coordinates of the splash screen image.
                splashImageRect = splash.ImageLocation;
                PositionImage();
            }
        }

        void PositionImage()
        {
            try
            {
                extendedSplashImage.SetValue(Canvas.LeftProperty, splashImageRect.X);
                extendedSplashImage.SetValue(Canvas.TopProperty, splashImageRect.Y);
                extendedSplashImage.Height = splashImageRect.Height;
                extendedSplashImage.Width = splashImageRect.Width;
            }
            catch (Exception)
            {

            }
        }

        // Include code to be executed when the system has transitioned from the splash screen to the extended splash screen (application's first view).
        void DismissedEventHandler(SplashScreen sender, object e)
        {
            dismissed = true;

            // Navigate away from the app's extended splash screen after completing setup operations here...
            // This sample navigates away from the extended splash screen when the "Learn More" button is clicked.
        }
    }
}
