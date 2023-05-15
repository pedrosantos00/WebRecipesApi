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
    public class CommentService
    {
        private readonly CommentRepository _commentRepository;
        public CommentService(CommentRepository commentRepository) => _commentRepository = commentRepository;

        //CRUD

        //CREATE
        public async Task<int> Create(Comment comment)
        {
            var id = 0;
            if (comment == null) throw new ArgumentNullException(nameof(comment));

            if (comment != null) id = await _commentRepository.Create(comment);

            return id;
        }
        //RETRIEVE
        public async Task<Comment> GetById(int id)
        {
            return await _commentRepository.GetById(id);
        }

        public async Task<Comment> GetByName(string name)
        {
            return await _commentRepository.GetByName(name);
        }

        public async Task<List<Comment>> Search(string? filterword)
        {
            IEnumerable<Comment> commentList = await _commentRepository.Search(filterword);
            return commentList.ToList();
        }
        //UPDATE
        public async Task<int> Update(Comment comment)
        {
            return await _commentRepository.Update(comment);
        }
        //DELETE
        public async Task<bool> Delete(int id)
        {
            Comment? commentToDelete = await _commentRepository.GetById(id);
            if (commentToDelete != null)
            {
                _commentRepository.Delete(commentToDelete);
                return true;
            }
            else return false;
        }

    }
}
