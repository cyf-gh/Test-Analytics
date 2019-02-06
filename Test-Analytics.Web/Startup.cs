using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Test_Analytics.Service;
using Test_Analytics.Service.Database;

namespace Test_Analytics.Web {
    public class Startup {
        public Startup( IConfiguration configuration ) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices( IServiceCollection services ) {
            services.AddDbContext<ACCContext>(
               options => {
                   options.UseSqlite( "Data Source=Test-Analytics.db" );
               } );
            services.AddScoped<ITestAnalyticsService, TestAnalyticsServiceImpl>();
            services.AddMvc().SetCompatibilityVersion( CompatibilityVersion.Version_2_1 );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure( IApplicationBuilder app, IHostingEnvironment env ) {
            if( env.IsDevelopment() ) {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            } else {
                app.UseExceptionHandler( "/Error" );
                app.UseHsts();
            }
            if( env.IsProduction() ) {
                app.UseForwardedHeaders( new ForwardedHeadersOptions {
                    ForwardedHeaders = Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedFor | Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedProto
                } );
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles( new StaticFileOptions {
                ServeUnknownFileTypes = true
            } );

            app.UseAuthentication();
            app.UseMvc( routes => {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}"
                    );
            } );
        }
    }
}
