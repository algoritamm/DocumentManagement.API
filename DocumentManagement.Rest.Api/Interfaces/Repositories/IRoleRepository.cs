using DocumentManagement.Rest.Api.Entities;

namespace DocumentManagement.Rest.Api.Interfaces.Repositories
{
    public interface IRoleRepository
    {
        public Role? GetRoleByNameAD(string nameAD);
        public void InsertAccountRole(AccountRole accountRole);
        public IEnumerable<Role> GetRoles();
    }
}
