namespace WebRecipesApi.Domain
{
    public class Comment
    {
        public int Id { get; set; }
        public string? Text { get; set; }
        public string? Name { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? RecipeId { get; set; }

        public int UserId { get; set; }
    }
}