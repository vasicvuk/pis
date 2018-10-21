using Microsoft.EntityFrameworkCore;
using PSD2Payment.Application.Command;
using PSD2Payment.Database;
using PSD2Payment.Repository;
using System;
using System.Linq;
using Xunit;

namespace PSD2Payment.Test
{
    public class TestPIS
    {
        public static bool Inited = false;
        public static PISDBContext DbContext { get; set; }
        public static AccountRepository AccountRepository { get; set; }
        public static PaymentRepository PaymentRepository { get; set; }

        public TestPIS()
        {
            if (!Inited)
            {
                DbContext = GetDbContext();
                DbContext.Database.EnsureCreated();
                AccountRepository = new AccountRepository(DbContext);
                PaymentRepository = new PaymentRepository(DbContext);
                Inited = true;
            }
        }

        public PISDBContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<PISDBContext>()
               .UseInMemoryDatabase(databaseName: "psd2")
               .Options;
            var context = new PISDBContext(options);
            return context;
        }

        [Fact]
        public void TestIfSeedDataExists()
        {
            var dbContextResult = DbContext.Accounts.Count();
            var accountRepositoryResult = AccountRepository.GetAccountList().Count;
            var result = dbContextResult == accountRepositoryResult && dbContextResult == 2;
            Assert.True(result);
        }

        [Fact]
        public void TestReturnAccountByIban()
        {
            string iban = "RS35105008123123123173";
            var account = AccountRepository.GetAccount(iban);
            Assert.Equal(iban, account.Iban);
        }


        [Fact]
        public void TestNonExistingAccount()
        {
            string iban = "RS35105008123123123171";
            var account = AccountRepository.GetAccount(iban);
            Assert.Null(account);
        }

        [Fact]
        public void TestPaymentWithNegativeAmount()
        {
            var paymentCommand = new InitiatePaymentCommand
            {
                DebtorAccount = new AccountReferenceIban
                {

                    Iban = "RS35105008123123123173",
                    Currency = "EUR"
                },
                CreditorName = "Dimitrije Vukicevic",
                CreditorAccount = new AccountReferenceIban
                {
                    Iban = "RS35105008123123123153",
                    Currency = "EUR"
                },
                CreditorAddress = new CreditorAddress
                {
                    BuildingNumber = "14",
                    City = "Belgrade",
                    Country = "Serbia",
                    PostalCode = "11210",
                    Street = "Ritimira Kojica 10"
                },
                InstructedAmount = new CurrencyAmount
                {
                    Amount = -100,
                    Currency = "EUR"
                }
            };
            Assert.Equal(PaymentCommandResult.AMOUNT_CANNOT_BE_NEGATIVE, PaymentRepository.MakePayment(paymentCommand));
        }

        [Fact]
        public void TestPaymentWithNonExistingAccount()
        {
            var paymentCommand = new InitiatePaymentCommand
            {
                DebtorAccount = new AccountReferenceIban
                {

                    Iban = "RS35105008123123123123",
                    Currency = "EUR"
                },
                CreditorName = "Dimitrije Vukicevic",
                CreditorAccount = new AccountReferenceIban
                {
                    Iban = "RS35105008123123123153",
                    Currency = "EUR"
                },
                CreditorAddress = new CreditorAddress
                {
                    BuildingNumber = "14",
                    City = "Belgrade",
                    Country = "Serbia",
                    PostalCode = "11210",
                    Street = "Ritimira Kojica 10"
                },
                InstructedAmount = new CurrencyAmount
                {
                    Amount = 100,
                    Currency = "EUR"
                }
            };
            Assert.Equal(PaymentCommandResult.ACCOUNT_NOT_FOUND, PaymentRepository.MakePayment(paymentCommand));
        }

        [Fact]
        public void TestPaymentWithNoCurrencyOnAccount()
        {
            var paymentCommand = new InitiatePaymentCommand
            {
                DebtorAccount = new AccountReferenceIban
                {

                    Iban = "RS35105008123123123173",
                    Currency = "EUR"
                },
                CreditorName = "Dimitrije Vukicevic",
                CreditorAccount = new AccountReferenceIban
                {
                    Iban = "RS35105008123123123153",
                    Currency = "EUR"
                },
                CreditorAddress = new CreditorAddress
                {
                    BuildingNumber = "14",
                    City = "Belgrade",
                    Country = "Serbia",
                    PostalCode = "11210",
                    Street = "Ritimira Kojica 10"
                },
                InstructedAmount = new CurrencyAmount
                {
                    Amount = 100,
                    Currency = "USD"
                }
            };
            Assert.Equal(PaymentCommandResult.NO_CURRENCY_ACCOUNT, PaymentRepository.MakePayment(paymentCommand));
        }


        [Fact]
        public void TestPaymentWithNoFundsOnAccount()
        {
            var paymentCommand = new InitiatePaymentCommand
            {
                DebtorAccount = new AccountReferenceIban
                {

                    Iban = "RS35105008123123123173",
                    Currency = "EUR"
                },
                CreditorName = "Dimitrije Vukicevic",
                CreditorAccount = new AccountReferenceIban
                {
                    Iban = "RS35105008123123123153",
                    Currency = "EUR"
                },
                CreditorAddress = new CreditorAddress
                {
                    BuildingNumber = "14",
                    City = "Belgrade",
                    Country = "Serbia",
                    PostalCode = "11210",
                    Street = "Ritimira Kojica 10"
                },
                InstructedAmount = new CurrencyAmount
                {
                    Amount = 1000000,
                    Currency = "EUR"
                }
            };
            Assert.Equal(PaymentCommandResult.INSUFISHENT_FUNDS, PaymentRepository.MakePayment(paymentCommand));
        }

        [Fact]
        public void TestSucessPayment()
        {
            var paymentIban = "RS35105008123123123173";
            var paymentCommand = new InitiatePaymentCommand
            {
                DebtorAccount = new AccountReferenceIban
                {

                    Iban = paymentIban,
                    Currency = "EUR"
                },
                CreditorName = "Dimitrije Vukicevic",
                CreditorAccount = new AccountReferenceIban
                {
                    Iban = "RS35105008123123123153",
                    Currency = "EUR"
                },
                CreditorAddress = new CreditorAddress
                {
                    BuildingNumber = "14",
                    City = "Belgrade",
                    Country = "Serbia",
                    PostalCode = "11210",
                    Street = "Ritimira Kojica 10"
                },
                InstructedAmount = new CurrencyAmount
                {
                    Amount = 100,
                    Currency = "EUR"
                }
            };
            var paymentResult = PaymentRepository.MakePayment(paymentCommand);
            var account = AccountRepository.GetAccount(paymentIban);
            var balance = account.Balances.Where(x => x.BalanceAmount.Currency.Equals("EUR")).FirstOrDefault();
            Assert.Equal(9900, balance.BalanceAmount.Amount);
        }
    }
}
