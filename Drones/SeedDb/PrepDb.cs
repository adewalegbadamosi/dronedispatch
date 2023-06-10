using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Drones.Models;
using Drones.Context;
using Microsoft.OpenApi.Writers;
using Drones.Interfaces;

namespace Drones.SeedDb
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder app, bool isProd)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())           {
               

                var dispatchRepository = serviceScope.ServiceProvider.GetRequiredService<IDispatchRepository>();

                try
                {
                    var response = dispatchRepository.AddDefaultData();

                    Console.WriteLine($"--> {response}");

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Could not initiailize database {ex.Message}");
                }
            }
        }

    }
    }