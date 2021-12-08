using System;
using System.Collections.Generic;
using System.Linq;
using api.Contexts;
using api.Enums.Media;
using api.Models.Media;
using api.Models.Media._2D;
using api.Models.Media._3D;
using api.Models.Media.Engine.Unity;
using api.Models.Tag;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories.Media
{
    public class MediaRepository : IMediaRepository
    {
        private readonly ApiContext _context;

        public MediaRepository(ApiContext context)
        {
            _context = context;
        }

        #region Media

        public MediaModel AddNewMedia(MediaModel media)
        {
            _context.Media.Add(media);
            _context.SaveChanges();
            return media;
        }

        public MediaModel GetMediaById(Guid id)
        {
            return _context.Media.Include(a => a.Tags)
                .Include(a => a.Organization)
                .ThenInclude(o => o.Users)
                .SingleOrDefault(a => a.Id == id);
        }

        public int CountMedias()
        {
            return _context.Media.Count();
        }

        public (IList<MediaModel>, int) GetMediasByOrganizationId(int offset, int limit, Guid orgId, Guid userId)
        {
            var query = _context.Media.OrderByDescending(p => p.DateCreation)
                .Include(a => a.Tags)
                .Include(a => a.Organization)
                .ThenInclude(o => o.Users)
                .Where(p => p.Organization.Id == orgId && p.Organization.Users.Any(u => u.Id == userId));

            return (query.Skip(offset).Take(limit).ToList(), query.Count());
        }

        public (IList<MediaModel>, int) GetMediasByTags(int offset, int limit, Guid orgId, Guid userId,
            IList<TagModel> tags)
        {
            var query = _context.Media.OrderByDescending(p => p.DateCreation)
                .Include(a => a.Tags)
                .Include(a => a.Organization)
                .ThenInclude(o => o.Users)
                .Where(p => p.Organization.Id == orgId && p.Organization.Users.Any(u => u.Id == userId))
                .Where(m => m.Tags.Any(tm => tags.Any(t => tm.Name == t.Name)));

            return (query.Skip(offset).Take(limit).ToList(), query.Count());
        }

        public MediaModel UpdateMedia(MediaModel updatedMedia)
        {
            _context.Media.Update(updatedMedia);
            _context.SaveChanges();
            return updatedMedia;
        }

        public int DeleteMedia(MediaModel media)
        {
            _context.Media.Remove(media);
            return _context.SaveChanges();
        }

        #endregion

        #region 2DMedia

        public Media2DModel AddNew2DMedia(Media2DModel media)
        {
            _context.Media2D.Add(media);
            _context.SaveChanges();
            return media;
        }

        public Media2DModel Get2DMediaById(Guid id)
        {
            return _context.Media2D.SingleOrDefault(a => a.Id == id);
        }

        public Media2DModel Get2DMediaByMediaId(Guid id)
        {
            return _context.Media2D.Include(a => a.Media).SingleOrDefault(a => a.Media.Id == id);
        }

        public Media2DModel Update2DMedia(Media2DModel updatedMedia)
        {
            _context.Media2D.Update(updatedMedia);
            _context.SaveChanges();
            return updatedMedia;
        }

        public int DeleteMedia(Media2DModel media)
        {
            _context.Media2D.Remove(media);
            return _context.SaveChanges();
        }

        public IList<Media2DModel> Get2DMediasByAspectRatio(AspectRatio aspectRatio)
        {
            return _context.Media2D
                .Include(m => m.Media)
                .Where(m => m.AspectRatio == aspectRatio)
                .ToList();
        }

        #endregion

        #region 3DMedia

        public Media3DModel AddNew3DMedia(Media3DModel media)
        {
            _context.Media3D.Add(media);
            _context.SaveChanges();
            return media;
        }

        public Media3DModel Get3DMediaById(Guid id)
        {
            return _context.Media3D.SingleOrDefault(a => a.Id == id);
        }

        public Media3DModel Get3DMediaByMediaId(Guid id)
        {
            return _context.Media3D.Include(a => a.Media).SingleOrDefault(a => a.Media.Id == id);
        }

        public Media3DModel Update3DMedia(Media3DModel updatedMedia)
        {
            _context.Media3D.Update(updatedMedia);
            _context.SaveChanges();
            return updatedMedia;
        }

        public int DeleteMedia(Media3DModel media)
        {
            _context.Media3D.Remove(media);
            return _context.SaveChanges();
        }

        public IList<Media3DModel> Get3DMediasBySize(int width, int height, int depth)
        {
            return _context.Media3D.Include(m => m.Media)
                .Where(m =>
                (m.Height > height * 0.8 && m.Height < height * 1.2) &&
                (m.Width > width * 0.8 && m.Width < width * 1.2) &&
                (m.Depth > depth * 0.8 && m.Depth < depth * 1.2))
                .ToList();
        }

        #endregion

        #region UnityMedia

        public MediaUnityModel AddNewUnityMedia(MediaUnityModel media)
        {
            _context.MediaUnity.Add(media);
            _context.SaveChanges();
            return media;
        }

        public MediaUnityModel GetUnityMediaById(Guid id)
        {
            return _context.MediaUnity.SingleOrDefault(a => a.Id == id);
        }

        public MediaUnityModel GetUnityMediaByMediaId(Guid id)
        {
            return _context.MediaUnity.Include(a => a.Media).SingleOrDefault(a => a.Media.Id == id);
        }

        public MediaUnityModel UpdateUnityMedia(MediaUnityModel updatedMedia)
        {
            _context.MediaUnity.Update(updatedMedia);
            _context.SaveChanges();
            return updatedMedia;
        }

        public int DeleteMedia(MediaUnityModel media)
        {
            _context.MediaUnity.Remove(media);
            return _context.SaveChanges();
        }

        #endregion

        #region media-query

        public MediaUnityModel GetUnityMediaByFilters(MediaQueryFilters filters)
        {
            IQueryable<MediaUnityModel> query = _context.MediaUnity;

            query = query.Include(m => m.Media).ThenInclude(m => m.Tags);
            query = query.Include(m => m.Media.Advertisements);
            query = query.Where(m => m.Media.State == MediaStateEnum.Processed);
            query = query.Where(m => filters.MediaIds.Any(id => id == m.Media.Id));
            query = query.Where(m => m.Media.Advertisements.Any());

            var mediasUnfiltered = query.ToList();
            var medias = mediasUnfiltered.Where(m => m.Media.Tags.Any(x => filters.Tags.Any(x.Equals)))
                .ToList();

            if (!medias.Any()) return null;
            
            var rand = new Random();
            return medias.ElementAt(rand.Next(medias.Count));
        }

        #endregion
    }
}