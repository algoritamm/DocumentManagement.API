using DocumentManagement.Rest.Api.Entities;

namespace DocumentManagement.Rest.Api.Interfaces.Repositories
{
    public interface ICaseRepository
    {
        Case GetCaseByRefNo(string refNo);
        void InsertCase(Case _case);
        Case GetCaseByDocumentId(int documentId);
    }
}
