using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebRecipesApi.Domain;

namespace WebRecipesApi.DAL
{
    public class StepRepository
    {
        private readonly WebRecipesDbContext _context;
        public StepRepository(WebRecipesDbContext context) => _context = context;
        //CRUD

        //CREATE
        public async Task<int> Create(Step step)
        {
            await _context.Steps.AddAsync(step);
            await _context.SaveChangesAsync();
            return step.Id;
        }

        //RETRIEVE
        public async Task<Step> GetById(int id)
        {
            return await _context.Steps.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<Step> GetByName(string step)
        {
            return await _context.Steps.FirstOrDefaultAsync(u => u.StepDescription == step);
        }

        public async Task<IEnumerable<Step>> Search(string? filterWord)
        {
            IEnumerable<Step> ListSteps = new List<Step>();

            ListSteps = _context.Steps.Where(u =>
            string.IsNullOrEmpty(filterWord) ||
            u.StepDescription.Contains(filterWord)
            );

            return ListSteps;
        }


        //UPDATE
        public async Task<int> Update(Step step)
        {
            _context.Steps.Update(step);
            await _context.SaveChangesAsync();
            return step.Id;
        }


        //DELETE
        public async void Delete(Step step)
        {
            _context.Steps.Remove(step);
            await _context.SaveChangesAsync();
        }

    }
}
