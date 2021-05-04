using System.Net;
using System.Security.Claims;
using api.Contexts;
using api.Errors;
using api.Helpers;
using api.Models.Advertisement;
using api.Repositories.Advertisement;
using AutoMapper;

namespace api.Business.Advertisements
{
    public class AdvertisementBusinessLogic : IAdvertisementBusinessLogic
    {
        private readonly IMapper _mapper;
        private readonly IAdvertisementRepository _repository;

        public AdvertisementBusinessLogic(ApiContext context, IMapper mapper)
        {
            _repository = new AdvertisementRepository(context);
            _mapper = mapper;
        }

        public AdvertisementModel GetAdvertisementById(string id, Claim currentUser)
        {
            var advertisement = _repository.GetAdvertisementById(GuidHelper.StringToGuidConverter(id));

            if (advertisement == null)
            {
                throw new ApiError(HttpStatusCode.NotFound, $"Couldn't find advertisement with Id: {id}");
            }

            return advertisement;
        }

        public (int page, int pageSize, int maxPage, AdvertisementModel[] advertisements) GetAdvertisements(
            PagingDto paging)
        {
            paging = PagingHelper.Check(paging);
            var maxPage = _repository.CountAdvertisements() / paging.PageSize + 1;
            var advertisements = _repository.GetAdvertisements((paging.Page - 1) * paging.PageSize, paging.PageSize);
            return (paging.Page, paging.PageSize, maxPage, advertisements);
        }

        public AdvertisementModel AddNewAdvertisement(AdvertisementCreationDto newAdvertisement)
        {
            var advertisement = _mapper.Map(newAdvertisement, new AdvertisementModel());

            return _repository.AddNewAdvertisement(advertisement);
        }

        public AdvertisementModel UpdateAdvertisementById(string id, AdvertisementUpdateDto updatedAdvertisement,
            Claim currentUser)
        {
            var advertisement = _repository.GetAdvertisementById(GuidHelper.StringToGuidConverter(id));

            if (advertisement == null)
            {
                throw new ApiError(HttpStatusCode.NotFound, $"Couldn't find advertisement with Id: {id}");
            }

            advertisement = _mapper.Map(updatedAdvertisement, advertisement);

            return _repository.UpdateAdvertisement(advertisement);
        }

        public int DeleteAdvertisementById(string id, Claim currentUser)
        {
            var advertisement = _repository.GetAdvertisementById(GuidHelper.StringToGuidConverter(id));

            if (advertisement == null)
            {
                throw new ApiError(HttpStatusCode.NotFound, $"Couldn't find user with Id: {id}");
            }

            return _repository.DeleteAdvertisement(advertisement);
        }
    }
}