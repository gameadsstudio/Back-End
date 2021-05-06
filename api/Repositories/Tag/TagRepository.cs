using System;
using System.Collections.Generic;
using System.Linq;
using api.Contexts;
using api.Models.Tag;

namespace api.Repositories.Tag
{
    public class TagRepository : ITagRepository
    {
        private readonly ApiContext _context;

        public TagRepository(ApiContext context)
        {
            _context = context;
        }

        public TagModel AddNewTag(TagModel tag)
        {
            _context.Tag.Add(tag);
            _context.SaveChanges();
            return tag;
        }

        public TagModel GetTagById(Guid id)
        {
            return _context.Tag.SingleOrDefault(a => a.Id == id);
        }

        public TagModel GetTagByName(string name)
        {
            return _context.Tag.SingleOrDefault(a => a.Name.ToLower() == name.ToLower());
        }

        public int CountTags()
        {
            return _context.Tag.Count();
        }

        public List<TagModel> GetAllTags()
        {
            return _context.Tag.OrderBy(p => p.Id).ToList();
        }

        public (List<TagModel>, int) GetTags(int offset, int limit)
        {
            return (_context.Tag.OrderBy(p => p.Id)
                    .Skip(offset)
                    .Take(limit)
                    .ToList(),
                _context.Tag.Count());
        }

        public (List<TagModel>, int) SearchTagsByNameDescription(int offset, int limit, TagFiltersDto filters,
            bool strict = false)
        {
            var tags = _context.Tag.OrderBy(p => p.Id);
            IQueryable<TagModel> countQuery = _context.Tag
                .OrderBy(p => p.Id);

            if (strict)
            {
                return (tags.Where(p => p.Name.ToLower().Contains(filters.Name.ToLower()) &&
                                        p.Description.ToLower().Contains(filters.Description.ToLower()))
                        .Skip(offset)
                        .Take(limit).ToList(),
                    countQuery
                        .Count(p => p.Name.ToLower().Contains(filters.Name.ToLower()) &&
                                    p.Description.ToLower().Contains(filters.Description.ToLower())));
            }

            return (tags.Where(p => p.Name.ToLower().Contains(filters.Name.ToLower()) ||
                                    p.Description.ToLower().Contains(filters.Description.ToLower()))
                    .Skip(offset)
                    .Take(limit).ToList(),
                countQuery
                    .Count(p => p.Name.ToLower().Contains(filters.Name.ToLower()) ||
                                p.Description.ToLower().Contains(filters.Description.ToLower())));
        }

        public TagModel UpdateTag(TagModel updatedTag)
        {
            _context.Update(updatedTag);
            _context.SaveChanges();
            return updatedTag;
        }

        public int DeleteTag(TagModel tag)
        {
            _context.Tag.Remove(tag);
            return _context.SaveChanges();
        }
    }
}