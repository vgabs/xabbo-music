using System.Collections.Generic;
using System.Windows.Input;
using System.Windows.Media;
using System.Threading;
using System.Windows;
using System.Linq;
using System.IO;
using System;

using xabbo_music.Extensions;
using xabbo_music.Controls;
using xabbo_music.Forms;
using xabbo_music.Misc;
using xabbo_music.Enum;

using Microsoft.Win32;

using Xabbo.GEarth;

namespace xabbo_music
{
    public partial class MainWindow : Window
    {
        public static bool HasEmptyInventories() => Instance.noteInventories.Any(x => x.IsEmpty);
        public static List<(int, int, string)> FullSong = new();
        public static WiredSpaceSelector TileSelectorWindow;
        public static MainWindow Instance;
        public static string CurrentNote;

        private CancellationTokenSource playSongCancellationToken;
        public Action<object, MouseButtonEventArgs>? MouseUp;
        private List<NoteInventory> noteInventories = new();
        public TimelineButton PlayButton, StopButton;

        public static MusicExtension Extension;

        public MainWindow()
        {
            InitializeComponent();
            Instance = this;
            /* Check repeated notes
            var _notes = new List<string>();
            var repeatedNotes = new List<string>();

            foreach (var noteList in Effects.EffectsAndNotes)
            {
                foreach (var note in noteList.Item2)
                {
                    if (_notes.Contains(note))
                    {
                        repeatedNotes.Add($"{note} in {noteList.Item1}");
                        continue;
                    }

                    _notes.Add(note);
                }
            }

            MessageBox.Show(string.Join("\n", repeatedNotes));
            */

            Extension = new MusicExtension(
                GEarthOptions.FromArgs(new string[0]) with
                {
                    Title = "Xabbo Music",
                    Version = "1.0",
                    Description = "Extension to create music using wired",
                    Author = "Odine",
                }
            );

            var handler = new GEarthApplicationHandler(App.Instance, Extension);
            _ = handler.RunAsync();
            _ = Timeline.StatusUpdater();

            EffectInventory.Clicked += InventoryItemClicked;

            #region NoteInventories
            for (var i = 0; i < 3; i++)
            {
                var noteInventory = new NoteInventory();
                noteInventory.VerticalAlignment = VerticalAlignment.Bottom;
                noteInventory.HorizontalAlignment = HorizontalAlignment.Left;
                noteInventory.Width = 163;
                noteInventory.Height = 179;
                Grid_NoteInventories.Children.Add(noteInventory);
                noteInventory.Margin = new System.Windows.Thickness((6 * (i + 1)) + (noteInventory.Width * i) + 4, 0, 0, EffectInventory.Height - 35 - noteInventory.Height);

                noteInventories.Add(noteInventory);
            }

            foreach (var noteInventory in noteInventories.SelectMany(x => x.NoteControls))
                noteInventory.Clicked += ClearSelection;

            void ClearSelection(object sender, MouseButtonEventArgs e)
            {
                var allItems = noteInventories.SelectMany(x => x.NoteControls).Where(x => !string.IsNullOrEmpty(x.CurrentNote));

                foreach (var item in allItems)
                {
                    item.IsSelected = false;
                    item.EmptyHighlightBorder.Visibility = Visibility.Hidden;
                }
            }

            OpenEffectInventory(Enum.Effect.Xylophone);
            #endregion

            Timeline.Initialize();

            #region Timelines
            #region Buttons
            var buttons = new List<(string, string, int, int, int, Action<object, EventArgs>)>()
            {
                ("/Images/button_timeline_save.png", "/Images/button_timeline_save_pressed.png", 36, 24, 309, SaveMusicEventHandler),
                ("/Images/button_timeline_load.png", "/Images/button_timeline_load_pressed.png", 36, 24, 271, Timeline.LoadEventHandler),
                ("/Images/button_timeline_build.png", "/Images/button_timeline_build_pressed.png", 36, 24, 233, BuildMusicEventHandler),
                ("/Images/button_timeline_trash.png", "/Images/button_timeline_trash_pressed.png", 36, 24, 195, Timeline.TrashEventHandler),

                ("/Images/button_zoom_out.png", "/Images/button_zoom_out_pressed.png", 36, 24, 149, Timeline.ZoomOutEventHandler),
                ("/Images/button_zoom_in.png", "/Images/button_zoom_in_pressed.png", 36, 24, 111, Timeline.ZoomInEventHandler),

                ("/Images/button_backward_yellow.png", "/Images/button_backward_yellow_pressed.png", 22, 24, 79, Timeline.BackwardEventHandler),
                ("/Images/button_forward_green.png", "/Images/button_forward_green_pressed.png", 36, 24, 41, TimelinePlayEventHandler),
                ("/Images/button_timeline_stop.png", "/Images/button_timeline_stop_pressed.png", 36, 24, 41, TimelineStopEventHandler),
                ("/Images/button_forward_yellow.png", "/Images/button_forward_yellow_pressed.png", 22, 24, 17, Timeline.ForwardEventHandler),
            };

            foreach (var button in buttons)
            {
                var _button = new TimelineButton();
                _button.Initialize(button.Item1, button.Item2, button.Item3, button.Item4);
                _button.Margin = new System.Windows.Thickness(0, 0, button.Item5, 0);
                _button.HorizontalAlignment = HorizontalAlignment.Right;
                _button.VerticalAlignment = VerticalAlignment.Bottom;
                _button.Clicked += button.Item6;

                if (button.Item6 == TimelinePlayEventHandler)
                    PlayButton = _button;

                if (button.Item6 == TimelineStopEventHandler)
                {
                    StopButton = _button;
                    _button.Visibility = Visibility.Hidden;
                }

                TimelineButtonsGrid.Children.Add(_button);
            }
            #endregion
            #endregion
        }

        #region Event handlers
        private void TopBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void Window_MouseUp(object sender, MouseButtonEventArgs e)
        {
            MouseUp?.Invoke(sender, e);
            e.Handled = true;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e) => Close();

        private void TimelineStopEventHandler(object sender, EventArgs e) => playSongCancellationToken.Cancel();

        private void TimelinePlayEventHandler(object sender, EventArgs e)
        {
            if (FullSong.Count == 0)
                return;

            if (!Extension.IsConnected || !Extension.RoomLoaded)
            {
                MessageBox.Show("Sorry, you must be connected and inside a room to hear this sample!");
                return;
            }

            StopButton.Visibility = Visibility.Visible;
            PlayButton.Visibility = Visibility.Hidden;
            playSongCancellationToken = new();

            _ = Extension.PlaySong(playSongCancellationToken);
        }

        private void BuildMusicEventHandler(object sender, EventArgs e)
        {
            if (!Extension.IsConnected)
            {
                MessageBox.Show("The extension is not connected to Habbo!");
                return;
            }

            if (!Extension.RoomLoaded)
            {
                MessageBox.Show("Room not loaded!");
                return;
            }

            if (!FullSong.Any())
            {
                MessageBox.Show("Please, write at least one note!");
                return;
            }

            TileSelectorWindow?.Close();
            TileSelectorWindow = new WiredSpaceSelector();

            TileSelectorWindow.TB_MagicTile.Text = Logger.ReadAllText(FileType.MagicTile);

            TileSelectorWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            TileSelectorWindow.Show();

            TileSelectorWindow.SetItemAmounts();
            TileSelectorWindow.HabboTileMap.RenderRoom(Extension.RoomHeightmap.OriginalString.Split("\r").ToList());
        }

        private void SaveMusicEventHandler(object sender, EventArgs e)
        {
            if (FullSong.Count == 0)
                return;

            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text files (*.txt)|*.txt";
            saveFileDialog.InitialDirectory = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "files", "songs");

            bool? result = saveFileDialog.ShowDialog();

            if (result != true)
                return;

            var filePath = saveFileDialog.FileName;
            using var writer = new StreamWriter(filePath);

            foreach ((int value1, int value2, string value3) in FullSong)
                writer.WriteLine($"{value1}|{value2}|{value3}");
        }

        private void InventoryItemClicked(object sender, MouseButtonEventArgs e) => OpenEffectInventory((sender as EffectItem).Name.ToEffect());
        #endregion

        private void OpenEffectInventory(Effect effect)
        {
            if (!HasEmptyInventories())
            {
                EffectInventory.SetCursor(Cursors.No);
                return;
            }

            var sameInventory = noteInventories.FirstOrDefault(x => x.CurrentEffect == effect);
            if (sameInventory != null) return;

            noteInventories.First(x => x.IsEmpty).Update(effect);
            EffectInventory.SetCursor(HasEmptyInventories() ? Cursors.ScrollE : Cursors.No);
        }

        public List<NoteHolder> GetAllNotesHoldersInUserControl()
        {
            var noteControls = new List<NoteHolder>();

            TraverseVisualTreeHolders(noteControls, (DependencyObject)Content);

            return noteControls;
        }

        private void TraverseVisualTreeHolders(List<NoteHolder> noteControls, DependencyObject element)
        {
            if (element is NoteHolder noteControl)
                noteControls.Add(noteControl);

            for (int childIndex = 0; childIndex < VisualTreeHelper.GetChildrenCount(element); childIndex++)
                TraverseVisualTreeHolders(noteControls, VisualTreeHelper.GetChild(element, childIndex));
        }
    }
}