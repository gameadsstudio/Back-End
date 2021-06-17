using System;
using System.Collections.Generic;
using System.Linq;
using api.Contexts;
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

        /*
        ** ===============================
        **             MEDIA
        ** ===============================
        */
        
        public MediaModel AddNewMedia(MediaModel media)
        {
            _context.Media.Add(media);
            _context.SaveChanges();
            return media;
        }

        public MediaModel GetMediaById(Guid id)
        {
            return _context.Media
                .Include(a => a.Tags)
                .Include(a => a.Organization)
                .SingleOrDefault(a => a.Id == id);
        }

        public int CountMedias()
        {
            return _context.Media.Count();
        }
        
        public (IList<MediaModel>, int) GetMediasByOrganizationId(int offset, int limit,
            Guid orgId, Guid userId)
        {
            var query = _context.Media.OrderByDescending(p => p.DateCreation)
                .Include(a => a.Tags)
                .Include(a => a.Organization)
                .ThenInclude(o => o.Users)
                .Where(p => p.Organization.Id == orgId && p.Organization.Users.Any(u => u.Id == userId));

            return (query.Skip(offset)
                        .Take(limit)
                        .ToList(),
                    query.Count());
        }

        public (IList<MediaModel>, int) GetMediasByTags(int offset, int limit, Guid orgId, Guid userId, IList<TagModel> tags)
        {
            var query = _context.Media.OrderByDescending(p => p.DateCreation)
                .Include(a => a.Tags)
                .Include(a => a.Organization)
                .ThenInclude(o => o.Users)
                .Where(p => p.Organization.Id == orgId && p.Organization.Users.Any(u => u.Id == userId));

            return (query.Skip(offset)
                        .Take(limit)
                        .ToList(),
                    query.Count());
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
        
        /*
        ** ===============================
        **            2D MEDIA
        ** ===============================
        */

        public Media2DModel AddNew2DMedia(Media2DModel media)
        {
            _context.Media2D.Add(media);
            _context.SaveChanges();
            return media;
        }

        public Media2DModel Get2DMediaById(Guid id)
        {
            return _context.Media2D
                .SingleOrDefault(a => a.Id == id);
        }

        public Media2DModel Get2DMediaByMediaId(Guid id)
        {
            return _context.Media2D
                .Include(a => a.Media)
                .SingleOrDefault(a => a.Media.Id == id);
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
        
        /*
        ** ===============================
        **            3D MEDIA
        ** ===============================
        */

        public Media3DModel AddNew3DMedia(Media3DModel media)
        {
            _context.Media3D.Add(media);
            _context.SaveChanges();
            return media;
        }

        public Media3DModel Get3DMediaById(Guid id)
        {
            return _context.Media3D
                .SingleOrDefault(a => a.Id == id);
        }

        public Media3DModel Get3DMediaByMediaId(Guid id)
        {
            return _context.Media3D
                .Include(a => a.Media)
                .SingleOrDefault(a => a.Media.Id == id);
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
        
        /*
        ** ===============================
        **           UNITY MEDIA
        ** ===============================
        */

        public MediaUnityModel AddNewUnityMedia(MediaUnityModel media)
        {
            _context.MediaUnity.Add(media);
            _context.SaveChanges();
            return media;
        }

        public MediaUnityModel GetUnityMediaById(Guid id)
        {
            return _context.MediaUnity
                .SingleOrDefault(a => a.Id == id);
        }

        public MediaUnityModel GetUnityMediaByMediaId(Guid id)
        {
            return _context.MediaUnity
                .Include(a => a.Media)
                .SingleOrDefault(a => a.Media.Id == id);
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
    }
}