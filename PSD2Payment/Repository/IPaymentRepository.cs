using PSD2Payment.Application.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PSD2Payment.Repository
{
    public interface IPaymentRepository
    {
        PaymentCommandResult MakePayment(InitiatePaymentCommand command);
    }
}
