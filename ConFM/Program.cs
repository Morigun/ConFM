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
        static string sCon;
        const string sRoot = @"C:\";
        public static string[] sPathsStr;
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
                string otherPat = @"_[a-zA-Z]+\s?";
                sTmpPath = sTecDir;
                /*Иди через разветвление после _*/
                Match m = Regex.Match(sCon, pattern);
                while (m.Success)
                {
                    switch (m.Value.ToUpper())
                    {
                        case "HELP":
                            Console.WriteLine("Основные команды: ");
                            Console.WriteLine("\t DIR_ROOT - Просмотреть корневой каталог C:\\;");
                            Console.WriteLine("\t DIR - Просмотреть текущий каталог;");
                            Console.WriteLine("\t DIR_FILES - Просмотр файлов в каталоге;");
                            Console.WriteLine("\t DIR_LD - Просмотр логических дисков;");
                            Console.WriteLine("\t DIR_PROCESS - Просмотр списка процессов;");
                            Console.WriteLine("\t DIR_EXT [NAME EXT] - поиск по расширению;");
                            Console.WriteLine("\t CD [NAME_DIRECTORY] - Переход в директорию;");
                            Console.WriteLine("\t CD.. - Перейти в каталог выше;");
                            Console.WriteLine("\t RUN [NAME_FILE] - Запустить приложение;");
                            Console.WriteLine("\t EXIT - Выход из программы;");
                            break;
                        case "DIR":
                            Match m2 = Regex.Match(sCon.Substring(3).ToUpper(), otherPat);
                            if (m2.Success)
                            {
                                switch(m2.Value.ToUpper())
                                {
                                    case "_ROOT" :
                                        DIR_CLASS.DIR(sRoot);
                                        break;
                                    case "_FILES":
                                        DIR_CLASS.DIR(sTecDir, true);
                                        break;
                                    case "_LD":
                                        if (DIR_CLASS.DIR_LD() != DIR_CLASS.eError.OK)
                                            Console.WriteLine("Ошибка");
                                        break;
                                    case "_PROCESS":
                                        if (DIR_CLASS.DIR_PROCESS() != DIR_CLASS.eError.OK)
                                            Console.WriteLine("Ошибка");
                                        break;
                                    case "_EXT ":
                                        if (DIR_CLASS.DIR_EXT(sTecDir, sCon.ToUpper().Substring(8)) != DIR_CLASS.eError.OK)
                                            Console.WriteLine("Ошибка {0}", sCon.ToUpper().Substring(8));
                                        break;
                                    case "IN_FILE ":
                                        
                                        break;
                                }
                            }
                            else
                            {
                                DIR_CLASS.DIR(sTecDir);
                            }
                            break;
                        case "DIR ":
                            
                            break;
                        case "CD..":
                            try
                            {
                                sTecDir = Directory.GetParent(sTecDir.Substring(0, sTecDir.Length-1)).ToString() + @"\";
                                DIR_CLASS.DIR(sTecDir);
                            }
                            catch (System.NullReferenceException)
                            {
                                Console.WriteLine("Нет директории выше.");
                            }
                            break;
                        case "CD ":
                            string sTmp;
                            sTmp = sCon.Substring(3).ToUpper();
                            if(!DIR_CLASS.LastSymb(sTmp,@"\"))
                            {
                                 sTmp += @"\";
                            }
                            try
                            {
                                if (sTecDir != sRoot)
                                    sTecDir = sTecDir + sTmp;
                                else
                                    sTecDir = sTecDir + sTmp;
                                switch (DIR_CLASS.DIR(sTecDir))
                                {
                                    case DIR_CLASS.eError.ArgEx:
                                        sTecDir = sTmp;
                                        DIR_CLASS.DIR(sTecDir);
                                        break;
                                    case DIR_CLASS.eError.NSEx:
                                        sTecDir = sTmp;
                                        DIR_CLASS.DIR(sTecDir);
                                        break;
                                    case DIR_CLASS.eError.NfoundEx:                                        
                                        Console.WriteLine("Проверьте корректность пути: {0}", sTecDir);
                                        sTecDir = sTmpPath;
                                        break;
                                    case DIR_CLASS.eError.AccessEx:                                            
                                        Console.WriteLine("Нет доступа до папки {0}", sTecDir);
                                        sTecDir = sTmpPath;
                                        break;
                                    case DIR_CLASS.eError.Other:
                                        Console.WriteLine("Необработанное исключение");
                                        break;
                                }
                            }
                            catch (System.NotSupportedException ex)
                            {
                                sTecDir = sTmp;
                                DIR_CLASS.DIR(sTecDir);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Нет такого пути");
                            }
                            break;
                        case "RUN ":                            
                            sOnRun = String.Format("{0}{1}", sTecDir, sCon.Substring(m.Value.Length));
                            switch (RUN_CLASS.RunOutApp(sOnRun))
                            {
                                case RUN_CLASS.eError.Other:
                                    Console.WriteLine("Ошибка");
                                    break;
                                case RUN_CLASS.eError.Win32Ex:
                                    Console.WriteLine("Проверь корректность введеного имени файла. Файл с именем {0} не найден.", sOnRun);
                                    break;
                                case RUN_CLASS.eError.OK:
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
    }
}
