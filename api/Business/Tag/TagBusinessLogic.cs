using System.Linq;
using System.Net;
using System.Security.Claims;
using api.Contexts;
using api.Errors;
using api.Helpers;
using api.Models.Tag;
using api.Repositories.Tag;
using AutoMapper;

namespace api.Business.Tag
{
    public class TagBusinessLogic : ITagBusinessLogic
    {
        private readonly IMapper _mapper;
        private readonly ITagRepository _repository;

        public TagBusinessLogic(ApiContext context, IMapper mapper)
        {
            _repository = new TagRepository(context);
            _mapper = mapper;
        }

        public TagPublicDto GetTagById(string id)
        {
            return _mapper.Map(_repository.GetTagById(GuidHelper.StringToGuidConverter(id)) ??
                throw new ApiError(HttpStatusCode.NotFound, $"Tag with id {id} not found"),
                new TagPublicDto());
        }

        public TagModel GetTagModelByName(string name)
        {
            return _repository.GetTagByName(name) ??
                   throw new ApiError(HttpStatusCode.NotFound, $"Tag with name {name} not found");
        }

        private TagModel GetTagModelById(string id)
        {
            return _repository.GetTagById(GuidHelper.StringToGuidConverter(id)) ??
                   throw new ApiError(HttpStatusCode.NotFound, $"Tag with id {id} not found");
        }

        public (int page, int pageSize, int maxPage, TagPublicDto[] tags) GetTags(PagingDto paging, string name,
            string description, bool noPaging)
        {
            if (noPaging)
            {
                return (0, _repository.CountTags(), 0, _repository.GetAllTags().Select(tag => _mapper.Map(tag, new TagPublicDto())).ToArray());
            }
            paging = PagingHelper.Check(paging);
            var (tags, maxPage) = _repository.SearchTagsByNameOrDescription((paging.Page - 1) * paging.PageSize,
                paging.PageSize, name ?? "", description ?? "");
            return (paging.Page, paging.PageSize, (maxPage / paging.PageSize + 1), tags.Select(tag => _mapper.Map(tag, new TagPublicDto())).ToArray());
        }

        public TagPublicDto AddNewTag(TagCreationDto newTag, Claim currentUser)
        {
            // Todo: Check if user is admin

            CheckTagExists(newTag.Name);
            var tag = _mapper.Map(newTag, new TagModel());
            return _mapper.Map(_repository.AddNewTag(tag), new TagPublicDto());
        }

        public TagPublicDto UpdateTagById(string id, TagUpdateDto updatedTag, Claim currentUser)
        {
            // Todo: Check if user is admin

            CheckTagExists(updatedTag.Name);
            var tag = _mapper.Map(updatedTag, GetTagModelById(id));
            return _mapper.Map(_repository.UpdateTag(tag), new TagPublicDto());
        }

        public void DeleteTagById(string id, Claim currentUser)
        {
            // Todo: Check if user is admin

            var tag = GetTagModelById(id);
            _repository.DeleteTag(tag);
        }

        private void CheckTagExists(string name)
        {
            if (!string.IsNullOrEmpty(name) && _repository.GetTagByName(name) != null)
            {
                throw new ApiError(HttpStatusCode.Conflict,
                    $"Tag with name: {name} already exists");
            }
        }
    }
}