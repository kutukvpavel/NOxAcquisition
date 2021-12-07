using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NOxAcquisition
{
    public static class Extensions
    {
        public static byte[] GetBytes(this ushort[] registers)
        {
            return registers.Reverse().Select(x => BitConverter.GetBytes(x)).Flatten().Cast<byte>().ToArray();
        }

        public static void LogHex(this byte[] b)
        {
            Console.WriteLine(string.Join(' ', b.Select(x => x.ToString("X2"))));
        }
    }
}
