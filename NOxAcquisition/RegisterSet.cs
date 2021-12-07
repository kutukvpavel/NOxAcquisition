using System;
using System.Collections.Generic;
using System.Text;

namespace NOxAcquisition
{
    public class RegisterSet
    {
        public event EventHandler<object> RegisterValueChanged;

        public List<FloatSingleRegister> SingleRegisters { get; private set; } = new List<FloatSingleRegister>();
        public List<FloatDoubleRegister> DoubleRegisters { get; private set; } = new List<FloatDoubleRegister>();

        public void SetSingleRegisters(IEnumerable<FloatSingleRegister> r)
        {
            foreach (var item in SingleRegisters)
            {
                item.ValueChanged -= InvokeValueChanged;
            }
            SingleRegisters.Clear();
            foreach (var item in r)
            {
                item.ValueChanged += InvokeValueChanged;
                SingleRegisters.Add(item);
            }
        }
        public void SetDoubleRegisters(IEnumerable<FloatDoubleRegister> r)
        {
            foreach (var item in DoubleRegisters)
            {
                item.ValueChanged -= InvokeValueChanged;
            }
            DoubleRegisters.Clear();
            foreach (var item in r)
            {
                item.ValueChanged += InvokeValueChanged;
                DoubleRegisters.Add(item);
            }
        }

        public IEnumerable<IModbusRegister> GetAll()
        {
            foreach (var item in SingleRegisters)
            {
                yield return item;
            }
            foreach (var item in DoubleRegisters)
            {
                yield return item;
            }
        }

        private void InvokeValueChanged<T>(object sender, T oldValue)
        {
            RegisterValueChanged?.Invoke(sender, oldValue);
        }
    }
}
