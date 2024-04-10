using ClientApiWrapper.Interfaces;

namespace TMClient.Model.Auth
{
    internal class SignInModel : BaseAuthModel
    {
        public async Task<IApi?> SignIn(string login, string password)
        {
            return await ApiProvider.CreateByLogin(login, password);
        }
    }
}
