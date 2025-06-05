using DocumentManagement.Rest.Api.Entities;
using DocumentManagement.Rest.Api.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DocumentManagement.Rest.Api.Repositories
{
    public class CaseWorkflowRepository : ICaseWorkflowRepository
    {
        private DocumentSystemContext _context;

        public CaseWorkflowRepository(DocumentSystemContext context) => _context = context; 

        public void InsertCaseWorkflow(CaseWorkflow caseWorkflow)
            => _context.CaseWorkflows.Add(caseWorkflow);
    }
}
