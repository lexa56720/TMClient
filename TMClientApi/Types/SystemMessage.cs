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

        public ActionKind Kind { get; init; }
        public User? Target { get; init; }

        public bool IsExecutorAreCurrentUser { get; set; }
        public bool IsTargetAreCurrentUser { get; set; }

        [SetsRequiredMembers]
        public SystemMessage(int id, DateTime sendTime, User author, User? target, ActionKind action, Chat destination)
                           : base(id, string.Empty, sendTime, author, destination, true, author.IsCurrentUser)
        {
            Kind = action;
            Target = target;
            IsExecutorAreCurrentUser = IsOwn;
            if (Target != null)
                IsTargetAreCurrentUser = Target.IsCurrentUser;
        }
    
    }
}
