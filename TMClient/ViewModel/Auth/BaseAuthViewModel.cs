using ApiWrapper.Interfaces;
using TMClient.Utils;
using TMClient.View;

namespace TMClient.ViewModel.Auth
{
    internal abstract class BaseAuthViewModel:BaseViewModel
    {
        protected readonly Func<IApi?,bool> ReturnApi;

        public BaseAuthViewModel(Func<IApi?, bool> returnApi)
        {
            ReturnApi = returnApi;
        }
    }
}
