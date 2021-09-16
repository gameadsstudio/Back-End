using System;
using System.Collections.Generic;
using System.Net;
using AutoMapper;
using api.Business.Organization;
using api.Contexts;
using api.Errors;
using api.Models.Post;
using api.Repositories.Post;
using api.Helpers;

namespace api.Business.Post
{
    public class PostBusinessLogic : IPostBusinessLogic
    {
        private readonly IMapper _mapper;

        private readonly IPostRepository _repository;

        public PostBusinessLogic(ApiContext context, IMapper mapper)
        {
            _repository = new PostRepository(context);
            _mapper = mapper;
        }

        public PostPublicDto AddNewPost(PostCreationDto newPost, ConnectedUser currentUser)
        {
            PostModel post = _mapper.Map(newPost, new PostModel());

            if (currentUser.Role != Enums.User.UserRole.Admin) {
                throw new ApiError(
                    HttpStatusCode.Forbidden,
                    "Cannot create a post if you aren't an administrator"
                );
            }
            return _mapper.Map(
                _repository.AddNewPost(post),
                new PostPublicDto()
            );
        }

        public PostPublicDto UpdatePostById(Guid id, PostUpdateDto updatedPost, ConnectedUser currentUser)
        {
            PostModel postMerge = _mapper.Map(
                updatedPost,
                _repository.GetPostById(id)
            );

            if (currentUser.Role != Enums.User.UserRole.Admin) {
                throw new ApiError(
                    HttpStatusCode.Forbidden,
                    "Cannot update a post if you aren't an administrator"
                );
            }
            return _mapper.Map(
                _repository.UpdatePost(postMerge),
                new PostPublicDto()
            );
        }

        public void DeletePostById(Guid id, ConnectedUser currentUser)
        {
            PostModel post = _repository.GetPostById(id);

            if (currentUser.Role != Enums.User.UserRole.Admin) {
                throw new ApiError(
                    HttpStatusCode.Forbidden,
                    "Cannot delete a post if you aren't an administrator"
                );
            }
            _repository.DeletePost(post);
        }

        public PostPublicDto GetPostById(Guid id)
        {
            return _mapper.Map(
                _repository.GetPostById(id),
                new PostPublicDto()
            );
        }

        public (int page, int pageSize, int maxPage, IList<PostPublicDto> posts) GetPosts(PagingDto paging, PostFiltersDto filters)
        {
            paging = PagingHelper.Check(paging);
            var (posts, maxPage) = _repository.GetPosts(
                (paging.Page - 1) * paging.PageSize,
                paging.PageSize
            );

            return (
                paging.Page,
                paging.PageSize,
                (maxPage / paging.PageSize + 1),
                _mapper.Map(posts, new List<PostPublicDto>())
            );
        }

        public PostModel GetPostModelById(Guid id)
        {
            PostModel post = _repository.GetPostById(id);

            if (post == null) {
                throw new ApiError(
                    HttpStatusCode.NotFound,
                    $"Couldn't find post with Id: {id}"
                );
            }
            return post;
        }
    }
}
