namespace WebRecipesApi.Domain
{
    public class Comment
    {
        public int Id { get; set; }
        public string Text { get; set; }


        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; } = null!;

        public int? UserId { get; set; }
        public User? CommentOwner { get; set; } = null!;
    }
}