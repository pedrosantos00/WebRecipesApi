using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebRecipesApi.Domain;

namespace WebRecipesApi.DAL
{
    public class RecipeRepository
    {
        private readonly WebRecipesDbContext _context;
        public RecipeRepository(WebRecipesDbContext context) => _context = context;
        //CRUD

        //CREATE
        public async Task<int> Create(Recipe recipe)
        {
            await _context.Recipes.AddAsync(recipe);
            await _context.SaveChangesAsync();
            return recipe.Id;
        }

        //RETRIEVE
        public async Task<Recipe> GetById(int id)
        {
            return await _context.Recipes.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<Recipe> GetByName(string title)
        {
            return await _context.Recipes.FirstOrDefaultAsync(u => u.Title == title);
        }


        public async Task<IEnumerable<Recipe>> Search(string? filterWord)
        {
            IEnumerable<Recipe> ListRecipes = new List<Recipe>();

            ListRecipes = _context.Recipes.Where(u =>
            string.IsNullOrEmpty(filterWord) ||
            u.Title.Contains(filterWord)
            );

            return ListRecipes;
        }


        //UPDATE
        public async Task<int> Update(Recipe recipe)
        {
            _context.Recipes.Update(recipe);
            await _context.SaveChangesAsync();
            return recipe.Id;
        }


        //DELETE
        public async void Delete(Recipe recipe)
        {
            _context.Recipes.Remove(recipe);
            await _context.SaveChangesAsync();
        }

    }
}
