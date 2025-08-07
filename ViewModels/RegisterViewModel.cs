using CommunityToolkit.Mvvm.ComponentModel;
using GMT_2025.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GMT_2025.ViewModels
{
    public partial class RegisterViewModel : ObservableObject
    {
        private Register _register;

        public RegisterViewModel(Register register)
        {
            _register = register;

            // 初始化欄位（可依需求改成深拷貝）
            name = register.Name;
            group = register.Group;
            groupbyFunction = register.GroupbyFunction;
            page = register.Page;
            unit = register.Unit;
            mapTableCalc = register.MapTableCalc;
            isVisible = register.IsVisible;
            itemSources = register.ItemSources;
        }

        [ObservableProperty]
        private string name;

        [ObservableProperty]
        private string group;

        [ObservableProperty]
        private string groupbyFunction;

        [ObservableProperty]
        private string page;

        [ObservableProperty]
        private string unit;

        [ObservableProperty]
        private string mapTableCalc;

        [ObservableProperty]
        private bool isVisible;

        [ObservableProperty]
        private List<DACValue> itemSources;

        [ObservableProperty]
        private object selectedItem;

        [ObservableProperty]
        private int selectedIndex = -1;
    }

}
