using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

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
            Other
        }
        static string sCon;
        const string sRoot = @"C:\";
        static string[] sPathsStr;
        static string sTecDir;
        static void Main(string[] args)
        {
            sTecDir = sRoot;
            while(true)
            {
                sCon = Console.ReadLine();
                string pattern = @"^[a-zA-Z]+[\s,\.\.]*";
                string otherPat = @"_[a-zA-Z]+";
                string sCdPat = @"[a-zA-Zа-яА-Я]+";
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
                            Console.WriteLine("TUT..");
                            try
                            {
                                sTecDir = Directory.GetParent(sTecDir.Substring(0, sTecDir.Length-1)).ToString();
                                DIR(sTecDir);
                            }
                            catch (System.NullReferenceException)
                            {
                                Console.WriteLine("Нет директории выше.");
                            }
                            break;
                        case "CD ":
                            Match m3 = Regex.Match(sCon.Substring(2).ToUpper(), sCdPat);
                            string sTmp;
                            if(m3.Success)
                            {
                                sTmp = m3.Value;
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
                            }
                            break;
                        case "RUN":
                            sTecDir += sCon.Substring(m.Value.Length + 1);
                            switch(RunOutApp(sTecDir))
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
                /*if (sCon.Length > 3)
                {                    
                    if (sCon.Substring(0, 3).ToUpper() == "CD ")
                    {
                        if (!LastSymb(sCon.Substring(3), @"\"))
                            sCon += @"\";                            
                        try
                        {
                            if (sTecDir != sRoot)
                                sTecDir = sTecDir + sCon.Substring(3);
                            else
                                sTecDir = sTecDir + sCon.Substring(3);
                            switch(DIR(sTecDir))
                            {
                                case eError.ArgEx :
                                    sTecDir = sCon.Substring(3);
                                    DIR(sTecDir);
                                    break;
                                case eError.NSEx :
                                    sTecDir = sCon.Substring(3);
                                    DIR(sTecDir);
                                    break;
                                case eError.NfoundEx :
                                    Console.WriteLine("Проверьте корректность пути: {0}", sTecDir);
                                    break;
                                case eError.Other :
                                    Console.WriteLine("Необработанное исключение");
                                    break;
                            }                          
                        }
                        catch(System.NotSupportedException ex)
                        {
                            sTecDir = sCon.Substring(3);
                            DIR(sTecDir);
                        }                        
                        catch(Exception ex)
                        {
                            Console.WriteLine("Нет такого пути");
                        }
                    }
                    else if (sCon.Substring(0, 4).ToUpper() == "CD..")
                    {
                        try
                        {
                            sTecDir = Directory.GetParent(sTecDir).ToString();
                            DIR(sTecDir);
                        }
                        catch(System.NullReferenceException)
                        {
                            Console.WriteLine("Нет директории выше.");
                        }
                    }
                }*/
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
