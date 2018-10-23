using Microsoft.EntityFrameworkCore;
using PSD2Payment.Database;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace PSD2Payment.Repository
{
    public class ConnectionRepository : IConnectionRepository
    {
        private readonly PISDBContext _dbContext;
        public ConnectionRepository(PISDBContext pISDBContext)
        {
            _dbContext = pISDBContext;
        }

        public bool CheckConnection()
        {
            DbConnection conn = _dbContext.Database.GetDbConnection();
            try
            {
                conn.Open();
                conn.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}