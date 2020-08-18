﻿using System.Threading.Tasks;
using Typin.Attributes;
using Typin.BlazorDemo.CLI.Services;

namespace Typin.BlazorDemo.CLI.Commands
{
    [Command("webhost start", Description = "Starts the webhost worker in background in the interactive mode.", InteractiveModeOnly = true)]
    public class WebHostStartCommand : ICommand
    {
        private readonly IBackgroundWebHostProvider _webHostProvider;

        public WebHostStartCommand(IBackgroundWebHostProvider webHostProvider)
        {
            _webHostProvider = webHostProvider;
        }

        public async ValueTask ExecuteAsync(IConsole console)
        {
            await _webHostProvider.StartAsync();
        }
    }
}
