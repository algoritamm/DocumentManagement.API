using DocumentManagement.Rest.Api.Entities;
using DocumentManagement.Rest.Api.Interfaces.Repositories;

namespace DocumentManagement.Rest.Api.Repositories
{
    public class DocumentWorkflowRepository : IDocumentWorkflowRepository
    {
        private readonly DocumentSystemContext _context;
        public DocumentWorkflowRepository(DocumentSystemContext context) => _context = context;

        public void InsertDocumentWorkflow(DocumentWorkflow workflow)
            => _context.DocumentWorkflows.Add(workflow);

    }
}
