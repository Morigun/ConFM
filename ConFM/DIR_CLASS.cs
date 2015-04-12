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
        public enum eError
        {
            OK,
            ArgEx,
            NSEx,
            NfoundEx,
            AccessEx,
            Win32Ex,
            Other
        }

        public static eError DIR(string path, bool file = false)
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
                    return eError.OK;
                }
                else
                {
                    Program.sPathsStr = Directory.GetFiles(path);
                    Console.WriteLine(path);
                    foreach (string p in Program.sPathsStr)
                    {
                        Console.WriteLine("\t {0}", p);
                    }
                    return eError.OK;
                }
            }
            catch (System.ArgumentException ex)
            {
                return eError.ArgEx;
            }
            catch (System.NotSupportedException ex)
            {
                return eError.NSEx;
            }
            catch (System.IO.DirectoryNotFoundException ex)
            {
                return eError.NfoundEx;
            }
            catch (System.UnauthorizedAccessException Ex)
            {
                return eError.AccessEx;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return eError.Other;
            }
        }

        public static eError DIR_LD()
        {
            try
            {
                Program.sPathsStr = Environment.GetLogicalDrives();
                foreach (string p in Program.sPathsStr)
                {
                    Console.WriteLine("\t {0}", p);
                }
                return eError.OK;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка {0}", ex.ToString());
                return eError.Other;
            }
        }
        public static eError DIR_PROCESS()
        {
            try
            {
                Process[] procList = Process.GetProcesses();
                foreach (Process p in procList)
                {
                    Console.WriteLine("\t {0}", p.ProcessName);
                }
                return eError.OK;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка {0}", ex.ToString());
                return eError.Other;
            }
        }

        public static eError DIR_EXT(string path, string sExt)
        {
            try
            {
                Program.sPathsStr = Directory.GetFiles(path, String.Format("*.{0}", sExt));
                foreach (string p in Program.sPathsStr)
                {
                    Console.WriteLine("\t {0}", p);
                }
                return eError.OK;
            }
            catch (System.ArgumentOutOfRangeException ex)
            {
                Console.WriteLine(ex.ToString());
                return eError.ArgEx;
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
