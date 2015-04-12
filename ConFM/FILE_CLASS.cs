using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ConFM
{
    class FILE_CLASS
    {
        static string sQ;
        static bool bCreate = false;
        public static Program.eError Create_file(string sName)
        {
            if(File.Exists(sName))
            {                
                Console.WriteLine("Файл существует, удалить? ");
                sQ = Console.ReadLine();
                if (sQ.ToUpper().Substring(0, 1) == "Y")
                    File.Delete(sName);
                else
                    bCreate = false;
            }
            if (!bCreate)
            {
                using (FileStream fs = File.Create(sName))
                {
                    Console.WriteLine("Файл создан: {0}", sName);
                }
                return Program.eError.OK;
            }
            Console.WriteLine("Файл уже существует");
            return Program.eError.Other;            
        }

        public static Program.eError Delete_file(string sName)
        {
            if (File.Exists(sName))
            {
                File.Delete(sName);
            }
            return Program.eError.OK;
        }
    }
}
