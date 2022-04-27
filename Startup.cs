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
using Hangfire;
using Hangfire.MemoryStorage;
using HangFireService.Jobs;
namespace HangFireService
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
            services.AddHangfire(config =>
                config.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseDefaultTypeSerializer()
                .UseMemoryStorage());
                //.UseSqlServerStorage(Configuration["Hangfire"]));

            services.AddHangfireServer();

            services.AddControllers();

            services.AddSingleton<IPrintJob, PrintJob>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app
        , IWebHostEnvironment env
        ,IBackgroundJobClient backgroundJobClient
        ,IRecurringJobManager recurringJobManager
        ,IServiceProvider serviceProvider)
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

            //app.UseHangfireDashboard();
            
            app.UseHangfireDashboard("/hangfire", new DashboardOptions()
            {
                
                Authorization = new List<NoAuthFilter> { new NoAuthFilter() },
                StatsPollingInterval = 60000 //can't seem to find the UoM on github - would love to know if this is seconds or ms
            }
            );
        


        backgroundJobClient.Enqueue(() => Console.WriteLine("HangFire job!"));
            recurringJobManager.AddOrUpdate(
                "Run every minute",
                () => serviceProvider.GetService<IPrintJob>().Print(),
                "* * * * *"
                );


        }
    }
}
