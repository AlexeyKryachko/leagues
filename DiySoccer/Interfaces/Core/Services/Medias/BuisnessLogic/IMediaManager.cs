using System.Web;

namespace Interfaces.Core.Services.Medias.BuisnessLogic
{
    public interface IMediaManager
    {
        MediaIOViewModel Get(string mediaId, int width, int height);

        MediaViewModel Upload(HttpPostedFile file);
    }
}
