using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace task2
{
    // Создайте файл, запишите в него произвольные данные и закройте файл. Затем снова откройте
    // этот файл, прочитайте из него данные и выведете их на консоль.
    class Program
    {
        static void Main(string[] args)
        {
            var fileName = "MyFile.txt";

            StreamWriter writer = File.CreateText(fileName);
            writer.WriteLine("Произвольные данные");
            writer.Close();

            Console.ReadKey();

            StreamReader reader = File.OpenText(fileName);
            Console.WriteLine(reader.ReadToEnd());
            reader.Close();

            Console.ReadKey();
        }
    }
}
