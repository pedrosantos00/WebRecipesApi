using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebRecipesApi.Domain;

namespace WebRecipesApi.DAL
{
    public class UserRepository
    {
        private readonly WebRecipesDbContext _context;

        public UserRepository(WebRecipesDbContext context) => _context = context;

        //CRUD

        //CREATE
        public async Task<int> Create(User user)
        {
            user.FullName = $"{user.FirstName} {user.LastName}";
            user.Role = "User";
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user.Id;
        }

        //RETRIEVE
        public async Task<User> GetById(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User> GetByName(string name)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.FullName == name);
        }

        public async Task<User> GetByEmail(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<IEnumerable<User>> Search(string? filterWord)
        {
            IEnumerable<User> ListUsers = new List<User>();

            ListUsers = _context.Users.Where(u =>
            string.IsNullOrEmpty(filterWord) ||
            u.FullName.Contains(filterWord) ||
            u.Email.Contains(filterWord)
            );

            return ListUsers;
        }


        //UPDATE
        public async Task<int> Update(User user)
        {
            user.FullName = $"{user.FirstName} {user.LastName}";
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user.Id;
        }


        //DELETE
        public async void Delete(User user)
        {
            _context.Users.Remove(user);
            _context.SaveChanges();
        }

        public async Task<bool> CheckEmailExistsAsync(string email) => await _context.Users.AnyAsync(u => u.Email == email);

        public bool RefreshTokenExists(object refreshtoken)
        {
            bool flag = _context.Users.Any(u => u.RefreshToken == refreshtoken);
            if (flag) return true; else return false;
        }


    }
}
