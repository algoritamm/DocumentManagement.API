using DocumentManagement.Rest.Api.Entities;
using DocumentManagement.Rest.Api.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DocumentManagement.Rest.Api.Repositories
{
    public class TemplateRepository : ITemplateRepository
    {
        private readonly DocumentSystemContext _context;
        public TemplateRepository(DocumentSystemContext context) => _context = context;

        public TemplateRoleApprovalOrder GetApprovalOrderByTemplateId(short templateId, short roleId)
        {
            return _context.TemplateRoleApprovalOrders
                .Where(t => t.TemplateId == templateId && t.RoleId == roleId)
                .SingleOrDefault();
        }

        public Template GetTemplateById(short templateId)
        {
            return _context.Templates
                .Include(tt => tt.TemplateType)
                .Where(t => t.TemplateId == templateId)
                .SingleOrDefault();
        }

    }
}
