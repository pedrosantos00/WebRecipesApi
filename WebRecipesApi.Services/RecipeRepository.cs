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
        public async Task<Recipe?> GetById(int id)
        {
            Recipe? recipe = await _context.Recipes
                .Include(r => r.Tags)
                .Include(r => r.Steps)
                .Include(r => r.Comments)
                .Include(r => r.FavoritedBy)
                .Include(r => r.Ingredients)
                .FirstOrDefaultAsync(u => u.Id == id);


            return recipe;
        }

        public async Task<Recipe> GetByName(string title)
        {
            return await _context.Recipes.FirstOrDefaultAsync(u => u.Title == title);
        }

        public async Task<IEnumerable<Recipe>> GetByUserId(int id)
        {
            IQueryable<Recipe> query = _context.Recipes
                .Include(r => r.Tags)
                .Include(r => r.Steps)
                .Include(r => r.FavoritedBy)
                .Include(r => r.Ingredients)
                .Where(u =>
                    u.Approved == true &&
                    u.UserId == id
                );

            var recipes = await query.ToListAsync();



            return recipes;
        }


        public async Task<IEnumerable<Recipe>> GetFavByUserId(int id)
        {
            IQueryable<Recipe> query = _context.Recipes
                .Include(r => r.Tags)
                .Include(r => r.Steps)
                .Include(r => r.FavoritedBy)
                .Include(r => r.Ingredients)
                .Where(u =>
                    u.FavoritedBy.Any(x => x.UserId == id)
                );

            var recipes = await query.ToListAsync();



            return recipes;
        }


        public async Task<IEnumerable<Recipe>> Search(string? filterWord)
        {
            IQueryable<Recipe> query = _context.Recipes
                .Include(r => r.Tags)
                .Include(r => r.Steps)
                .Include(r => r.FavoritedBy)
                .Include(r => r.Ingredients)
                .Where(u =>
                    u.Approved == true &&
                    (string.IsNullOrEmpty(filterWord) || u.Title.Contains(filterWord))
                );

            var recipes = await query.ToListAsync();

          

            return recipes;
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
            _context.SaveChanges();
        }

        public async Task<IEnumerable<Recipe>> ToApprove()
        {
            IEnumerable<Recipe> ListRecipes = new List<Recipe>();

            ListRecipes = _context.Recipes
       .Include(r => r.Tags)
       .Include(r => r.Steps)
       .Include(r => r.Comments)
       .Include(r => r.FavoritedBy)
       .Include(r => r.Ingredients)
       .Where(u =>
           u.Approved == false
       );

            return ListRecipes;
        }

       
    }
}
