using DocumentManagement.Rest.Api.Entities;

namespace DocumentManagement.Rest.Api.Interfaces.Repositories
{
    public interface ITemplateRepository
    {
        Template GetTemplateById(short templateId);
        TemplateRoleApprovalOrder GetApprovalOrderByTemplateId(short templateId, short roleId);
    }
}
