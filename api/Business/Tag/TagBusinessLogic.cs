using System.Security.Claims;
using api.Configuration;
using api.Contexts;
using api.Helpers;
using api.Models.Tag;
using api.Repositories.Tag;
using AutoMapper;
using Microsoft.Extensions.Options;

namespace api.Business.Tag
{
    public class TagBusinessLogic : ITagBusinessLogic
    {
        private readonly AppSettings _appSettings;
        private readonly IMapper _mapper;
        private readonly ITagRepository _repository;

        public TagBusinessLogic(ApiContext context, IOptions<AppSettings> appSettings, IMapper mapper)
        {
            _repository = new TagRepository(context);
            _appSettings = appSettings.Value;
            _mapper = mapper;
        }

        public TagModel GetTagById(string id)
        {
            throw new System.NotImplementedException();
        }

        public (int page, int pageSize, int maxPage, TagModel[] tags) GetTags(PagingDto paging, string name, string description)
        {
            throw new System.NotImplementedException();
        }

        public TagModel AddNewTag(TagCreationModel newTag, Claim currentUser)
        {
            throw new System.NotImplementedException();
        }

        public TagModel UpdateTagById(string id, TagUpdateModel updatedTag, Claim currentUser)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteTagById(string id, Claim currentUser)
        {
            throw new System.NotImplementedException();
        }
    }
}