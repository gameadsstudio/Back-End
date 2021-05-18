using System;
using System.Collections.Generic;
using System.Linq;
using api.Contexts;
using api.Models.User;

namespace api.Repositories.User
{
    public class UserRepository : IUserRepository
    {
        private readonly ApiContext _context;

        public UserRepository(ApiContext context)
        {
            _context = context;
        }

        public UserModel GetUserById(Guid id)
        {
            return _context.User.SingleOrDefault(a => a.Id == id);
        }

        public UserModel GetUserByUsername(string username)
        {
            return _context.User.SingleOrDefault(a => a.Username == username);
        }

        public UserModel GetUserByEmail(string email)
        {
            return _context.User.SingleOrDefault(a => a.Email == email);
        }

        public List<UserModel> GetUsers(int offset, int limit)
        {
            return _context.User.OrderBy(p => p.Id)
                .Skip(offset)
                .Take(limit)
                .ToList();
        }

        public UserModel AddNewUser(UserModel user)
        {
            _context.User.Add(user);
            _context.SaveChanges();
            return user;
        }

        public UserModel UpdateUser(UserModel updatedUser)
        {
            _context.Update(updatedUser);
            _context.SaveChanges();
            return updatedUser;
        }

        public int DeleteUser(UserModel user)
        {
            _context.User.Remove(user);
            return _context.SaveChanges();
        }

        public int CountUsers()
        {
            return _context.User.Count();
        }

        public (List<UserModel> users, int count) SearchUser(int offset, int limit, string search)
        {
                var query = _context.User.Where(user =>
                    user.Username.ToLower().Contains(search.ToLower()) ||
                    user.Email.ToLower().Contains(search.ToLower()));

                return (query
                    .OrderBy(user => user.Username)
                    .Skip(offset)
                    .Take(limit)
                    .ToList(), query.Count());
        }
    }
}