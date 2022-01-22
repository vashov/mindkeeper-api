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
            await CreateNodes(connection);
            await CreateNodeNode(connection);
        }

        private static async Task<bool> IsTableExist(IDbConnection connection, string tableName)
        {
            string checkQuery = $"SELECT EXISTS (SELECT FROM pg_tables WHERE tablename = '{tableName}');";
            return await connection.QuerySingleAsync<bool>(checkQuery);
        }

        private static async Task CreateUsers(IDbConnection connection)
        {
            if (await IsTableExist(connection, "users"))
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

        private static async Task CreateNodes(IDbConnection connection)
        {
            if (await IsTableExist(connection, "nodes"))
                return;

            const string createQuery = @"
                CREATE TABLE nodes (
                    id serial PRIMARY KEY,
	                name varchar(100),
	                description varchar(1000),
	                type_id int NOT NULL,
	                created_by int NOT NULL,
	                created_at timestamp with time zone NOT NULL,
	                updated_by int NOT NULL,
	                CONSTRAINT fk_user_created_by FOREIGN KEY (created_by) REFERENCES users(id),
	                CONSTRAINT fk_user_updated_by FOREIGN KEY (updated_by) REFERENCES users(id)
                );
                CREATE INDEX lower_name_idx ON nodes (lower(name));
                CREATE INDEX created_at_idx ON nodes (created_at);
            ";

            await connection.ExecuteAsync(createQuery);
        }

        private static async Task CreateNodeNode(IDbConnection connection)
        {
            if (await IsTableExist(connection, "node_node"))
                return;

            const string createQuery = @"
                CREATE TABLE node_node (
                    parent_id int NOT NULL,
                    child_id int NOT NULL,
                    PRIMARY KEY(parent_id, child_id),
                    CONSTRAINT fk_nodes_parent_id FOREIGN KEY (parent_id) REFERENCES nodes(id),
                    CONSTRAINT fk_nodes_child_id FOREIGN KEY (child_id) REFERENCES nodes(id)
                );
            ";

            await connection.ExecuteAsync(createQuery);
        }
    }
}
