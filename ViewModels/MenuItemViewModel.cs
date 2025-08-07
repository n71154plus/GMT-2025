using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
namespace GMT_2025.ViewModels
{
    public class MenuItemViewModel
    {
        public string Header { get; set; }
        public ICommand Command { get; set; }
        public ObservableCollection<MenuItemViewModel> Children { get; set; }

        public MenuItemViewModel()
        {
            Children = new ObservableCollection<MenuItemViewModel>();
        }
    }
}