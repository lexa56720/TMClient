﻿using System.Collections.Generic;
using System.Text;
using TMClient.Controls;
using TMClient.Utils;

namespace TMClient.Model.Chats
{
    internal class ChatModel : BaseModel
    {
        protected Chat Chat { get; }
        public int Count { get; }

        public ChatModel(Chat chat, int count)
        {
            Chat = chat;
            Count = count;
        }

        public async Task<Message[]> GetHistory(Message lastMessage)
        {
            return await Api.Messages.GetMessages(Chat.Id, lastMessage.Id, Count);
        }
        public async Task<Message[]> GetHistory(int offset)
        {
            return await Api.Messages.GetMessagesByOffset(Chat.Id, Count, offset);
        }

        public async Task<bool> MarkAsReaded(Message[] messages)
        {
            return await Api.Messages.MarkAsReaded(messages.Where(m => !m.IsReaded)
                                                           .ToArray());
        }
        public async Task<Message?> SendMessage(string? text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return null;
            return await Api.Messages.SendMessage(text, Chat.Id);
        }

        public async Task<Message?> SendMessage(string? text, string[] filePaths)
        {
            text ??= string.Empty;
            using var tks = new CancellationTokenSource();
            return await Api.Messages.SendMessage(text, Chat.Id, tks.Token, filePaths);
        }
        public void SetIsReaded(IEnumerable<Message> messages)
        {
            if (messages.Count() == 0)
                return;
            App.MainThread.Invoke(() =>
            {
                foreach (var message in messages)
                {
                    message.IsReaded = true;
                }
            });
        }
    }
}
