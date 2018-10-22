using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PSD2Payment.Application.Command;
using PSD2Payment.Filters;
using PSD2Payment.Repository;

namespace PSD2Payment.Controllers
{
    [Route("psd2/v1")]
    [ApiController]
    [ValidateModel]
    public class PSD2PaymentController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IPaymentRepository _paymentRepository;

        public PSD2PaymentController(IAccountRepository accountRepository, IPaymentRepository paymentRepository)
        {
            _accountRepository = accountRepository;
            _paymentRepository = paymentRepository;
        }

        [HttpGet("accounts")]
        public IActionResult GetAccounts()
        {
            return Ok(_accountRepository.GetAccountList());
        }

        [HttpGet("accounts/{accountId}")]
        public IActionResult GetAccountInformation([FromRoute] string accountId)
        {
            var result = _accountRepository.GetAccount(accountId);
            if (result == null)
            {
                return BadRequest(new { error = "Could not find account with provided IBAN" });
            }
            return Ok(result);
        }


        [HttpPost("payments")]
        public IActionResult Post([FromBody] InitiatePaymentCommand command)
        {
            var paymentResult = _paymentRepository.MakePayment(command);
            switch (paymentResult)
            {
                case PaymentCommandResult.ACCOUNT_NOT_FOUND:
                    return BadRequest(new { error = "Account specified not found" });
                case PaymentCommandResult.INSUFISHENT_FUNDS:
                    return BadRequest(new { error = "No funds for translation on specified account" });
                case PaymentCommandResult.NO_CURRENCY_ACCOUNT:
                    return BadRequest(new { error = "Account for currency not found" });
                case PaymentCommandResult.OK:
                    var result = _accountRepository.GetAccount(command.DebtorAccount.Iban);
                    return Ok(result);
                default:
                    return BadRequest(new { error = "Temporary error" });
            }
        }

        [HttpGet(".well-known/health")]
        public IActionResult GetHealth()
        {
            return Ok();
        }

    }
}
