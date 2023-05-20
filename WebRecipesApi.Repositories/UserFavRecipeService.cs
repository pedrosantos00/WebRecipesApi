using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebRecipesApi.DAL;
using WebRecipesApi.Domain;

namespace WebRecipesApi.BusinessLogic
{
    public class UserFavoriteRecipeService
    {
        private readonly UserFavRecipeRepository _userFavRecipeServiceRepository;
        public UserFavoriteRecipeService(UserFavRecipeRepository userFavRecipeServiceRepository) => _userFavRecipeServiceRepository = userFavRecipeServiceRepository;

        //CRUD

        //CREATE
        public async Task<int> Create(UserFavoriteRecipe userFavRecipeService)
        {
            var id = 0;
            if (userFavRecipeService == null) throw new ArgumentNullException(nameof(userFavRecipeService));

            if (userFavRecipeService != null) id = await _userFavRecipeServiceRepository.Create(userFavRecipeService);

            return id;
        }
        //RETRIEVE
        public async Task<UserFavoriteRecipe> GetById(int id)
        {
            return await _userFavRecipeServiceRepository.GetById(id);
        }


        public async Task<UserFavoriteRecipe> Exists(int recipeId, int userId)
        {
            return await _userFavRecipeServiceRepository.Exists(recipeId, userId);
        }


        public async Task<UserFavoriteRecipe> GetByName(string name)
        {
            return await _userFavRecipeServiceRepository.GetByName(name);
        }


        public async Task<List<UserFavoriteRecipe>> Search(string? filterword)
        {
            IEnumerable<UserFavoriteRecipe> userFavRecipeServiceList = await _userFavRecipeServiceRepository.Search(filterword);
            return userFavRecipeServiceList.ToList();
        }
        //UPDATE
        public async Task<int> Update(UserFavoriteRecipe userFavRecipeService)
        {
            return await _userFavRecipeServiceRepository.Update(userFavRecipeService);
        }
        //DELETE
        public async Task<bool> Delete(int id)
        {
            UserFavoriteRecipe? userFavRecipeServiceToDelete = await _userFavRecipeServiceRepository.GetById(id);
            if (userFavRecipeServiceToDelete != null)
            {
                _userFavRecipeServiceRepository.Delete(userFavRecipeServiceToDelete);
                return true;
            }
            else return false;
        }

    }
}