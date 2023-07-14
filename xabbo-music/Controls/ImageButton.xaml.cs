using System.Windows.Media.Imaging;
using System.Windows.Controls;
using System.Threading.Tasks;
using System.Windows.Input;
using System;

namespace xabbo_music.Controls
{
    public partial class ImageButton : UserControl
    {
        public Action<object, MouseButtonEventArgs>? Clicked;
        private bool mouseDown;

        public string? ImagePath { get; set; }
        public string? PressedImagePath { get; set; }

        public ImageButton(string imagePath, string pressedImagePath)
        {
            InitializeComponent();
            ImagePath = imagePath;
            PressedImagePath = pressedImagePath;

            BT_Image.Source = new BitmapImage(new Uri(  ImagePath, UriKind.Relative));
        }

        private void BT_MouseDown(object sender, MouseButtonEventArgs e)
        {
            mouseDown = true;

            if (string.IsNullOrEmpty(ImagePath))
                return;

            if (string.IsNullOrEmpty(PressedImagePath))
                PressedImagePath = ImagePath;

            BT_Image.Source = new BitmapImage(new Uri(PressedImagePath, UriKind.Relative));
        }

        private void BT_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (!mouseDown || string.IsNullOrEmpty(ImagePath))
                return;

            mouseDown = false;
            Clicked?.Invoke(this, e);
            BT_Image.Source = new BitmapImage(new Uri(ImagePath, UriKind.Relative));
        }

        private void BT_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!mouseDown || string.IsNullOrEmpty(ImagePath))
                return;

            mouseDown = false;
        }

        public async Task PerformClick()
        {
            if (string.IsNullOrEmpty(ImagePath))
                return;

            if (string.IsNullOrEmpty(PressedImagePath))
                PressedImagePath = ImagePath;

            Clicked?.Invoke(this, null);
            BT_Image.Source = new BitmapImage(new Uri(PressedImagePath, UriKind.Relative));
            await Task.Delay(150);
            BT_Image.Source = new BitmapImage(new Uri(ImagePath, UriKind.Relative));
        }
    }
}
