using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using xLiAd.ProjectFactory.Core;
using xLiAd.ProjectFactory.WebApp.Models;

namespace xLiAd.ProjectFactory.WebApp
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
            var conf = Configuration.Get<ConfigModel>();
            services.Configure<ConfigModel>(Configuration);
            services.AddScoped<IConfigModel>(x => x.GetService<IOptionsSnapshot<ConfigModel>>().Value);
            services.AddSingleton<CodeLoader>(x =>
            {
                return new CodeLoader(conf.SolutionPath, conf.ProjectPre);
            });
            services.AddSingleton<IConvertService>(x =>
            {
                var codeLoader = x.GetService<CodeLoader>();
                return new ConvertService(codeLoader);
            });

            services.AddControllersWithViews().AddRazorRuntimeCompilation().AddNewtonsoftJson(opt =>
            {
                var resolver = opt.SerializerSettings.ContractResolver;
                if (resolver != null)
                {
                    var res = resolver as Newtonsoft.Json.Serialization.DefaultContractResolver;
                    res.NamingStrategy = null;  // <<!-- this removes the camelcasing
                }
                opt.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Local;
                opt.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
