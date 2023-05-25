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
        public async Task<Recipe?> GetById(int id)
        {
            return await _recipeRepository.GetById(id);
        }

        public async Task<Recipe> GetByName(string name)
        {
            return await _recipeRepository.GetByName(name);
        }

        public async Task<List<Recipe>> Search(string? filterword)
        {
            IEnumerable<Recipe> recipeList = await _recipeRepository.Search(filterword);

            return recipeList.ToList();
        }

        public async Task<List<Recipe>> GetByUserId(int id)
        {
            IEnumerable<Recipe> recipeList = await _recipeRepository.GetByUserId(id);

            return recipeList.ToList();
        }

        public async Task<List<Recipe>> GetFavByUserId(int id)
        {
            IEnumerable<Recipe> recipeList = await _recipeRepository.GetFavByUserId(id);

            return recipeList.ToList();
        }


        public async Task<List<Recipe>> ToApprove()
        {
            IEnumerable<Recipe> recipeList = await _recipeRepository.ToApprove();

            return recipeList.ToList();
        }


        //UPDATE
        public async Task<int> Update(Recipe recipe, Recipe updatedRecipe)
        {
            if (!string.IsNullOrEmpty(updatedRecipe.Title) && recipe.Title != updatedRecipe.Title)
                recipe.Title = updatedRecipe.Title;

            if (!string.IsNullOrEmpty(updatedRecipe.Description) && recipe.Description != updatedRecipe.Description)
                recipe.Description = updatedRecipe.Description;

            if (updatedRecipe.Img != null && recipe.Img != updatedRecipe.Img)
                recipe.Img = updatedRecipe.Img;

            if (updatedRecipe.EstimatedTime != null && recipe.EstimatedTime != updatedRecipe.EstimatedTime)
                recipe.EstimatedTime = updatedRecipe.EstimatedTime;

            if (!string.IsNullOrEmpty(updatedRecipe.Difficulty) && recipe.Difficulty != updatedRecipe.Difficulty)
                recipe.Difficulty = updatedRecipe.Difficulty;

            if (updatedRecipe.MealsPerRecipe != null && recipe.MealsPerRecipe != updatedRecipe.MealsPerRecipe)
                recipe.MealsPerRecipe = updatedRecipe.MealsPerRecipe;

            if (updatedRecipe.Approved != null && recipe.Approved != updatedRecipe.Approved)
                recipe.Approved = updatedRecipe.Approved;

            // Check for deleted or updated tags
            if (updatedRecipe.Tags != null)
            {
                foreach (var existingTag in recipe.Tags.ToList())
                {
                    var updatedTag = updatedRecipe.Tags.FirstOrDefault(t => t.Id == existingTag.Id);

                    if (updatedTag != null)
                    {
                        if (existingTag.TagName != updatedTag.TagName)
                            existingTag.TagName = updatedTag.TagName;
                    }
                    else
                    {
                        recipe.Tags.Remove(existingTag);
                    }
                }

                // Check for new tags
                foreach (var newTag in updatedRecipe.Tags)
                {
                    if (!recipe.Tags.Any(t => t.Id == newTag.Id))
                        recipe.Tags.Add(newTag);
                }
            }
            else
            {
                recipe.Tags = null;
            }

            // Check for deleted or updated ingredients
            if (updatedRecipe.Ingredients != null)
            {
                foreach (var existingIngredient in recipe.Ingredients.ToList())
                {
                    var updatedIngredient = updatedRecipe.Ingredients.FirstOrDefault(i => i.Name == existingIngredient.Name);

                    if (updatedIngredient != null)
                    {
                        if (existingIngredient.Name != updatedIngredient.Name ||
                            existingIngredient.Quantity != updatedIngredient.Quantity ||
                            existingIngredient.QuantityType != updatedIngredient.QuantityType)
                        {
                            existingIngredient.Name = updatedIngredient.Name;
                            existingIngredient.Quantity = updatedIngredient.Quantity;
                            existingIngredient.QuantityType = updatedIngredient.QuantityType;
                        }
                    }
                    else
                    {
                        recipe.Ingredients.Remove(existingIngredient);
                    }
                }

                // Check for new ingredients
                foreach (var newIngredient in updatedRecipe.Ingredients)
                {
                    if (!recipe.Ingredients.Any(i => i.Name == newIngredient.Name))
                        recipe.Ingredients.Add(newIngredient);
                }
            }
            else
            {
                recipe.Ingredients = null;
            }

            // Check for deleted or updated steps
            if (updatedRecipe.Steps != null)
            {
                foreach (var existingStep in recipe.Steps.ToList())
                {
                    var updatedStep = updatedRecipe.Steps.FirstOrDefault(s => s.StepId == existingStep.StepId);

                    if (updatedStep != null)
                    {
                        if (existingStep.StepDescription != updatedStep.StepDescription)
                            existingStep.StepDescription = updatedStep.StepDescription;
                    }
                    else
                    {
                        recipe.Steps.Remove(existingStep);
                    }
                }

                // Check for new steps
                foreach (var newStep in updatedRecipe.Steps)
                {
                    if (!recipe.Steps.Any(s => s.StepId == newStep.StepId))
                        recipe.Steps.Add(newStep);
                }
            }
            else
            {
                recipe.Steps = null;
            }

            // Check for deleted or updated comments
            if (updatedRecipe.Comments != null)
            {
                foreach (var existingComment in recipe.Comments.ToList())
                {
                    var updatedComment = updatedRecipe.Comments.FirstOrDefault(c => c.Text == existingComment.Text);

                    if (updatedComment != null)
                    {
                        if (existingComment.Name != updatedComment.Name ||
                            existingComment.UserId != updatedComment.UserId)
                        {
                            existingComment.Name = updatedComment.Name;
                            existingComment.UserId = updatedComment.UserId;
                        }
                    }
                    else
                    {
                        recipe.Comments.Remove(existingComment);
                    }
                }

                // Check for new comments
                foreach (var newComment in updatedRecipe.Comments)
                {
                    if (!recipe.Comments.Any(c => c.Text == newComment.Text))
                        recipe.Comments.Add(newComment);
                }
            }
            else
            {
                recipe.Comments = null;
            }

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

        public async Task<int> UpdateRate(Recipe recipe)
        {
            return await _recipeRepository.Update(recipe);
        }

        public async Task<int> UpdateFav(Recipe recipe)
        {
            return await _recipeRepository.Update(recipe);
        }

        public async Task<int> UpdateComment(Recipe recipe)
        {
            return await _recipeRepository.Update(recipe);
        }
    }
}
