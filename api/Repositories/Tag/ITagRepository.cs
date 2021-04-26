using System;
using api.Models.Tag;

namespace api.Repositories.Tag
{
    public interface ITagRepository
    {
        TagModel AddNewTag(TagModel tag);
        TagModel GetTagById(Guid id);
        TagModel GetTagByName(string name);
        int CountTags();
        TagModel[] GetAllTags();
        (TagModel[], int) GetTags(int offset, int limit);
        public (TagModel[], int) SearchTagsByNameAndDescription(int offset, int limit, string name, string description);
        public (TagModel[], int) SearchTagsByNameOrDescription(int offset, int limit, string name, string description);
        TagModel UpdateTag(TagModel updatedTag);
        int DeleteTag(TagModel tag);
    }
}