using DocumentManagement.Rest.Api.Entities;
using DocumentManagement.Rest.Api.Interfaces.Repositories;

namespace DocumentManagement.Rest.Api.Repositories
{
    public class DocumentParticipantProcessRepository : IDocumentParticipantProcessRepository
    {
        private readonly DocumentSystemContext _context;
        public DocumentParticipantProcessRepository(DocumentSystemContext context) => _context = context;

        public void InsertDocumentParticipantsProcess(IEnumerable<DocumentParticipantProcess> participants)
            => _context.DocumentParticipantProcesses.AddRange(participants);
    }
}
