using DocumentManagement.Rest.Api.Entities;
using DocumentManagement.Rest.Api.Interfaces.Repositories;

namespace DocumentManagement.Rest.Api.Repositories
{
    public class EntityRepository : IEntityRepository
    {
        private readonly DocumentSystemContext _context;
        public EntityRepository(DocumentSystemContext context) => _context = context;

        public Entity GetEntityById(int entityId) =>  _context.Entities.Find(entityId);

        public void InsertEntity(Entity entity) => _context.Entities.Add(entity);

        public void UpdateEntity(Entity entity) => _context.Entities.Update(entity);
    }
}
