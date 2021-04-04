using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
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

namespace task4
{
    public partial class MainWindow : Window
    {
        // Создайте приложение WPF Application, позволяющее пользователям сохранять данные в
        // изолированное хранилище.
        // Для выполнения этого задания необходимо наличие библиотеки Xceed.Wpf.Toolkit.dll. Ее
        // можно получить через References -> Manage NuGet Packages… -> в поиске написать Extended
        // WPF Toolkit (помимо интересующей нас библиотеки будут установлены и другие), или же
        // скачать непосредственно на сайте http://wpftoolkit.codeplex.com/ и подключить в проект только
        // интересующую нас бибилиотеку (References -> Add Reference …).
        // 1. Разместите в окне Label и Button.
        // 2. Разместите в окне ColorPicker (данный инструмент предоставляется вышеуказанной
        // библиотекой). Для этого необходимо в XAML коде в теге Window подключить пространство
        // имен xmlns:xctk="http://schemas.xceed.com/wpf/xaml/toolkit" . Также, нам понадобиться событие
        // Loaded окна.
        // 3. Реализуйте, чтобы при выборе цвета из ColorPicker в Label выводилось название
        // выбранного цвета и в этот цвет закрашивался фон Label. По нажатию на кнопку, данные о
        // цвете сохраняются в изолированное хранилище. При запуске приложения заново, цвет фона
        // Label остается таким, каким был сохранен при предыдущих запусках приложения.
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) 
        {
            var userStorage = IsolatedStorageFile.GetUserStoreForAssembly();

            var settingsFileName = "UserSettings.set";
            if (userStorage.GetFileNames(settingsFileName).Any())
            {
                IsolatedStorageFileStream userStream = new IsolatedStorageFileStream(settingsFileName, FileMode.OpenOrCreate, userStorage);

                using (var userReader = new StreamReader(userStream))
                {
                    try
                    {
                        myColorPicker.SelectedColor = ((SolidColorBrush)new BrushConverter().ConvertFromString(userReader.ReadToEnd())).Color;
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Будет установлен цвет по умолчанию.");
                    }
                }
            }
        }

        private void SaveColor_Click(object sender, RoutedEventArgs e)
        {
            var userStorage = IsolatedStorageFile.GetUserStoreForAssembly();

            using (var userStream = new IsolatedStorageFileStream("UserSettings.set", FileMode.Create, userStorage))
            {
                using (var userWriter = new StreamWriter(userStream))
                {
                    userWriter.WriteLine(myColorPicker.SelectedColorText);
                    MessageBox.Show("Данные успешно сохранены");
                }
            }
        }
    }
}
