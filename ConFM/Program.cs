using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.Security.Principal;
using System.Security.AccessControl;

namespace ConFM
{
    class Program
    {
        enum eError
        {
            OK,
            ArgEx,
            NSEx,
            NfoundEx,
            AccessEx,
            Other
        }
        static string sCon;
        const string sRoot = @"C:\";
        static string[] sPathsStr;
        static string sTecDir;
        static string sTmpPath;
        static string sOnRun;
        static void Main(string[] args)
        {
            sTecDir = sRoot;
            while(true)
            {
                sCon = Console.ReadLine();
                string pattern = @"^[a-zA-Z]+[\s,\.\.]*";
                string otherPat = @"_[a-zA-Z]+";
                sTmpPath = sTecDir;
                /*Иди через разветвление после _*/
                Match m = Regex.Match(sCon, pattern);
                while (m.Success)
                {
                    switch (m.Value.ToUpper())
                    {
                        case "HELP":
                            Console.WriteLine("Привет, это начальная версия моей программы");
                            Console.WriteLine(" dir_root - Просмотреть корневой каталог;");
                            Console.WriteLine(" dir - Просмотреть текущий каталог;");
                            Console.WriteLine(" cd [Name_directory] - Переход в директорию;");
                            Console.WriteLine(" cd.. - Перейти в каталог выше;");
                            Console.WriteLine(" dir_files - Просмотр файлов в каталоге;");
                            Console.WriteLine(" dir_ld - Просмотр логических дисков;");
                            Console.WriteLine(" run - Запустить приложение;");
                            Console.WriteLine(" exit - Выход из программы;");
                            break;
                        case "DIR":
                            Match m2 = Regex.Match(sCon.Substring(3).ToUpper(), otherPat);
                            if (m2.Success)
                            {
                                switch(m2.Value.ToUpper())
                                {
                                    case "_ROOT" :
                                        DIR(sRoot);
                                        break;
                                    case "_FILES":
                                        DIR(sTecDir, true);
                                        break;
                                }
                            }
                            else
                            {
                                DIR(sTecDir);
                            }
                            break;
                        case "CD..":
                            try
                            {
                                sTecDir = Directory.GetParent(sTecDir.Substring(0, sTecDir.Length-1)).ToString() + @"\";
                                DIR(sTecDir);
                            }
                            catch (System.NullReferenceException)
                            {
                                Console.WriteLine("Нет директории выше.");
                            }
                            break;
                        case "CD ":
                            string sTmp;
                            sTmp = sCon.Substring(3).ToUpper();
                            if(!LastSymb(sTmp,@"\"))
                            {
                                 sTmp += @"\";
                            }
                            try
                            {
                                if (sTecDir != sRoot)
                                    sTecDir = sTecDir + sTmp;
                                else
                                    sTecDir = sTecDir + sTmp;
                                switch (DIR(sTecDir))
                                {
                                    case eError.ArgEx:
                                        sTecDir = sTmp;
                                        DIR(sTecDir);
                                        break;
                                    case eError.NSEx:
                                        sTecDir = sTmp;
                                        DIR(sTecDir);
                                        break;
                                    case eError.NfoundEx:                                        
                                        Console.WriteLine("Проверьте корректность пути: {0}", sTecDir);
                                        sTecDir = sTmpPath;
                                        break;
                                    case eError.AccessEx:                                            
                                        Console.WriteLine("Нет доступа до папки {0}", sTecDir);
                                        sTecDir = sTmpPath;
                                        break;
                                    case eError.Other:
                                        Console.WriteLine("Необработанное исключение");
                                        break;
                                }
                            }
                            catch (System.NotSupportedException ex)
                            {
                                sTecDir = sTmp;
                                DIR(sTecDir);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Нет такого пути");
                            }
                            break;
                        case "RUN ":                            
                            sOnRun = String.Format("{0}{1}", sTecDir, sCon.Substring(m.Value.Length));
                            switch (RunOutApp(sOnRun))
                            {
                                case eError.Other:
                                    Console.WriteLine("Ошибка");
                                    break;
                                case eError.OK:
                                    Console.WriteLine("Приложение запущено.");
                                    break;
                            }
                            break;
                        case "EXIT":
                            Environment.Exit(0);
                            break;
                        default:
                            Console.WriteLine("DEF {0}1",m.Value.ToUpper());
                            break;
                    }
                    m = m.NextMatch();
                }                
                Console.Write("{0}>",sTecDir);
            }
        }

        static eError DIR(string path, bool file = false)
        {
            try
            {
                if (!file)
                {
                    sPathsStr = Directory.GetDirectories(path);
                    Console.WriteLine(path);
                    foreach (string p in sPathsStr)
                    {
                        Console.WriteLine("\t {0}", p);
                    }
                    return eError.OK;
                }
                else
                {
                    sPathsStr = Directory.GetFiles(path);
                    Console.WriteLine(path);
                    foreach (string p in sPathsStr)
                    {
                        Console.WriteLine("\t {0}", p);
                    }
                    return eError.OK;
                }
            }
            catch(System.ArgumentException ex)
            {
                return eError.ArgEx;
            }
            catch(System.NotSupportedException ex)
            {
                return eError.NSEx;
            }
            catch(System.IO.DirectoryNotFoundException ex)
            {
                return eError.NfoundEx;
            }
            catch(System.UnauthorizedAccessException Ex)
            {
                return eError.AccessEx;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return eError.Other;
            }
        }

        static eError RunOutApp(string sPathApp)
        {
            try
            {
                Process.Start(sPathApp);
                return eError.OK;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return eError.Other;
            }
        }

        static bool LastSymb(string s, string sSymb)
        {
            if(s.Substring(s.Length-1, 1) == sSymb)
            {
                return true;
            }
            return false;
        }       
        
    }
}
