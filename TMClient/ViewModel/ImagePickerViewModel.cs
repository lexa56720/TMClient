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
using TMClient.Model;
using TMClient.Utils;

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
        private BitmapSource imageSource = null!;
        public ICommand AcceptCommand => new Command(Accept);
        public ICommand FileChangedCommand => new AsyncCommand<string>(FileChanged);

        private readonly Action<byte[]> DialogCompleted;
        private readonly ImagePickerModel Model = new();

        public ImagePickerViewModel(string path, Action<byte[]> dialogCompleted)
        {
            ImageSource = Model.Load(path);
            DialogCompleted = dialogCompleted;
        }
        private async Task FileChanged(string? path)
        {
            if (!string.IsNullOrEmpty(path))
                ImageSource = await Model.LoadAsync(path);
        }

        private void Accept(object? source)
        {
            if (source == null || source is not CroppedBitmap croppedBitmap)
                return;
            DialogCompleted(Model.Convert(croppedBitmap));
        }
    }
}
