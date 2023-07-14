using System.Windows.Media.Imaging;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows;
using System.Linq;
using System.IO;
using System;

using Microsoft.Win32;

namespace xabbo_music.Controls
{
    public partial class TimelineControl : UserControl
    {
        private List<(int, TextBlock)> timelineTimespans = new();
        public List<List<NoteHolder>> Timelines = new();
        private List<List<Grid>> indicators = new();

        private int CurrentTimelineSpacing = 50;
        public bool ControllerMousePressed;
        public int CurrentSelectorIndex;

        public TimelineControl() => InitializeComponent();

        public void Initialize()
        {
            MainWindow.Instance.MouseUp += MainWindowMouseUpEventHandler;

            for (var timelineIndex = 0; timelineIndex < 4; timelineIndex++)
            {
                var timeline = new Timeline() { Margin = new Thickness(10, 0, 0, 2) };
                timeline.TimelineNumber.Text = $"{timelineIndex + 1}";
                Grid.SetRow(timeline, timelineIndex);

                var controlList = new List<NoteHolder>();
                var indicatorList = new List<Grid>();

                for (var noteHolderIndex = 0; noteHolderIndex < 27; noteHolderIndex++)
                {
                    var noteHolder = new NoteHolder(timelineIndex) 
                    { 
                        CurrentDelay = noteHolderIndex * CurrentTimelineSpacing, Width = 30, 
                        Margin = new Thickness(noteHolderIndex * 26, 0, 0, 0), 
                        HorizontalAlignment = HorizontalAlignment.Left, 
                        VerticalAlignment = VerticalAlignment.Center 
                    };

                    timeline.TimelineGrid.Children.Add(noteHolder);
                    controlList.Add(noteHolder);

                    if (noteHolderIndex >= 26)
                        continue;

                    var indicatorGrid = new Grid() { Height = 14, Width = 7, Visibility = Visibility.Hidden, VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Left, Margin = new Thickness(noteHolderIndex * 26 + 24, 0, 0, 0) };

                    Panel.SetZIndex(indicatorGrid, 100 + noteHolderIndex);

                    indicatorGrid.Children.Add(new Image() { Source = new BitmapImage(new Uri("/Images/more_notes_icon.png", UriKind.Relative)) });
                    timeline.TimelineGrid.Children.Add(indicatorGrid);
                    indicatorList.Add(indicatorGrid);
                }

                Timelines.Add(controlList);
                indicators.Add(indicatorList);
                TimelinePlaceholder.Children.Add(timeline);
            }

            var count = 0;

            for (var i = 0; i < 27; i++)
            {
                if (i % 2 == 0)
                {
                    count++;
                    continue;
                }

                if (count % 2 == 0)
                    continue;

                var textBlock = new TextBlock() 
                { 
                    HorizontalAlignment = HorizontalAlignment.Left, 
                    VerticalAlignment = VerticalAlignment.Bottom, 
                    TextAlignment = TextAlignment.Center, 
                    FontSize = 10, 
                    Margin = new Thickness(26 * i, 0, 0, 0), 
                    Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 0xb6, 0xdc, 0xdf)),
                    Text = $"{(i * CurrentTimelineSpacing + CurrentTimelineSpacing) / 1000.0}s"
                };

                Time_Grid.Children.Add(textBlock);
                timelineTimespans.Add((i, textBlock));
            }
        }

        public async Task StatusUpdater()
        {
            while (true)
            {
                await Task.Delay(2500);
                LB_Connected.Content = $"Connected to Habbo: {MainWindow.Extension.IsConnected}";
                LB_RoomLoaded.Content = $"Room loaded: {MainWindow.Extension.RoomLoaded}";
            }
        }

        #region Event handlers
        public void ZoomOutEventHandler(object sender, EventArgs e)
        {
            var possibleTimelineSpacings = new List<int>() { 50, 100, 250, 500, 1000 };
            CurrentTimelineSpacing = CurrentTimelineSpacing == 1000 ? 1000 : possibleTimelineSpacings[possibleTimelineSpacings.IndexOf(CurrentTimelineSpacing) + 1];
            LB_TimelineSpeed.Content = $"Timeline speed: {CurrentTimelineSpacing}ms";

            for (var j = 0; j < Timelines.Count; j++)
            {
                for (var i = 0; i < Timelines[j].Count; i++)
                {
                    var note = Timelines[j][i];

                    if (!string.IsNullOrEmpty(note.CurrentNote))
                        note.ClearCell();

                    Timelines[j][i].CurrentDelay = CurrentTimelineSpacing * i;
                }

                RenderTimeline(j);
            }
        }

        public void ZoomInEventHandler(object sender, EventArgs e)
        {
            var possibleTimelineSpacings = new List<int>() { 50, 100, 250, 500, 1000 };
            CurrentTimelineSpacing = CurrentTimelineSpacing == 50 ? 50 : possibleTimelineSpacings[possibleTimelineSpacings.IndexOf(CurrentTimelineSpacing) - 1];
            LB_TimelineSpeed.Content = $"Timeline speed: {CurrentTimelineSpacing}ms";

            for (var j = 0; j < Timelines.Count; j++)
            {
                for (var i = 0; i < Timelines[j].Count; i++)
                {
                    var note = Timelines[j][i];

                    if (!string.IsNullOrEmpty(note.CurrentNote))
                        note.ClearCell();

                    Timelines[j][i].CurrentDelay = CurrentTimelineSpacing * i;
                }

                RenderTimeline(j);
            }
        }

        public void LoadEventHandler(object sender, EventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files (*.txt)|*.txt";
            openFileDialog.InitialDirectory = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "files", "songs");

            var result = openFileDialog.ShowDialog();

            // Process the dialog result
            if (result != true)
                return;

            var filePath = openFileDialog.FileName;

            using var reader = new StreamReader(filePath);
            MainWindow.FullSong = new();
            string line;

            while ((line = reader.ReadLine()) != null)
            {
                string[] values = line.Split('|');
                if (values.Length == 3 && int.TryParse(values[0], out int value1) && int.TryParse(values[1], out int value2))
                {
                    MainWindow.FullSong.Add((value1, value2, values[2]));
                }
            }

            RenderTimelines();
        }

        public void TrashEventHandler(object sender, EventArgs e)
        {
            if (MainWindow.FullSong.Count == 0 || MessageBox.Show("Are you sure you want to delete this record?", "Confirmation", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
                return;

            MainWindow.FullSong = new();
            RenderTimelines();
        }

        public void BackwardEventHandler(object sender, EventArgs e)
        {
            CurrentSelectorIndex--;
            CurrentSelectorIndex = CurrentSelectorIndex < 0 ? 0 : CurrentSelectorIndex;
            TimelineSelector.Margin = new Thickness(-624 + CurrentSelectorIndex * 52, TimelineSelector.Margin.Top, 31, -137);
        }

        public void ForwardEventHandler(object sender, EventArgs e)
        {
            CurrentSelectorIndex++;
            CurrentSelectorIndex = CurrentSelectorIndex > 25 ? 25 : CurrentSelectorIndex;
            TimelineSelector.Margin = new Thickness(-624 + CurrentSelectorIndex * 52, TimelineSelector.Margin.Top, 31, -137);
        }

        private void PreviewMouseWheelEventHandler(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta > 0)
            {
                if (Timelines.First().First().CurrentDelay <= 0)
                    return;

                for (var i = 0; i < Timelines.Count; i++)
                {
                    foreach (var note in Timelines[i])
                        note.CurrentDelay -= CurrentTimelineSpacing;

                    RenderTimeline(i);
                }
            }

            else if (e.Delta < 0)
            {
                if (Timelines.First().Last().CurrentDelay >= 194950)
                    return;

                for (var i = 0; i < Timelines.Count; i++)
                {
                    foreach (var note in Timelines[i])
                        note.CurrentDelay += CurrentTimelineSpacing;

                    RenderTimeline(i);
                }
            }
        }

        private void MainWindowMouseUpEventHandler(object sender, MouseButtonEventArgs e)
        {
            if (ControllerMousePressed)
            {
                var currentSelectorXPosition = (int)TimelineSelector.Margin.Left;
                var validPoints = new List<double>();
                var maxAllowedX = 676;
                var minAllowedX = -624;
                var timelineLenght = minAllowedX * -1 + maxAllowedX;
                var noteHolderWidth = timelineLenght / 27;

                for (var i = 0; i < 27; i++)
                {
                    validPoints.Add(minAllowedX + (i * noteHolderWidth));
                }

                ControllerMousePressed = false;
                CurrentSelectorIndex = FindClosestIndex(currentSelectorXPosition + 27, validPoints) - 1;

                if (CurrentSelectorIndex > 11)
                    CurrentSelectorIndex--;

                TimelineSelector.Margin = new Thickness(minAllowedX + CurrentSelectorIndex * 52, TimelineSelector.Margin.Top, 31, -137);
            }
        }

        private void ControllerMouseDownEventHandler(object sender, MouseButtonEventArgs e) => _ = ControllerMouseDown(sender, e);
        #endregion

        private void RenderTimeline(int timeline)
        {
            //Clear all cells
            for (var i = 0; i < Timelines[timeline].Count; i++)
            {
                var note = Timelines[timeline][i];

                if (!string.IsNullOrEmpty(note.CurrentNote))
                    note.ClearCell();
            }

            //Hide all indicators
            for (var i = 0; i < Timelines[timeline].Count - 1; i++)
            {
                indicators[timeline][i].Visibility = Visibility.Hidden;
            }

            //Update the timespan texts
            foreach (var timespan in timelineTimespans)
            {
                timespan.Item2.Text = $"{(timespan.Item1 * CurrentTimelineSpacing + CurrentTimelineSpacing + Timelines.First().First().CurrentDelay) / 1000.0}s";
            }

            //Get notes from the music that match the current timespans
            var timelineNotes = MainWindow.FullSong.Where(x => x.Item2 == timeline).Where(x => x.Item1 % CurrentTimelineSpacing == 0).ToList();
            var otherNotes = MainWindow.FullSong.Where(x => !timelineNotes.Contains(x)).ToList();

            for (var i = 0; i < timelineNotes.Count; i++)
            {
                var noteHolder = Timelines[timeline].FirstOrDefault(x => x.CurrentDelay == timelineNotes[i].Item1);

                if (noteHolder == null)
                    continue;

                noteHolder.ChangeNote(timelineNotes[i].Item3, false);
            }

            for (var i = 0; i < Timelines[timeline].Count; i++)
            {
                if (i + 1 >= Timelines[timeline].Count)
                    continue;

                var noteHolder = Timelines[timeline][i];
                var nextNoteHolder = Timelines[timeline][i + 1];

                if (otherNotes.Any(x => x.Item1 > noteHolder.CurrentDelay && x.Item1 < nextNoteHolder.CurrentDelay && x.Item2 == timeline))
                    indicators[timeline][i].Visibility = Visibility.Visible;
            }
        }

        private void RenderTimelines()
        {
            for (var i = 0; i < Timelines.Count; i++)
                RenderTimeline(i);
        }

        private int FindClosestIndex(int targetValue, List<double> values)
        {
            int closestIndex = -1;
            double minDifference = double.MaxValue;

            for (int i = 0; i < values.Count; i++)
            {
                double difference = Math.Abs(targetValue - values[i]);

                if (difference < minDifference)
                {
                    minDifference = difference;
                    closestIndex = i;
                }
            }

            return closestIndex;
        }

        private async Task ControllerMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var minLimit = -624;
                var maxLimit = 676;
                var initialMouseXpos = (int)e.GetPosition(MainWindow.Instance.MainGrid).X;
                var initialGridXpos = (int)TimelineSelector.Margin.Left;
                ControllerMousePressed = true;

                while (ControllerMousePressed)
                {
                    var updatedPosition = e.GetPosition(MainWindow.Instance.MainGrid);
                    var updatedLeftMargin = initialGridXpos - (initialMouseXpos - updatedPosition.X) * 2;
                    updatedLeftMargin = updatedLeftMargin < minLimit ? minLimit : updatedLeftMargin > maxLimit ? maxLimit : updatedLeftMargin;

                    TimelineSelector.Margin = new Thickness(updatedLeftMargin, TimelineSelector.Margin.Top, 31, -137);

                    await Task.Delay(50);
                    continue;
                }
            }
        }
    }
}