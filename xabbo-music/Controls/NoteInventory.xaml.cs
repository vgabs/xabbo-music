using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Linq;
using System;

using xabbo_music.Extensions;
using xabbo_music.Enum;

namespace xabbo_music.Controls
{
    public partial class NoteInventory : UserControl
    {
        public Effect CurrentEffect = Enum.Effect.Unknown;
        public List<NoteControl> NoteControls = new();
        public bool IsEmpty = true;
        private bool mouseDown;

        public NoteInventory()
        {
            InitializeComponent();

            var slotIndex = 0;

            for (var columnIndex = 0; columnIndex < 4; columnIndex++)
            {
                for (var rowIndex = 0; rowIndex < 4; rowIndex++)
                {
                    var grid = new Grid();
                    grid.Name = $"Grid_{slotIndex}";
                    grid.SetValue(Grid.RowProperty, 1 + columnIndex);  
                    grid.SetValue(Grid.ColumnProperty, 1 + rowIndex);

                    var noteControl = new NoteControl();
                    noteControl.Name = $"Slot_{slotIndex}";

                    slotIndex++;
                    NoteControls.Add(noteControl);
                    grid.Children.Add(noteControl);
                    InventoryGrid.Children.Add(grid);
                }
            }

            BT_Close.Visibility = System.Windows.Visibility.Hidden;
        }

        public void Update(Effect effect)
        {
            if (effect == Enum.Effect.Unknown)
            {
                var emptyBrush = new SolidColorBrush(Color.FromArgb(0xFF, 0x3e, 0x60, 0x6e));

                EffectName.Text = "";
                EffectName.Foreground = emptyBrush;

                for (var noteIndex = 0; noteIndex < Effects.ConvertedNotes.Length; noteIndex++)
                {
                    NoteControls[noteIndex].TB_Note.Text = "";
                    NoteControls[noteIndex].MainBorder.Background = emptyBrush;
                    NoteControls[noteIndex].CurrentNote = "";
                    NoteControls[noteIndex].NoteChanged?.Invoke(this, EventArgs.Empty);
                }

                BT_Close.Visibility = System.Windows.Visibility.Hidden;
                CurrentEffect = Enum.Effect.Unknown;
                IsEmpty = true;

                return;
            }

            var effectBrush = effect.ToBrush();
            var notes = Effects.IdentifiersAndNotes.First(x => x.Item1 == effect.ToIdentifier()).Item2;

            EffectName.Text = effect.ToText();
            var effectColor = ((SolidColorBrush)effectBrush).Color;
            var newColor = Color.FromArgb(255, (byte)(effectColor.R / 2), (byte)(effectColor.G / 2), (byte)(effectColor.B / 2));
            EffectName.Foreground = new SolidColorBrush(newColor);

            for (var noteIndex = 0; noteIndex < Effects.ConvertedNotes.Length; noteIndex++)
            {
                NoteControls[noteIndex].TB_Note.Text = Effects.ConvertedNotes[noteIndex];
                NoteControls[noteIndex].MainBorder.Background = effectBrush;
                NoteControls[noteIndex].CurrentNote = notes[noteIndex];
                NoteControls[noteIndex].NoteChanged?.Invoke(this, EventArgs.Empty);
            }

            BT_Close.Visibility = System.Windows.Visibility.Visible;
            CurrentEffect = effect;
            IsEmpty = false;
        }

        private void BT_Close_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (!mouseDown)
                return;

            mouseDown = false;

            Update(Enum.Effect.Unknown);
            MainWindow.Instance.EffectInventory.SetCursor(MainWindow.HasEmptyInventories() ? Cursors.ScrollE : Cursors.No);
        }

        private void BT_Close_MouseLeave(object sender, MouseEventArgs e) => mouseDown = false;

        private void BT_Close_MouseDown(object sender, MouseButtonEventArgs e) => mouseDown = true;
    }
}
