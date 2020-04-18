using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P1.Data.Repositories
{
    public interface IMediaRepository : IRepository<Media>
    {
        Media GetByPath(string path);
        void Update(Media entity);
    }
}
