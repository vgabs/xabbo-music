using System.Threading.Tasks;
using System.Windows;
using System;

using Xabbo.GEarth;

namespace xabbo_music.Misc
{
    public class GEarthApplicationHandler
    {
        public Application Application { get; }
        public Window Window => Application.MainWindow;
        public GEarthExtension Extension { get; }

        public GEarthApplicationHandler(Application application, GEarthExtension extension)
        {
            Application = application;

            Extension = extension;
            Extension.Clicked += OnExtensionClicked;
        }

        public async Task RunAsync()
        {
            try
            {
                await Extension.RunAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Shutdown();
            }
        }

        private void OnExtensionClicked(object sender, EventArgs e)
        {
            if (!Window.IsVisible)
            {
                Window.Show();
            }

            Window.Activate();
            Window.BringIntoView();
        }
    }
}
