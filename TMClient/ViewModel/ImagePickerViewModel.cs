using AsyncAwaitBestPractices.MVVM;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
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

namespace TMClient.ViewModel
{
    internal class ImagePickerViewModel : BaseViewModel
    {
        public ImageSource ImageSource
        {
            get => imageSource;
            set
            {
                imageSource = value;
                OnPropertyChanged(nameof(ImageSource));
            }
        }
        private ImageSource imageSource;

        public ICommand AcceptCommand => new AsyncCommand<ImageSource>(Accept);


        private readonly Action<Image?> DialogCompleted;

        public ImagePickerViewModel(string path, Action<Image?> dialogCompleted)
        {
            ImageSource = Load(path);
            DialogCompleted = dialogCompleted;
        }

        private ImageSource Load(string path)
        {
            using var image = Image.Load(path);
            int desiredSize = 800;
            if (image.Height < desiredSize || image.Width < desiredSize)
            {
                float scaleY = (float)desiredSize / image.Height;
                float scaleX = (float)desiredSize / image.Width;

                float scale = scaleX < scaleY ? scaleX : scaleY;

                image.Mutate(i => i.Resize((int)(image.Width * scale), (int)(image.Height * scale)));
            }
            using var ms = new MemoryStream();
            image.SaveAsBmp(ms);

            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.StreamSource = ms;
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.EndInit();
            bitmap.Freeze();
            return bitmap;
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
