using System.Collections.Generic;

namespace Interfaces.Core.Services.Medias.DataAccess
{
    public interface IMediaRepository
    {
        MediaDb Get(string mediaId);

        IEnumerable<MediaDb> GetRange(IEnumerable<string> mediaIds);

        MediaDb Add(MediaDb entity);
    }
}
