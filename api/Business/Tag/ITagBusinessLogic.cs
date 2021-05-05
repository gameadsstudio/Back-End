using System.Security.Claims;
using api.Helpers;
using api.Models.Tag;

namespace api.Business.Tag
{
    public interface ITagBusinessLogic
    {
        TagPublicDto GetTagById(string id);
        (int page, int pageSize, int maxPage, TagPublicDto[] tags) GetTags(PagingDto paging, string name,
            string description, bool noPaging);
        TagPublicDto AddNewTag(TagCreationDto newTag, Claim currentUser);
        TagPublicDto UpdateTagById(string id, TagUpdateDto updatedTag, Claim currentUser);
        TagModel GetTagModelByName(string name);
        void DeleteTagById(string id, Claim currentUser);
    }
}