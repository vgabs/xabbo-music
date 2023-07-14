using Rectangle = System.Windows.Shapes.Rectangle;
using Color = System.Windows.Media.Color;
using Point = System.Drawing.Point;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows;

using xabbo_music.Forms;

namespace xabbo_music.Controls
{
    public partial class HabboTile : UserControl
    {
        public bool Selected => IsControllerTile || IsWiredTile || IsSFXTile;
        public bool IsControllerTile, IsWiredTile, IsSFXTile;
        private bool mouseDownL, mouseDownR;
        private List<Rectangle> rectangles;
        public Point Coordinates;

        private List<(SolidColorBrush, (bool, bool, bool))> tileOptions = new()
        {
            (new SolidColorBrush(Color.FromArgb(0xFF, 0x18, 0xfe, 0x00)), (true, false, false)), // Controller Furni
            (new SolidColorBrush(Color.FromArgb(0xFF, 0xd6, 0x00, 0xfe)), (false, false, true)), // SFX Furni
            (new SolidColorBrush(Color.FromArgb(0xFF, 0xfe, 0xb4, 0x00)), (false, true, false)) // Wired Furni
        };

        public HabboTile()
        {
            InitializeComponent();
            rectangles = GetAllRectanglesInUserControl();
        }

        #region Event handlers
        private void Tile_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                mouseDownL = true;

            else if (e.RightButton == MouseButtonState.Pressed)
                mouseDownR = true;
        }

        private void Tile_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (mouseDownL)
            {
                mouseDownL = false;
                SelectTile();
            }

            if (mouseDownR)
            {
                mouseDownR = false;
                UnselectTile();
            }
        }

        private void Tile_MouseLeave(object sender, MouseEventArgs e)
        {
            mouseDownL = false;
            mouseDownR = false;
        }

        private void Rectangle_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                SelectTile();
                return;
            }

            if (e.RightButton == MouseButtonState.Pressed)
            {
                UnselectTile();
            }
        }
        #endregion

        private (SolidColorBrush, (bool, bool, bool)) GetCurrentTile()
        {
            if (WiredSpaceSelector.SelectingControllerTile)
                return tileOptions[0];

            if (WiredSpaceSelector.SelectingSFXTile)
                return tileOptions[1];

            if (WiredSpaceSelector.SelectingWiredTiles)
                return tileOptions[2];

            return (new SolidColorBrush(Color.FromArgb(0xFF, 0x94, 0x7b, 0x54)), (false, false, false));
        }

        private void SelectTile()
        {
            var tile = GetCurrentTile();
            var shouldChange = false;

            if (tile.Item2.Item1 && !IsControllerTile && WiredSpaceSelector.ControllerTilesLeft > 0)
            {
                WiredSpaceSelector.ControllerTilesLeft--;
                MainWindow.TileSelectorWindow.ControllerItem.AmountTB.Text = $"{WiredSpaceSelector.ControllerTilesLeft}";
                MainWindow.TileSelectorWindow.ControllerItem.ResizeAmountTextBlock();
                shouldChange = true;
            }

            else if (tile.Item2.Item2 && !IsWiredTile && WiredSpaceSelector.WiredTilesLeft > 0)
            {
                WiredSpaceSelector.WiredTilesLeft--;
                MainWindow.TileSelectorWindow.WiredItem.AmountTB.Text = $"{WiredSpaceSelector.WiredTilesLeft}";
                MainWindow.TileSelectorWindow.WiredItem.ResizeAmountTextBlock();
                shouldChange = true;
            }

            else if (tile.Item2.Item3 && !IsSFXTile && WiredSpaceSelector.SFXTilesLeft > 0)
            {
                WiredSpaceSelector.SFXTilesLeft--;
                MainWindow.TileSelectorWindow.SFXItem.AmountTB.Text = $"{WiredSpaceSelector.SFXTilesLeft}";
                MainWindow.TileSelectorWindow.SFXItem.ResizeAmountTextBlock();
                shouldChange = true;
            }

            if (shouldChange)
            {
                UnselectTile();

                IsControllerTile = tile.Item2.Item1;
                IsWiredTile = tile.Item2.Item2;
                IsSFXTile = tile.Item2.Item3;

                foreach (var rectangle in rectangles)
                {
                    rectangle.Fill = tile.Item1;
                }
            }
        }

        private void UnselectTile()
        {
            if (!Selected)
                return;

            if (IsControllerTile)
            {
                WiredSpaceSelector.ControllerTilesLeft++;
                MainWindow.TileSelectorWindow.ControllerItem.AmountTB.Text = $"{WiredSpaceSelector.ControllerTilesLeft}";
                MainWindow.TileSelectorWindow.ControllerItem.ResizeAmountTextBlock();
            }

            if (IsSFXTile)
            {
                WiredSpaceSelector.SFXTilesLeft++;
                MainWindow.TileSelectorWindow.SFXItem.AmountTB.Text = $"{WiredSpaceSelector.SFXTilesLeft}";
                MainWindow.TileSelectorWindow.SFXItem.ResizeAmountTextBlock();
            }

            if (IsWiredTile)
            {
                WiredSpaceSelector.WiredTilesLeft++;
                MainWindow.TileSelectorWindow.WiredItem.AmountTB.Text = $"{WiredSpaceSelector.WiredTilesLeft}";
                MainWindow.TileSelectorWindow.WiredItem.ResizeAmountTextBlock();
            }

            IsControllerTile = false;
            IsWiredTile = false;
            IsSFXTile = false;

            foreach (var rectangle in rectangles)
            {
                rectangle.Fill = new SolidColorBrush(Color.FromArgb(0xFF, 0x6d, 0x62, 0x4b));
            }
        }

        private List<Rectangle> GetAllRectanglesInUserControl()
        {
            var rectangles = new List<Rectangle>();

            TraverseVisualTree(rectangles, (DependencyObject)Content);

            return rectangles;
        }

        private void TraverseVisualTree(List<Rectangle> rectangles, DependencyObject element)
        {
            if (element is Rectangle rectangle)
                rectangles.Add(rectangle);

            for (int childIndex = 0; childIndex < VisualTreeHelper.GetChildrenCount(element); childIndex++)
                TraverseVisualTree(rectangles, VisualTreeHelper.GetChild(element, childIndex));
        }
    }
}
