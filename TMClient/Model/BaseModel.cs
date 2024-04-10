using ClientApiWrapper.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMClient.Model
{
    internal class BaseModel
    {
        public static IApi Api { protected get; set; } = null!;
    }
}
