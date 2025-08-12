using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GMT_2025.Models;
using GMT_2025.ViewModels;
using GMT_2025;
using System.Collections.ObjectModel;
using System;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Serialization;
using System.Windows;
using System.IO;
using System.Threading.Tasks;
namespace GMT_2025.ViewModels
{
    public partial class ProductsViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<Product> productItems = new();

        [ObservableProperty]
        private ObservableCollection<MenuItemViewModel> menuItems = new();

        public ProductsViewModel()
        {
            _ = LoadProductsAsync().ContinueWith(t =>
            {
                // 錯誤處理
            }, TaskContinuationOptions.OnlyOnFaulted);
            InitializeMenuItems();
        }

        private async Task LoadProductsAsync()
        {
            string yamlPath = "product.yaml";

            if (!File.Exists(yamlPath))
                return;

            try
            {
                using (var stream = new FileStream(yamlPath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, useAsync: true))
                using (var reader = new StreamReader(stream))
                {
                    var yamlText = await reader.ReadToEndAsync();

                    var deserializer = new DeserializerBuilder()
                        .WithNamingConvention(PascalCaseNamingConvention.Instance)
                        .Build();

                    var product = deserializer.Deserialize<Product>(yamlText);

                    ProductItems.Clear();
                    ProductItems.Add(product);
                    // 其他產品加入
                }
            }
            catch (Exception ex)
            {
                // 錯誤處理
            }
        }



        private void InitializeMenuItems()
        {
            MenuItems.Clear();
            MenuItems.Add(new MenuItemViewModel
            {
                Header = "_File",
                Children =
            {
                new MenuItemViewModel { Header = "_New" },
                new MenuItemViewModel { Header = "_Open" },
                new MenuItemViewModel { Header = "_Exit", Command = new RelayCommand(Exit) }
            }
            });
            MenuItems.Add(new MenuItemViewModel
            {
                Header = "_Edit",
                Children =
            {
                new MenuItemViewModel { Header = "_Copy" },
                new MenuItemViewModel { Header = "_Paste" }
            }
            });
        }


        private void Exit()
        {

            Application.Current.Shutdown();
        }

        [RelayCommand]
        private void OpenProductWindow(Product product)
        {
            var productWindow = new ProductWindow(product);
            productWindow.Show();
        }
    }
}