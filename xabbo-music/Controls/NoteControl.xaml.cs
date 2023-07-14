using System.Windows.Controls;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows;
using System;

namespace xabbo_music.Controls
{
    public partial class NoteControl : UserControl
    {
        public Action<object, MouseButtonEventArgs> Clicked;
        public Action<object, EventArgs> NoteChanged;

        private bool mouseDown, _isDropped;
        private Point _initialPosition;

        public string CurrentNote;
        public bool IsSelected;

        public NoteControl()
        {
            InitializeComponent();
            NoteChanged += OnNoteChanged;
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            mouseDown = true;

            if (string.IsNullOrEmpty(CurrentNote))
                return;

            var border = (Border)sender;
            _initialPosition = e.GetPosition(border);
            border.CaptureMouse();
            _isDropped = false;
        }

        private void Border_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (mouseDown)
            {
                mouseDown = false;
                Clicked?.Invoke(this, e);
                _ = Select();
            }

            if (string.IsNullOrEmpty(CurrentNote))
                return;

            var border = (Border)sender;
            border.ReleaseMouseCapture();

            if (_isDropped)
                return;

            var noteHolders = MainWindow.Instance.GetAllNotesHoldersInUserControl();

            foreach (var noteHolder in noteHolders)
            {
                var noteHolderPosition = e.GetPosition(noteHolder);
                if (noteHolderPosition.X >= 0 && noteHolderPosition.Y >= 0 &&
                    noteHolderPosition.X <= noteHolder.ActualWidth && noteHolderPosition.Y <= noteHolder.ActualHeight)
                {
                    if (noteHolder.CurrentDelay >= 194950)
                    {
                        MessageBox.Show("Sorry, but the maximum length for the music is 194,95 seconds :(");
                        continue;
                    }

                    noteHolder.ChangeNote(CurrentNote);
                    border.RenderTransform = null;
                    _isDropped = true;
                    return;
                }
            }

            border.RenderTransform = null;
        }

        private void Border_MouseMove(object sender, MouseEventArgs e)
        {
            if (string.IsNullOrEmpty(CurrentNote))
                return;

            var border = (Border)sender;
            if (border.IsMouseCaptured)
            {
                var currentPosition = e.GetPosition(border.Parent as UIElement);
                var offset = currentPosition - _initialPosition;

                border.RenderTransform = new TranslateTransform(offset.X, offset.Y);
            }
        }

        private void Grid_MouseLeave(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void OnNoteChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(CurrentNote))
            {
                EmptyHighlightBorder.Visibility = Visibility.Visible;
                HighlightBorder.Visibility = Visibility.Hidden;
                return;
            }

            EmptyHighlightBorder.Visibility = Visibility.Hidden;
            HighlightBorder.Visibility = Visibility.Visible;
        }

        private async Task Select()
        {
            await Task.Delay(50);
            IsSelected = true;
            EmptyHighlightBorder.Visibility = Visibility.Visible;
            MainWindow.CurrentNote = CurrentNote;
        }
    }
}
