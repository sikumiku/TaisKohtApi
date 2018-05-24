using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DAL.Interfaces;
using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

namespace DAL.TaisKoht.EF
{
    public class ApplicationDbContext : IdentityDbContext<User, Role, string, IdentityUserClaim<string>, UserRole, IdentityUserLogin<string>, IdentityRoleClaim<string>, IdentityUserToken<string>>, IDataContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<RestaurantUser>()
                .HasKey(x => new {x.RestaurantId, x.UserId});

            builder.Entity<MenuDish>()
                .HasKey(x => new {x.MenuId, x.DishId});

            builder.Entity<DishIngredient>()
                .HasKey(x => new {x.IngredientId, x.DishId});

            foreach (var relationship in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            base.OnModelCreating(builder);

            builder.Entity<User>().ToTable("User");
            builder.Entity<User>().Property(p => p.Id).HasColumnName("UserId");
            builder.Entity<Role>().ToTable("Role");
            builder.Entity<Role>().Property(p => p.Id).HasColumnName("RoleId");
            builder.Entity<UserRole>().ToTable("UserRole");
            builder.Entity<UserRole>().Property(p => p.UserId).HasColumnName("UserId");
            builder.Entity<UserRole>().Property(p => p.RoleId).HasColumnName("RoleId");
            builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaim");
            builder.Entity<IdentityUserClaim<string>>().Property(p => p.Id).HasColumnName("UserClaimId");
            builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogin");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaim");
            builder.Entity<IdentityRoleClaim<string>>().Property(p => p.Id).HasColumnName("RoleClaimId");
            builder.Entity<IdentityUserToken<string>>().ToTable("UserToken");
        }

        public DbSet<Address> Addresses { get; set; }
        public DbSet<Dish> Dishes { get; set; }
        public DbSet<DishIngredient> DishIngredients { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<MenuDish> MenuDishes { get; set; }
        public DbSet<Promotion> Promotions { get; set; }
        public DbSet<RatingLog> RatingLogs { get; set; }
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<RestaurantUser> RestaurantUsers { get; set; }
        public new DbSet<User> Users { get; set; }
        public new DbSet<UserRole> UserRoles { get; set; }
        public new DbSet<Role> Roles { get; set; }
    }
}
