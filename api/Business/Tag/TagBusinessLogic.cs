using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

        public (int page, int pageSize, int totalItemCount, List<TagPublicDto> tags) GetTags(PagingDto paging, TagFiltersDto filters)
        {
            paging = PagingHelper.Check(paging);
            var (tags, totalItemCount) = _repository.SearchTags((paging.Page - 1) * paging.PageSize,
                paging.PageSize, filters);
            return (paging.Page, paging.PageSize, totalItemCount, _mapper.Map(tags, new List<TagPublicDto>()));
        }

        public TagPublicDto AddNewTag(TagCreationDto newTag, ConnectedUser currentUser)
        {
            CheckTagExists(newTag.Name);
            var tag = _mapper.Map(newTag, new TagModel());
            return _mapper.Map(_repository.AddNewTag(tag), new TagPublicDto());
        }

        public TagPublicDto UpdateTagById(string id, TagUpdateDto updatedTag, ConnectedUser currentUser)
        {
            CheckTagExists(updatedTag.Name);
            var tag = _mapper.Map(updatedTag, GetTagModelById(id));
            return _mapper.Map(_repository.UpdateTag(tag), new TagPublicDto());
        }

        public void DeleteTagById(string id, ConnectedUser currentUser)
        {
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
        
        public IList<TagModel> ResolveTags(IEnumerable<string> tagNames)
        {

            if (tagNames == null || tagNames.Count() == 0)
            {
                return new List<TagModel>();
            }
            
            return (from tagName in tagNames
                where !string.IsNullOrEmpty(tagName)
                select this.GetTagModelByName(tagName)).ToList();
        }
    }
}