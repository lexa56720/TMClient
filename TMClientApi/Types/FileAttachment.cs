using ApiTypes.Communication.Medias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ClientApiWrapper.Types
{
    public class FileAttachment : Attachment
    {
        public FileAttachment(string name,string url) : base(name, url)
        {
        }
    }
}
