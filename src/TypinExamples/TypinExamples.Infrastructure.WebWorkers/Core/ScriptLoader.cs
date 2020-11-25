﻿namespace TypinExamples.Infrastructure.WebWorkers.Core
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.JSInterop;

    public class ScriptLoader
    {
        private const string JS_FILE = "BlazorWebWorker.js";

        private static readonly IReadOnlyDictionary<string, string> escapeScriptTextReplacements =
            new Dictionary<string, string> { { @"\", @"\\" }, { "\r", @"\r" }, { "\n", @"\n" }, { "'", @"\'" }, { "\"", @"\""" } };

        private readonly IJSRuntime jsRuntime;

        public ScriptLoader(IJSRuntime jSRuntime)
        {
            jsRuntime = jSRuntime;
        }

        public async Task InitScript()
        {
            if (await IsLoaded())
                return;

            string scriptContent;
            string assemblyName = typeof(ScriptLoader).Assembly.GetName().Name ?? throw new InvalidOperationException($"Unable to initialize {JS_FILE}");
            var stream = GetType().Assembly.GetManifestResourceStream($"{assemblyName}.{JS_FILE}");
            using (stream)
            using (var streamReader = new StreamReader(stream))
                scriptContent = await streamReader.ReadToEndAsync();

            await ExecuteRawScriptAsync(scriptContent);
            var loaderLoopBreaker = 0;
            while (!await IsLoaded())
            {
                loaderLoopBreaker++;
                await Task.Delay(100);

                // Fail after 3s not to block and hide any other possible error
                if (loaderLoopBreaker > 25)
                    throw new InvalidOperationException($"Unable to initialize {JS_FILE}");
            }
        }

        private async Task<bool> IsLoaded()
        {
            return await jsRuntime.InvokeAsync<bool>("window.hasOwnProperty", "BlazorWorker");
        }

        private async Task ExecuteRawScriptAsync(string scriptContent)
        {
            scriptContent = escapeScriptTextReplacements.Aggregate(scriptContent, (r, pair) => r.Replace(pair.Key, pair.Value));

            string blob = $"URL.createObjectURL(new Blob([\"{scriptContent}\"],{{ \"type\": \"text/javascript\"}}))";
            string bootStrapScript = $"(function(){{var d = document; var s = d.createElement('script'); s.async=false; s.src={blob}; d.head.appendChild(s); d.head.removeChild(s);}})();";

            await jsRuntime.InvokeVoidAsync("eval", bootStrapScript);
        }
    }
}
