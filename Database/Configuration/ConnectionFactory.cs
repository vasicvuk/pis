using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PSD2Payment.Database.Configuration
{
    public enum DBType
    {
        InMemory,
        MSSQL
    }

    public class ConnectionFactory
    {
        public static string CreateConnectionString(
              DBType type,
              string host,
              string port,
              string database,
              string user,
              string password
            )
        {
            switch (type)
            {
                case DBType.InMemory:
                    return database;
            }
            throw new Exception("Unknown database type encountered!");
        }


        public static void DatabaseConfiguration(string schema, DbContextOptionsBuilder options, IConfiguration configuration, string migrationAssembly = null, bool isMigration = false)
        {
            options.UseInMemoryDatabase("psd2");
        }
    }
}