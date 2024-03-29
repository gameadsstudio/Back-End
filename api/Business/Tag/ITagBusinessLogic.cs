using System.Collections.Generic;
using api.Helpers;
using api.Models.Tag;

namespace api.Business.Tag
{
    public interface ITagBusinessLogic
    {
        TagPublicDto GetTagById(string id);
        (int page, int pageSize, int totalItemCount, List<TagPublicDto> tags) GetTags(PagingDto paging, TagFiltersDto filters);
        TagPublicDto AddNewTag(TagCreationDto newTag, ConnectedUser currentUser);
        TagPublicDto UpdateTagById(string id, TagUpdateDto updatedTag, ConnectedUser currentUser);
        TagModel GetTagModelByName(string name);
        void DeleteTagById(string id, ConnectedUser currentUser);
        public IList<TagModel> ResolveTags(IEnumerable<string> tagNames);
    }
}