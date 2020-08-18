﻿using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace Typin.BlazorDemo.CLI.Services
{
    public interface IWebHostRunnerService
    {
        IWebHost GetWebHost();

        Task RunAsync(CancellationToken cancellationToken = default);
        Task<IWebHost> StartAsync(CancellationToken cancellationToken = default);
    }
}
