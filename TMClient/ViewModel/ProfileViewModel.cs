using AsyncAwaitBestPractices.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TMClient.Model;
using TMClient.Utils;
using TMClient.View;

namespace TMClient.ViewModel
{
    internal class ProfileViewModel : BaseViewModel
    {
        public ICommand ChangeAvatarCommand => new AsyncCommand(ChangeAvatar);

        private readonly ProfileModel Model = new();

        private async Task ChangeAvatar()
        {
            await Messenger.Send(Messages.ModalOpened, true);
            var imageData = FileImageData.GetImageData();
            if (imageData.Length > 0)
                await ChangeAvatar(imageData);
            await Messenger.Send(Messages.ModalClosed, true);
        }
        private async Task ChangeAvatar(byte[] imageData)
        {
            await Messenger.Send(Messages.LoadingStart, true);
            await Model.ChangeAvatar(imageData);
            await Messenger.Send(Messages.LoadingOver, true);
        }
    }
}
