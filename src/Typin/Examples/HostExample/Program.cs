namespace SimpleAppExample
{
    using System.Threading.Tasks;
    using Microsoft.Extensions.Hosting;
    using Typin.Hosting;

    public static class Program
    {
        private static readonly string[] Arguments = { "-125", "--str", "hello world", "-i", "-13", "-b", "-vx" };

        public static async Task<int> Main(string[] args)
        {
            await Host.CreateDefaultBuilder(args)
                .ConfigureCliHost(builder =>
                {
                    builder.UseStartup<Startup>();
                })
                .RunConsoleAsync();

            //https://github.com/dotnet/runtime/blob/01b7e73cd378145264a7cb7a09365b41ed42b240/src/libraries/Microsoft.Extensions.Hosting/src/HostingHostBuilderExtensions.cs#L161

            return 0;
        }
    }
}