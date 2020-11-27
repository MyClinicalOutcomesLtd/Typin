﻿namespace TypinExamples.Infrastructure.WebWorkers.Hil
{
    using System;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions;

    public class CancelMessage : IMessage
    {
        public ulong WorkerId { get; init; }
        public ulong CallId { get; init; }
        public Exception? Exception { get; init; }
    }
}
