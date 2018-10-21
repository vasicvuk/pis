using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PSD2Payment.Database.Configuration;
using PSD2Payment.Database.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace PSD2Payment.Database
{
    public class PISDBContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }

        public PISDBContext(DbContextOptions<PISDBContext> options) : base(options)
        {

        }

        public PISDBContext()
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("psd2");

            modelBuilder.Entity<Account>().HasData(new Account
            {
                ResourceId = Guid.NewGuid().ToString(),
                Balances = new List<Balance>
                {
                    new Balance
                    {
                        BalanceAmount = new BalanceAmount
                        {
                            Amount = 10000,
                            Currency = "EUR"
                        },
                        LastChangeDateTime = DateTime.Now,
                        BalanceType = "current"
                    },
                     new Balance
                    {
                        BalanceAmount = new BalanceAmount
                        {
                            Amount = 128000,
                            Currency = "RSD"
                        },
                        LastChangeDateTime = DateTime.Now,
                        BalanceType = "current"
                    }
                },
                Iban = "RS35105008123123123173",
                Name = "Multi-currency Current account",
                Product = "current-account",
                Status = "active",
                Currency = "RSD",
                CashAccountType = "multi-currency"
            },
            new Account
            {
                ResourceId = Guid.NewGuid().ToString(),
                Balances = new List<Balance>
                {
                    new Balance
                    {
                        BalanceAmount = new BalanceAmount
                        {
                            Amount = 6500,
                            Currency = "EUR"
                        },
                        LastChangeDateTime = DateTime.Now,
                        BalanceType = "current"
                    },
                     new Balance
                    {
                        BalanceAmount = new BalanceAmount
                        {
                            Amount = 765000,
                            Currency = "RSD"
                        },
                        LastChangeDateTime = DateTime.Now,
                        BalanceType = "current"
                    }
                },
                Iban = "RS3510523423123123123173",
                Name = "Multi-currency Current account",
                Product = "current-account",
                Status = "active",
                Currency = "EUR",
                CashAccountType = "multi-currency"
            });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var envName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            if (optionsBuilder.IsConfigured) return;
            IConfiguration configuration = new ConfigurationBuilder()
                  .SetBasePath(Path.GetDirectoryName(GetType().GetTypeInfo().Assembly.Location))
                  .AddJsonFile($"configuration/appsettings.json", optional: false, reloadOnChange: false)
                  .AddJsonFile($"configuration/appsettings.{envName}.json", optional: true)
                  .AddEnvironmentVariables()
                  .Build();
            ConnectionFactory.DatabaseConfiguration("psd2", optionsBuilder, configuration, null, true);
        }
        
    }
}
