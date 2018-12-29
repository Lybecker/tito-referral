﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebAPI.Configuration;
using WebAPI.Middelware;
using WebAPI.Services;

namespace WebAPI
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            var config = new TitoConfiguration();
            Configuration.Bind("Tito", config);
            services.AddSingleton<ITitoConfiguration>(config);
            
            services.AddHttpClient<ITitoClient, TitoClient>();
            services.AddSingleton<ITitoRequestVerifyer, TitoRequestVerifyer>();
            services.AddSingleton<IDiscount_CodeBuilder, Discount_CodeBuilder>();
            services.AddSingleton<ITitoTicketDiscountLinkGenerator, TitoTicketDiscountLinkGenerator>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                
            }
            app.UseTitoPayloadVerifierMiddleware();
            app.UseMvc();
        }
    }
}