using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PSD2Payment.Application.Command
{
    public enum PaymentCommandResult
    {
        OK,
        INSUFISHENT_FUNDS,
        TEMPORARY_ERROR,
        ACCOUNT_NOT_FOUND,
        NO_CURRENCY_ACCOUNT,
        AMOUNT_CANNOT_BE_NEGATIVE
    }
    public class CreditorAddress
    {
        public string Street { get; set; }
        public string BuildingNumber { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
    }
    public class CurrencyAmount { 
        [Required]
        public string Currency { get; set; }
        [Required]
        public decimal Amount { get; set; }
    }
    public class AccountReferenceIban
    {
        [Required]
        public string Iban { get; set; }
        [Required]
        public string Currency { get; set; }
        public string Description { get; set; }

    }
    public class InitiatePaymentCommand
    {
        [Required]
        public AccountReferenceIban DebtorAccount { get; set; }
        [Required]
        public CurrencyAmount InstructedAmount { get; set; }
        [Required]
        public AccountReferenceIban CreditorAccount { get; set; }
        [Required]
        public string CreditorName { get; set; }
        public string EndToEndIdentification { get; set; }
        public string CreditorAgent { get; set; }
        public CreditorAddress CreditorAddress { get; set; }
        public string RemittanceInformationUnstructured { get; set; }
    }
}
