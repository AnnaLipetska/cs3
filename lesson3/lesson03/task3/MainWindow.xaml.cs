using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.IO.Compression;
using Microsoft.Win32;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace task3
{
    // Напишите приложение для поиска заданного файла на диске. Добавьте код, использующий
    // класс FileStream и позволяющий просматривать файл в текстовом окне.В заключение
    // добавьте возможность сжатия найденного файла.

    public partial class MainWindow : Window
    {
        public List<SelectableText> TheDrivesList { get; set; }


        public MainWindow()
        {
            InitializeComponent();
            GetAllDrives();
            this.DataContext = this;
        }

        private void GetAllDrives()
        {
            TheDrivesList = new List<SelectableText>();

            DriveInfo[] driveInfo = DriveInfo.GetDrives();

            foreach (var drive in driveInfo)
            {
                if (drive.DriveType == DriveType.CDRom)
                {
                    continue;
                }

                TheDrivesList.Add(new SelectableText { IsSelected = false, TheText = drive.Name });
            }
        }

        string file;

        bool SearchFile(string directory, string fileName)
        {
            DirectoryInfo dir = new DirectoryInfo(directory);

            if (!dir.Exists)
            {
                return false;
            }

            FileInfo[] fileInfo = null;
            try
            {
                fileInfo = dir.GetFiles(fileName);
            }
            catch (Exception)
            {
                return false;
            }

            if (fileInfo.Length == 0)
            {
                DirectoryInfo[] dirInfo = dir.GetDirectories();

                if (dirInfo.Length == 0)
                {
                    return false;
                }

                foreach (var item in dirInfo)
                {
                    if (item.Attributes.Equals(FileAttributes.Directory))
                    {
                        continue;
                    }

                    if (SearchFile(item.FullName, fileName))
                    {
                        return true;
                    }
                }
                return false;
            }
            else
            {
                file = fileInfo[0].FullName;
                return true;
            }
        }

        private void Search_Click(object sender, EventArgs e)
        {
            outputTextBlock.Text = string.Empty;

            var selectedDrivesCounter = 0;

            if (!string.IsNullOrEmpty(fileNameTextBox.Text))
            {
                foreach (SelectableText drive in drivesListBox.Items)
                {
                    if (drive.IsSelected == true)
                    {
                        selectedDrivesCounter++;
                        if (SearchFile(drive.TheText, fileNameTextBox.Text))
                        {
                            outputTextBlock.Text = "Файл " + file + " найден!";
                        }
                    }
                }
            }
            else
            {
                outputTextBlock.Text = "Please enter a file name";
            }
            if (string.IsNullOrEmpty(outputTextBlock.Text))
            {
                outputTextBlock.Text = "Файл " + fileNameTextBox.Text + " не найден!";
                if (selectedDrivesCounter == 0)
                {
                    outputTextBlock.Text += Environment.NewLine + "Выберите диски, на которых следует произвести поиск";
                }
            }
        }

        private void Show_Click(object sender, EventArgs e)
        {
            if (System.IO.Path.GetExtension(file) != "txt")
            {
                using (StreamReader reader = File.OpenText(file))
                {
                    outputTextBlock.Text = reader.ReadToEnd();
                }
            }
            else
            {
                outputTextBlock.Text = $"Файл {file} не текстовый, поэтому его невозможно прочитать :(";
            }
        }

        private void Archive_Click(object sender, EventArgs e)
        {
            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Zip files (*.zip)|*.zip|All files (*.*)|*.*";
            if (saveFileDialog.ShowDialog() == true)
            {
                FileStream source = File.OpenRead(file);
                FileStream destination = File.Create(saveFileDialog.FileName);

                using (GZipStream compressor = new GZipStream(destination, CompressionMode.Compress))
                {
                    int theByte = source.ReadByte();
                    while (theByte != -1)
                    {
                        compressor.WriteByte((byte)theByte);
                        theByte = source.ReadByte();
                    }
                    outputTextBlock.Text = "Файл успешно заархивирован";
                }
            }
        }
    }

    public class SelectableText

    {
        public string TheText { get; set; }
        public bool IsSelected { get; set; }
    }
}
