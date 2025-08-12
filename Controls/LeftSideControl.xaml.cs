using GMT_2025.Models;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace GMT_2025.Controls
{
    public partial class LeftSideControl : UserControl
    {
        public LeftSideControl()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty ControlCommandsProperty =
            DependencyProperty.Register(nameof(ControlCommands), typeof(IDictionary<string, ControlCommand>), typeof(LeftSideControl));

        public IDictionary<string, ControlCommand> ControlCommands
        {
            get => (IDictionary<string, ControlCommand>)GetValue(ControlCommandsProperty);
            set => SetValue(ControlCommandsProperty, value);
        }
    }
}
