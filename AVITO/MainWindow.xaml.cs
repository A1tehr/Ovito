using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;


namespace AVITO
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow Instance { get; private set; }
        public Account Account { set; get; }
        public List<byte[]> ImagesToAdd { set; get; }
        private DataContext db = new DataContext();
        private List<Announcement> currentAnnouncements;

        private bool addImage1 = false;
        private bool addImage2 = false;
        private bool addImage3 = false;
        private bool addImage4 = false;

        public MainWindow()
        {
            Instance = this;
            InitializeComponent();
            ImagesToAdd = new List<byte[]>();
            currentAnnouncements = new List<Announcement>();
            RefreshAnnounsments();
        }

        private void Button_AddAnnouncment(object sender, RoutedEventArgs e)
        {
            if (Account == null)
            {
                MessageBox.Show("Вы не авторизованы", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            addImage1 = false;
            addImage2 = false;
            addImage3 = false;
            addImage4 = false;
            TextBox_Name.Text = "";
            TextBox_Price.Text = "";
            TextBox_Place.Text = "";
            Button_Nav_AddAnnouncment.Visibility = Visibility.Collapsed;
            Grid_AddAnnouncment.Visibility = Visibility.Visible;
            BitmapImage bitmap = new BitmapImage();

            bitmap.BeginInit();
            bitmap.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.UriSource = new Uri("add_photo.png", UriKind.RelativeOrAbsolute);
            bitmap.EndInit();
            bitmap.Freeze();

            Add_Image_1.Source = bitmap;
            Add_Image_2.Source = bitmap;
            Add_Image_3.Source = bitmap;
            Add_Image_4.Source = bitmap;
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
                ImagesToAdd.Add(File.ReadAllBytes(fileName));
            }
        }

        private void Button_CreateAnnouncment(object sender, RoutedEventArgs e)
        {
            string name = TextBox_Name.Text;
            string place = TextBox_Place.Text;
            double price = Convert.ToDouble(TextBox_Price.Text);

            Announcement announcement = new Announcement(name, price, place, DateTime.Now.ToString(), Account.Id);
            db.Announcements.Add(announcement);
            db.SaveChanges();
            foreach(var an in db.Announcements)
            {
                if(an == announcement)
                {
                    foreach(var bytes in ImagesToAdd)
                    {
                        db.Images.Add(new ImageDB() { AnnounsmentID = an.Id, ImageBytes = bytes });
                    }
                    
                    ImagesToAdd.Clear();
                }
            }
            db.SaveChanges();
            MessageBox.Show("Вы успешно подали объявление", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            Button_Nav_AddAnnouncment.Visibility = Visibility.Visible;
            Grid_AddAnnouncment.Visibility = Visibility.Collapsed;
            RefreshAnnounsments();
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
            if(phone.Length < 7)
            {
                MessageBox.Show("Неверно указан телефон", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else if(login.Length < 5)
            {
                MessageBox.Show("Минимальная длина логина 5 символов", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else if(password.Length < 4)
            {
                MessageBox.Show("Минимальная длина пароля 4 символа", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            } else if(password != confPassword)
            {
                MessageBox.Show("Пароли не совпадают", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            foreach(var ac in db.Accounts)
            {
                if(ac.Login == login)
                {
                    MessageBox.Show("Данный логин уже занят", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }


            Account account = new Account()
            {
                Login = login,
                Password = password,
                Telephone = phone
            };
            Account = account;
            Button_Nav_Login.Visibility = Visibility.Collapsed;

            db.Accounts.Add(account);
            db.SaveChanges();
            MessageBox.Show("Вы успешно зарегестрировались");
            Grid_LogIn.Visibility = Visibility.Collapsed;
            TextBlock_Reg_Login.Text = "";
            TextBlock_Reg_Phone.Text = "";
            TextBlock_Reg_Password.Password = "";
            TextBlock_Reg_PasswordConfirm.Password = "";
            StackPanel_Login.Visibility = Visibility.Visible;
            StackPanel_Registartion.Visibility = Visibility.Collapsed;
        }

        private void Button_GoToRegistration(object sender, RoutedEventArgs e)
        {
            StackPanel_Login.Visibility = Visibility.Collapsed;
            StackPanel_Registartion.Visibility = Visibility.Visible;
        }

        private void Button_Authorization(object sender, RoutedEventArgs e)
        {
            string login = TextBox_Login.Text;
            string password = TextBox_Password.Password;
            foreach(var acc in db.Accounts)
            {
                if(acc.Login == login)
                {
                    if(acc.Password == password)
                    {
                        MessageBox.Show("Вы успешно вошли", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        Account = acc;
                        Grid_LogIn.Visibility = Visibility.Collapsed;
                        TextBox_Login.Text = "";
                        TextBox_Password.Password = "";
                        Button_Nav_Login.Visibility = Visibility.Collapsed;
                        return;
                    } else
                    {
                        MessageBox.Show("Неверный пароль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }
            }
            MessageBox.Show("Неверный логин", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public void RefreshAnnounsments()
        {
            Panel_1.Children.Clear();
            Panel_2.Children.Clear();
            Panel_3.Children.Clear();
            List<Announcement> announcements = new List<Announcement>();
            currentAnnouncements.Clear();
            foreach(var ad in db.Announcements)
            {
                announcements.Add(ad);
            }
            foreach (var ad in announcements)
            {
                foreach (var image in db.Images)
                {
                    if (image.AnnounsmentID == ad.Id)
                    {
                        currentAnnouncements.Add(new Announcement(ad.Name, ad.Price, ad.Place, ad.CreateTime, image.GetBitmapImage()));
                        break;
                    }
                }
            }
        }
    }
}
