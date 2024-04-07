using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMClient.View;

namespace TMClient.Utils
{
    internal static class FileLoader
    {
        public static byte[] GetImageData()
        {
            var path = OpenFilePickerWindow();
            if (!string.IsNullOrEmpty(path))
            {
                var mainWindow = App.Current.MainWindow;
                var imageCutter = new ImagePickerWindow(path)
                {
                    Owner = mainWindow,
                    ShowInTaskbar = false
                };
                if (imageCutter.ShowDialog() == true)
                    return imageCutter.Image;
            }
            return [];
        }
        private static string OpenFilePickerWindow()
        {
            var dialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Изображения|*.jpg;*.jpeg;*.png",
                CheckFileExists = true,
                CheckPathExists = true,
                ValidateNames = true,
                Multiselect = false,
            };
            bool? result = dialog.ShowDialog();
            if (result == true)
                return dialog.FileName;
            return string.Empty;
        }

        public static string[] PickFiles()
        {
            var dialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "All|*.*",
                CheckFileExists = true,
                CheckPathExists = true,
                ValidateNames = true,
                Multiselect = true,
            };
            bool? result = dialog.ShowDialog();
            if (result == true)
                return dialog.FileNames;
            return [];
        }
    }
}
