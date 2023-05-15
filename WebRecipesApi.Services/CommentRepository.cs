using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebRecipesApi.Domain;

namespace WebRecipesApi.DAL
{
    public class CommentRepository
    {
        private readonly WebRecipesDbContext _context;
        public CommentRepository(WebRecipesDbContext context) => _context = context;
        //CRUD

        //CREATE
        public async Task<int> Create(Comment comment)
        {
            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();
            return comment.Id;
        }

        //RETRIEVE
        public async Task<Comment> GetById(int id)
        {
            return await _context.Comments.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<Comment> GetByName(string text)
        {
            return await _context.Comments.FirstOrDefaultAsync(u => u.Text == text);
        }

        public async Task<IEnumerable<Comment>> Search(string? filterWord)
        {
            IEnumerable<Comment> ListComments = new List<Comment>();

            ListComments = _context.Comments.Where(u =>
            string.IsNullOrEmpty(filterWord) ||
            u.Text.Contains(filterWord)
            );

            return ListComments;
        }


        //UPDATE
        public async Task<int> Update(Comment comment)
        {
            _context.Comments.Update(comment);
            await _context.SaveChangesAsync();
            return comment.Id;
        }


        //DELETE
        public async void Delete(Comment comment)
        {
            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
        }

    }
}
