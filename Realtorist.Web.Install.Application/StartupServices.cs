using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Realtorist.Extensions.Base;
using System;
using System.IO;
using System.Reflection;

namespace Realtorist.Web.Install.Application
{
    public class StartupServices : IConfigureServicesExtension
    {
        public int Priority => 3;

        public void ConfigureServices(IServiceCollection services, IServiceProvider serviceProvider)
        {
            var env = serviceProvider.GetService<IWebHostEnvironment>();

            var assemblyPath = Assembly.GetExecutingAssembly().Location;
            var assemblyDirectory = Path.GetDirectoryName(assemblyPath);

            var fileProvider = new PhysicalFileProvider(assemblyDirectory + "/wwwroot");
            env.WebRootFileProvider = new CompositeFileProvider(fileProvider, env.WebRootFileProvider);
        }
    }
}
