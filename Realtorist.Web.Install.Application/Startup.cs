using ExtCore.Infrastructure.Actions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Realtorist.Web.Install.Application
{
    public class Startup : IConfigureServicesAction, IConfigureAction
    {
        int IConfigureServicesAction.Priority => 3;
        int IConfigureAction.Priority => 0;

        void IConfigureServicesAction.Execute(IServiceCollection services, IServiceProvider serviceProvider)
        {
            var env = serviceProvider.GetService<IWebHostEnvironment>();

            var assemblyPath = Assembly.GetExecutingAssembly().Location;
            var assemblyDirectory = Path.GetDirectoryName(assemblyPath);

            var fileProvider = new PhysicalFileProvider(assemblyDirectory + "/wwwroot");
            env.WebRootFileProvider = new CompositeFileProvider(fileProvider, env.WebRootFileProvider);
        }

        void IConfigureAction.Execute(IApplicationBuilder app, IServiceProvider serviceProvider)
        {
            app.Use(async (context, next) =>
            {
                if (!context.Request.Path.Value.StartsWith("/install") && !context.Request.Path.Value.StartsWith("/api/install"))
                {
                    context.Response.Redirect("/install/index.html");
                    return;
                }

                await next();
            });
            app.UseDefaultFiles(new DefaultFilesOptions { DefaultFileNames = new List<string> { "index.html" } });
        }
    }
}
