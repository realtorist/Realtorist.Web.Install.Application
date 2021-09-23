using Microsoft.AspNetCore.Builder;
using Realtorist.Extensions.Base;
using System;
using System.Collections.Generic;

namespace Realtorist.Web.Install.Application
{
    public class StartupApplication : IConfigureApplicationExtension
    {
        public int Priority => 0;

        public void ConfigureApplication(IApplicationBuilder app, IServiceProvider serviceProvider)
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
