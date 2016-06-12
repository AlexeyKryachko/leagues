using System.Web;

namespace Interfaces.Core.Services.Medias.BuisnessLogic
{
    public interface IMediaManager
    {
        MediaIOViewModel GetCutImage(string mediaId, int? width, int? height);
        MediaIOViewModel GetImage(string mediaId, int? width, int? height);
        MediaViewModel Upload(HttpPostedFile file);
    }
}
