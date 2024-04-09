using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMClient.View;

namespace TMClient.Utils
{
    internal static class PathPicker
    {
        public static string[] PickFiles(string filter,bool isMultiselect)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = filter,
                CheckFileExists = true,
                CheckPathExists = true,
                ValidateNames = true,
                Multiselect = isMultiselect,
            };
            bool? result = dialog.ShowDialog();
            if (result == true)
                return dialog.FileNames;
            return [];
        }

        public static string PickFolder()
        {
            var dialog = new Microsoft.Win32.OpenFolderDialog
            {
                ValidateNames = true,
                Multiselect = false,  
            };
            bool? result = dialog.ShowDialog();
            if (result == true)
                return dialog.FolderName;
            return string.Empty;
        }
    }
}
