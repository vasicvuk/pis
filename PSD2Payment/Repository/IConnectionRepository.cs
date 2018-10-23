using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PSD2Payment.Repository
{
    public interface IConnectionRepository
    {
        bool CheckConnection();
    }
}
