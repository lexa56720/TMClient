using ApiWrapper.Interfaces;

namespace TMClient.Model.Auth
{
    internal class SignUpModel
    {
        public static async Task<IApi?> Registration(string name, string login, string password)
        {
            var provider = AuthModel.ApiProvider;
            return await provider.CreateByRegistration(name, login, password);
        }
    }
}
