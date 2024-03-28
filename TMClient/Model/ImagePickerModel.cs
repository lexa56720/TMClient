using SixLabors.ImageSharp.Formats.Bmp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System.Windows.Media.Imaging;

namespace TMClient.Model
{
    internal class ImagePickerModel
    {
        public BitmapSource Load(string path)
        {
            using var image = Image.Load(path);
            ScaleImage(image);
            using var ms = new MemoryStream();
            image.Save(ms, BmpFormat.Instance);
            return SaveAsBitmap(ms);
        }
        public async Task<BitmapSource> LoadAsync(string path)
        {
            using var image = await Image.LoadAsync(path);
            ScaleImage(image);
            using var ms = new MemoryStream();
            await image.SaveAsync(ms, BmpFormat.Instance);
            return SaveAsBitmap(ms);
        }

        private int GetMinDimension()
        {
            var sizes = new double[]
            {
                System.Windows.SystemParameters.PrimaryScreenHeight,
                System.Windows.SystemParameters.PrimaryScreenWidth,
                1000,
            };
            return (int)sizes.Min();
        }

        private void ScaleImage(Image image)
        {
            int desiredSize = GetMinDimension();

            float scaleY = (float)desiredSize / image.Height;
            float scaleX = (float)desiredSize / image.Width;

            float scale = scaleX < scaleY ? scaleX : scaleY;

            image.Mutate(i => i.Resize((int)(image.Width * scale), (int)(image.Height * scale)));
        }

        private BitmapImage SaveAsBitmap(Stream stream)
        {
            stream.Seek(0, SeekOrigin.Begin);
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.StreamSource = stream;
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.EndInit();
            bitmap.Freeze();
            return bitmap;
        }

        public byte[] Convert(CroppedBitmap bitmap)
        {
            BitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmap));
            using var ms = new MemoryStream();
            encoder.Save(ms);
            return ms.ToArray();
        }

    }
}
