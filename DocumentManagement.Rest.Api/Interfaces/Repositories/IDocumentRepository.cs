using DocumentManagement.Rest.Api.Entities;

namespace DocumentManagement.Rest.Api.Interfaces.Repositories
{
    public interface IDocumentRepository
    {
        Document GetDocumentByRefNo(string refNo);
        void InsertDocument(Document document);
        void UpdateDocument(Document document);
        void InsertDocumentVersion(DocumentVersion documentVersion);
    }
}
