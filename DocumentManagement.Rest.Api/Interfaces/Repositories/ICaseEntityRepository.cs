using DocumentManagement.Rest.Api.Entities;

namespace DocumentManagement.Rest.Api.Interfaces.Repositories
{
    public interface ICaseEntityRepository
    {
        void InsertCaseEntity(CaseEntity caseEntity);
    }
}
