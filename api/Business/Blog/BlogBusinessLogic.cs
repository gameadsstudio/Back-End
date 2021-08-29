using System;
using System.Collections.Generic;
using System.Net;
using AutoMapper;
using api.Business.Organization;
using api.Contexts;
using api.Errors;
using api.Models.Blog;
using api.Repositories.Blog;
using api.Helpers;

namespace api.Business.Blog
{
    public class BlogBusinessLogic : IBlogBusinessLogic
    {
        private readonly IMapper _mapper;

        private readonly IBlogRepository _repository;

        public BlogBusinessLogic(ApiContext context, IMapper mapper)
        {
            _repository = new BlogRepository(context);
            _mapper = mapper;
        }

        public BlogPublicDto AddNewPost(BlogCreationDto newPost, ConnectedUser currentUser)
        {
            BlogModel post = _mapper.Map(newPost, new BlogModel());

            if (currentUser.Role != Enums.User.UserRole.Admin) {
                throw new ApiError(
                    HttpStatusCode.Forbidden,
                    "Cannot create a post if you aren't an administrator"
                );
            }
            return _mapper.Map(
                _repository.AddNewPost(post),
                new BlogPublicDto()
            );
        }

        public BlogPublicDto UpdatePostById(Guid id, BlogUpdateDto updatedPost, ConnectedUser currentUser)
        {
            BlogModel postMerge = _mapper.Map(
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
                new BlogPublicDto()
            );
        }

        public void DeletePostById(Guid id, ConnectedUser currentUser)
        {
            BlogModel post = _repository.GetPostById(id);

            if (currentUser.Role != Enums.User.UserRole.Admin) {
                throw new ApiError(
                    HttpStatusCode.Forbidden,
                    "Cannot delete a post if you aren't an administrator"
                );
            }
            _repository.DeletePost(post);
        }

        public BlogPublicDto GetPostById(Guid id)
        {
            return _mapper.Map(
                _repository.GetPostById(id),
                new BlogPublicDto()
            );
        }

        public (int page, int pageSize, int maxPage, IList<BlogPublicDto> posts) GetPosts(PagingDto paging, BlogFiltersDto filters)
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
                _mapper.Map(posts, new List<BlogPublicDto>())
            );
        }

        public BlogModel GetBlogModelById(Guid id)
        {
            BlogModel post = _repository.GetPostById(id);

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
