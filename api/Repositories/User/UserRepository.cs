﻿using System;
using System.Collections.Generic;
using System.Linq;
using api.Contexts;
using api.Enums.User;
using api.Models.User;
using Microsoft.EntityFrameworkCore;

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
            return _context.User.Include(a => a.Organizations).SingleOrDefault(a => a.Id == id);
        }

        public UserModel GetUserByMSId(string id)
        {
            return _context.User.Include(a => a.Organizations).SingleOrDefault(a => a.MicrosoftId == id);
        }

        public UserModel GetUserByUsername(string username)
        {
            return _context.User.SingleOrDefault(a => a.Username == username);
        }

        public UserModel GetUserByEmail(string email)
        {
            return _context.User.SingleOrDefault(a => a.Email == email);
        }

        public UserModel GetUserByPasswordResetId(Guid passwordResetId)
        {
            return _context.User.SingleOrDefault(a => a.PasswordResetId == passwordResetId);
        }

        public (IList<UserModel> users, int totalItemCount) GetUsers(int offset, int limit, UserFiltersDto filters)
        {
            IQueryable<UserModel> query = _context.User.OrderBy(p => p.Username);

            query = query.Where(user => user.Role != UserRole.Admin);
            
            if (!string.IsNullOrEmpty(filters.Username))
            {
                query = query.Where(user => user.Username.ToLower().Equals(filters.Username.ToLower()));
            }

            if (!string.IsNullOrEmpty(filters.Email))
            {
                query = query.Where(user => user.Email.ToLower().Equals(filters.Email.ToLower()));
            }

            if (filters.OrganizationId != Guid.Empty)
            {
                query = query.Where(user =>
                    user.Organizations.Any(organization => organization.Id == filters.OrganizationId));
            }

            return (query.Skip(offset).Take(limit).ToList(), query.Count());
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
            IQueryable<UserModel> query = _context.User.OrderBy(p => p.Username);

            query = query.Where(user => user.Role != UserRole.Admin);
            query = query.Where(user =>
                user.Username.ToLower().Contains(search.ToLower()) || user.Email.ToLower().Contains(search.ToLower()));

            return (query.Skip(offset).Take(limit).ToList(), query.Count());
        }

        public UserModel GetUserByRefreshToken(UserRefreshDto refreshDto)
        {
            return _context.User.SingleOrDefault(a => a.RefreshToken == refreshDto.RefreshToken);
        }
    }
}