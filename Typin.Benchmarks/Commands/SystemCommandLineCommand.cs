﻿namespace Typin.Benchmarks.Commands
{
    using System.CommandLine;
    using System.CommandLine.Invocation;
    using System.Threading.Tasks;

    public class SystemCommandLineCommand
    {
        public static int ExecuteHandler(string s, int i, bool b)
        {
            return 0;
        }

        public Task<int> ExecuteAsync(string[] args)
        {
            var command = new RootCommand
            {
                new Option(new[] {"--str", "-s"})
                {
                    Argument = new Argument<string?>()
                },
                new Option(new[] {"--int", "-i"})
                {
                    Argument = new Argument<int>()
                },
                new Option(new[] {"--bool", "-b"})
                {
                    Argument = new Argument<bool>()
                }
            };

            command.Handler = CommandHandler.Create(typeof(SystemCommandLineCommand).GetMethod(nameof(ExecuteHandler)));

            return command.InvokeAsync(args);
        }
    }
}