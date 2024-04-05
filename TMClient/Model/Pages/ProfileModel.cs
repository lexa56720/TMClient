using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMClient.Model.Pages
{
    internal class ProfileModel : BaseModel
    {

        public async Task ChangeAvatar(byte[] data)
        {
            await Api.Users.ChangeProfilePic(data);
        }

        internal async Task<bool> SaveName(string name)
        {
            return await Api.Users.ChangeName(name);
        }

        internal async Task<bool> SavePassword(string currentPass, string newPass)
        {
            return await Api.Users.ChangePassword(currentPass, newPass);
        }


        public bool IsCurrentPasswordValid(string password)
        {
            return !string.IsNullOrEmpty(password) &&
                   !GetPasswordHash(password).Equals(Api.PasswordHash);
        }
        private string GetPasswordHash(string password)
        {
            return ApiTypes.Shared.HashGenerator.GenerateHashSmall(password);
        }
    }
}
