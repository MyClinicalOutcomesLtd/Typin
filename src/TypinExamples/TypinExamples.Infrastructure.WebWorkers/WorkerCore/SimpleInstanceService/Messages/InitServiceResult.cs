﻿namespace TypinExamples.Infrastructure.WebWorkers.WorkerCore.SimpleInstanceService.Messages
{
    public class InitServiceResult
    {
        public static readonly string Prefix = $"{SimpleInstanceService.MessagePrefix}{SimpleInstanceService.InitServiceResultMessagePrefix}";

        public static bool CanDeserialize(string message)
        {
            return message.StartsWith(Prefix);
        }

        public static InitServiceResult Deserialize(string message)
        {
            return new InitServiceResult();
        }

        public string Serialize()
        {
            return Prefix;
        }
    }
}
