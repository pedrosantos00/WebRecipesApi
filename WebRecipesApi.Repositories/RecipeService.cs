using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebRecipesApi.Domain;
using WebRecipesApi.DAL;

namespace WebRecipesApi.BusinessLogic
{
    public class RecipeService
    {
        private readonly RecipeRepository _recipeRepository;
        public RecipeService(RecipeRepository recipeRepository) => _recipeRepository = recipeRepository;

        //CRUD

        //CREATE
        public async Task<int> Create(Recipe recipe)
        {
            var id = 0;
            if (recipe == null) throw new ArgumentNullException(nameof(recipe));

            if (recipe != null) id = await _recipeRepository.Create(recipe);

            return id;
        }
        //RETRIEVE
        public async Task<Recipe> GetById(int id)
        {
            return await _recipeRepository.GetById(id);
        }

        public async Task<Recipe> GetByName(string name)
        {
            return await _recipeRepository.GetByName(name);
        }

        public async Task<List<Recipe>> Search (string? filterword)
        {
            IEnumerable<Recipe> recipeList = await _recipeRepository.Search(filterword);
            return recipeList.ToList();
        }
        //UPDATE
        public async Task<int> Update(Recipe recipe)
        {
            return await _recipeRepository.Update(recipe);
        }
        //DELETE
        public async Task<bool> Delete(int id)
        {
            Recipe? recipeToDelete = await _recipeRepository.GetById(id);
            if (recipeToDelete != null)
            {
                _recipeRepository.Delete(recipeToDelete);
                return true;
            }
            else return false;
        }

    }
}
