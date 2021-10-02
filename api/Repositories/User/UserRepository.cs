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

        public (IList<UserModel> users, int totalItemCount) GetUsers(int offset, int limit, UserFiltersDto filters)
        {
            IQueryable<UserModel> query = _context.User.OrderBy(p => p.Username);

            if (!string.IsNullOrEmpty(filters.Username))
            {
                query = query.Where(user => user.Username.ToLower().Equals(filters.Username.ToLower()));
            }
            if (!string.IsNullOrEmpty(filters.Email))
            {
                query = query.Where(user => user.Email.ToLower().Equals(filters.Email.ToLower()));
            }

            return (query
                .Skip(offset)
                .Take(limit)
                .ToList(), query.Count());
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

        public (IList<UserModel> users, int totalItemCount) SearchUser(int offset, int limit, string search)
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