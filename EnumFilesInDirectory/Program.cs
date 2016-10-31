using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace EnumFilesInDirectory
{
    class Program
    {
        static String[] m_FilesMask = {".exe", ".dll"};

        private static bool isAccess(string path)
        {
            try
            {
                DirectoryInfo dir = new DirectoryInfo(path);

                foreach (String s in m_FilesMask)
                {
                    FileInfo[] files = dir.GetFiles("*.*");
                }
                return true;
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine("Error: access to \"" + path + "\" is denied.");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
        }

        private static string GetFileVersion(string fileName)
        {
            return FileVersionInfo.GetVersionInfo(fileName).ProductVersion;
        }

        private static void PrintFilesList(string path)
        {
            if (!isAccess(path))
            {
                return;
            }

            DirectoryInfo dir = new DirectoryInfo(path);
            foreach (FileInfo file in dir.GetFiles("*.*"))
            {
                String fullName = file.FullName;
                foreach (String ext in m_FilesMask)
                {
                    if (fullName.ToLower().EndsWith(ext.ToLower()))
                    {
                        Console.WriteLine(fullName + " v " + GetFileVersion(fullName));
                    }
                }
            }

            foreach (DirectoryInfo di in dir.GetDirectories("*.*"))
            {
                    PrintFilesList(di.FullName);
            }

        }

        static void Main(string[] args)
        {
            if (args.Length <= 0)
            {
                String appName = System.Reflection.Assembly.GetEntryAssembly().GetName().Name;
                Console.WriteLine("Error: Wrong input parameters");
                Console.WriteLine(appName + " path");
                return;
            }

            String path = args[0];
            if (!(isAccess(path)))
            {
                Console.WriteLine("Error: \"" + path + "\": access denied!");
                return;
            }

            PrintFilesList(path);

            Console.WriteLine("OK.");
            Console.ReadLine();
        }
    }
}
