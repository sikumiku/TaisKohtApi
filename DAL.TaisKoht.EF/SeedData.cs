using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace DAL.TaisKoht.EF
{
    public static class SeedData
    {

        public static void Initialize(IServiceProvider serviceProvider)
        {

            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
            context.Database.EnsureCreated();
            if (!context.Roles.Any())
            {
                context.Roles.Add(new Role { Name = "admin", NormalizedName = "ADMIN", AccessLevel = "200", Description = "System administrator with god-like abilities"});
                context.Roles.Add(new Role { Name = "premiumUser", NormalizedName = "PREMIUMUSER", AccessLevel = "100", Description = "Subscribed user with advanced privileges"});
                context.Roles.Add(new Role { Name = "normalUser", NormalizedName = "NORMALUSER", AccessLevel = "1", Description = "Normal user"});
                context.SaveChanges(); 
            }
        }
    }
}
