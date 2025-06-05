using DocumentManagement.Rest.Api.Entities;

namespace DocumentManagement.Rest.Api.Interfaces.Repositories
{
    public interface IDocumentParticipantProcessRepository
    {
        void InsertDocumentParticipantsProcess(IEnumerable<DocumentParticipantProcess> participants);
    }
}
