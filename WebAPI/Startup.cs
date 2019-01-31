using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebAPI.Configuration;
using WebAPI.Middelware;
using WebAPI.Services;
using WebAPI.Validation;

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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            //services.Configure<TitoConfiguration>(Configuration.GetSection("Tito"));
            //services.PostConfigure<TitoConfiguration>(settings =>
            //{
            //    settings.ValidateAndFailIfError();
            //});

            var config = new TitoConfiguration();
            Configuration.Bind("Tito", config);
            config.ValidateAndFailIfError();
            services.AddSingleton<TitoConfiguration>(config);

            services.AddHttpClient<ITitoClient, TitoClient>();
            services.AddSingleton<ITitoRequestVerifyer, TitoRequestVerifyer>();
            services.AddSingleton<IDiscount_CodeBuilder, Discount_CodeBuilder>();
            services.AddSingleton<ITitoTicketDiscountLinkGenerator, TitoTicketDiscountLinkGenerator>();
            services.AddSingleton<IMailSender, MandrillSender>();
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