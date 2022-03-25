using NOxAcquisition;
using System;
using System.Net.Sockets;
using System.Text.Json;
using System.Text;

public class SocketProvider : StorageProviderBase
{
    UdpClient _Client;

    public int Socket { get; private set; } = -1;

    public SocketProvider(int socket, RegisterSet regs) : base(regs)
    {
        try
        {
            _Client = new UdpClient(socket);
            Socket = socket;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }

    public async override void Store()
    {
        if (_Client == null) return;
        string json = JsonSerializer.Serialize(_regs);
        var buf = Encoding.ASCII.GetBytes(json);
        await _Client.SendAsync(buf, buf.Length);
    }

    public override void Dispose()
    {
        if (_Client != null) _Client.Dispose();
    }
}