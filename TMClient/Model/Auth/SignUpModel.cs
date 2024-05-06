using ClientApiWrapper.Interfaces;

namespace TMClient.Model.Auth
{
    internal class SignUpModel:BaseAuthModel
    {
        public async Task<IApi?> Registration(string name, string login, string password)
        {
            return await GetApiProvider().CreateByRegistration(name, login, password);
        }

        public bool IsNameValid(string name)
        {
            return ApiTypes.Shared.DataConstraints.IsNameLegal(name);
        }
    }
}
