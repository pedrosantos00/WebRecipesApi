using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebRecipesApi.Domain;

namespace WebRecipesApi.DAL
{
    public class WebRecipesDbContext : DbContext
    {
        public WebRecipesDbContext(DbContextOptions<WebRecipesDbContext> options) : base (options) { }

        public DbSet<Comment> Comments { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Recipe> Recipes{ get; set; }
        public DbSet<Step> Steps { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<User> Users{ get; set; }
        public DbSet<UserFavoriteRecipe> UserFavoriteRecipes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Recipe>()
                .HasOne(r => r.User)
                .WithMany(u => u.Recipes)
                .HasForeignKey(r => r.UserId);


            //modelBuilder.Entity<UserFavoriteRecipe>()
            //    .HasMany(uf => uf.user)
            //    .WithMany(u => u.FavoriteRecipes)
            //    .HasForeignKey(uf => uf.UserId);

            //modelBuilder.Entity<UserFavoriteRecipe>()
            //    .HasOne(uf => uf.Recipe)
            //    .WithMany(r => r.FavoritedBy)
            //    .HasForeignKey(uf => uf.RecipeId);
        }
    }

}
