using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P1.Data.Repositories
{
    public class MediaRepository : Repository<Media>, IMediaRepository
    {
        public MediaRepository(DBModelContainer context) : base(context)
        {
        }

        public Media GetByPath(string path)
        {
            Media result = _context.Media
                .Where(m => m.Path == path)
                .Include("Tags")
                .FirstOrDefault<Media>();
            return result;

        }

        public void Update(Media entity)
        {
            var item = _context.Media.Find(entity.Id);
            if (item == null)
            {
                return;
            }

            _context.Entry(item).CurrentValues.SetValues(entity);
            _context.SaveChanges();
        }

    }
}
