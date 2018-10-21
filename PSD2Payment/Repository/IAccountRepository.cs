using PSD2Payment.Database.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PSD2Payment.Repository
{
    public interface IAccountRepository
    {
        List<Account> GetAccountList();
        Account GetAccount(string account);
    }
}
