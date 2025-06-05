using DocumentManagement.Rest.Api.Entities;
using DocumentManagement.Rest.Api.Interfaces.Repositories;

namespace DocumentManagement.Rest.Api.Repositories
{
    public class CaseEntityRepository : ICaseEntityRepository
    {
        private readonly DocumentSystemContext _context;
        public CaseEntityRepository(DocumentSystemContext context) => _context = context;

        public void InsertCaseEntity(CaseEntity caseEntity)
            => _context.CaseEntities.Add(caseEntity);
        
    }
}
