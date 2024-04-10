using ApiTypes.Communication.Info;
using ApiTypes.Shared;
using ClientApiWrapper.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApiWrapper.ApiWrapper.Wrapper
{
    public class ClientDataValidator : IDataValidator
    {
        private readonly ServerInfo Info;

        public ClientDataValidator(ServerInfo info)
        {
            Info = info;
        }

        public bool IsFilesCountLegal(int count)
        {
            return count < Info.MaxAttachments;
        }

        public bool IsFileValidForMessages(string path)
        {
            var fileInfo = new FileInfo(path);
            return fileInfo.Length < (Info.MaxFileSizeMB * Math.Pow(10, 6));
        }

        public bool IsMessageLegal(string text, params string[] files)
        {
            return (files.Length == 0 && DataConstraints.IsMessageLegal(text)) ||
                (files.Length > 0 && DataConstraints.IsMessageWithFilesLegal(text) && IsFilesCountLegal(files.Length) && files.All(IsFileValidForMessages));
        }
    }
}
