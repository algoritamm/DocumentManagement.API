using DocumentManagement.Rest.Api.Entities;
using DocumentManagement.Rest.Api.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DocumentManagement.Rest.Api.Repositories
{
    public class CaseRepository : ICaseRepository
    {
        private DocumentSystemContext _context;

        public CaseRepository(DocumentSystemContext context) => _context = context;

        public Case GetCaseByDocumentId(int documentId)
            => _context.Cases.Where(c => c.Documents.Any(d => d.DocumentId == documentId)).SingleOrDefault();
        

        public Case GetCaseByRefNo(string refNo) 
            => _context.Cases.FirstOrDefault(x => x.CaseRefNo.Equals(refNo));

        public void InsertCase(Case _case) 
            => _context.Cases.Add(_case);


    }
}
