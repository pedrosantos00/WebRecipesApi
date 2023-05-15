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
    public class TagService
    {
        private readonly TagRepository _tagRepository;
        public TagService(TagRepository tagRepository) => _tagRepository = tagRepository;

        //CRUD

        //CREATE
        public async Task<int> Create(Tag tag)
        {
            var id = 0;
            if (tag == null) throw new ArgumentNullException(nameof(tag));

            if (tag != null) id = await _tagRepository.Create(tag);

            return id;
        }
        //RETRIEVE
        public async Task<Tag> GetById(int id)
        {
            return await _tagRepository.GetById(id);
        }

        public async Task<Tag> GetByName(string name)
        {
            return await _tagRepository.GetByName(name);
        }


        public async Task<List<Tag>> Search (string? filterword)
        {
            IEnumerable<Tag> tagList = await _tagRepository.Search(filterword);
            return tagList.ToList();
        }
        //UPDATE
        public async Task<int> Update(Tag tag)
        {
            return await _tagRepository.Update(tag);
        }
        //DELETE
        public async Task<bool> Delete(int id)
        {
            Tag? tagToDelete = await _tagRepository.GetById(id);
            if (tagToDelete != null)
            {
                _tagRepository.Delete(tagToDelete);
                return true;
            }
            else return false;
        }

    }
}
