using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApiWrapper.Types
{
    public abstract class Attachment
    {
        public string FileName { get; private set; } = string.Empty;

        public string Url { get; private set; } = string.Empty;

        public Attachment(string fileName,string url)
        {
            FileName = fileName;
            Url = url;
        }
    }
}
