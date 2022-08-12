using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZFX;
namespace ZOS
{
    class Program
    {
        /// <summary>
        /// Start the os
        /// </summary>
        /// <param name="c">CPU instance.</param>
        public static void _start(CPU c)
        {
            c.Display = new Display();
            c.Network = new Network();
            Console.Clear();
            Kernel.kernel_main(c);
        }
        static void Main(string[] args)
        {
            string startingmsg = "Z is starting...";
            int x = Console.BufferHeight / 2;
            int y = Console.BufferWidth / 2 - (startingmsg.Length / 2);
            Console.SetCursorPosition(y, x - 2);
            Console.Write(startingmsg);
            CPU c = new CPU(2000);
            _start(c);
        }
    }
}