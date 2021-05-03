using xLiAdProjectTemplate.Entities;
using xLiAdProjectTemplate.Entities.Dtos;
using xLiAdProjectTemplate.Services.Assets;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using xLiAd.AspectCache.Memory;
using xLiAd.AspectCache.Redis;
using xLiAd.DapperEx.Repository;
using xLiAd.DiagnosticLogCenter.Agent;
using xLiAd.Gaia.Core;
using xLiAd.Limiter.Memory;

namespace xLiAdProjectTemplate.WebApp
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
            services.Configure<FormOptions>(options =>
            {
                options.ValueCountLimit = 5000; // 5000 items max
                options.ValueLengthLimit = 1024 * 1024 * 100; // 100MB max len form data
            });
            services.AddHttpClient();//��ʹ�� HttpClientFactory ����ɾ�����
            services.AddHttpContextAccessor();//����Ҫ HttpContext ����ɾ�����
            var conf = Configuration.Get<ConfigEntity>();
            services.Configure<ConfigEntity>(Configuration);
            services.AddScoped<IConfigEntity>(x => x.GetService<IOptionsSnapshot<ConfigEntity>>().Value);
            services.AddScoped<IDbConnection>(x => new SqlConnection(x.GetService<IConfigEntity>().SqlConnectionString));
            services.AddScoped<IConnectionHolder>(x => { var conn = x.GetService<IDbConnection>(); return new ConnectionHolder(conn); });
            services.AddDiagnosticLog(x =>
            {
                x.CollectServerAddress = conf.ExceptionLogCenterUrl;
                x.ClientName = conf.ExceptionLogCenterClient;
                x.EnvName = conf.ExceptionLogCenterEnv;
                x.ForbbidenPath = new string[]
                {
                    "/css/(.*)","/js/(.*)","/images/(.*)"
                };
            });
            //����
            if (conf.CacheType?.Equals("Redis", StringComparison.OrdinalIgnoreCase) ?? false)
                services.AddRedisAspectCache(x => { x.RedisUrl = conf.RedisUrl; x.CacheDefaultLifeTime = TimeSpan.FromMinutes(conf.CacheTimeoutMinute); });
            else
                services.AddMemoryAspectCache(x => x.CacheDefaultLifeTime = TimeSpan.FromMinutes(conf.CacheTimeoutMinute));
            services.AddLimiterMemory();
            //����Ҫ����Ļ����Ͳ����±����
            services.AddCors(options =>
            {
                // this defines a CORS policy called "default"
                options.AddPolicy("default", policy =>
                {
                    policy
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });
            services.AddResponseCompression(options =>
            {
                options.Providers.Add<GzipCompressionProvider>();
            });
            //services.AddControllersWithViews().AddRazorRuntimeCompilation().AddJsonOptions(opt =>
            //{
            //    opt.JsonSerializerOptions.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(System.Text.Unicode.UnicodeRanges.All);
            //    opt.JsonSerializerOptions.Converters.Add(new DateTimeConverter());
            //    opt.JsonSerializerOptions.PropertyNamingPolicy = null;//��������� camle ������� pascal
            //});
            services.AddControllersWithViews().AddRazorRuntimeCompilation().AddNewtonsoftJson(opt =>
            {
                var resolver = opt.SerializerSettings.ContractResolver;
                if (resolver != null)
                {
                    var res = resolver as DefaultContractResolver;
                    res.NamingStrategy = null;  // <<!-- this removes the camelcasing
                }
                opt.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Local;
                opt.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            });
            services.AddGaiaWithSqlServer();
        }
        public void ConfigureContainer(IServiceCollection services)
        {
            var repoTypes = typeof(Infrastructure.AuthRoleRepository).Assembly.GetTypes()
                .Where(x => x.Name.EndsWith("repository", StringComparison.OrdinalIgnoreCase) ||
                x.Name.EndsWith("service", StringComparison.OrdinalIgnoreCase));
            var servTypes = typeof(Services.AuthRoleService).Assembly.GetTypes()
                .Where(x => x.Name.EndsWith("service", StringComparison.OrdinalIgnoreCase));
            foreach (var repoType in repoTypes)
            {
                var inter = repoType.GetInterfaces().Where(x => !x.FullName.StartsWith("xLiAd")).FirstOrDefault();
                if (inter != null)
                    services.AddScoped(inter, repoType);
            }
            foreach (var servType in servTypes)
            {
                var inter = servType.GetInterfaces().FirstOrDefault();
                if (inter != null)
                    services.AddScoped(inter, servType);
            }
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
            app.UseResponseCompression();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseCors("default");//����Ҫ����Ļ����Ͳ������
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
