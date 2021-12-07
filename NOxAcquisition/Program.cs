using System;
using System.Collections.Generic;

namespace NOxAcquisition
{
    class Program
    {
        private static RegisterSet _set = new RegisterSet();
        private static ModbusProvider _provider;
        private static bool _cancel = false;

        static void Main(string[] args)
        {
            Console.CancelKeyPress += Console_CancelKeyPress;
            Console.WriteLine("Modbus toolkit started.");
            _set.SetSingleRegisters(new List<FloatSingleRegister>()
            {
                new FloatSingleRegister("NO2 Stability", 16),
                new FloatSingleRegister("NO Stability", 60),
                new FloatSingleRegister("NO2 Concentration 1", 12),
                new FloatSingleRegister("NO Concentration 1", 56)
            });
            _set.SetDoubleRegisters(new List<FloatDoubleRegister>()
            {

            });
            _set.RegisterValueChanged += set_RegisterValueChanged;
            _provider = new ModbusProvider("10.208.146.181", 502);
            while (!_cancel)
            {
                _provider.ReadAll(_set);
                System.Threading.Thread.Sleep(10000);
            }
            _provider.Dispose();
        }

        private static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            _cancel = true;
            e.Cancel = true;
        }

        private static void set_RegisterValueChanged(object sender, object e)
        {
            var reg = (IModbusRegister)sender;
            Console.WriteLine($"{reg.Name} = {e} -> {reg.GetValue()}");
        }
    }
}
