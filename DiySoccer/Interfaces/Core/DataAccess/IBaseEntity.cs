namespace Interfaces.Core.DataAccess
{
    public interface IBaseEntity
    {
        string EntityId { get; set; }

        string LeagueId { get; set; }
    }
}
