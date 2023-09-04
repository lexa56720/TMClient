using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TMApi;

namespace TMClient.Model.Auth
{
    internal class SignUpModel
    {
        public static async Task<Api?> Registration(string name, string login, string password)
        {
            var provider = AuthModel.ApiProvider;
            return await provider.GetApiRegistration(name, login, password);
        }
    }
}
