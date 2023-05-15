using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using WebRecipesApi.Domain;

namespace WebRecipesApi.DAL
{
    public class IngredientRepository
    {
        private readonly WebRecipesDbContext _context;
        public IngredientRepository(WebRecipesDbContext context) => _context = context;
        //CRUD

        //CREATE
        public async Task<int> Create(Ingredient ingredient)
        {
            await _context.Ingredients.AddAsync(ingredient);
            await _context.SaveChangesAsync();
            return ingredient.Id;
        }

        //RETRIEVE
        public async Task<Ingredient> GetById(int id)
        {
            return await _context.Ingredients.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<Ingredient> GetByName(string name)
        {
            return await _context.Ingredients.FirstOrDefaultAsync(u => u.Name == name);
        }


        public async Task<IEnumerable<Ingredient>> Search(string? filterWord)
        {
            IEnumerable<Ingredient> ListIngridients = new List<Ingredient>();

            ListIngridients = _context.Ingredients.Where(u =>
            string.IsNullOrEmpty(filterWord) ||
            u.Name.Contains(filterWord)
            );

            return ListIngridients;
        }


        //UPDATE
        public async Task<int> Update(Ingredient ingredient)
        {
            _context.Ingredients.Update(ingredient);
            await _context.SaveChangesAsync();
            return ingredient.Id;
        }


        //DELETE
        public async void Delete(Ingredient ingredient)
        {
            _context.Ingredients.Remove(ingredient);
            await _context.SaveChangesAsync();
        }

    }
}
