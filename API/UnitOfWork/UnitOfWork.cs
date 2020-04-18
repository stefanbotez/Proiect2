using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using P1.Data.Repositories;

namespace P1.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DBModelContainer _context;

        public UnitOfWork()
        {
            _context = new DBModelContainer();
            Media = new MediaRepository(_context);
            Tags = new TagsRepository(_context);
        }

        public IMediaRepository Media { get; private set; }

        public ITagsRepository Tags { get; private set; }

        public Media RemoveTagFromMedia(Media media, int tagId)
        {
            if (media == null)
                return media;
            Tags tag = Tags.Get(tagId);
            media.Tags.Remove(tag);
            Tags.Delete(tag.Id);
            Complete();
            return media;
        }

        public Media AddTagToMedia(Media media, Tags tag)
        {
            if (media == null)
                return media;
            media.Tags.Add(tag);
            Complete();
            return media;
        }

        public int Complete()
        {
            try
            {
                return _context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
