using System;
using System.Collections.Generic;
using System.Text;

namespace NOxAcquisition
{
    public interface IModbusRegister
    {
        public string Name { get; }
        public RegisterTypes Type { get; }
        public ushort Address { get; }
        public ushort Length { get; }

        public void SetValue(ushort[] raw);
        public object GetValue();
    }
}
