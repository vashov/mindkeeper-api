using System;

namespace MindKeeper.Api.Core
{
    public static class SqlConnectionStringBuilder
    {
        public static string Build()
        {
            string databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL")
                ?? throw new NotImplementedException("Setup DATABASE URL.");

            var connectionInfo = databaseUrl.Split('@');
            const string postgresWord = "postgres://";
            string[] userInfo = connectionInfo[0].Remove(0, postgresWord.Length).Split(':');

            string userId = userInfo[0];
            string userPassword = userInfo[1];

            string[] databaseInfo = connectionInfo[1].Split('/');

            string databaseName = databaseInfo[1];
            string[] databaseServerPort = databaseInfo[0].Split(':');

            string databaseServer = databaseServerPort[0];
            string databasePort = databaseServerPort[1];

            string connectionString =
                $"Server={databaseServer};" +
                $"Port={databasePort};" +
                $"Database={databaseName};" +
                $"User Id={userId};" +
                $"Password={userPassword};" +
                $"SSL Mode=Require;" +
                $"Trust Server Certificate=true;";

            return connectionString;
        }
    }
}
