using Dapper;
using MindKeeper.Api.Data.Constants;
using System;
using System.Data;
using System.Linq;
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
            await CreateNodeTypes(connection);

            await DbPopulation.Populate(connection);
        }

        private static async Task<bool> IsTableExist(IDbConnection connection, string tableName)
        {
            string checkQuery = $"SELECT EXISTS (SELECT FROM pg_tables WHERE tablename = '{tableName}');";
            return await connection.QuerySingleAsync<bool>(checkQuery);
        }

        private static async Task CreateUsers(IDbConnection connection)
        {
            const string createQuery = @"
                CREATE TABLE IF NOT EXISTS users (
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
            const string createQuery = @"
                CREATE TABLE IF NOT EXISTS nodes (
                    id serial PRIMARY KEY,
	                name varchar(100),
	                description varchar(1000),
	                type_id int NOT NULL,
	                created_by int NOT NULL,
	                created_at timestamp with time zone NOT NULL,
	                updated_by int NOT NULL,
                    updated_at timestamp with time zone NOT NULL,
	                CONSTRAINT fk_user_created_by FOREIGN KEY (created_by) REFERENCES users(id),
	                CONSTRAINT fk_user_updated_by FOREIGN KEY (updated_by) REFERENCES users(id)
                );
                CREATE INDEX IF NOT EXISTS lower_name_idx ON nodes (lower(name));
                CREATE INDEX IF NOT EXISTS created_at_idx ON nodes (created_at);
            ";

            await connection.ExecuteAsync(createQuery);
        }

        private static async Task CreateNodeNode(IDbConnection connection)
        {
            const string createQuery = @"
                CREATE TABLE IF NOT EXISTS node_node (
                    parent_id int NOT NULL,
                    child_id int NOT NULL,
                    PRIMARY KEY(parent_id, child_id),
                    CONSTRAINT fk_nodes_parent_id FOREIGN KEY (parent_id) REFERENCES nodes(id),
                    CONSTRAINT fk_nodes_child_id FOREIGN KEY (child_id) REFERENCES nodes(id)
                );
            ";

            await connection.ExecuteAsync(createQuery);
        }

        private static async Task CreateNodeTypes(IDbConnection connection)
        {
            if (await IsTableExist(connection, "node_types"))
                return;

            const string createQuery = @"
                CREATE TABLE IF NOT EXISTS node_types (
                    id int PRIMARY KEY,
                    name varchar(100),
                    is_editable bool NOT NULL
                );

                DO $$
                BEGIN
                    IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'fk_node_types_type_id') THEN
                        ALTER TABLE nodes
                            ADD CONSTRAINT fk_node_types_type_id
                            FOREIGN KEY (type_id) REFERENCES node_types(id);
                    END IF;
                END;
                $$;
            ";

            await connection.ExecuteAsync(createQuery);
        }
    }
}
