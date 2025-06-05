using DocumentManagement.Rest.Api.Entities;

namespace DocumentManagement.Rest.Api.Interfaces.Repositories
{
    public interface IEntityLegalRepository
    {
        EntityLegal GetEntityLegal(string pib);
        void InsertEntityLegal(EntityLegal entityLegal);
        void UpdateEntityLegal(EntityLegal entityLegal);
    }
}
