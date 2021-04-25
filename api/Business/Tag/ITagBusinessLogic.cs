using System.Security.Claims;
using api.Helpers;
using api.Models.Tag;

namespace api.Business.Tag
{
    public interface ITagBusinessLogic
    {
        TagModel GetTagById(string id);
        (int page, int pageSize, int maxPage, TagModel[] tags) GetTags(PagingDto paging);
        TagModel AddNewTag(TagCreationModel newTag);
        TagModel UpdateTagById(string id, TagUpdateModel updatedTag, Claim currentUser);
        void DeleteTagById(string id, Claim currentUser);
    }
}