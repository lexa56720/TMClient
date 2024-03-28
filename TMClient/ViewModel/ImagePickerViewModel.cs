using AsyncAwaitBestPractices.MVVM;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Bmp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TMClient.ViewModel
{
    internal class ImagePickerViewModel : BaseViewModel
    {
        public BitmapSource ImageSource
        {
            get => imageSource;
            set
            {
                imageSource = value;
                OnPropertyChanged(nameof(ImageSource));
            }
        }
        private BitmapSource imageSource;

        public ICommand AcceptCommand => new AsyncCommand<ImageSource>(Accept);

        public ICommand FileChangedCommand => new AsyncCommand<string>(FileChanged);



        private readonly Action<Image?> DialogCompleted;

        public ImagePickerViewModel(string path, Action<Image?> dialogCompleted)
        {
            ImageSource = Load(path);
            DialogCompleted = dialogCompleted;
        }
        private BitmapSource Load(string path)
        {
            using var image = Image.Load(path);
            ScaleImage(image);
            using var ms = new MemoryStream();
            image.Save(ms, BmpFormat.Instance);
            return SaveAsBitmap(ms);
        }
        private async Task<BitmapSource> LoadAsync(string path)
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

        private async Task FileChanged(string? path)
        {
            if (!string.IsNullOrEmpty(path))
                ImageSource = await LoadAsync(path);
        }

        private async Task Accept(ImageSource? source)
        {
            if (source == null || source is not CroppedBitmap croppedBitmap ||
                               croppedBitmap.Source is not BitmapImage bitmap)
                return;

            BitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(croppedBitmap));
            var ms = new MemoryStream();
            encoder.Save(ms);

            //DialogCompleted(); 
        }
    }
}
