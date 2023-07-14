using System.Collections.Generic;
using System.Windows.Controls;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.Linq;

namespace xabbo_music.Controls
{
    public partial class HabboTileMap : UserControl
    {
        private string tileHeights = "0123456789abcdefghijklmnopqrst";
        private List<(Thickness, Cursor, Thickness)> mapping;
        private bool mouseInside = false;
        private Grid? isometricGrid;

        public List<HabboTile> Tiles { get; private set; } = new();

        public HabboTileMap() => InitializeComponent();

        #region Event handlers
        private void OnMouseEnter(object sender, MouseEventArgs e) => _ = MouseEntered();

        private void OnMouseLeave(object sender, MouseEventArgs e)
        {
            mouseInside = false;
            Cursor = Cursors.Arrow;
        }
        #endregion

        private async Task MouseEntered()
        {
            if (isometricGrid == null)
                return;

            mouseInside = true;

            var borderThickness = 30;
            var justFound = true;
            var speed = 7;

            while (mouseInside)
            {
                var mousePoint = Mouse.GetPosition(IsometricGrid);
                await Task.Delay(50);

                mapping = new List<(Thickness, Cursor, Thickness)>
                {
                    (new Thickness(ActualWidth / 4, 0, ActualWidth / 4 * 3, ActualHeight / 4 > borderThickness ? borderThickness : ActualHeight / 4), Cursors.ScrollN, new Thickness(isometricGrid.Margin.Left, isometricGrid.Margin.Top + speed, 0, 0)), //T - 0
                    (new Thickness(0, ActualHeight / 4, ActualWidth / 4 > borderThickness ? borderThickness : ActualWidth / 4, ActualHeight / 4 * 3), Cursors.ScrollW, new Thickness(isometricGrid.Margin.Left + speed, isometricGrid.Margin.Top, 0, 0)), //L - 1
                    (new Thickness(ActualWidth / 4 * 3 < ActualWidth - borderThickness ? ActualWidth - borderThickness : ActualWidth / 4 * 3, ActualHeight / 4, ActualWidth, ActualHeight / 4 * 3), Cursors.ScrollE, new Thickness(isometricGrid.Margin.Left - speed, isometricGrid.Margin.Top, 0, 0)), //R - 2
                    (new Thickness(ActualWidth / 4, ActualHeight / 4 * 3 < ActualHeight - borderThickness ? ActualHeight - borderThickness : ActualHeight / 4 * 3, ActualWidth / 4 * 3, ActualHeight), Cursors.ScrollS, new Thickness(isometricGrid.Margin.Left, isometricGrid.Margin.Top - speed, 0, 0)), //B - 3
                    (new Thickness(0, 0, ActualWidth / 4, ActualHeight / 4), Cursors.ScrollNW, new Thickness(isometricGrid.Margin.Left + speed, isometricGrid.Margin.Top + speed, 0, 0)), //TL
                    (new Thickness(ActualWidth / 4 * 3, 0, ActualWidth, ActualHeight / 4), Cursors.ScrollNE, new Thickness(isometricGrid.Margin.Left - speed, isometricGrid.Margin.Top + speed, 0, 0)), //TR
                    (new Thickness(0, ActualHeight / 4 * 3, ActualWidth / 4, ActualHeight), Cursors.ScrollSW, new Thickness(isometricGrid.Margin.Left + speed, isometricGrid.Margin.Top - speed, 0, 0)), //BL
                    (new Thickness(ActualWidth / 4 * 3, ActualHeight / 4 * 3, ActualWidth, ActualHeight), Cursors.ScrollSE, new Thickness(isometricGrid.Margin.Left - speed, isometricGrid.Margin.Top - speed, 0, 0)) //BR
                };

                var foundMapping = mapping.FirstOrDefault(x => IsMouseInside(mousePoint, x.Item1));

                if (foundMapping.Item2 == null)
                {
                    Cursor = Cursors.Arrow;
                    justFound = true;
                    continue;
                }

                if (justFound)
                {
                    await Task.Delay(600);

                    mousePoint = Mouse.GetPosition(IsometricGrid);
                    if (!IsMouseInside(mousePoint, foundMapping.Item1))
                        continue;

                    justFound = false;
                }

                Cursor = foundMapping.Item2;
                isometricGrid.Margin = foundMapping.Item3;
            }
        }

        private int GetTileHeight(char tile)
        {
            if (!tileHeights.Contains(tile))
                return 1;

            return tileHeights.IndexOf(tile) + 1;
        }

        public void RenderRoom(List<string> roomToRender)
        {
            Tiles = new();

            var tileWidth = 20;
            var tileHeight = 10;
            var gridWidth = 5000;
            var gridHeight = 5000;

            isometricGrid = new Grid();
            isometricGrid.Width = gridWidth;
            isometricGrid.Height = gridHeight;
            isometricGrid.Margin = new Thickness(-2500 + (roomToRender.First().Length * tileWidth / 2), -2500 + (roomToRender.Count * tileHeight / 2), 0, 0);

            for (var columnIndex = 0; columnIndex < roomToRender.Count; columnIndex++)
            {
                var line = roomToRender[columnIndex];
                if (line.Length == 0) continue;

                for (var rowIndex = 0; rowIndex < line.Length; rowIndex++)
                {
                    var value = line[rowIndex];

                    var xPos = (rowIndex - columnIndex) * (tileWidth + 4);
                    var yPos = (rowIndex + columnIndex) * (tileHeight + 2) - 150;

                    if (value == 'x')
                        continue;

                    var ActualHeight = GetTileHeight(value) * 24;

                    var tile = new HabboTile();
                    tile.Coordinates = new System.Drawing.Point(rowIndex, columnIndex);
                    tile.Margin = new Thickness(xPos, yPos - ActualHeight, 0, 0);

                    Tiles.Add(tile);
                    isometricGrid.Children.Add(tile);
                }
            }

            IsometricGrid.Children.Clear();
            IsometricGrid.Children.Add(isometricGrid);
        }

        private bool IsMouseInside(Point mousePosition, Thickness rectangle)
        {
            return mousePosition.X >= rectangle.Left &&
                   mousePosition.Y >= rectangle.Top &&
                   mousePosition.X <= rectangle.Right &&
                   mousePosition.Y <= rectangle.Bottom;
        }
    }
}
