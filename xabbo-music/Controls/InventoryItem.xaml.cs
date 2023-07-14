using System.Windows.Controls;
using System.Windows.Input;
using System;

namespace xabbo_music.Controls
{
    public partial class InventoryItem : UserControl
    {
        public Action<object, MouseButtonEventArgs> Clicked;
        private bool mouseDown;

        public InventoryItem() => InitializeComponent();

        public void ResizeAmountTextBlock() => AmountBorder.Width = 8 * AmountTB.Text.Length + 2;

        private void Item_MouseDown(object sender, MouseButtonEventArgs e) => mouseDown = true;

        private void Item_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (!mouseDown)
                return;

            mouseDown = false;
            Clicked?.Invoke(this, e);
        }

        private void Item_MouseLeave(object sender, MouseEventArgs e) => mouseDown = false;
    }
}
