using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebRecipesApi.Domain;

namespace WebRecipesApi.DAL
{
    public class TagRepository
    {
        private readonly WebRecipesDbContext _context;
        public TagRepository(WebRecipesDbContext context) => _context = context;
        //CRUD

        //CREATE
        public async Task<int> Create(Tag tag)
        {
            await _context.Tags.AddAsync(tag);
            await _context.SaveChangesAsync();
            return tag.Id;
        }

        //RETRIEVE
        public async Task<Tag> GetById(int id)
        {
            return await _context.Tags.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<Tag> GetByName(string name)
        {
            return await _context.Tags.FirstOrDefaultAsync(u => u.TagName == name);
        }

        public async Task<IEnumerable<Tag>> Search(string? filterWord)
        {
            IEnumerable<Tag> ListTags = new List<Tag>();

            ListTags = _context.Tags.Where(u =>
            string.IsNullOrEmpty(filterWord) ||
            u.TagName.Contains(filterWord)
            );

            return ListTags;
        }


        //UPDATE
        public async Task<int> Update(Tag tag)
        {
            _context.Tags.Update(tag);
            await _context.SaveChangesAsync();
            return tag.Id;
        }


        //DELETE
        public async void Delete(Tag tag)
        {
            _context.Tags.Remove(tag);
            await _context.SaveChangesAsync();
        }

    }
}
