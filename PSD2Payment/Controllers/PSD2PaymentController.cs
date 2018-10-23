using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        private readonly IConnectionRepository _connectionRepository;

        private readonly ILogger<PSD2PaymentController> _logger;

        public PSD2PaymentController(
            IAccountRepository accountRepository,
            IPaymentRepository paymentRepository,
            IConnectionRepository connectionRepository,
            ILogger<PSD2PaymentController> logger
            )
        {
            _logger = logger;
            _connectionRepository = connectionRepository;
            _accountRepository = accountRepository;
            _paymentRepository = paymentRepository;
        }

        [HttpGet("accounts")]
        public IActionResult GetAccounts()
        {
            _logger.LogInformation("Getting information about accounts");
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
                    _logger.LogError("Provided account: {account} not found", command.DebtorAccount.Iban);
                    return BadRequest(new { error = "Account specified not found" });
                case PaymentCommandResult.INSUFISHENT_FUNDS:
                    _logger.LogError("There is no enough funds on account {account}", command.DebtorAccount.Iban);
                    return BadRequest(new { error = "No funds for translation on specified account" });
                case PaymentCommandResult.NO_CURRENCY_ACCOUNT:
                    _logger.LogError("No specified currency on account: {account}", command.DebtorAccount.Iban);
                    return BadRequest(new { error = "Account for currency not found" });
                case PaymentCommandResult.OK:
                    _logger.LogInformation("Translation completed from account: {account} to account: {creditorAccount}", command.DebtorAccount.Iban, command.CreditorAccount.Iban);
                    var result = _accountRepository.GetAccount(command.DebtorAccount.Iban);
                    return Ok(result);
                default:
                    _logger.LogCritical("Critical error with transaction from account: {account}", command.DebtorAccount.Iban);
                    return BadRequest(new { error = "Temporary error" });
            }
        }

        [HttpGet(".well-known/health")]
        public IActionResult GetHealth()
        {
            bool checkStatus = _connectionRepository.CheckConnection();
            if (checkStatus)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

    }
}
