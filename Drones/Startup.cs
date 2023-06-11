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
using Drones.SeedDb;
using AutoMapper;
using Drones.Mapper;

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
                     
            services.AddControllers();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            //services.AddAutoMapper(typeof(ProfileMapper));


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

        
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();             
            });

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();
            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Drones.WebApi");
            });

            PrepDb.PrepPopulation(app, env.IsProduction());

        }
    }
}
