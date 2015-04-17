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
        static string sQ;
        static bool bCreate = false;
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

        public static Program.eError MOVE_DIR(string sName, string sNewPath, bool bMove = false)
        {
            if (Directory.Exists(sNewPath))
            {
                Console.WriteLine("Папка существует, удалить? ");
                sQ = Console.ReadLine();
                if (sQ.ToUpper().Substring(0, 1) == "Y")
                    Directory.Delete(sNewPath);
                else
                    bCreate = false;
            }
            if (bMove)
            {
                Directory.Move(sName, sNewPath);
            }
            else
            {
                DirectoryCopy(sName, sNewPath, bMove);//Directory.Copy(sName, sNewPath);
            }
            return Program.eError.OK;
        }

        private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);
            DirectoryInfo[] dirs = dir.GetDirectories();

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, false);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }
    }
}
