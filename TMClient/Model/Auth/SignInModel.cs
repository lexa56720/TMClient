using ClientApiWrapper.Interfaces;

namespace TMClient.Model.Auth
{

    //Модель входа
    internal class SignInModel : BaseAuthModel
    {
        public async Task<IApi?> SignIn(string login, string password)
        {
            return await GetApiProvider().CreateByLogin(login, password);
        }
    }
}
