using DocumentManagement.Rest.Api.Entities;

namespace DocumentManagement.Rest.Api.Interfaces.Repositories
{
    public interface IServiceDefRepository
    {
        IEnumerable<ServiceDef> GetServiceDefWithBPByTemplateIdAndRoles(short templateId, short[] roleIds);
    }
}
