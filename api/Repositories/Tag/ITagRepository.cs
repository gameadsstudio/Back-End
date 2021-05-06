using System;
using System.Collections.Generic;
using api.Models.Tag;

namespace api.Repositories.Tag
{
    public interface ITagRepository
    {
        TagModel AddNewTag(TagModel tag);
        TagModel GetTagById(Guid id);
        TagModel GetTagByName(string name);
        int CountTags();
        List<TagModel> GetAllTags();
        (List<TagModel>, int) GetTags(int offset, int limit);
        public (List<TagModel>, int) SearchTagsByNameDescription(int offset, int limit, TagFiltersDto filters, bool strict = false);
        TagModel UpdateTag(TagModel updatedTag);
        int DeleteTag(TagModel tag);
    }
}