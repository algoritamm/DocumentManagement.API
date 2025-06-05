using DocumentManagement.Rest.Api.Dto;

namespace DocumentManagement.Rest.Api.Interfaces.Services
{
    public interface IDocumentService
    {
        (bool, string) DocumentImport(DocumentManagementDto DocumentManagementDto);
        (bool, string) ValidateDocument(DocumentManagementDto dataDto);
    }
}
