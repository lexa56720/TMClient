using ApiWrapper.Interfaces;
using TMClient.Utils;
using TMClient.View;

namespace TMClient.ViewModel.Auth
{
    internal abstract class BaseAuthViewModel:BaseViewModel
    {

        protected async Task OpenMainWindow(IApi api)
        {    
            await Messenger.Send(Messages.AuthCompleted, api);
            var mainWindow =new MainWindow();
            mainWindow.Show();
            await Messenger.Send(Messages.CloseAuth);
            return;
        }
    }
}
