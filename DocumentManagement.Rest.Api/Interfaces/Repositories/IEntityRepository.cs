using DocumentManagement.Rest.Api.Entities;

namespace DocumentManagement.Rest.Api.Interfaces.Repositories
{
    public interface IEntityRepository
    {
        Entity GetEntityById(int entityId);
        void InsertEntity(Entity entity);
        void UpdateEntity(Entity entity);
    }
}
