using System.Security.Claims;
using api.Contexts;
using api.Helpers;
using api.Models.AdContainer;
using api.Repositories.AdContainer;
using AutoMapper;

namespace api.Business.AdContainer
{
    public class AdContainerBusinessLogic : IAdContainerBusinessLogic

    {
        private readonly IMapper _mapper;
        private readonly IAdContainerRepository _repository;

        public AdContainerBusinessLogic(ApiContext context, IMapper mapper)
        {
            _repository = new AdContainerRepository(context);
            _mapper = mapper;
        }

        public AdContainerModel GetAdContainerById(string id)
        {
            throw new System.NotImplementedException();
        }

        public (int page, int pageSize, int maxPage, AdContainerModel[] tags) GetAdContainers(PagingDto paging, Claim currentUser)
        {
            throw new System.NotImplementedException();
        }

        public AdContainerModel AddNewAdContainer(AdContainerCreationModel newAdContainer, Claim currentUser)
        {
            throw new System.NotImplementedException();
        }

        public AdContainerModel UpdateAdContainerById(string id, AdContainerUpdateModel updatedAdContainer, Claim currentUser)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteAdContainerById(string id, Claim currentUser)
        {
            throw new System.NotImplementedException();
        }
    }
}