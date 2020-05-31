using BarCrawlers.Data.DBModels;
using BarCrawlers.Data.ModelSettings;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace BarCrawlers.Data
{
    public class BCcontext : IdentityDbContext<User, Role, Guid>
    {
        public BCcontext(DbContextOptions<BCcontext> options)
    :       base(options)
        {
        }

        public DbSet<Bar> Bars { get; set; }
        public DbSet<BarUserComment> BarComments { get; set; }
        public DbSet<Cocktail> Cocktails { get; set; }
        public DbSet<CocktailBar> CocktailBars { get; set; }
        public DbSet<CocktailIngredient> CocktailIngredients { get; set; }
        public DbSet<CocktailUserComment> CocktailComments { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<UserBarRating> BarRatings { get; set; }
        public DbSet<UserCocktailRating> CocktailRatings { get; set; }



        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new BarSettings());
            builder.ApplyConfiguration(new BarUserCommentSettings());
            builder.ApplyConfiguration(new CocktailBarSettings());
            builder.ApplyConfiguration(new CocktailIngredientSettings());
            builder.ApplyConfiguration(new CocktailSettings());
            builder.ApplyConfiguration(new CocktailUserCommentSettings());
            builder.ApplyConfiguration(new IngredientSettings());
            builder.ApplyConfiguration(new LocationSettings());
            builder.ApplyConfiguration(new UserBarRatingSettings());
            builder.ApplyConfiguration(new UserCocktailRatingSettings());
            

            base.OnModelCreating(builder);

            //Global filters for IsDeleted == false:

            //builder.Entity<Cocktail>().HasQueryFilter(c => !c.IsDeleted);
            //builder.Entity<Bar>().HasQueryFilter(b => !b.IsDeleted);

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
    }
}
