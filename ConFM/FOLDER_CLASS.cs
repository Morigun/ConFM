using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConFM
{
    class FOLDER_CLASS
    {
        public static Program.eError CREATE_DIR(string sName)
        {
            try
            {
                Directory.CreateDirectory(sName);
                return Program.eError.OK;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при создании директории: {0}", ex.ToString());
                return Program.eError.Other;
            }
        }

        public static Program.eError DELETE_DIR(string sName)
        {
            try
            {
                Directory.Delete(sName);
                return Program.eError.OK;
            }
            catch (System.IO.IOException ex)
            {
                string sSymb;
                Console.WriteLine("Папка содержит файлы, удалить? y/n");
                sSymb = Console.ReadLine();
                if (sSymb.Substring(0, 1).ToUpper() == "Y")
                    Directory.Delete(sName, true);
                return Program.eError.OK;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при удалении директории: {0}", ex.ToString());
                return Program.eError.Other;
            }
        }
    }
}
