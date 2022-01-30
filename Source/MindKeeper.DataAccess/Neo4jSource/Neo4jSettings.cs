using System;

namespace MindKeeper.DataAccess.Neo4jSource
{
    public static class Neo4jSettings
    {
        static Neo4jSettings()
        {
            Uri = Environment.GetEnvironmentVariable("NEO4J_URL")
                ?? throw new NotImplementedException("Setup NEO4J_URL evn.");

            Username = Environment.GetEnvironmentVariable("NEO4J_USERNAME")
                ?? throw new NotImplementedException("Setup NEO4J_USERNAME evn.");

            Password = Environment.GetEnvironmentVariable("NEO4J_PASSWORD")
                ?? throw new NotImplementedException("Setup NEO4J_PASSWORD evn.");
        }

        public static string Uri { get; private set; }
        public static string Username { get; private set; }
        public static string Password { get; private set; }
    }
}
