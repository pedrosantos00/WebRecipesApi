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
    public class StepService
    {
        private readonly StepRepository _stepRepository;

        public StepService(StepRepository stepRepository) => _stepRepository = stepRepository;

        //CRUD

        //CREATE
        public async Task<int> Create(Step step)
        {
            var id = 0;
            if (step == null) throw new ArgumentNullException(nameof(step));

            if (step != null) id = await _stepRepository.Create(step);

            return id;
        }
        //RETRIEVE
        public async Task<Step> GetById(int id)
        {
            return await _stepRepository.GetById(id);
        }

        public async Task<Step> GetByName(string name)
        {
            return await _stepRepository.GetByName(name);
        }


        public async Task<List<Step>> Search (string? filterword)
        {
            IEnumerable<Step> stepList = await _stepRepository.Search(filterword);
            return stepList.ToList();
        }
        //UPDATE
        public async Task<int> Update(Step step)
        {
            return await _stepRepository.Update(step);
        }
        //DELETE
        public async Task<bool> Delete(int id)
        {
            Step? stepToDelete = await _stepRepository.GetById(id);
            if (stepToDelete != null)
            {
                _stepRepository.Delete(stepToDelete);
                return true;
            }
            else return false;
        }

    }
}
