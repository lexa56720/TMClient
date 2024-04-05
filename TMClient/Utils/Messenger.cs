using static System.Net.Mime.MediaTypeNames;

namespace TMClient.Utils
{
    public static class Messenger
    {
        private class EventValue
        {
            public Type? Type { get; private set; }

            public List<Delegate> Delegates { get; private set; } = new();

            public EventValue()
            {
            }
            public EventValue(Type type)
            {
                Type = type;
            }
        }

        private static Dictionary<string, EventValue> Events = new();

        public static bool Subscribe(Messages messages, Action handler)
        {
            return Subscribe(messages.ToString(), handler);
        }
        public static bool Subscribe<T>(Messages messages, EventHandler<T> handler)
        {
            return Subscribe(messages.ToString(), handler);
        }

        public static bool Subscribe(string text, Action handler)
        {
            if (!Events.ContainsKey(text))
                Events.Add(text, new EventValue());

            if (Events[text].Type == null)
            {
                Events[text].Delegates.Add(handler);
                return true;
            }
            return false;
        }
        public static bool Subscribe<T>(string text, EventHandler<T> handler)
        {
            if (!Events.ContainsKey(text))
                Events.Add(text, new EventValue(typeof(T)));

            var type = Events[text].Type;
            if (type != null && type.Equals(typeof(T)))
            {
                Events[text].Delegates.Add(handler);
                return true;
            }
            return false;
        }
        public static void Unsubscribe(Messages message, Action handler)
        {
            if (Events.TryGetValue(message.ToString(), out var value))
            {
                value.Delegates.Remove(handler);
                if (value.Delegates.Count == 0)
                    Events.Remove(message.ToString());
            }
        }
        public static void Unsubscribe<T>(Messages message, EventHandler<T> handler)
        {
            Unsubscribe(message.ToString(), handler);
        }
        public static void Unsubscribe<T>(string text, EventHandler<T> handler)
        {
            if (Events.TryGetValue(text, out var value))
            {
                value.Delegates.Remove(handler);
                if (value.Delegates.Count == 0)
                    Events.Remove(text);
            }
        }


        public static Task Send(string text, bool inSameThread)
        {
            if (inSameThread)
            {
                if (Events.TryGetValue(text, out var value) && value.Type == null)
                {
                    value.Delegates.ForEach(x => ((Action)x)());
                }
                return Task.CompletedTask;
            }
            return Task.Run(() =>
            {
                if (Events.TryGetValue(text, out var value) && value.Type == null)
                    value.Delegates
                    .AsParallel()
                    .ForAll(x => ((Action)x)());
            });
        }
        public static Task Send<T>(string text, T message,bool inSameThread ,object? sender = null)
        {
            if (inSameThread)
            {
                if (Events.TryGetValue(text, out var value) && value.Type != null && value.Type.Equals(typeof(T)))
                {
                    value.Delegates.ForEach(x => ((EventHandler<T>)x)(sender, message));
                }
                return Task.CompletedTask;
            }
            return Task.Run(() =>
            {
                if (Events.TryGetValue(text, out var value) && value.Type != null && value.Type.Equals(typeof(T)))
                    value.Delegates
                    .AsParallel()
                    .ForAll(x => ((EventHandler<T>)x)(sender, message));
            });
        }
        public static Task Send(Messages message, bool inSameThread = false)
        {
            return Send(message.ToString(), inSameThread);
        }
        public static Task Send<T>(Messages message, T messageObj, bool inSameThread = false, object? sender = null)
        {
            return Send(message.ToString(), messageObj, inSameThread, sender);
        }
    }
}