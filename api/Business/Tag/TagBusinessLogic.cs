using System;
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

        public TagModel GetTagById(string id)
        {
            try
            {
                return _repository.GetTagById(Guid.Parse(id)) ??
                       throw new ApiError(HttpStatusCode.NotFound, $"Could not find tag with Id: {id}");
            }
            catch (FormatException e)
            {
                throw new ApiError(HttpStatusCode.BadRequest, e.Message);
            }
        }

        public (int page, int pageSize, int maxPage, TagModel[] tags) GetTags(PagingDto paging, string name,
            string description, bool noPaging)
        {
            if (noPaging)
            {
                return (0, _repository.CountTags(), 0, _repository.GetAllTags());
            }
            paging = PagingHelper.Check(paging);
            var (tags, maxPage) = _repository.SearchTagsByNameOrDescription((paging.Page - 1) * paging.PageSize,
                paging.PageSize, name ?? "", description ?? "");
            return (paging.Page, paging.PageSize, (maxPage / paging.PageSize + 1), tags);
        }

        public TagModel AddNewTag(TagCreationModel newTag, Claim currentUser)
        {
            // Todo: Check if user is admin

            CheckTagExists(newTag.Name);
            var tag = _mapper.Map(newTag, new TagModel());
            return _repository.AddNewTag(tag);
        }

        public TagModel UpdateTagById(string id, TagUpdateModel updatedTag, Claim currentUser)
        {
            // Todo: Check if user is admin

            CheckTagExists(updatedTag.Name);
            var tag = _mapper.Map(updatedTag, GetTagById(id));
            return _repository.UpdateTag(tag);
        }

        public void DeleteTagById(string id, Claim currentUser)
        {
            // Todo: Check if user is admin

            var tag = GetTagById(id);
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