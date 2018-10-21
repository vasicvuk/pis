using PSD2Payment.Database;
using PSD2Payment.Database.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PSD2Payment.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly PISDBContext _context;

        public AccountRepository(PISDBContext context)
        {
            _context = context;
        }

        public Account GetAccount(string iban)
        {
            return _context.Accounts.Where(x => x.Iban.Equals(iban)).FirstOrDefault();
        }

        public List<Account> GetAccountList()
        {
            return _context.Accounts.ToList();
        }
    }
}
