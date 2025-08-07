using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMT_2025.Models
{
    public class RegisterTable
    {
        public string Name { get; set; }
        public List<byte> DeviceAddress { get; set; }
        public Dictionary<string, Register> Registers { get; set; }
        public Dictionary<string, Register> TestModeRegisters { get; set; }
        public Dictionary<string, ControlCommand> Commands { get; set; }
        public ControlCommand UnLockKey { get; set; } = null;
        public Dictionary<int,byte> DefaultCode { get; set; }
        public Dictionary<string,DesignRule> DesignRules { get; set; }
    }
}
