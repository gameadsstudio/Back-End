using System;
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
            return _context.Tag.SingleOrDefault(a => a.Name == name);
        }

        public (TagModel[], int) GetTags(int offset, int limit)
        {
            return (_context.Tag.OrderBy(p => p.Id)
                    .Skip(offset)
                    .Take(limit)
                    .ToArray(),
                _context.Tag.Count());
        }

        public (TagModel[], int) SearchTagsByNameOrDescription(int offset, int limit, string name, string description)
        {
            var tags = _context.Tag.OrderBy(p => p.Id)
                .Where(p => p.Name.Contains(name) || p.Description.Contains(description))
                .Skip(offset)
                .Take(limit)
                .ToArray();
            var count = _context.Tag
                .OrderBy(p => p.Id)
                .Count(p => p.Name.Contains(name) || p.Description.Contains(description));
            return (tags, count);
        }

        public (TagModel[], int) SearchTagsByNameAndDescription(int offset, int limit, string name, string description)
        {
            var tags = _context.Tag.OrderBy(p => p.Id)
                .Where(p => p.Name.Contains(name) && p.Description.Contains(description))
                .Skip(offset)
                .Take(limit)
                .ToArray();
            var count = _context.Tag
                .OrderBy(p => p.Id)
                .Count(p => p.Name.Contains(name) && p.Description.Contains(description));
            return (tags, count);
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