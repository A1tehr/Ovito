using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace AVITO
{
    public class Announcement
    {
        public int Id { get; set; }
        public string Name { set; get; }
        public double Price { set; get; }
        public string Place { set; get; }
        public string CreateTime { set; get; }
        public int AccountID { set; get; }

        private StackPanel panel;
        private Image image;
        private TextBlock labelName;
        private TextBlock labelPrice;
        private TextBlock labelPlace;
        private TextBlock labelCreateDate;

        public Announcement() { }
        public Announcement(string name, double price, string place, string dateOfCreate, int accountID)
        {
            Name = name;
            Price = price;
            Place = place;
            CreateTime = dateOfCreate;
            AccountID = accountID;
        }
        public Announcement(string name, double price, string place, string dateOfCreate, BitmapImage bitmap)
        {
            panel = new StackPanel()
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(5)
            };
            

            image = new Image()
            {
                Cursor = Cursors.Hand,
                Stretch = Stretch.Fill,
                Height = 200,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Source = bitmap
            };
            RenderOptions.SetBitmapScalingMode(image, BitmapScalingMode.HighQuality);

            labelName = CreateTextBlock(17, new SolidColorBrush((Color)ColorConverter.ConvertFromString("#009cf0")), FontWeights.Bold);
            labelName.Text = name;

            labelPrice = CreateTextBlock(20, Brushes.Gray, FontWeights.Bold);
            labelPrice.Text = price.ToString();

            labelPlace = CreateTextBlock(17, Brushes.Gray, FontWeights.Medium);
            labelPlace.Text = place;

            labelCreateDate = CreateTextBlock(17, Brushes.Gray, FontWeights.Medium);
            labelCreateDate.Text = dateOfCreate;

            Button button = new Button();
            button.Content = image;
            button.Background = Brushes.Transparent;
            button.BorderThickness = new Thickness(0);

            button.Click += (a, b) =>
            {
                MainWindow.Instance.TextBlock_Name.Text = Name;
                MainWindow.Instance.TextBlock_Price.Text = Price.ToString() + " P";
                MainWindow.Instance.TextBlock_Place.Text = Place;
                var db = MainWindow.Instance.GetDB();
                string phone = "";
                foreach(var ac in db.Accounts)
                {
                    if(ac.Id == AccountID)
                    {
                        phone = ac.Telephone;
                        break;
                    }
                }
                MainWindow.Instance.StackPanel_ForImages.Children.Clear();
                foreach (var img in db.Images)
                {
                    if (img.AnnounsmentID == Id)
                    {
                        // <Image x:Name="Image_1" Source="add_photo.png" Width="200" Margin="0,0,10,0" Stretch="Fill" Height="200"/>
                        var image = new Image()
                        {
                            Stretch = Stretch.Fill,
                            Height = 200,
                            Width = 200,
                            Margin = new Thickness(0,0,10,0),
                            HorizontalAlignment = HorizontalAlignment.Stretch,
                            Source = img.GetBitmapImage()
                        };
                        MainWindow.Instance.StackPanel_ForImages.Children.Add(image);
                    }
                }
                MainWindow.Instance.TextBlock_Number.Text = phone;
                MainWindow.Instance.TextBlock_DateCreate.Text = CreateTime;
                MainWindow.Instance.Grid_Announcement.Visibility = Visibility.Visible;
            };

            panel.Children.Add(button);
            panel.Children.Add(labelName);
            panel.Children.Add(labelPrice);
            panel.Children.Add(labelPlace);
            panel.Children.Add(labelCreateDate);

            int panel1 = MainWindow.Instance.Panel_1.Children.Count;
            int panel2 = MainWindow.Instance.Panel_2.Children.Count;
            int panel3 = MainWindow.Instance.Panel_3.Children.Count;

            if(panel1 == panel2 && panel1 == panel3)
            {
                MainWindow.Instance.Panel_1.Children.Add(panel);
            } else if (panel2 == panel3)
            {
                MainWindow.Instance.Panel_2.Children.Add(panel);
            } else
            {
                MainWindow.Instance.Panel_3.Children.Add(panel);
            }
        }

        private TextBlock CreateTextBlock(double fontSize, Brush foreGround, FontWeight fontWeight)
        {
            return new TextBlock()
            {
                FontSize = fontSize,
                Foreground = foreGround,
                FontWeight = fontWeight,
                HorizontalAlignment = HorizontalAlignment.Center,
                TextWrapping = TextWrapping.Wrap
            };
        }

    }
}
