using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZCPU;

namespace ZOS.frontend
{
    public class Kernel
    {
        public static string hostname = "";
        public static string kernel = "ZOS Kernel";
        public static string zos_ver = "0.9.5";
        public static void RunShellFile(CPU c, string file)
        {
            string[] f = File.ReadAllLines(file);
            foreach(var line in f)
            {
                kint(c, line);
            }
        }
        public static void kint(CPU c, string input)
        {
            //string totest = input.Split(' ')[0];
            if (input.StartsWith("echo"))
            {
                Console.WriteLine(input.Replace("echo ", ""));
            }
            //else if (input.StartsWith("ri")) c.initd(c.bitsize);
            else if (input.StartsWith("math"))
            {
                try
                {
                    //buggy
                    var result = Convert.ToUInt64((new DataTable()).Compute(input.Replace("math ", ""), ""));
                    Console.WriteLine((result).ToString());
                    c.nl();
                }
                catch
                {
                    c.panic(CPU.PanicType.matherror);
                }
            }
            else if (input == "shutdown") Environment.Exit(0);
            else if (input.StartsWith("shutdown"))
            {
                if (input.Replace("shutdown ", "") == "-r")
                {
                    c.clear();
                    kmain(c, hostname);
                }
                else Environment.Exit(0);
            }
            //Cringe code
            //else if (input.StartsWith("ver"))
            //{
            //    Console.WriteLine("Z beta 1 codename ready");
            //    c.nl();
            //}
            else if (input.StartsWith("curl")) Console.WriteLine(c.Network.get(input.Replace("curl ", "")));
            else if (input.StartsWith("clear")) c.clear();
            else if (input.StartsWith("exit")) Environment.Exit(0);
            else if (input.StartsWith("run"))
            {
                GAZ gi = new GAZ();
                gi.Run(input.Replace("run ", ""), c);
            }
            else if (input.StartsWith("wget"))
            {
                Console.WriteLine("Getting content...");
                byte[] content = c.Network.getBytes(input.Split(' ')[1]);
                Console.WriteLine("Saving...");
                File.WriteAllBytes(input.Split(' ')[2], content);
                Console.WriteLine("Done!");
            }
            else if (input == ("hostnamectl")) Console.WriteLine("Hostname: " + hostname);
            else if (input.StartsWith("hostnamectl set-hostname "))
            {
                hostname = input.Replace("hostnamectl set-hostname ", "");
                if (File.Exists(Directory.GetCurrentDirectory() + "\\etc\\hostname.cfg")) File.WriteAllText(Directory.GetCurrentDirectory() + "\\etc\\hostname.cfg", hostname);
            }
            else if (input.StartsWith("nano"))
            {
                //Nano n = new Nano(c, input.Replace("nano ", ""));
                //Console.Clear();

                //Very alpha function, not implemented fully yet.
            }
            else if (input.StartsWith("ls"))
            {
                foreach(var i in Directory.GetFiles(Directory.GetCurrentDirectory()))
                {
                    Console.WriteLine(new FileInfo(i).Name);
                }
                var old = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Blue;
                foreach (var i in Directory.GetDirectories(Directory.GetCurrentDirectory()))
                {
                    Console.WriteLine(new DirectoryInfo(i).Name);
                }
                Console.ForegroundColor = old;
            }
            else if (input.StartsWith("cat"))
            {
                Console.WriteLine(File.ReadAllText(Directory.GetCurrentDirectory() + "\\" + input.Replace("cat ", "")));
            }
            else if (input.StartsWith("mkdir"))
            {
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + "\\" + input.Replace("mkdir ", ""));
            }
            else if (input.StartsWith("touch"))
            {
                File.WriteAllText(Directory.GetCurrentDirectory() + "\\" + input.Replace("touch ", ""), "");
            }
            else if (input.StartsWith("sh"))
            {
                RunShellFile(c, Directory.GetCurrentDirectory() + "\\" + input.Replace("sh ", ""));
            }
            else if (input.StartsWith("uname"))
            {
                Console.WriteLine(kernel + " " + zos_ver);
            }
            else if (input.StartsWith("cd"))
            {
                if(Directory.Exists(Directory.GetCurrentDirectory() + "\\" + input.Replace("cd ", "")))
                {
                    Directory.SetCurrentDirectory(Directory.GetCurrentDirectory() + "\\" + input.Replace("cd ", ""));
                }
                else if(input.StartsWith("cd .."))
                {
                    Directory.SetCurrentDirectory(Directory.GetParent(Directory.GetCurrentDirectory()).FullName);
                }
                else
                {
                    Directory.SetCurrentDirectory(input.Replace("cd ", ""));
                }
            }
        }
        public static string get_text_after_last_occurance_of_string(string a, string b)
        {

            return a.Substring(a.LastIndexOf(b));
        }
        public static void kmain(CPU c, string hn)
        {
            hostname = hn;
            Console.WriteLine("Welcome to Z!\n");

            c.nl();
            while (true)
            {
                var old = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("root@" + hostname);
                Console.ForegroundColor = old;
                Console.Write(":");
                Console.ForegroundColor = ConsoleColor.Blue;
                string e;
                if (Directory.GetCurrentDirectory().Contains("\\ZOS")) e = get_text_after_last_occurance_of_string(Directory.GetCurrentDirectory(), "\\ZOS").Replace("\\ZOS", "").Replace("\\", "/");
                else e = Directory.GetCurrentDirectory();
                if (e.Length>0) 
                {
                    if(e[0]=='/')
                    {
                        var k = e.ToList();
                        k.RemoveAt(0);
                        e = new string(k.ToArray());
                    }
                }
                if (!e.Contains(":")) e = "/" + e;
                Console.Write(e);
                Console.ForegroundColor = old;
                Console.Write("$ ");
                //Memclean is not needed here as c.prnt is deprecated
                c.rde();
                kint(c, intarrtostr(c.rd()));
                //IMPORTANT! If the memclean wasnt there, the new command would overwrite the new one.
                c.memclean(0, c.rd().Length);
            }
        }
        public static string intarrtostr(long[] intarr)
        {
            string a = "";
            foreach (long i in intarr) a += (char)i;
            return a;
        }
    }
}
