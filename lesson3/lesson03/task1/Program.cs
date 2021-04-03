using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace task1
{
    class Program
    {
        // Создайте на диске 100 директорий с именами от Folder_0 до Folder_99, затем удалите их.
        static void Main(string[] args)
        {
            DriveInfo[] drives = DriveInfo.GetDrives();

            string lastDriveName = string.Empty;
            for (int i = drives.Count() - 1; i >= 0; i--)
            {
                if (drives[i].DriveType == DriveType.CDRom)
                {
                    continue;
                }
                lastDriveName = (drives[i]).Name;
                break;
            }

            DirectoryInfo directory = new DirectoryInfo(lastDriveName);

            if (directory.Exists)
            {
                var directoryInfos = new List<DirectoryInfo>();
                for (int i = 0; i < 100; i++)
                {
                    directoryInfos.Add(directory.CreateSubdirectory("Folder_" + i));
                }

                foreach (var directoryInfo in directoryInfos)
                {
                    Console.WriteLine(string.Format(@"Директория {0} успешно создана", directoryInfo.Name));
                    Console.Write(Environment.NewLine);

                    Console.WriteLine("FullName    : {0}", directoryInfo.FullName);
                    Console.WriteLine("Name        : {0}", directoryInfo.Name);
                    Console.WriteLine("Parent      : {0}", directoryInfo.Parent);
                    Console.WriteLine("CreationTime: {0}", directoryInfo.CreationTime);
                    Console.WriteLine("Attributes  : {0}", directoryInfo.Attributes.ToString());
                    Console.WriteLine("Root        : {0}", directoryInfo.Root);

                    Console.Write(Environment.NewLine);
                    Console.WriteLine(new string('-', 30));
                }

                Console.ReadKey();

                for (int i = 0; i < 100; i++)
                {
                    try
                    {
                        Directory.Delete($"{lastDriveName}Folder_{i}", true);
                        Console.WriteLine($"Каталог Folder_{i} успешно удален.");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

                Console.ReadKey();
            }
        }
    }
}
