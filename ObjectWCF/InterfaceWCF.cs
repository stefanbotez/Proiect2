using P1.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ObjectWCF
{
    [ServiceContract]
    public interface InterfaceMedia
    {
        [OperationContract]
        Media GetMedia(int id);
        [OperationContract]
        IEnumerable<Media> GetAllMedia();
        [OperationContract]
        List<Media> QueryMedia(string toSearch);
        [OperationContract]
        List<Media> QueryMediaDate(string toSearch, DateTime from, DateTime to);
        [OperationContract]
        Media AddMedia(Media entity);
        [OperationContract]
        void DeleteMedia(int id);
        [OperationContract]
        Media GetMediaByPath(string path);
        [OperationContract]
        void UpdateMedia(Media entity);
        [OperationContract]
        Media AddTagToMedia(Media media, Tags tag);
        [OperationContract]
        Media RemoveTagFromMedia(string path, int tagId);
    }
    [ServiceContract]
    public interface InterfaceTags
    {
        [OperationContract]
        Tags GetTag(int id);
        [OperationContract]
        List<int> QueryTags(string toSearch);
        [OperationContract]

        IEnumerable<Tags> GetAllTags();
        [OperationContract]
        Tags AddTag(Tags entity);
        [OperationContract]
        void DeleteTag(int id);
        [OperationContract]
        ICollection<Tags> GetTagByName(string name);
    }
    [ServiceContract]
    public interface IMediaTags : InterfaceMedia, InterfaceTags
    {
        [OperationContract]
        void Complete();
    }
}
