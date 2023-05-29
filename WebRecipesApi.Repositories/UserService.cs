using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WebRecipesApi.Domain;
using WebRecipesApi.DAL;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace WebRecipesApi.BusinessLogic
{
    public class UserService
    {
        private readonly UserRepository _userRepository;
        private readonly RecipeRepository _recipeRepository;



        public UserService(UserRepository userRepository, RecipeRepository recipeRepository)
        {
            _userRepository = userRepository;

            _recipeRepository = recipeRepository;
        }

        //CRUD

        //CREATE
        public async Task<int> Create(User user)
        {
            var id = 0;
            if (user == null) throw new ArgumentNullException(nameof(user));
            user.Password = PasswordHasher.HashPassword(user.Password);
            if (user != null) id = await _userRepository.Create(user);

            return id;
        }
        //RETRIEVE
        public async Task<User> GetById(int id)
        {
            return await _userRepository.GetById(id);
        }

        public async Task<User> GetByName(string name)
        {
            return await _userRepository.GetByName(name);
        }

        public async Task<User> GetByEmail(string email)
        {
            return await _userRepository.GetByEmail(email);
        }

        public async Task<List<User>> Search(string? filterword)
        {
            IEnumerable<User> userList = await _userRepository.Search(filterword);
            return userList.ToList();
        }
        //UPDATE
        public async Task<int> Update(User user, User updatedUser)
        {
            if (!string.IsNullOrEmpty(updatedUser.Email) && user.Email != updatedUser.Email)
                user.Email = updatedUser.Email;

            if (!string.IsNullOrEmpty(updatedUser.FirstName) && user.FirstName != updatedUser.FirstName)
                user.FirstName = updatedUser.FirstName;

            if (!string.IsNullOrEmpty(updatedUser.LastName) && user.LastName != updatedUser.LastName)
                user.LastName = updatedUser.LastName;

            if (!string.IsNullOrEmpty(updatedUser.FullName) && user.FullName != updatedUser.FullName)
                user.FullName = updatedUser.FullName;

            if (!string.IsNullOrEmpty(updatedUser.Role) && user.Role != updatedUser.Role)
                user.Role = updatedUser.Role;

            if (user.IsBlocked != updatedUser.IsBlocked)
                user.IsBlocked = updatedUser.IsBlocked;

            if (!string.IsNullOrEmpty(updatedUser.Password) && user.Password != updatedUser.Password)
            {
                user.Password = updatedUser.Password;
                user.Password = PasswordHasher.HashPassword(user.Password);
            }
            return await _userRepository.Update(user);
        }
      

        public async Task<bool> CheckEmailExistsAsync(string email) => await _userRepository.CheckEmailExistsAsync(email);

        public string CheckPasswordStrength(string password)
        {
            StringBuilder passwordRequirements = new StringBuilder();

            //Tamanho minimo
            if (password.Length < 8) passwordRequirements.Append("Minimum password length is 8. " + Environment.NewLine);
            if (!Regex.IsMatch(password, "[<,>,@,!,#,$,%,^,&,*,(,),_,+,\\[,\\],{,},?,:,;,|,',\\,.,/,~,`,-,=]")) passwordRequirements.Append("Should contain special characters" + Environment.NewLine);

            return passwordRequirements.ToString();
        }

        public bool RefreshTokenExists(string refreshtoken)
        {
            bool flag = _userRepository.RefreshTokenExists(refreshtoken);
            if (flag) return true; else return false;
        }


        //DELETE
        public async Task<bool> Delete(int id)
        {
            User? userToDelete = await _userRepository.GetById(id);
            if (userToDelete != null)
            {
                IEnumerable<Recipe> recipeList = await _recipeRepository.GetByUserId(userToDelete.Id, 0, 999);
                recipeList.ToList();

                if (recipeList.Count() != 0 )
                {
                    foreach(Recipe recipe in recipeList)
                    {
                        _recipeRepository.Delete(recipe);
                    }
                } 

                _userRepository.Delete(userToDelete);
                return true;
            }
            else { return false; }
        }
    }
}
