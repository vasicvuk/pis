using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PSD2Payment.Application.Command
{
    public class RequestAccountInformationCommand
    {
        [Required]
        public string Iban { get; set; }
    }
}
