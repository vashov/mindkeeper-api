using Dapper;
using System;
using System.Data;
using System.Threading.Tasks;

namespace MindKeeper.Api.Data.Migrations
{
    public class DbMigration
    {
        private readonly IServiceProvider _services;

        public DbMigration(IServiceProvider services)
        {
            _services = services;
        }

        public async Task InitDatabase()
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;

            using var connection = (IDbConnection)_services.GetService(typeof(IDbConnection));

            await CreateUsers(connection);
        }

        private static async Task<bool> IsTableExists(IDbConnection connection, string tableName)
        {
            string checkQuery = $"SELECT EXISTS (SELECT FROM pg_tables WHERE tablename = '{tableName}');";
            return await connection.QuerySingleAsync<bool>(checkQuery);
        }

        private static async Task CreateUsers(IDbConnection connection)
        {
            if (await IsTableExists(connection, "users"))
                return;

            const string createQuery = @"
                CREATE TABLE users (
                    id serial PRIMARY KEY,
                    mail varchar(128) NOT NULL,
                    normalized_mail varchar(128) NOT NULL UNIQUE,
	                password_hash varchar(128) NOT NULL
                );
            ";
            await connection.ExecuteAsync(createQuery);
        }
    }
}
