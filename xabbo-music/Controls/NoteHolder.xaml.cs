using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows;
using System.Linq;

using xabbo_music.Extensions;
using xabbo_music.Enum;

namespace xabbo_music.Controls
{
    public partial class NoteHolder : UserControl
    {
        private Effect currentEffect = Enum.Effect.Unknown;
        private bool mouseDownL, mouseDownR;
        private int timeline;

        public string CurrentNote { get; private set; }
        public int CurrentDelay { get; set; }

        public NoteHolder(int _timeline)
        {
            InitializeComponent();
            timeline = _timeline;
        }

        #region Event handlers
        private void Control_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.RightButton == MouseButtonState.Pressed)
                mouseDownR = true;

            if (e.LeftButton == MouseButtonState.Pressed)
                mouseDownL = true;
        }

        private void Control_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (mouseDownL && !string.IsNullOrEmpty(MainWindow.CurrentNote))
            {
                ChangeNote(MainWindow.CurrentNote);
            }

            if (mouseDownR)
            {
                mouseDownR = false;

                if (currentEffect != Enum.Effect.Unknown)
                    ClearCell(true);
            }
        }

        private void Control_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (MainWindow.Instance.Timeline.ControllerMousePressed)
                return;

            if (e.LeftButton == MouseButtonState.Pressed)
                if (!string.IsNullOrEmpty(MainWindow.CurrentNote))
                    ChangeNote(MainWindow.CurrentNote);

            if (e.RightButton == MouseButtonState.Pressed)
                if (currentEffect != Enum.Effect.Unknown)
                    ClearCell(true);
        }

        private void Control_MouseLeave(object sender, MouseEventArgs e)
        {
            mouseDownL = false;
            mouseDownR = false;
        }
        #endregion

        public void ChangeNote(string newNote, bool addToSong = true)
        {
            if (newNote.Length == 0)
                return;

            if (addToSong)
                MainWindow.FullSong.Add((CurrentDelay, timeline, newNote));

            var notes = Effects.Notes().FirstOrDefault(x => x.Contains(newNote));
            var effect = Effects.GetEffect(newNote);

            if (effect == Enum.Effect.Unknown)
                return;

            if (MainWindow.FullSong.Contains((CurrentDelay, timeline, CurrentNote)))
                MainWindow.FullSong.Remove((CurrentDelay, timeline, CurrentNote));

            Cursor = Cursors.Hand;
            CurrentNote = newNote;
            Recolor(effect, notes, newNote);
            MainBorder.BorderBrush = Brushes.Black;
            HighlightBorder.Visibility = Visibility.Visible;
            DarkHighlightBorder.Visibility = Visibility.Visible;
        }

        private void Recolor(Effect effect, string[] notes, string text)
        {
            var effectColor = effect.ToBrush();
            var index = notes.ToList().IndexOf(text);

            MainBorder.Background = effectColor;
            TB_Note.Text = Effects.ConvertedNotes[index];
            TB_Note.IsEnabled = false;
            currentEffect = effect;
        }

        public void ClearCell(bool removeNoteFromSong = false)
        {
            if (removeNoteFromSong)
                MainWindow.FullSong.Remove((CurrentDelay, timeline, CurrentNote));

            MainBorder.BorderBrush = new SolidColorBrush(Color.FromArgb(0xFF, 0x22, 0x35, 0x3c));
            MainBorder.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x3e, 0x60, 0x6e));
            DarkHighlightBorder.Visibility = Visibility.Hidden;
            HighlightBorder.Visibility = Visibility.Hidden;
            currentEffect = Enum.Effect.Unknown;
            TB_Note.IsEnabled = true;
            Cursor = Cursors.Arrow;
            TB_Note.Text = "";
            CurrentNote = "";
        }
    }
}
