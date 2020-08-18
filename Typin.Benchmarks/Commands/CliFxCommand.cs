﻿namespace Typin.Benchmarks.Commands
{
    using System.Threading.Tasks;
    using CliFx;
    using CliFx.Attributes;

    [Command]
    public class CliFxCommand : ICommand
    {
        [CommandOption("str", 's')]
        public string? StrOption { get; set; }

        [CommandOption("int", 'i')]
        public int IntOption { get; set; }

        [CommandOption("bool", 'b')]
        public bool BoolOption { get; set; }

        public ValueTask ExecuteAsync(IConsole console)
        {
            return default;
        }
    }
}
