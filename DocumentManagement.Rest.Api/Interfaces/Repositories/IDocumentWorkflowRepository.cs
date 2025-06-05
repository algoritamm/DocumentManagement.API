using DocumentManagement.Rest.Api.Entities;

namespace DocumentManagement.Rest.Api.Interfaces.Repositories
{
    public interface IDocumentWorkflowRepository
    {
        void InsertDocumentWorkflow(DocumentWorkflow workflow);
    }
}
