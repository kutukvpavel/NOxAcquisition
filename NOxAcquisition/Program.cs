using System;
using System.Collections.Generic;
using System.IO;

namespace NOxAcquisition
{
    class Program
    {
        private static RegisterSet _set = new RegisterSet();
        private static ModbusProvider _provider;
        private static StorageProviderBase _storage = null;
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
            _provider = new ModbusProvider(args.Length > 1 ? args[1] : "167.116.185.10", 502);
            try
            {
                if (!Directory.Exists(args[0])) Directory.CreateDirectory(args[0]);
                _storage = new CsvProvider(args[0], _set);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                if (_storage == null) Console.WriteLine("WARNING: failed to initialize storage provider.");
            }
            while (!_cancel)
            {
                _provider.ReadAll(_set);
                if (_storage != null) _storage.Store();
                System.Threading.Thread.Sleep(10000);
            }
            try 
            {
                _provider.Dispose();
                _storage.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
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
