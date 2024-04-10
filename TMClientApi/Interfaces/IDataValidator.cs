using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApiWrapper.Interfaces
{
    public interface IDataValidator
    {
        public bool IsMessageLegal(string text, params string[] files);
        public bool IsFileValidForMessages(string path);
        public bool IsFilesCountLegal(int count);

    }
}
