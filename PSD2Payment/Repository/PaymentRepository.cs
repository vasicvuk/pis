using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PSD2Payment.Application.Command;
using PSD2Payment.Database;
using PSD2Payment.Database.Model;

namespace PSD2Payment.Repository
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly PISDBContext _context;

        public PaymentRepository(PISDBContext context)
        {
            _context = context;
        }

        public PaymentCommandResult MakePayment(InitiatePaymentCommand command)
        {
            Account deptorAccount = _context.Accounts.Where(x => x.Iban.Equals(command.DebtorAccount.Iban)).FirstOrDefault();
            if (deptorAccount == null)
            {
                return PaymentCommandResult.ACCOUNT_NOT_FOUND;
            }
            if (deptorAccount.Balances == null)
            {
                return PaymentCommandResult.NO_CURRENCY_ACCOUNT;
            }
            var balance = deptorAccount.Balances.Where(x => x.BalanceAmount.Currency.Equals(command.InstructedAmount.Currency)).FirstOrDefault();
            if (balance == null)
            {
                return PaymentCommandResult.NO_CURRENCY_ACCOUNT;
            }
            if (balance.BalanceAmount.Amount < command.InstructedAmount.Amount)
            {
                return PaymentCommandResult.INSUFISHENT_FUNDS;
            }
            if(command.InstructedAmount.Amount < 0)
            {
                return PaymentCommandResult.AMOUNT_CANNOT_BE_NEGATIVE;
            }
            balance.BalanceAmount.Amount = balance.BalanceAmount.Amount - command.InstructedAmount.Amount;
            _context.Update(deptorAccount);
            _context.SaveChanges();
            return PaymentCommandResult.OK;
        }
    }
}
