using DocumentManagement.Rest.Api.Entities;
using DocumentManagement.Rest.Api.Interfaces.Repositories;

namespace DocumentManagement.Rest.Api.Repositories
{
    public class EntityLegalRepository : IEntityLegalRepository
    {
        private readonly DocumentSystemContext _context;
        public EntityLegalRepository(DocumentSystemContext context) => _context = context;

        public EntityLegal GetEntityLegal(string pib)
            => _context.EntityLegals.Where(el => el.Pib.Trim().Equals(pib.Trim())).OrderByDescending(el => el.EntityId).FirstOrDefault();    

        public void InsertEntityLegal(EntityLegal entityLegal) => _context.EntityLegals.Add(entityLegal);

        public void UpdateEntityLegal(EntityLegal entityLegal) => _context.EntityLegals.Update(entityLegal);
    }

}
