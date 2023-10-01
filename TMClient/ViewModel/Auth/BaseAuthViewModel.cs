using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMApi;
using TMClient.Utils;
using TMClient.View;

namespace TMClient.ViewModel.Auth
{
    internal abstract class BaseAuthViewModel:BaseViewModel
    {

        protected async Task OpenMainWindow(Api api)
        {    
            var mainWindow =await MainWindow.GetInstance(api);
            mainWindow.Show();
            await Messenger.Send(Messages.CloseAuth);
            return;
        }
    }
}
