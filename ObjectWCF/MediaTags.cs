using P1.Data;
using P1.Data.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ObjectWCF
{
    public class MediaTags : IMediaTags
    {
        IUnitOfWork u;
        public MediaTags()
        {
            u = new UnitOfWork();
        }
        public Media AddMedia(Media entity)
        {
            var result = u.Media.Add(entity);
            return result;
        }

        public Tags AddTag(Tags entity)
        {
            var result = u.Tags.Add(entity);
            return result;
        }

        public Media AddTagToMedia(Media media, Tags tag)
        {
            media = u.Media.GetByPath(media.Path);
            var result = u.AddTagToMedia(media, tag);
            return result;
        }

        public void DeleteMedia(int id)
        {
            u.Media.Delete(id);
        }

        public void DeleteTag(int id)
        {
            u.Tags.Delete(id);
        }

        public IEnumerable<Media> GetAllMedia()
        {
            return u.Media.GetAll();
        }

        public IEnumerable<Tags> GetAllTags()
        {
            return u.Tags.GetAll();
        }

        public ICollection<Tags> GetTagByName(string name)
        {
            return u.Tags.GetByName(name);
        }

        public Media GetMediaByPath(string path)
        {
            return u.Media.GetByPath(path);
        }

        public Media GetMedia(int id)
        {
            return u.Media.Get(id);
        }

        public Tags GetTag(int id)
        {
            return u.Tags.Get(id);
        }

        public Media RemoveTagFromMedia(string path, int tagId)
        {
            Media media = u.Media.GetByPath(path);
            var result = u.RemoveTagFromMedia(media, tagId);
            return result;
        }

        public void UpdateMedia(Media entity)
        {
            u.Media.Update(entity);
        }

        public List<Media> QueryMediaDate(string toSearch, DateTime from, DateTime to)
        {
            return u.Media.Query(m => m.Description.ToLower().Contains(toSearch)
                    && m.Creation_Date >= from
                    && m.Creation_Date <= to)
                    .ToList();
        }

        public List<Media> QueryMedia(string toSearch)
        {
            return u.Media.Query(m => m.Description.ToLower().Contains(toSearch)).ToList();
        }

        public List<int> QueryTags(string toSearch)
        {
            return u.Tags.Query(t => t.Name.ToLower().Contains(toSearch)).Select(t => t.MediaId).ToList();
        }

        public void Complete()
        {
            u.Complete();
        }
    }
}
