using DocumentManagement.Rest.Api.Entities;
using DocumentManagement.Rest.Api.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DocumentManagement.Rest.Api.Repositories
{
    public class ServiceDefRepository : IServiceDefRepository
    {
        private readonly DocumentSystemContext _context;
        public ServiceDefRepository(DocumentSystemContext context) => _context = context;

        public IEnumerable<ServiceDef> GetServiceDefWithBPByTemplateIdAndRoles(short templateId, short[] roleIds)
            => _context.ServiceDefs.Include(bp => bp.BusinessProcess)
                                   .ThenInclude(bps => bps.BusinessProcessSteps)
                                   .Where(x => x.TemplateId == templateId && roleIds.Contains(x.RoleId));
           
    }
}
