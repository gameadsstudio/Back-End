﻿using System;
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
        
        public UserPublicModel[] GetUsers(int offset, int limit)
        {
            return _context.User.OrderBy(p => p.Id)
                .Select(p => new UserPublicModel
                {
                    Id = p.Id,
                    Username = p.Username,
                    Email = p.Email
                })
                .Skip(offset)
                .Take(limit)
                .ToArray();
        }

        public UserModel AddNewUser(UserModel user)
        {
            _context.User.Add(user);
            _context.SaveChanges();
            return user;
        }

        public UserModel UpdateUser(UserModel updatedUser, UserModel targetUser)
        {
            throw new NotImplementedException();
        }

        public int DeleteUser(UserModel user)
        {
            throw new NotImplementedException();
        }
    }
}