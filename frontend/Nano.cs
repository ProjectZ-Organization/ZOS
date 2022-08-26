using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCPU;
namespace ZOS.frontend
{
    class Nano
    {

        public Nano(CPU c, string filename)
        {
            var lines = File.ReadAllLines(filename);
            Run(lines);
        }

        public void Run(string[] lines)
        {
            var m = lines.ToList();
            int line = 0;
            while (true)
            {
                foreach (var i in m)
                {
                    Console.WriteLine(i);
                }
                var a = Console.ReadKey();
                if (a.Key == ConsoleKey.Backspace)
                {
                    var oldl = Console.CursorLeft;
                    var oldt = Console.CursorTop;
                    bool combinelines = false;
                    if(oldl == 0)
                    {
                        combinelines = true;
                    }
                    Console.Clear();
                    for(int i = 0; i < m.Count(); i++)
                    {
                        if(i == line)
                        {
                            if(combinelines)
                            {
                                m[i] = m[i] + m[i + 1];
                                m.RemoveAt(i+1);
                                i++;
                                continue;
                            }
                            else
                            {
                                string line_ = m[i];
                                var k = line_.ToList();
                                k.RemoveAt(oldl);
                                line_ = new string(k.ToArray());
                                m[i] = line_;
                            }
                        }
                    }
                    foreach (var i in m)
                    {
                        Console.WriteLine(i);
                    }
                }
            }
        }
    }
}
