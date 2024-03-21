using ApiWrapper.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using TMApi.ApiRequests.Security;
using ApiTypes.Communication.Messages;
using System.Diagnostics.CodeAnalysis;
using System.ComponentModel;

namespace ClientApiWrapper.Types
{
    public class SystemMessage : ApiWrapper.Types.Message
    {
        public override bool IsSystem => true;

        public ActionKind Kind { get; init; }
        public User? Target { get; init; }

        [SetsRequiredMembers]
        public SystemMessage(int id, DateTime sendTime, User author, User? target, ActionKind action, Chat destination, bool isReaded, bool isOwn)
                           : base(id, GetMessageText(author, target, action), sendTime, author, destination, isReaded, isOwn)
        {
            Target = target;
            if (Target != null)
                Target.PropertyChanged += MessageChanged;
            Author.PropertyChanged += MessageChanged;
        }

        private void MessageChanged(object? sender, PropertyChangedEventArgs e)
        {
            Text = GetMessageText(Author, Target, Kind);
        }

        private static string GetMessageText(User executor, User? target, ActionKind action)
        {
            return action switch
            {
                ActionKind.UserInvite => $"{executor.Name} пригласил {target?.Name} в чат",
                ActionKind.UserEnter => $"{executor.Name} вошёл в чат",
                ActionKind.UserLeave => $"{executor.Name} покинул чат",
                ActionKind.UserKicked => $"{executor.Name} выгнал {target?.Name} из чата",
                _ => "ничего не произошло...",
            };
        }
    }
}
