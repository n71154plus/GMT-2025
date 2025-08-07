using System.Collections;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace GMT_2025.Controls
{
    public class GroupedGrid : ContentControl
    {
        private INotifyCollectionChanged? _currentCollection;
        public static readonly DependencyProperty ItemClickCommandProperty =
            DependencyProperty.Register(
                "ItemClickCommand",
                typeof(ICommand),
                typeof(GroupedGrid),
                new PropertyMetadata(null, OnItemsSourceChanged));

        public ICommand ItemClickCommand
        {
            get => (ICommand)GetValue(ItemClickCommandProperty);
            set => SetValue(ItemClickCommandProperty, value);
        }


        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register(nameof(ItemsSource), typeof(IEnumerable), typeof(GroupedGrid),
                new PropertyMetadata(null, OnItemsSourceChanged));

        public static readonly DependencyProperty GroupByProperty =
            DependencyProperty.Register(nameof(GroupBy), typeof(string), typeof(GroupedGrid),
                new PropertyMetadata(null, OnItemsSourceChanged));

        public IEnumerable ItemsSource
        {
            get => (IEnumerable)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        public string GroupBy
        {
            get => (string)GetValue(GroupByProperty);
            set => SetValue(GroupByProperty, value);
        }

        private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is GroupedGrid grid)
            {
                // 移除舊集合的事件監聽
                if (grid._currentCollection != null)
                    grid._currentCollection.CollectionChanged -= grid.OnCollectionChanged;

                // 新集合監聽
                if (e.NewValue is INotifyCollectionChanged newCollection)
                {
                    grid._currentCollection = newCollection;
                    grid._currentCollection.CollectionChanged += grid.OnCollectionChanged;
                }
                else
                {
                    grid._currentCollection = null;
                }

                grid.BuildGrid();
            }
        }
        private void OnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            // 集合內容變更時也要重新建構畫面
            BuildGrid();
        }
        private void BuildGrid()
        {
            if (ItemsSource == null || !ItemsSource.Cast<object>().Any() ||  string.IsNullOrEmpty(GroupBy) || ItemClickCommand == null)
                return;
            var layout = new Grid
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
            };

            layout.RowDefinitions.Clear();
            layout.ColumnDefinitions.Clear();
            layout.Children.Clear();

            var grouped = ItemsSource.Cast<object>()
                .GroupBy(item =>
                {
                    var prop = item.GetType().GetProperty(GroupBy);
                    return prop?.GetValue(item)?.ToString() ?? "";
                })
                .ToList();

            // 找最大欄位數 (最大元素數)
            int maxItems = grouped.Max(g => g.Count());

            // 設定外層 Grid 欄位數: 1(分組標題) + maxItems (元素欄位)
            layout.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto }); // Group 標題欄

            for (int c = 0; c < maxItems; c++)
            {
                layout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            }

            int row = 0;
            foreach (var group in grouped)
            {
                layout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

                // Group 標題欄位
                var groupLabel = new TextBlock
                {
                    Text = group.Key,
                    FontStretch = FontStretches.UltraCondensed,
                    FontWeight = FontWeights.Bold,
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(5)
                };
                var groupBorder = new Border
                {
                    BorderBrush = Brushes.Gray,
                    BorderThickness = new Thickness(0, 0, 1, 1),
                    CornerRadius = new CornerRadius(4),
                    Background = Brushes.LightBlue,
                    Child = groupLabel
                };
                Grid.SetRow(groupBorder, row);
                Grid.SetColumn(groupBorder, 0);
                layout.Children.Add(groupBorder);

                // 加入每個元素的欄位
                int col = 1;
                foreach (var item in group)
                {
                    var btn = new Button
                    {
                        Content = item.ToString(),
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        VerticalAlignment = VerticalAlignment.Stretch,
                        FontStretch = FontStretches.UltraCondensed,
                        FontWeight = FontWeights.Bold,
                        Background = Brushes.Transparent,
                        BorderThickness = new Thickness(0),
                        Margin = new Thickness(0)
                    };
                    btn.CommandParameter = item;
                    btn.Command = ItemClickCommand;
                    var btnBorder = new Border
                    {
                        BorderBrush = Brushes.LightGray,
                        BorderThickness = new Thickness(0, 0, 1, 1),
                        CornerRadius = new CornerRadius(4),
                        Background = Brushes.Transparent,
                        Child = btn
                    };
                    Grid.SetRow(btnBorder, row);
                    Grid.SetColumn(btnBorder, col++);
                    layout.Children.Add(btnBorder);
                }

                // 不足的欄位填空白 Border (維持線條對齊)
                for (; col <= maxItems; col++)
                {
                    var emptyBorder = new Border
                    {
                        BorderBrush = Brushes.LightGray,
                        BorderThickness = new Thickness(0, 0, 1, 1),
                        CornerRadius = new CornerRadius(4),
                        Background = Brushes.Transparent
                    };
                    Grid.SetRow(emptyBorder, row);
                    Grid.SetColumn(emptyBorder, col);
                    layout.Children.Add(emptyBorder);
                }

                row++;
            }

            this.Content = layout;
        }

    }
}
