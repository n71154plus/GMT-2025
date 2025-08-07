using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMT_2025.Models
{
    public class ControlCommand
    {
        public string Name { get; set; }
        public byte RegIndex { get; set; }
        public byte Command { get; set; }
        public string Description { get; set; }
        public bool ReadRegister { get; set; }
        public bool WriteRegister { get; set; }

    }
}
