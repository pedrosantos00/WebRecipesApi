using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebRecipesApi.Domain
{
    public class User
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? FullName { get; set; }
        public string Email { get; set; }
        public byte[]? ProfilePicture { get; set; }
        public string Password { get; set; }
        public bool IsBlocked { get; set; } = false;
        public ICollection<Recipe>? Recipes { get; set; }
        public ICollection<UserFavoriteRecipe>? FavoriteRecipes { get; set; }

        public ICollection<Comment>? Comments { get; set; }

        //JWT TOKEN PROPERTIES
        public string? Token { get; set; }
        public string? Role { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }




    }
}
