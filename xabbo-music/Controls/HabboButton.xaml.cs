using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System;

namespace xabbo_music.Controls
{
    public partial class HabboButton : UserControl
    {
        public Action<object, MouseButtonEventArgs> Clicked;
        private bool mouseDown;

        public HabboButton() => InitializeComponent();

        private void Button_MouseDown(object sender, MouseButtonEventArgs e)
        {
            mouseDown = true;
            Button.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0xde, 0xd8, 0xd8));
        }

        private void Button_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (!mouseDown)
                return;

            mouseDown = false;
            Button.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0xe8, 0xe8, 0xe8));

            if (string.IsNullOrEmpty(TextBlock.Text))
                return;

            Clicked?.Invoke(this, e);
        }

        private void Button_MouseLeave(object sender, MouseEventArgs e)
        {
            mouseDown = false;
            Button.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0xe8, 0xe8, 0xe8));
        }
    }
}
