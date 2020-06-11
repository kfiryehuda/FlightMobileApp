using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FlightMobileApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddMemoryCache();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.Use(async (context, next) =>
            {
                var url = context.Request.Path.Value;

                if (url.Contains("screenshot"))
                {
                    context.Response.Redirect("http://10.0.2.2:8080/screenshot");
                    return;
                }
                await next();
            });
        }
    }
}
