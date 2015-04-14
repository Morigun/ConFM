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
                            Console.WriteLine("\t CREATE_FILE [NAME_FILE].[EXT] - Создать файл;");
                            Console.WriteLine("\t DELETE_FILE [NAME_FILE].[EXT] - Удалить файл;");
                            Console.WriteLine("\t CREATE_DIR [NAME_DIR] - Создать папку;");
                            Console.WriteLine("\t DELETE_DIR [NAME_DIR] - Удалить папку;");
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
                                        if (DIR_CLASS.DIR_LD() != eError.OK)
                                            Console.WriteLine("Ошибка");
                                        break;
                                    case "_PROCESS":
                                        if (DIR_CLASS.DIR_PROCESS() != eError.OK)
                                            Console.WriteLine("Ошибка");
                                        break;
                                    case "_EXT ":
                                        if (DIR_CLASS.DIR_EXT(sTecDir, sCon.ToUpper().Substring(8)) != eError.OK)
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
                        case "CREATE":
                            Match m3 = Regex.Match(sCon.Substring(6).ToUpper(), otherPat);
                            if (m3.Success)
                            {
                                switch (m3.Value.ToUpper())
                                {
                                    case "_FILE ":
                                        FILE_CLASS.Create_file(String.Format("{0}{1}", sTecDir, sCon.ToUpper().Substring(12)));
                                        break;
                                    case "_DIR ":
                                        FOLDER_CLASS.CREATE_DIR(String.Format("{0}{1}", sTecDir, sCon.ToUpper().Substring(11)));
                                        break;
                                }
                            }
                            break;
                        case "DELETE":
                            Match m4 = Regex.Match(sCon.Substring(6).ToUpper(), otherPat);
                            if (m4.Success)
                            {
                                switch (m4.Value.ToUpper())
                                {
                                    case "_FILE ":
                                        FILE_CLASS.Delete_file(String.Format("{0}{1}", sTecDir, sCon.ToUpper().Substring(12)));
                                        break;
                                    case "_DIR ":
                                        FOLDER_CLASS.DELETE_DIR(String.Format("{0}{1}", sTecDir, sCon.ToUpper().Substring(11)));
                                        break;
                                }
                            }
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
                                    case eError.ArgEx:
                                        sTecDir = sTmp;
                                        DIR_CLASS.DIR(sTecDir);
                                        break;
                                    case eError.NSEx:
                                        sTecDir = sTmp;
                                        DIR_CLASS.DIR(sTecDir);
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
                                case eError.Other:
                                    Console.WriteLine("Ошибка");
                                    break;
                                case eError.Win32Ex:
                                    Console.WriteLine("Проверь корректность введеного имени файла. Файл с именем {0} не найден.", sOnRun);
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
    }
}
