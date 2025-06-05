using DocumentManagement.Rest.Api.Entities;

namespace DocumentManagement.Rest.Api.Interfaces.Repositories
{
    public interface ICaseWorkflowRepository
    {
        void InsertCaseWorkflow(CaseWorkflow caseWorkflow);
    }
}
