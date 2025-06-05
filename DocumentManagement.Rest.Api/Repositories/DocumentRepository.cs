using DocumentManagement.Rest.Api.Entities;
using DocumentManagement.Rest.Api.Interfaces.Repositories;

namespace DocumentManagement.Rest.Api.Repositories
{
    public class DocumentRepository : IDocumentRepository
    {
        private readonly DocumentSystemContext _context;
        public DocumentRepository(DocumentSystemContext context) => _context = context;

        public Document GetDocumentByRefNo(string refNo)
            => _context.Documents.FirstOrDefault(x => x.DocumentRefNumber.Equals(refNo));

        public void InsertDocument(Document document) => _context.Documents.Add(document);

        public void InsertDocumentVersion(DocumentVersion documentVersion)
        {
            _context.DocumentVersions.Add(documentVersion);
        }

        public void UpdateDocument(Document document)
        {
            _context.Update(document);
        }

    }
}
