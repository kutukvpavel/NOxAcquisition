using System;
using System.Collections.Generic;
using System.Text;

namespace NOxAcquisition
{
    public enum RegisterTypes
    {
        DiscreteInput,
        Input
    }

    public abstract class ModbusRegisterBase<T> : IModbusRegister where T : IComparable
    {
        public event EventHandler<T> ValueChanged;

        public ModbusRegisterBase(string name, RegisterTypes type, ushort address, ushort length)
        {
            Name = name;
            Type = type;
            Address = address;
            Length = length;
        }

        public string Name { get; }
        public RegisterTypes Type { get; }
        public ushort Address { get; }
        public ushort Length { get; }
        public T Value
        {
            get => _value;
            protected set
            {
                T last = _value;
                _value = value;
                if (last.CompareTo(_value) != 0) ValueChanged?.Invoke(this, last);
            }
        }

        public abstract void SetValue(ushort[] raw);

        public object GetValue()
        {
            return Value;
        }

        private T _value;
    }
}
