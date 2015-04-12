using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConFM
{
    class DIR_CLASS
    {

        public static Program.eError DIR(string path, bool file = false)
        {
            
            try
            {
                if (!file)
                {
                    Program.sPathsStr = Directory.GetDirectories(path);
                    Console.WriteLine(path);
                    foreach (string p in Program.sPathsStr)
                    {
                        Console.WriteLine("\t {0}", p);
                    }
                    return Program.eError.OK;
                }
                else
                {
                    Program.sPathsStr = Directory.GetFiles(path);
                    Console.WriteLine(path);
                    foreach (string p in Program.sPathsStr)
                    {
                        Console.WriteLine("\t {0}", p);
                    }
                    return Program.eError.OK;
                }
            }
            catch (System.ArgumentException ex)
            {
                return Program.eError.ArgEx;
            }
            catch (System.NotSupportedException ex)
            {
                return Program.eError.NSEx;
            }
            catch (System.IO.DirectoryNotFoundException ex)
            {
                return Program.eError.NfoundEx;
            }
            catch (System.UnauthorizedAccessException Ex)
            {
                return Program.eError.AccessEx;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return Program.eError.Other;
            }
        }

        public static Program.eError DIR_LD()
        {
            try
            {
                Program.sPathsStr = Environment.GetLogicalDrives();
                foreach (string p in Program.sPathsStr)
                {
                    Console.WriteLine("\t {0}", p);
                }
                return Program.eError.OK;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка {0}", ex.ToString());
                return Program.eError.Other;
            }
        }
        public static Program.eError DIR_PROCESS()
        {
            try
            {
                Process[] procList = Process.GetProcesses();
                foreach (Process p in procList)
                {
                    Console.WriteLine("\t {0}", p.ProcessName);
                }
                return Program.eError.OK;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка {0}", ex.ToString());
                return Program.eError.Other;
            }
        }

        public static Program.eError DIR_EXT(string path, string sExt)
        {
            try
            {
                Program.sPathsStr = Directory.GetFiles(path, String.Format("*.{0}", sExt));
                foreach (string p in Program.sPathsStr)
                {
                    Console.WriteLine("\t {0}", p);
                }
                return Program.eError.OK;
            }
            catch (System.ArgumentOutOfRangeException ex)
            {
                Console.WriteLine(ex.ToString());
                return Program.eError.ArgEx;
            }
        }

        public static bool LastSymb(string s, string sSymb)
        {
            if (s.Substring(s.Length - 1, 1) == sSymb)
            {
                return true;
            }
            return false;
        } 
    }

}
