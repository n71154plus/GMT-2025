using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMT_2025.Models
{
    public class Register
    {
        public string Name { get; set; }
        public string Group { get; set; }
        public string GroupbyFunction { get; set; }
        public string Page { get; set; }
        public string Unit { get; set; }
        public string MapTableCalc { get; set; }
        public bool IsVisible { get; set; } = true;
        public List<DACValue> ItemSources { get; set; }

    }
}
