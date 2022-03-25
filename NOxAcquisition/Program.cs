using System;
using System.Collections.Generic;
using System.IO;

namespace NOxAcquisition
{
    class Program
    {
        private static RegisterSet _set = new RegisterSet();
        private static ModbusProvider _provider;
        private static List<StorageProviderBase> _storage = new List<StorageProviderBase>();
        private static bool _cancel = false;
        private static bool _changed = false;

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine(@"Usage:
    NOxAcquisiton.exe ""Folder to save CSVs to"" [Instrument IP address, default = 167.116.185.10; UDP port number, default = 44128]");
                return;
            }
            Console.CancelKeyPress += Console_CancelKeyPress;
            string ip = args.Length > 1 ? args[1] : "167.116.185.10";
            Console.WriteLine($"Modbus toolkit started. Using IP = {ip}");
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
            _provider = new ModbusProvider(ip, 502);
            try
            {
                if (!Directory.Exists(args[0])) Directory.CreateDirectory(args[0]);
                _storage.Add(new CsvProvider(args[0], _set));
            }
            catch (Exception ex)
            {
                Console.WriteLine("WARNING: failed to initialize CSV storage provider.");
                Console.WriteLine(ex);
            }
            try
            {
                _storage.Add(new SocketProvider(args.Length > 2 ? int.Parse(args[2]) : 44128, _set));
            }
            catch (Exception ex)
            {
                Console.WriteLine("WARNING: failed to initialize socket storage provider.");
                Console.WriteLine(ex);
            }
            while (!_cancel)
            {
                _changed = false;
                _provider.ReadAll(_set);
                if (_changed) foreach (var item in _storage) item.Store();
                System.Threading.Thread.Sleep(10000);
            }
            try 
            {
                _provider.Dispose();
                foreach (var item in _storage) item.Dispose();
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
            _changed = true;
            var reg = (IModbusRegister)sender;
            Console.WriteLine($"{reg.Name} = {e} -> {reg.GetValue()}");
        }
    }
}
