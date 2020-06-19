using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(BarCrawlers.Areas.Identity.IdentityHostingStartup))]
namespace BarCrawlers.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
            });
        }
    }
}