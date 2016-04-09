using System.IO;

namespace Interfaces.Core.Services.Medias.BuisnessLogic
{
    public class MediaIOViewModel
    {
        public Stream Stream { get; set; }

        public string ContentType { get; set; }
    }
}
