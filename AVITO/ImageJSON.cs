using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Windows.Media.Imaging;

namespace AVITO
{
    public class ImageJSON
    {
        public byte[] ImageBytes { set; get; }


        public BitmapImage GetBitmapImage()
        {
            if (ImageBytes == null || ImageBytes.Length == 0) return null;
            var image = new BitmapImage();
            using (var mem = new MemoryStream(ImageBytes))
            {
                mem.Position = 0;
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = null;
                image.StreamSource = mem;
                image.EndInit();
            }
            image.Freeze();
            return image;
        }
        public string Serialize()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
