using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization;

namespace GMT_2025.Models
{
    public class DACValue
    {
        private object _value;
        public object Value
        {
            get => _value;
            set
            {
                object convertedValue;
                // 尝试转换为 decimal
                if (decimal.TryParse(value?.ToString(), out decimal decimalValue))
                {
                    convertedValue = decimalValue;
                }
                // 尝试转换为 bool
                else if (bool.TryParse(value?.ToString(), out bool boolValue))
                {
                    convertedValue = boolValue;
                }
                // 失败则存为 string
                else
                {
                    convertedValue = value?.ToString();
                }
                _value = convertedValue;
            }
        }
        [YamlIgnore]
        public string Type => Value?.GetType().Name ?? "null";
    }
}
