using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using AutoMapper;
using BusinessLayer.Interfaces;
using BusinessLayer.Services;

namespace api
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
            services.AddMvc();
            services.AddAutoMapper();
            services.AddSignalR();

            services.AddScoped<IPlayerAuthService, FakePlayerAuthService>();

            services.AddSingleton<IPlayerInfoService, PlayerInfoService>();
            services.AddSingleton<IDistressSignalService, DistressSignalService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSignalR(routes =>
            {
                routes.MapHub<DistressHub>("/distress");
            });

            app.UseMvc();
        }
    }
}
