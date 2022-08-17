using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZCPU;
namespace ZOS
{
    public class Kernel
    {
        public static void kernel_read(CPU c)
        {

            string input = intarrtostr(c.rd());
            string totest = input.Split(' ')[0];
            if (input.StartsWith("echo "))
            {
                c.prnt(input.Replace("echo ", ""));
                c.nl();
            }
            else if (input.StartsWith("ri"))
            {
                c.initd(c.bitsize);
            }
            else if (input.StartsWith("math "))
            {
                try
                {
                    var result = Convert.ToUInt64((new DataTable()).Compute(input.Replace("math ", ""), ""));
                    c.prnt((result).ToString());
                    c.nl();
                }
                catch
                {
                    c.panic(CPU.PanicType.matherror);
                }
            }
            else if (input == "shutdown")
            {
                Environment.Exit(0);
            }
            else if (input.StartsWith("shutdown"))
            {
                if (input.Replace("shutdown ", "") == "-r")
                {
                    c.clear();
                    kernel_main(c);
                }
                else
                {
                    Environment.Exit(0);
                }
            }
            else if (input.StartsWith("ver"))
            {
                c.prnt("Z beta 1 codename ready");
                c.nl();
            }
            else if (input.StartsWith("gTest"))
            {

                int a = Console.BufferHeight / 3;
                Color[] cc = new Color[3] { Color.White, Color.Red, Color.Black };
                for (int x = 0; x < Console.BufferWidth; x++)
                {
                    for (int y = 0; y < Console.BufferHeight; y++)
                    {
                        c.Display.setPixel(cc[Convert.ToInt32(Math.Floor((double)y / a))], x, y);
                    }
                }
                System.Threading.Thread.Sleep(3000);
                Color bgc = Color.Black;
                for (int x = 0; x < Console.BufferWidth; x++)
                {
                    for (int y = 0; y < Console.BufferHeight; y++)
                    {
                        c.Display.setPixel(bgc, x, y);
                    }
                }
                Console.SetCursorPosition(0, 0);


            }
            else if (input.StartsWith("curl"))
            {
                c.prnt(c.Network.get(input.Replace("curl ", "")));

            }
            else if (input.StartsWith("clear"))
            {
                c.clear();
            }
            else if (input.StartsWith("exit"))
            {
                Environment.Exit(0);
            }
            else if (input.StartsWith("run"))
            {

                GAZ gi = new GAZ();
                gi.Run(input.Replace("run ", ""), c);
            }

        }
        public static void kernel_main(CPU c)
        {

            c.prnt("Welcome to Z!\n");

            c.nl();
            while (true)
            {
                c.prnt("> ", false);
                c.memclean(0, 30);
                c.rde();
                kernel_read(c);
            }
        }
        public static string intarrtostr(long[] intarr)
        {
            string a = "";
            foreach (long i in intarr)
            {
                a += (char)i;
            }
            return a;
        }
    }
}
