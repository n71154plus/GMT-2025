using GMT_2025.Controls;
using GMT_2025.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Windows;
using System.Diagnostics;
using System.Collections.ObjectModel;

namespace GMT_2025.ViewModels
{
    public partial class ProductViewModel : ObservableObject
    {
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(TotalCommandRows))]
        [NotifyPropertyChangedFor(nameof(CheckSum))]
        [NotifyPropertyChangedFor(nameof(ControlCommands))]
        [NotifyPropertyChangedFor(nameof(RegisterViewModels))]
        private Product product;

        public int TotalCommandRows => (Product?.RegisterTable?.Commands.Count ?? 0) + 2;

        public int CheckSum => Product?.RegisterTable?.DefaultCode?.Values?.Sum(v => (int)v) ?? 0;

        public Dictionary<string, ControlCommand> ControlCommands => Product?.RegisterTable?.Commands;

        public ObservableCollection<RegisterViewModel> RegisterViewModels { get; } = new();
        public Dictionary<string, RegisterViewModel> RegisterViewModelMap { get; } = new();

        public ProductViewModel(Product product)
        {
            Product = product;
        }



        partial void OnProductChanged(Product value)
        {
            UnhookRegisterViewModelEvents();

            RegisterViewModels.Clear();
            if (value?.RegisterTable?.Registers is { } registers)
            {
                foreach (var item in registers.Values)
                {
                    var vm = new RegisterViewModel(item);
                    RegisterViewModels.Add(vm);
                    RegisterViewModelMap.Add(item.Name, vm);
                }
            }

            HookRegisterViewModelEvents();

            // 如果有用 PropertyChanged 通知也可以加上
            OnPropertyChanged(nameof(RegisterViewModels));
        }
        private void HookRegisterViewModelEvents()
        {
            foreach (var vm in RegisterViewModels)
            {
                vm.PropertyChanged -= RegisterViewModel_PropertyChanged;
                vm.PropertyChanged += RegisterViewModel_PropertyChanged;
            }
        }

        private void UnhookRegisterViewModelEvents()
        {
            foreach (var vm in RegisterViewModels)
            {
                vm.PropertyChanged -= RegisterViewModel_PropertyChanged;
            }
        }
        private void RegisterViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(RegisterViewModel.SelectedIndex))
            {
                // 這裡就可以處理：任一個 selectedIndex 被改變的事件
                // 例如：
                if (sender is RegisterViewModel vm)
                {
                    Debug.WriteLine($"SelectedIndex changed in: {vm.Name}, value: {vm.SelectedIndex}");
                }

                // 或是觸發某些 Command 或 Callback
            }
        }

    }
}
