using GMT_2025.Models;
using GMT_2025.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GMT_2025.Controls
{
    public partial class MainContentUniformGrid : UserControl
    {
        public static readonly DependencyProperty GroupByProperty =
            DependencyProperty.Register(nameof(GroupBy), typeof(string), typeof(MainContentUniformGrid),
                new PropertyMetadata(null, OnLayoutChanged));

        public static readonly DependencyProperty ColumnsProperty =
            DependencyProperty.Register(nameof(Columns), typeof(int?), typeof(MainContentUniformGrid),
                new PropertyMetadata(null, OnLayoutChanged));

        private Dictionary<string, RegisterControl> RegisterControls { get; } = new();

        private class GroupPanel
        {
            public string GroupName { get; set; } = string.Empty;
            public Brush Background { get; set; } = Brushes.Transparent;
            public ObservableCollection<RegisterControl> Registers { get; } = new();
        }

        public ObservableCollection<GroupPanel> Panels { get; } = new();

        public MainContentUniformGrid()
        {
            InitializeComponent();
            DataContextChanged += MainContentUniformGrid_DataContextChanged;
        }

        private void MainContentUniformGrid_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (DataContext is ProductViewModel productViewModel)
            {
                RegisterControls.Clear();

                if (productViewModel.Product?.RegisterTable?.Registers is { } registers &&
                    productViewModel.RegisterViewModelMap is { } registerViewModels)
                {
                    foreach (var item in registers.Values)
                    {
                        var control = new RegisterControl(registerViewModels[item.Name]);
                        RegisterControls.Add(item.Name, control);
                    }
                }
            }
        }

        public int? Columns
        {
            get => (int?)GetValue(ColumnsProperty);
            set => SetValue(ColumnsProperty, value);
        }

        public string GroupBy
        {
            get => (string)GetValue(GroupByProperty);
            set => SetValue(GroupByProperty, value);
        }

        private static void OnLayoutChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is MainContentUniformGrid grid)
                grid.BuildGrid();
        }

        private void BuildGrid()
        {
            if (RegisterControls == null || string.IsNullOrEmpty(GroupBy))
                return;

            Panels.Clear();

            var rand = new Random();
            var usedColors = new HashSet<Color>();
            var groupedRegisters = RegisterGroupBy(RegisterControls, GroupBy);

            foreach (var group in groupedRegisters)
            {
                int maxRows = Math.Min(9, group.Value.Count + 1);
                int count = 0;

                GroupPanel currentPanel = null;

                void StartNewPanel()
                {
                    var color = GenerateDistinctColor(rand, usedColors);
                    currentPanel = new GroupPanel
                    {
                        GroupName = group.Key,
                        Background = new SolidColorBrush(color)
                    };
                    Panels.Add(currentPanel);
                    count = 1;
                }

                StartNewPanel();

                foreach (var register in group.Value)
                {
                    if (!RegisterControls.TryGetValue(register.Name, out var control))
                        continue;

                    if (count >= maxRows)
                    {
                        StartNewPanel();
                    }

                    currentPanel.Registers.Add(control);
                    count++;
                }
            }
        }

        private static Dictionary<string, List<RegisterViewModel>> RegisterGroupBy(IDictionary<string, RegisterControl> registers, string groupPropertyName)
        {
            return registers
                .Values
                .Where(r => r != null && r.DataContext is RegisterViewModel)
                .Select(r => r.DataContext as RegisterViewModel)
                .GroupBy(r =>
                {
                    var prop = typeof(RegisterViewModel).GetProperty(groupPropertyName);
                    return prop?.GetValue(r) as string ?? "";
                })
                .OrderBy(g => g.Key)
                .ToDictionary(g => g.Key, g => g.ToList());
        }

        private static Color GenerateDistinctColor(Random rand, HashSet<Color> usedColors)
        {
            Color color;
            do
            {
                color = Color.FromRgb(
                    (byte)rand.Next(192, 255),
                    (byte)rand.Next(192, 255),
                    (byte)rand.Next(192, 255));
            } while (!usedColors.Add(color));
            return color;
        }
    }
}
