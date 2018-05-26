using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using DAL.Interfaces;
using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

namespace DAL.TaisKoht.EF
{
    public class ApplicationDbContext : IdentityDbContext<User, Role, string>, IDataContext
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
            
            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                // https://www.meziantou.net/2017/07/10/entity-framework-core-soft-delete-using-query-filters
                // Implement soft Delete for all Entities
                // 1. Add the Active property
                entityType.GetOrAddProperty("Active", typeof(bool));

                // 2. Create the query filter
                var parameter = Expression.Parameter(entityType.ClrType);

                // EF.Property<bool>(post, "Active")
                var boolPropertyMethodInfo = typeof(Microsoft.EntityFrameworkCore.EF).GetMethod("Property").MakeGenericMethod(typeof(bool));
                var activeProperty = Expression.Call(boolPropertyMethodInfo, parameter, Expression.Constant("Active"));

                // EF.Property<bool>(post, "Active") == true
                BinaryExpression compareExpression = Expression.MakeBinary(ExpressionType.Equal, activeProperty, Expression.Constant(true));

                // post => EF.Property<bool>(post, "Active") == true
                var lambda = Expression.Lambda(compareExpression, parameter);

                builder.Entity(entityType.ClrType).HasQueryFilter(lambda);

                // Add default values to timestamps
                // 1. Add the properties
                entityType.GetOrAddProperty("AddTime", typeof(DateTime));

                builder.Entity(entityType.ClrType).Property("AddTime").
                    HasComputedColumnSql("GetUtcDate()");

                entityType.GetOrAddProperty("UpdateTime", typeof(DateTime));

                builder.Entity(entityType.ClrType).Property("UpdateTime").
                    HasComputedColumnSql("GetUtcDate()");
            }


            builder.Entity<User>().ToTable("User");
            builder.Entity<User>().Property(p => p.Id).HasColumnName("UserId");
            builder.Entity<Role>().ToTable("Role");
            builder.Entity<Role>().Property(p => p.Id).HasColumnName("RoleId");
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRole");
            builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaim");
            builder.Entity<IdentityUserClaim<string>>().Property(p => p.Id).HasColumnName("UserClaimId");
            builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogin");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaim");
            builder.Entity<IdentityRoleClaim<string>>().Property(p => p.Id).HasColumnName("RoleClaimId");
            builder.Entity<IdentityUserToken<string>>().ToTable("UserToken");
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            OnBeforeSaving();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            OnBeforeSaving();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void OnBeforeSaving()
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.CurrentValues["Active"] = true;
                        break;

                    case EntityState.Deleted:
                        entry.State = EntityState.Modified;
                        entry.CurrentValues["Active"] = false;
                        entry.CurrentValues["UpdateTime"] = DateTime.UtcNow;
                        break;

                    case EntityState.Modified:
                        entry.CurrentValues["UpdateTime"] = DateTime.UtcNow;
                        break;
                }
            }
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
        public new DbSet<Role> Roles { get; set; }
    }
}
