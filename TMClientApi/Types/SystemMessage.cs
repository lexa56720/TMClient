using ApiWrapper.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiTypes.Communication.Messages;
using System.Diagnostics.CodeAnalysis;
using System.ComponentModel;
using TMApi;

namespace ClientApiWrapper.Types
{
    public class SystemMessage : ApiWrapper.Types.Message
    {
        public override bool IsSystem => true;

        public required ActionKind Kind { get; init; }
        public required User? Target { get; init; }

        public required bool IsExecutorAreCurrentUser { get; init; }
        public required bool IsTargetAreCurrentUser { get; init; }

        public SystemMessage()
        {

        }
  
    }
}
