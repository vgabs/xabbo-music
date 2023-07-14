using System.Windows.Controls;
using System.Windows.Input;
using System;

namespace xabbo_music.Controls
{
    public partial class TimelineButton : UserControl
    {
        public Action<object, MouseButtonEventArgs>? Clicked;
        private ImageButton imageButton;
        private bool mouseDown;

        public TimelineButton() => InitializeComponent();

        public void Initialize(string imagePath, string pressedImagePath, int width, int height)
        {
            Width = width;
            Height = height;
            Highlight_Border1.Width = width - 2;
            Highlight_Border1.Height = height - 2;
            Highlight_Border2.Width = width - 2;
            Highlight_Border2.Height = height - 2;

            imageButton = new ImageButton(imagePath, pressedImagePath);
            BT_Grid.Children.Add(imageButton);
            Clicked = imageButton.Clicked;
        }

        private void BT_MouseDown(object sender, MouseButtonEventArgs e) => mouseDown = true;

        private void BT_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (!mouseDown)
                return;

            mouseDown = false;
            Clicked?.Invoke(this, e);
            _ = imageButton.PerformClick();
        }

        private void BT_MouseLeave(object sender, MouseEventArgs e) => mouseDown = false;
    }
}
