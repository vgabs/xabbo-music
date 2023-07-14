using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System;

namespace xabbo_music.Controls
{
    public partial class EffectItem : UserControl
    {
        public Action<object, MouseButtonEventArgs> Clicked;
        private Brush originalColor;
        private bool mouseDown;

        public EffectItem() => InitializeComponent();

        private void MainBorder_MouseEnter(object sender, MouseEventArgs e)
        {
            if (string.IsNullOrEmpty(TB_Text.Text))
                return;

            originalColor = MainBorder.Background;
            MainBorder.Background = new SolidColorBrush(Color.FromArgb(0x50, 0xFF, 0xFF, 0xFF));
        }

        private void Effect_MouseDown(object sender, MouseButtonEventArgs e) => mouseDown = true;

        private void Effect_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (!mouseDown)
                return;

            mouseDown = false;

            if (string.IsNullOrEmpty(TB_Text.Text))
                return;

            Clicked?.Invoke(this, e);
        }

        private void MainBorder_MouseLeave(object sender, MouseEventArgs e)
        {
            mouseDown = false;
            MainBorder.Background = originalColor;
        }
    }
}