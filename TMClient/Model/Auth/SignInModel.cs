using ApiWrapper.Interfaces;

namespace TMClient.Model.Auth
{
    internal class SignInModel
    {
        public static async Task<IApi?> SignIn(string login, string password)
        {
            var provider = AuthModel.ApiProvider;
            return await provider.CreateByLogin(login, password);
        }
    }
}
