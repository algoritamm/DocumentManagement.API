using DocumentManagement.Rest.Api.Entities;
using DocumentManagement.Rest.Api.Interfaces.Repositories;

namespace DocumentManagement.Rest.Api.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly DocumentSystemContext _context;
        public RoleRepository(DocumentSystemContext context) => _context = context;

        public Role? GetRoleByNameAD(string nameAD) => _context.Roles.Where(x => x.RoleNameAd.Equals(nameAD)).FirstOrDefault();

        public IEnumerable<Role> GetRoles()
        {
            return _context.Roles;
        }

        public void InsertAccountRole(AccountRole accountRole) => _context.AccountRoles.Add(accountRole);
    }
}
