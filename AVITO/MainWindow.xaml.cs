using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace AVITO
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow Instance { get; private set; }
        public Account Account { set; get; }

        private bool addImage1 = false;
        private bool addImage2 = false;
        private bool addImage3 = false;
        private bool addImage4 = false;

        public MainWindow()
        {
            Instance = this;
            InitializeComponent();
        }

        private void Button_AddAnnouncment(object sender, RoutedEventArgs e)
        {
            addImage1 = false;
            addImage2 = false;
            addImage3 = false;
            addImage4 = false;
            TextBox_Name.Text = "";
            TextBox_Price.Text = "";
            TextBox_Place.Text = "";
            Button_Nav_AddAnnouncment.Visibility = Visibility.Collapsed;
            Grid_AddAnnouncment.Visibility = Visibility.Visible;
        }

        private void Button_AddImageClick(object sender, RoutedEventArgs e)
        {
            if (addImage4)
            {
                MessageBox.Show("Вы больше не можете добавить фотографий", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = false;
            if (dialog.ShowDialog() == true)
            {
                var fileName = dialog.FileName;
                if (!fileName.Contains('.'))
                {
                    MessageBox.Show("Фото не выбрано", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (!fileName.Contains(".png") && !fileName.Contains(".jpg"))
                {
                    MessageBox.Show("Данный формат не поддерживается", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (File.ReadAllBytes(fileName).Length > 512000)
                {
                    MessageBox.Show("Размер изображения не должен превышать 0.5 Мб", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                BitmapImage bitmapImage;
                using (var stream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                {
                    bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.StreamSource = stream;
                    bitmapImage.EndInit();
                    bitmapImage.Freeze();
                }
                if (!addImage1)
                {
                    Add_Image_1.Source = bitmapImage;
                    addImage1 = true;
                }
                else if (!addImage2)
                {
                    Add_Image_2.Source = bitmapImage;
                    addImage2 = true;
                }
                else if (!addImage3)
                {
                    Add_Image_3.Source = bitmapImage;
                    addImage3 = true;
                }
                else if (!addImage4)
                {
                    Add_Image_4.Source = bitmapImage;
                    addImage4 = true;
                }
            }
        }

        private void Button_CreateAnnouncment(object sender, RoutedEventArgs e)
        {
            string name = TextBox_Name.Text;
            string place = TextBox_Place.Text;
            double price = Convert.ToDouble(TextBox_Price.Text);
            // ТУТ ИСПРАВИТЬ ID
            Announcement announcement = new Announcement(name, price, place, DateTime.Now.ToString(), 1);
            DataContext db = new DataContext();
            db.Announcements.Add(announcement);
            db.SaveChanges();
            MessageBox.Show("Вы успешно подали объявление", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            Button_Nav_AddAnnouncment.Visibility = Visibility.Visible;
            Grid_AddAnnouncment.Visibility = Visibility.Collapsed;
        }

        private void Button_LogIn(object sender, RoutedEventArgs e)
        {
            Grid_LogIn.Visibility = Visibility.Visible;
        }

        private void Button_CloseLogin(object sender, RoutedEventArgs e)
        {
            Grid_LogIn.Visibility = Visibility.Collapsed;
        }

        private void Button_Registr(object sender, RoutedEventArgs e)
        {
            string login = TextBlock_Reg_Login.Text;
            string phone = TextBlock_Reg_Phone.Text;
            string password = TextBlock_Reg_Password.Password;
            string confPassword = TextBlock_Reg_PasswordConfirm.Password;
        }
    }
}
