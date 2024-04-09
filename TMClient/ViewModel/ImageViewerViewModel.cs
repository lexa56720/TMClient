using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace TMClient.ViewModel
{
    internal class ImageViewerViewModel
    {
        public ImageSource Image { get; }

        public ImageViewerViewModel(ImageSource image)
        {
            Image = image;
        }

    }
}
