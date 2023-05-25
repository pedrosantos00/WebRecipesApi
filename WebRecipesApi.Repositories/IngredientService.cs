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
    public class IngredientService
    {
        private readonly IngredientRepository _ingredientRepository;
        public IngredientService(IngredientRepository ingredientRepository) => _ingredientRepository = ingredientRepository;

        //CRUD

        //CREATE
        public async Task<int> Create(Ingredient ingredient)
        {
            var id = 0;
            if (ingredient == null) throw new ArgumentNullException(nameof(ingredient));

            if (ingredient != null) id = await _ingredientRepository.Create(ingredient);

            return id;
        }
        //RETRIEVE
        public async Task<Ingredient> GetById(int id)
        {
            return await _ingredientRepository.GetById(id);
        }

        public async Task<Ingredient> GetByName(string name)
        {
            return await _ingredientRepository.GetByName(name);
        }


        public async Task<List<Ingredient>> Search(string? filterword)
        {
            IEnumerable<Ingredient> ingredientList = await _ingredientRepository.Search(filterword);
            return ingredientList.ToList();
        }
        //UPDATE
        public async Task<int> Update(Ingredient ingredient)
        {
            return await _ingredientRepository.Update(ingredient);
        }
        //DELETE
        public async Task<bool> Delete(int id)
        {
            Ingredient? ingredientToDelete = await _ingredientRepository.GetById(id);
            if (ingredientToDelete != null)
            {
                _ingredientRepository.Delete(ingredientToDelete);
                return true;
            }
            else return false;
        }

    }
}
