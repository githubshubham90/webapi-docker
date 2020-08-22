using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserService.Models
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder app)
        {
            try
            {
                using (var serviceScope = app.ApplicationServices.CreateScope())
                {
                    SeedData(serviceScope.ServiceProvider.GetService<UserContext>());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("PrePopulation method");
                Console.WriteLine(ex.Message);
            }

        }

        public static void SeedData(UserContext context)
        {
            try
            {
                Console.WriteLine("Appling Migration.....");
                var connection = context.Database.GetDbConnection();
                Console.WriteLine("Connection string is:  " + connection.ConnectionString);
                context.Database.Migrate();

                if (!context.Users.Any())
                {
                    Console.WriteLine("Adding Data - seeding......");
                    context.Users.AddRange(
                        new User { Name = "John", Age= 23, Email = "john.doe@google.com" }
                        );
                    context.SaveChanges();
                }
                else
                {
                    Console.WriteLine("Already have data...... No Seeding");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
    }
}
