using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TMApi;

namespace TMClient.Model
{
    internal class SignInModel
    {
        public static async Task<Api?> SignIn(string login, string password)
        {
            var provider = AuthModel.ApiProvider;
            return await provider.GetApiLogin(login, password);
        }
    }
}
