using DocumentManagement.Rest.Api.Entities;
using DocumentManagement.Rest.Api.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DocumentManagement.Rest.Api.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly DocumentSystemContext _context;
        public AccountRepository(DocumentSystemContext context) => _context = context;


        public IEnumerable<Account> GetAccountsByIDs(short[] ids)
            => _context.Accounts.Where(acc => ids.Contains(acc.AccountId));


        public Account GetAccountById(int id)
            => _context.Accounts.Include(ar => ar.AccountRoles)
                    .ThenInclude(r => r.Role)
                    .SingleOrDefault(x => x.AccountId == id);

        public Account GetAccountByUsername(string username)
            => _context.Accounts.Include(ar => ar.AccountRoles)
                    .ThenInclude(r => r.Role)
                    .SingleOrDefault(x => x.UserName.Trim().Equals(username.Trim()));

        public void InsertAccount(Account account) => _context.Accounts.Add(account);

        public void UpdateAccount(Account account) => _context?.Accounts.Update(account);    

    }
}
