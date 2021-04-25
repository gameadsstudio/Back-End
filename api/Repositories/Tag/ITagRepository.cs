using System;
using api.Models.Tag;

namespace api.Repositories.Tag
{
    public interface ITagRepository
    {
        TagModel AddNewTag(TagModel tag);
        TagModel GetTagById(Guid id);
        TagModel GetTagByName(string name);
        TagModel[] GetTags(int offset, int limit);
        TagModel UpdateTag(TagModel updatedTag);
        int DeleteTag(TagModel tag);
    }
}