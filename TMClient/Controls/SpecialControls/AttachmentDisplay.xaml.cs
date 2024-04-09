using AsyncAwaitBestPractices.MVVM;
using ClientApiWrapper.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
using TMClient.Utils;

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

        public static readonly DependencyProperty OpenFullCommandProperty =
            DependencyProperty.Register(nameof(OpenFullCommand),
                                        typeof(ICommand),
                                        typeof(AttachmentDisplay),
                                        new PropertyMetadata(null));
        public ICommand OpenFullCommand
        {
            get
            {
                return (ICommand)GetValue(OpenFullCommandProperty);
            }
            set
            {
                SetValue(OpenFullCommandProperty, value);
            }
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
        public ICommand DownloadCommand => new AsyncCommand(Download, (o) => !IsDownloading);
        public ICommand DownloadCancelCommand => new Command(CancelDownload, (o) => IsDownloading);
        public ICommand OpenFolderCommand => new Command(OpenFolder, (o) => IsSaved == true && !string.IsNullOrEmpty(SavedPath));
        public float DownloadProgress
        {
            get => downloadProgress;
            set
            {
                downloadProgress = value;
                OnPropertyChanged(nameof(DownloadProgress));
            }
        }
        private float downloadProgress;
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
        public bool IsDownloading
        {
            get => isDownloading;
            set
            {
                isDownloading = value;
                OnPropertyChanged(nameof(IsDownloading));
            }
        }
        private bool isDownloading;

        private string SavedPath { get; set; } = string.Empty;

        private CancellationTokenSource TokenSource;

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
            IsDownloading = false;
            IsImage = attachment is ImageAttachment;
            if (IsImage)
                Image = new BitmapImage(new Uri(attachment.Url));
            else
                Image = null;
        }

        private async Task Download()
        {
            var path = GetPath(GetSavingFolder(), IsImage, Attachment.FileName);
            if (string.IsNullOrEmpty(path))
                return;

            TokenSource = new CancellationTokenSource();

            using var fs = new FileStream(path, FileMode.Create, FileAccess.Write);
            IsDownloading = true;
            try
            {
                if (IsImage)
                    IsSaved = SaveImage(fs);
                else
                    IsSaved = await SaveFile(fs, TokenSource.Token);
            }
            catch
            {
                IsSaved = false;
            }
            if(TokenSource.IsCancellationRequested)
            {
                fs.Close();
                DownloadProgress = 0;
                if (File.Exists(path))
                    File.Delete(path);
            }

            SavedPath = IsSaved ? path : string.Empty;
            IsDownloading = false;

            TokenSource.Dispose();
        }

        private string GetPath(string folderPath, bool isImage, string fileName)
        {
            if (string.IsNullOrEmpty(folderPath))
                return string.Empty;

            var path = Path.Combine(folderPath, fileName);
            if (isImage)
                path += ".jpg";
            if (!Path.Exists(folderPath))
                Directory.CreateDirectory(folderPath);
            return path;
        }
        private string GetSavingFolder()
        {
            if (string.IsNullOrEmpty(Preferences.Default.SavingFolder))
            {
                var savingFolder = Utils.PathPicker.PickFolder();
                if (string.IsNullOrEmpty(savingFolder))
                    return string.Empty;
                Preferences.Default.SavingFolder = savingFolder;
                Preferences.Default.Save();
            }
            return Preferences.Default.SavingFolder;
        }

        private async Task<bool> SaveFile(Stream fs, CancellationToken token)
        {
            using var client = new HttpClient();
            using var response = await client.GetAsync(Attachment.Url, token);
            if (!response.IsSuccessStatusCode)
                return false;
            using var stream = await response.Content.ReadAsStreamAsync(token);
            if (stream == null || response.Content.Headers.ContentLength == null)
                return false;

            DownloadProgress = 0;
            var bufferSize = 81920;
            float progressStep = (bufferSize * 100f) / response.Content.Headers.ContentLength.Value;
            var buffer = new Memory<byte>(new byte[bufferSize]);
            int read;
            while (!token.IsCancellationRequested && (read = await stream.ReadAsync(buffer, token)) != 0)
            {
                await fs.WriteAsync(buffer, token);
                DownloadProgress += progressStep;
                await Task.Delay(100);
            }
            if (token.IsCancellationRequested)
                return false;
            DownloadProgress = 100;
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

        private void CancelDownload()
        {
            TokenSource.Cancel();
        }
        private void OpenFolder()
        {
            if (!File.Exists(SavedPath))
                return;
            Process.Start(new ProcessStartInfo()
            {
                FileName = "explorer",
                Arguments = $"e, /select, \"{SavedPath}\"",
                UseShellExecute = true,
                Verb = "open"
            });
        }

    }
}
