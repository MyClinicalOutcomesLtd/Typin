﻿namespace TypinExamples.Infrastructure.WebWorkers.Core
{
    using System;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Json;
    using System.Threading.Tasks;
    using Microsoft.JSInterop;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions;
    using TypinExamples.Infrastructure.WebWorkers.BlazorBoot;

    public class WorkerFactory : IWorkerFactory
    {
        private string[]? _assemblies;

        private readonly IJSRuntime jsRuntime;
        private readonly HttpClient _httpClient;

        public WorkerFactory(IJSRuntime jsRuntime, HttpClient httpClient)
        {
            this.jsRuntime = jsRuntime;
            _httpClient = httpClient;
        }

        public async Task<IWorker> CreateAsync<T>()//WorkerInitOptions initOptions)
            where T : class, IWebWorkerEntryPoint
        {
            _assemblies ??= await GetAssembliesToLoad() ?? throw new ApplicationException("Failed to fetch assemblies list.");

            var worker = new WorkerProxy<T>(jsRuntime, _assemblies);
            //await worker.InitAsync(initOptions);
            return worker;
        }

        private async Task<string[]?> GetAssembliesToLoad()
        {
            BlazorBootModel? response = await _httpClient.GetFromJsonAsync<BlazorBootModel>("_framework/blazor.boot.json");
            string[]? assemblies = response?.Resources.Assembly.Keys.ToArray();

            return assemblies;
        }
    }
}
