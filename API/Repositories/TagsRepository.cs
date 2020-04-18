using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P1.Data.Repositories
{
    public class TagsRepository : Repository<Tags>, ITagsRepository
    {
        public TagsRepository(DBModelContainer context) : base(context)
        {

        }

        public ICollection<Tags> GetByName(string name)
        {
            throw new NotImplementedException();
        }
    }
}
