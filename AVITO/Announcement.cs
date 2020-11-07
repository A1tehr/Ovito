using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace AVITO
{
    public class Announcement
    {
        private StackPanel panel;
        private Image image;
        private TextBlock labelName;
        private TextBlock labelPrice;
        private TextBlock labelPlace;
        private TextBlock labelCreateDate;

        public Announcement(string name, double price, string place, string dateOfCreate)
        {
            panel = new StackPanel()
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(5)
            };
            image = new Image();
            // Картинка не реализована

            labelName = CreateTextBlock(17, new SolidColorBrush((Color)ColorConverter.ConvertFromString("#009cf0")), FontWeights.Bold);
            labelName.Text = name;

            labelPrice = CreateTextBlock(20, Brushes.Gray, FontWeights.Bold);
            labelPrice.Text = price.ToString();

            labelPlace = CreateTextBlock(17, Brushes.Gray, FontWeights.Medium);
            labelPlace.Text = place;

            labelCreateDate = CreateTextBlock(17, Brushes.Gray, FontWeights.Medium);
            labelCreateDate.Text = dateOfCreate;

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
