using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMClient.Model
{
    internal class ProfileModel : BaseModel
    {

        public async Task ChangeAvatar(byte[] data)
        {
            await Api.Users.ChangeProfilePic(data);
        }
    }
}
