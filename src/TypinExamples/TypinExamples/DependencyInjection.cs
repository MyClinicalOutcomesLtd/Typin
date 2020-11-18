﻿namespace TypinExamples
{
    using System;
    using System.Net.Http;
    using Blazor.Extensions.Logging;
    using Blazor.Extensions.Storage;
    using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using TypinExamples.Common.Extensions;
    using TypinExamples.Configuration;
    using TypinExamples.Services;
    using TypinExamples.Services.Terminal;

    public static class DependencyInjection
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration, IWebAssemblyHostEnvironment environment)
        {
            services.AddOptions();
            services.AddStorage();

            services.AddLogging(builder => builder.AddBrowserConsole()
                                                  .SetMinimumLevel(environment.IsDevelopment() ? LogLevel.Trace : LogLevel.Information));

            services.AddConfiguration<ApplicationSettings>(configuration)
                    .AddConfiguration<HeaderSettings>(configuration)
                    .AddConfiguration<FooterSettings>(configuration)
                    .AddConfiguration<ExamplesSettings>(configuration);

            services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(environment.BaseAddress) })
                    .AddScoped<IMarkdownService, MarkdownService>()
                    .AddScoped<MonacoEditorService>()
                    .AddScoped<TerminalManager>()
                    .AddTransient<ExampleRunnerService>();

            return services;
        }
    }
}
