using ApiTypes.Communication.LongPolling;
using ApiTypes.Communication.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMClient.Utils;

namespace TMClient.Types.Storages
{
    class UpdateStorage
    {
        public UpdateStorage()
        {
            App.Api.LongPolling.Start();
            App.Api.LongPolling.NewMessages += async (o, ids) => 
            { 
                await NotifyAboutMessages(await App.Api.Messages.GetMessages(ids));
            };
        }

        private async Task NotifyAboutMessages(ApiMessage[] messages)
        {
            if (messages != null && messages.Any())
                await Messenger.Send(Messages.NewMessagesArived,
                                     await ApiConverter.Convert(App.UserData, messages));
        }
    }
}
