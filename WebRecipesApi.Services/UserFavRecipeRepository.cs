using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebRecipesApi.Domain;

namespace WebRecipesApi.DAL
{
    public class UserFavRecipeRepository
    {
        private readonly WebRecipesDbContext _context;
        public UserFavRecipeRepository(WebRecipesDbContext context) => _context = context;
        //CRUD

        //CREATE
        public async Task<int> Create(UserFavoriteRecipe userFavRecipe)
        {
            await _context.UserFavoriteRecipes.AddAsync(userFavRecipe);
            await _context.SaveChangesAsync();
            return userFavRecipe.UserId;
        }

        //RETRIEVE
        public async Task<UserFavoriteRecipe> GetById(int id)
        {
            return await _context.UserFavoriteRecipes.FirstOrDefaultAsync(u => u.UserId == id);
        }


        public async Task<UserFavoriteRecipe> Exists(int recipeId, int userId)
        {
            return await _context.UserFavoriteRecipes
            .FirstOrDefaultAsync(uf => uf.RecipeId == recipeId && uf.UserId == userId);
        }

        public async Task<UserFavoriteRecipe> GetByName(string name)
        {
            return await _context.UserFavoriteRecipes.FirstOrDefaultAsync(u => u.RecipeId == 2);
        }


        public async Task<IEnumerable<UserFavoriteRecipe>> Search(string? filterWord)
        {
            IEnumerable<UserFavoriteRecipe> ListIngridients = new List<UserFavoriteRecipe>();

            return ListIngridients;
        }


        //UPDATE
        public async Task<int> Update(UserFavoriteRecipe userFavRecipe)
        {
            _context.UserFavoriteRecipes.Update(userFavRecipe);
            await _context.SaveChangesAsync();
            return userFavRecipe.UserId;
        }


        //DELETE
        public async void Delete(UserFavoriteRecipe userFavRecipe)
        {
            _context.UserFavoriteRecipes.Remove(userFavRecipe);
            await _context.SaveChangesAsync();
        }

    }
}
