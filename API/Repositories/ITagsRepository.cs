using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P1.Data.Repositories
{
    public interface ITagsRepository : IRepository<Tags>
    {
        ICollection<Tags> GetByName(string name);
    }
}
