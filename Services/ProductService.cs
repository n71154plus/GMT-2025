using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using GMT_2025.Models;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace GMT_2025.Services
{
    public class ProductService : IProductService
    {
        public async Task<IEnumerable<Product>> LoadProductsAsync()
        {
            var products = new List<Product>();
            const string yamlPath = "product.yaml";

            if (!File.Exists(yamlPath))
                return products;

            using var stream = new FileStream(yamlPath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, useAsync: true);
            using var reader = new StreamReader(stream);
            var yamlText = await reader.ReadToEndAsync();

            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(PascalCaseNamingConvention.Instance)
                .Build();

            var product = deserializer.Deserialize<Product>(yamlText);
            products.Add(product);
            return products;
        }
    }
}
