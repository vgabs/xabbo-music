using Effect = xabbo_music.Enum.Effect;
using xabbo_music.Extensions;

using System.Windows.Media.Imaging;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows;
using System.Linq;
using System;

namespace xabbo_music.Controls
{
    public partial class EffectInventory : UserControl
    {
        private Grid? getNextFreeSlot() => slotGrids.FirstOrDefault(x => x.Item1 == Enum.Effect.Unknown).Item2;
        private int totalPages() => _effects.Count / 3;

        public Action<object, MouseButtonEventArgs> Clicked;
        private List<(Effect, Grid)> slotGrids = new();
        private List<Effect> _effects = new();
        private int currentPage;

        public EffectInventory()
        {
            InitializeComponent();

            var effectBackwardButton = new ImageButton("/Images/button_backward_green.png", "/Images/button_backward_green_pressed.png");
            effectBackwardButton.Clicked += (object sender, MouseButtonEventArgs e) => ChangePage(currentPage - 1);
            BT_Backward_Placeholder.Children.Add(effectBackwardButton);

            var effectForwardButton = new ImageButton("/Images/button_forward_green.png", "/Images/button_forward_green_pressed.png");
            effectForwardButton.Clicked += (object sender, MouseButtonEventArgs e) => ChangePage(currentPage + 1);
            BT_Forward_Placeholder.Children.Add(effectForwardButton);

            AddItems(Effects.List);
            Initialize();
            ChangePage(0);
        }

        public void Initialize()
        {
            for (var slotIndex = 0; slotIndex < 3; slotIndex++)
            {
                var grid = new Grid();
                grid.SetValue(Grid.RowProperty, slotIndex);
                grid.Margin = new Thickness(20, 0, 20, 0);
                grid.Name = $"Grid_{slotIndex}";

                var effectItem = new EffectItem();
                effectItem.Clicked += ItemClicked;
                effectItem.Name = $"EmptySlot_{slotIndex}";

                grid.Children.Add(effectItem);
                MainGrid.Children.Add(grid);
                slotGrids.Add((Enum.Effect.Unknown, grid));
            }
        }

        private void LoadPage(int page)
        {
            ClearView();

            for (var slotIndex = 0; slotIndex < slotGrids.Count; slotIndex++)
            {
                var effectIndex = page * 3 + slotIndex;
                if (effectIndex >= _effects.Count)
                    return;

                var nextFreeSlot = getNextFreeSlot();
                if (nextFreeSlot == null)
                    return;

                var effect = _effects[effectIndex];
                var effectItem = NewEffectItem(effect);
                nextFreeSlot.Children.Add(effectItem);

                slotGrids[slotGrids.IndexOf(slotGrids.First(x => x.Item1 == Enum.Effect.Unknown))] = (effect, nextFreeSlot);
            }
        }

        private void ChangePage(int page)
        {
            page = page < 0 ? 0 : page;

            if (page >= totalPages())
                page = totalPages();

            LoadPage(page);
            currentPage = page;

            if (MainWindow.Instance != null)
                SetCursor(MainWindow.HasEmptyInventories() ? Cursors.ScrollE : Cursors.No);

            TB_PageSelector.Text = $"{currentPage + 1}/{totalPages() + 1}";
        }

        private void ClearView()
        {
            for (var slotIndex = 0; slotIndex < slotGrids.Count; slotIndex++)
            {
                var effectAndGrid = slotGrids[slotIndex];

                var effectItem = new EffectItem();
                effectItem.Clicked += ItemClicked;
                effectItem.Name = $"EmptySlot_{slotIndex}";

                effectAndGrid.Item2.Children.Clear();
                effectAndGrid.Item2.Children.Add(effectItem);
                slotGrids[slotIndex] = (Enum.Effect.Unknown, effectAndGrid.Item2);
            }
        }

        public void SetCursor(Cursor cursor)
        {
            foreach (var effectItem in GetEffectItems())
                effectItem.Cursor = cursor;
        }

        private List<EffectItem> GetEffectItems()
        {
            var effectItems = new List<EffectItem>();

            TraverseVisualTree(effectItems, (DependencyObject)Content);

            return effectItems;
        }

        public void AddItems(List<Effect> effects)
        {
            foreach (var effect in effects)
            {
                _effects.Add(effect);
                var nextFreeSlot = getNextFreeSlot();

                if (nextFreeSlot == null)
                    continue;

                slotGrids.Remove(slotGrids.First(x => x.Item2 == nextFreeSlot));

                var effectItem = NewEffectItem(effect);

                nextFreeSlot.Children.Clear();
                nextFreeSlot.Children.Add(effectItem);

                slotGrids.Add((effect, nextFreeSlot));
            }
        }

        private EffectItem NewEffectItem(Effect effect)
        {
            var effectItem = new EffectItem();
            effectItem.Clicked += ItemClicked;
            effectItem.Name = effect.ToText().Replace(" ", "_");

            var bitmap = new BitmapImage(new Uri(effect.ToMiniature(), UriKind.Relative));
            effectItem.TB_Text.Text = effect.ToText();
            effectItem.Img_Miniature.Source = bitmap;

            return effectItem;
        }

        private void TraverseVisualTree(List<EffectItem> noteControls, DependencyObject element)
        {
            if (element is EffectItem noteControl)
                noteControls.Add(noteControl);

            for (int childIndex = 0; childIndex < VisualTreeHelper.GetChildrenCount(element); childIndex++)
            {
                TraverseVisualTree(noteControls, VisualTreeHelper.GetChild(element, childIndex));
            }
        }

        private void ItemClicked(object sender, MouseButtonEventArgs e) => Clicked?.Invoke(sender, e);
    }
}
