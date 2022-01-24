using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using NModbus;

namespace NOxAcquisition
{
    public class ModbusProvider : IDisposable
    {
        public ModbusProvider(string host, int port)
        {
            _master = new ModbusFactory().CreateMaster(new TcpClient(host, port));
        }

        public byte SlaveId { get; set; } = 1;

        public void ReadRegister(IModbusRegister r)
        {
            switch (r.Type)
            {
                case RegisterTypes.DiscreteInput:
                    throw new NotImplementedException();
                case RegisterTypes.Input:
                    r.SetValue(_master.ReadInputRegisters(SlaveId, r.Address, r.Length));
                    break;
                default:
                    throw new ArgumentException("Invalid register type.");
            }
        }

        public void ReadAll(RegisterSet s)
        {
            foreach (var item in s.GetAll())
            {
                try
                {
                    ReadRegister(item);
                }
                catch (NotImplementedException)
                {

                }
            }
        }

        public void Dispose()
        {
            _master.Dispose();
        }

        private IModbusMaster _master;
    }
}
