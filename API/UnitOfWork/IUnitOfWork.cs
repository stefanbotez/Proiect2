using P1.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P1.Data.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IMediaRepository Media { get; }
        ITagsRepository Tags { get; }
        Media AddTagToMedia(Media media, Tags tag);
        Media RemoveTagFromMedia(Media media, int tagId);
        int Complete();
    }
}
