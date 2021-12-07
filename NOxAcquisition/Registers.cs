using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MoreLinq;

namespace NOxAcquisition
{
    public class FloatSingleRegister : ModbusRegisterBase<float>
    {
        public FloatSingleRegister(string name, ushort address) : base(name, RegisterTypes.Input, address, 2) { }

        public override void SetValue(ushort[] raw)
        {
            Value = BitConverter.ToSingle(raw.GetBytes());
        }
    }

    public class FloatDoubleRegister : ModbusRegisterBase<double>
    {
        public FloatDoubleRegister(string name, ushort address) : base(name, RegisterTypes.Input, address, 4) { }

        public override void SetValue(ushort[] raw)
        {
            Value = BitConverter.ToDouble(raw.GetBytes());
        }
    }
}