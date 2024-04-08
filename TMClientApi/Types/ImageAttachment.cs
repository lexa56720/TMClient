using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApiWrapper.Types
{
    public class ImageAttachment:Attachment
    {
        public ImageAttachment(string name, string url) : base(name, url)
        {
        }
    }
}
