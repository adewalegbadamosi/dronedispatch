using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Drones.Context;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore.InMemory;
using Drones.Models;
using Drones.Interfaces;
using Drones.Repository;

namespace Drones
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

            // Database connection
            services.AddDbContext<ApplicationContext>(opt => opt.UseInMemoryDatabase(databaseName: "DroneDispatchDb")); 

            
            //services.AddDbContext<DataContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DbConnectionString")));
            //var conString = Configuration.GetConnectionString("DbConnectionString");
            //services.AddDbContext<DataContext>(options => options.UseMySql(conString, ServerVersion.AutoDetect(conString)));

            services.AddControllers();

            //services.AddMvc();

            services.AddSwaggerGen(swagger =>
            {
                //This is to generate the Default UI of Swagger Documentation    
                swagger.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "DronesDispatch.WebApi",
                    Description = "Api documentation for Drones Dispatch."
                });
               
            });

            #region Repositories            
            
            services.AddTransient<IDispatchRepository, DispatchRepository>();
            services.AddTransient<IAuditTrailRepository, AuditTrailRepository>();
            #endregion

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage(); 
            }

            app.UseHttpsRedirection();

            app.UseRouting();


            app.UseAuthorization();

            //loggerFactory.CreateLogger(Configuration.GetSection("Logging:LogLevel").Value);
            

            //var context = app.ApplicationServices.GetService<ApplicationContext>();
            //AddTestData(context);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                //endpoints.MapControllerRoute(
                //    name: "default",
                //    pattern: "{controller=Dispatch}/{action=InitializeDb}/{id?}"
                //    );
            });

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();
            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Drones.WebApi");

                // To serve SwaggerUI at application's root page, set the RoutePrefix property to an empty string.
                //c.RoutePrefix = string.Empty;
            });

           

            //app.UseMvc();
        }

       



    }
}
