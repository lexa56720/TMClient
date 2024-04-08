using AsyncAwaitBestPractices.MVVM;
using ClientApiWrapper.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
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

namespace TMClient.Controls
{
    /// <summary>
    /// Логика взаимодействия для AttachmentDisplay.xaml
    /// </summary>
    public partial class AttachmentDisplay : UserControl, INotifyPropertyChanged
    {
        public static readonly DependencyProperty AttachmentProperty =
            DependencyProperty.Register(nameof(Attachment),
                                        typeof(Attachment),
                                        typeof(AttachmentDisplay),
                                        new PropertyMetadata(null, AttachmentChanged));

        public Attachment Attachment
        {
            get { return (Attachment)GetValue(AttachmentProperty); }
            set { SetValue(AttachmentProperty, value); }
        }
        public bool IsImage
        {
            get => isImage;
            set
            {
                isImage = value;
                OnPropertyChanged(nameof(IsImage));
            }
        }
        private bool isImage;
        public ImageSource? Image
        {
            get => image;
            set
            {
                image = value;
                OnPropertyChanged(nameof(Image));
            }
        }
        private ImageSource? image;
        public ICommand DownloadCommand => new AsyncCommand(Download, (o) => !IsBusy);
        public ICommand DownloadCancelCommand => new AsyncCommand(CancelDownload);
        public ICommand OpenFullCommand => new AsyncCommand(OpenFull);
        public int DownloadProgress
        {
            get => downloadProgress;
            set
            {
                downloadProgress = value;
                OnPropertyChanged(nameof(DownloadProgress));
            }
        }
        private int downloadProgress;
        public bool IsSaved
        {
            get => isSaved;
            set
            {
                isSaved = value;
                OnPropertyChanged(nameof(IsSaved));
            }
        }
        private bool isSaved;
        public bool IsBusy
        {
            get => isBusy;
            set
            {
                isBusy = value;
                OnPropertyChanged(nameof(IsBusy));
            }
        }
        private bool isBusy;

        public event PropertyChangedEventHandler? PropertyChanged;
        public AttachmentDisplay()
        {
            InitializeComponent();
        }
        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private static void AttachmentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is not Attachment newAttachment || d is not AttachmentDisplay display)
                return;
            display.AttachmentChanged(newAttachment);
        }

        private void AttachmentChanged(Attachment attachment)
        {
            DownloadProgress = 0;
            IsSaved = false;
            IsBusy = false;
            IsImage = attachment is ImageAttachment;
            if (IsImage)
                Image = new BitmapImage(new Uri(attachment.Url));
            else
                Image = null;
        }

        private async Task Download()
        {
            IsBusy = true;
            var path = $@"D:\TmApi\{Attachment.FileName}";
            if (IsImage)
                path += ".jpg";
            Directory.CreateDirectory(System.IO.Path.GetDirectoryName(path));
            using var fs = new FileStream(path, FileMode.Create, FileAccess.Write);
            if (IsImage)
                IsSaved = SaveImage(fs);
            else
                IsSaved = await SaveFile(fs);

            IsBusy = false;
        }

        private async Task<bool> SaveFile(Stream fs)
        {
            using var client = new HttpClient();
            using var response = await client.GetAsync(Attachment.Url);
            if (!response.IsSuccessStatusCode)
                return false;
            using var stream = await response.Content.ReadAsStreamAsync();
            if (stream == null || response.Content.Headers.ContentLength == null)
                return false;

            var percent = (int)(response.Content.Headers.ContentLength / 100);
            var buffer = new byte[percent];
            while (true)
            {
                var readed = await stream.ReadAsync(buffer);
                if (readed == 0)
                    break;
                await fs.WriteAsync(buffer,0,readed);
                DownloadProgress++;
            }
            return true;
        }

        private bool SaveImage(Stream fs)
        {
            BitmapEncoder encoder = new JpegBitmapEncoder();
            if (Image is not BitmapImage image)
                return false;

            encoder.Frames.Add(BitmapFrame.Create(image));
            encoder.Save(fs);
            return true;
        }

        private async Task CancelDownload()
        {
            throw new NotImplementedException();
        }
        private async Task OpenFull()
        {
            throw new NotImplementedException();
        }

    }
}
