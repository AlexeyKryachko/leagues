using MongoDB.Bson;

namespace Interfaces.Core.DataAccess
{
    public interface IBaseEntity
    {
        string Id { get; set; }
    }
}
