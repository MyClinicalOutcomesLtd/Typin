﻿namespace TypinExamples.Infrastructure.WebWorkers.Hil
{
    using TypinExamples.Infrastructure.WebWorkers.Hil.Messages.Base;

    public class MethodCallParamsMessage : BaseMessage
    {
        public ulong WorkerId { get; init; }
        public string? ProgramClass { get; init; }
    }
}
