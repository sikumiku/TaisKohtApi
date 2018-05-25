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
        private static readonly UserManager<User> _userManager;

        public static void Initialize(IServiceProvider serviceProvider)
        {

            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
            context.Database.EnsureCreated();
            if (!context.Roles.Any())
            {
                context.Roles.Add(new Role { Name = "admin", AccessLevel = "200", Description = "System administrator with god-like abilities", UpdateTime = DateTime.UtcNow, AddTime = DateTime.UtcNow, Active = true });
                context.Roles.Add(new Role { Name = "premiumUser", AccessLevel = "100", Description = "Subscribed user with advanced privileges", UpdateTime = DateTime.UtcNow, AddTime = DateTime.UtcNow, Active = true });
                context.Roles.Add(new Role { Name = "normalUser", AccessLevel = "1", Description = "Normal user", UpdateTime = DateTime.UtcNow, AddTime = DateTime.UtcNow, Active = true });
                context.SaveChanges();
            }
        }
    }
}
