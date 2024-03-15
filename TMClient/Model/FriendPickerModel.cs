using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMClient.ViewModel;

namespace TMClient.Model
{
    class FriendPickerModel : BaseModel
    {
        public UserRequest[] Search(IEnumerable<UserRequest> users,string query)
        {
           List<UserRequest> result= new List<UserRequest>();

            foreach(var user in users)
                if(user.User.Name.Contains(query, StringComparison.CurrentCultureIgnoreCase) ||
                   user.User.Login.Contains(query, StringComparison.CurrentCultureIgnoreCase))
                {
                    result.Add(user);
                }
            return result.ToArray();
        }

    }
}
