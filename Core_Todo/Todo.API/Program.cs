using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Core_Todo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                 //.UseApplicationInsights()
                .CaptureStartupErrors(true)
                .UseSetting("detailedErrors", "true")
                .UseKestrel()
                .UseIISIntegration() // Necessary for Azure.
                .UseStartup<Startup>()
                .Build();
    }
}
