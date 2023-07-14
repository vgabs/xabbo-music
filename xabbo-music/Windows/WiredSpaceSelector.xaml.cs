using System.Windows.Media.Imaging;
using System.Windows.Controls;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Windows;
using System.Linq;
using System;

using xabbo_music.Enum;
using xabbo_music.Misc;

namespace xabbo_music.Forms
{
    public partial class WiredSpaceSelector : Window
    {
        public static bool SelectingWiredTiles { get; set; } = true;
        public static bool SelectingControllerTile { get; set; }
        public static bool SelectingSFXTile { get; set; }

        public static int WiredTilesLeft, ControllerTilesLeft, SFXTilesLeft;

        public WiredSpaceSelector()
        {
            InitializeComponent();

            CancelButton.TextBlock.Text = "Cancel";
            BuildButton.TextBlock.Text = "Build";

            BuildButton.Clicked += BuildButton_Click;
            CancelButton.Clicked += (object sender, MouseButtonEventArgs e) => Close();

            WiredItem.Image.Source = new BitmapImage(new Uri("/Images/wired_miniature.png", UriKind.Relative));
            SFXItem.Image.Source = new BitmapImage(new Uri("/Images/sfx_xylo_miniature.png", UriKind.Relative));
            ControllerItem.Image.Source = new BitmapImage(new Uri("/Images/glowball_miniature.png", UriKind.Relative));

            WiredItem.Clicked += SelectWiredButton_Click;
            SFXItem.Clicked += SelectSFXButton_Click;
            ControllerItem.Clicked += SelectControllerButton_Click;

            SetInventoryButtonsSelection(false, true, false);
        }

        private void TopBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void SelectWiredButton_Click(object sender, RoutedEventArgs e) => SetInventoryButtonsSelection(false, true, false);

        private void SelectSFXButton_Click(object sender, RoutedEventArgs e) => SetInventoryButtonsSelection(false, false, true);

        private void SelectControllerButton_Click(object sender, RoutedEventArgs e) => SetInventoryButtonsSelection(true, false, false);

        private void TB_MagicTile_TextChanged(object sender, TextChangedEventArgs e) => Logger.WriteAllText(FileType.MagicTile, TB_MagicTile.Text);

        private void BuildButton_Click(object sender, MouseButtonEventArgs e)
        {
            var controllerTiles = HabboTileMap.Tiles.Where(x => x.IsControllerTile).Select(x => x.Coordinates).ToList();
            var wiredTiles = HabboTileMap.Tiles.Where(x => x.IsWiredTile).Select(x => x.Coordinates).ToList();
            var sfxTiles = HabboTileMap.Tiles.Where(x => x.IsSFXTile).Select(x => x.Coordinates).ToList();

            if (ControllerTilesLeft > 0)
            {
                MessageBox.Show("Please, select the tile where the Controller is gonna be");
                return;
            }

            if (SFXTilesLeft > 0)
            {
                MessageBox.Show("Please, select the tile where the SFX furni is gonna be");
                return;
            }

            if (WiredTilesLeft > 0)
            {
                MessageBox.Show("Please, select the tiles where all the Wired furni is gonna be");
                return;
            }

            if (TB_MagicTile.Text.Length == 0)
            {
                MessageBox.Show("Please, write the ID of the magic tile you want to use to stack the SFX furni");
                return;
            }

            if (!int.TryParse(TB_MagicTile.Text, out int magicTileId))
            {
                MessageBox.Show("Please, write a valid magic tile ID!");
                return;
            }

            _ = Task.Run(() => MainWindow.Extension.BuildAsync(wiredTiles, controllerTiles.First(), sfxTiles.First(), magicTileId));
            Close();
        }

        private void SetInventoryButtonsSelection(bool controllerSelected, bool wiredSelected, bool sfxSelected)
        {
            SelectingControllerTile = controllerSelected;
            SelectingWiredTiles = wiredSelected;
            SelectingSFXTile = sfxSelected;

            ControllerItem.SelectedBorder.Visibility = controllerSelected ? Visibility.Visible : Visibility.Hidden;
            WiredItem.SelectedBorder.Visibility = wiredSelected ? Visibility.Visible : Visibility.Hidden;
            SFXItem.SelectedBorder.Visibility = sfxSelected ? Visibility.Visible : Visibility.Hidden;
        }

        public void SetItemAmounts()
        {
            WiredTilesLeft = MainWindow.Extension.GetCurrentSoundNotes().Distinct().Count() + 2;
            ControllerTilesLeft = 1;
            SFXTilesLeft = 1;

            ControllerItem.AmountTB.Text = $"{ControllerTilesLeft}";
            WiredItem.AmountTB.Text = $"{WiredTilesLeft}";
            SFXItem.AmountTB.Text = $"{SFXTilesLeft}";

            ControllerItem.ResizeAmountTextBlock();
            WiredItem.ResizeAmountTextBlock();
            SFXItem.ResizeAmountTextBlock();
        }
    }
}