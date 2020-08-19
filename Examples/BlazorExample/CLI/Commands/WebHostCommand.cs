﻿namespace BlazorExample.CLI.Commands
{
    using System.Threading.Tasks;
    using BlazorExample.CLI.Services;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;
    using Typin.Exceptions;

    [Command("webhost", Description = "Management of the background webhost in the interactive mode.")]
    public class WebHostCommand : ICommand
    {
        private readonly ICliContext _cliContext;
        private readonly IWebHostRunnerService _webHostRunnerService;

        [CommandParameter(0, Name = "message", Description = "Exception message.")]
        public string Message { get; set; } = "";

        public WebHostCommand(ICliContext cliContext, IWebHostRunnerService webHostRunnerService)
        {
            _cliContext = cliContext;
            _webHostRunnerService = webHostRunnerService;
        }

        public async ValueTask ExecuteAsync(IConsole console)
        {
            if (_cliContext.IsInteractiveMode)
                throw new CommandException(Message, exitCode: 0, showHelp: false);

            await _webHostRunnerService.RunAsync(console.GetCancellationToken());
        }
    }
}