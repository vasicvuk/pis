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
                case DBType.MSSQL:
                    System.Data.SqlClient.SqlConnectionStringBuilder mssqlCSB
                        = new System.Data.SqlClient.SqlConnectionStringBuilder();
                    string portDefault = "1433";
                    if (port != null && !port.Equals(""))
                    {
                        portDefault = port;
                    }
                    mssqlCSB.DataSource = host + "," + portDefault;
                    mssqlCSB.UserID = user;
                    mssqlCSB.Password = password;
                    mssqlCSB.InitialCatalog = database;
                    return mssqlCSB.ConnectionString;
            }
            throw new Exception("Unknown database type encountered!");
        }


        public static void DatabaseConfiguration(string schema, DbContextOptionsBuilder options, IConfiguration configuration, string migrationAssembly = null, bool isMigration = false)
        {
            string connectionString = null;
            string dbName = Environment.GetEnvironmentVariable("DATABASE_NAME");
            string dbType = Environment.GetEnvironmentVariable("DATABASE_TYPE");
            if (dbName != null && dbType != null)
            {
                string dbUser = Environment.GetEnvironmentVariable("DATABASE_USER");
                string dbPass = Environment.GetEnvironmentVariable("DATABASE_PASS");
                string dbPort = Environment.GetEnvironmentVariable("DATABASE_PORT");
                string dbHost = Environment.GetEnvironmentVariable("DATABASE_HOST");
                bool converted = Enum.TryParse(dbType, out DBType type);
                if (converted)
                {
                    connectionString = CreateConnectionString(type, dbHost, dbPort, dbName, dbUser, dbPass);
                    switch (type)
                    {
                        case DBType.InMemory:
                            options.UseInMemoryDatabase(dbName);
                            break;
                        case DBType.MSSQL:
                            options.UseSqlServer(
                                 connectionString,
                                    b =>
                                    {
                                        b.MigrationsHistoryTable(HistoryRepository.DefaultTableName, schema);
                                    }
                                );
                            break;
                    }
                }
            }
            else
            {
                connectionString = configuration.GetConnectionString("DevelopmentConnectionString");
                options.UseSqlServer(
                     connectionString,
                     b => {
                         b.MigrationsHistoryTable(HistoryRepository.DefaultTableName, schema);
                         if (migrationAssembly != null)
                         {
                             b.MigrationsAssembly(migrationAssembly);
                         }
                     }
                );
            }
        }
    }
}