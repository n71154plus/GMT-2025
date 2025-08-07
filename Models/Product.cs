using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace GMT_2025.Models
{
    public class Product
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public string Application { get; set; }
        public string Package { get; set; }
        public int AppVersion { get; set; }
        public string Description { get; set; }
        public string Postfix { get; set; }
        public string RFQVendor { get; set; }
        public RegisterTable RegisterTable { get; set; }

        public override string ToString() => Name;
    }
}
