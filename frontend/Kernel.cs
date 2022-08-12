using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZFX;

namespace ZOS
{
    class Kernel
    {
        public static void kernel_read(CPU c)
        {

            string input = intarrtostr(c.rd());
            string totest = input.Split(' ')[0];
            if (totest == "su") { c.ring = 0; }
            if (input.StartsWith("echo "))
            {
                c.prnt(input.Replace("echo ", ""));
                c.nl();
            }
            else if (input.StartsWith("ri"))
            {
                c.initd(c.bitsize,c.ring);
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
                if(c.ring == 0)
                {
                    Environment.Exit(0);
                }else
                {
                    c.panic(CPU.PanicType.permdenied);
                }
            }else if (input.StartsWith("shutdown"))
            {
                if (input.Replace("shutdown ", "") == "-r")
                {
                    c.clear();
                    kernel_main(c);
                }else
                {
                    Environment.Exit(0);
                }
            }else if(input.StartsWith("gTest"))
            {
                if (c.ring == 0)
                {
                    int a = Console.WindowHeight / 3;
                    Color[] cc = new Color[3] { Color.White, Color.Red, Color.Black };
                    for (int x = 0; x < Console.WindowWidth; x++)
                    {
                        for (int y = 0; y < Console.WindowHeight; y++)
                        {
                            c.Display.setPixel(cc[Convert.ToInt32(Math.Floor((double)y / a))], x, y);
                        }
                    }
                    System.Threading.Thread.Sleep(3000);
                    Color bgc = Color.Black;
                    for (int x = 0; x < Console.WindowWidth; x++)
                    {
                        for (int y = 0; y < Console.WindowHeight; y++)
                        {
                            c.Display.setPixel(bgc, x, y);
                        }
                    }
                    Console.SetCursorPosition(0, 0);
                }else
                {
                    c.prnt("You do not have permission to run this command!");
                }

            }else if(input.StartsWith("curl"))
            {
                c.prnt(c.Network.get(input.Replace("curl ", "")));
                
            }else if (input.StartsWith("clear"))
            {
                c.clear();
            }else if(input.StartsWith("exit"))
            {
                if (c.ring == 0)
                {
                    c.ring = 3;
                }else
                {
                    Environment.Exit(0);
                }
            }
            

        }
        public static void kernel_main(CPU c)
        {

            c.prnt("Welcome to Z!\n");

            c.nl();
            while (true)
            {
                string usm = "user";
                if(c.ring == 0)
                {
                    usm = "root";
                }
                c.prnt(usm + "> ", false);
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
