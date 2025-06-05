using DocumentManagement.Rest.Api.Entities;
using System.Collections.Generic;

namespace DocumentManagement.Rest.Api.Interfaces.Repositories
{
    public interface IAccountRepository
    {
        Account GetAccountById(int id);
        Account GetAccountByUsername(string username);
        IEnumerable<Account> GetAccountsByIDs(short[] ids);
        void InsertAccount(Account account);
        void UpdateAccount(Account account);
    }
}
