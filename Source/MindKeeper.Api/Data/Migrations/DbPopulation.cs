﻿using Dapper;
using MindKeeper.Api.Core.Auth;
using MindKeeper.Api.Data.Constants;
using MindKeeper.Api.Data.Migrations.Countries;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace MindKeeper.Api.Data.Migrations
{
    public static class DbPopulation
    {
        public static async Task Populate(IDbConnection connection)
        {
            await InsertSystemUser(connection);
            await InsertNodeTypes(connection);
            await InsertCountries(connection);
        }

        private static async Task InsertNodeTypes(IDbConnection connection)
        {
            const string query = @"
                SELECT EXISTS (
                    SELECT id FROM node_types
                    LIMIT 1
                );
            ";

            var isNodeTypesLoaded = await connection.QuerySingleAsync<bool>(query);
            if (isNodeTypesLoaded)
                return;

            var nodeTypes = Enum.GetValues(typeof(NodeTypeEnum)).Cast<NodeTypeEnum>();
            foreach (var nodeType in nodeTypes)
            {
                bool isEditable = nodeType == NodeTypeEnum.Common;
                string populateQuery = @$"
                    INSERT INTO node_types (id, name, is_editable)
                    VALUES ({(int)nodeType}, '{nodeType}', {isEditable})
                    ON CONFLICT DO NOTHING
                    ;";

                await connection.ExecuteAsync(populateQuery);
            }
        }

        private static async Task InsertSystemUser(IDbConnection connection)
        {
            var userId = SystemUser.Id;

            string query = @$"
                SELECT EXISTS (
                    SELECT id FROM users WHERE id = {userId}
                    LIMIT 1
                );
            ";

            var isSystemUserExist = await connection.QuerySingleAsync<bool>(query);
            if (isSystemUserExist)
                return;

            var name = SystemUser.Name;
            var normalizedName = name.ToLower();

            var random = new Random();
            var password = random.Next(int.MaxValue / 10, int.MaxValue).ToString();
            password += random.Next(int.MaxValue / 10, int.MaxValue).ToString();
            var passwordHasher = new PasswordHasher();
            var passwordHash = passwordHasher.CreateHash(password);

            const string populateQuery = @"
                    INSERT INTO users (id, mail, normalized_mail, password_hash)
                    VALUES (@userId, @name, @normalizedName, @passwordHash)
                    ON CONFLICT DO NOTHING
                    ;";

            var builder = new SqlBuilder();
            var template = builder.AddTemplate(
                populateQuery,
                new { userId, name, normalizedName, passwordHash });

            await connection.ExecuteAsync(template.RawSql, template.Parameters);
        }

        private static async Task InsertCountries(IDbConnection connection)
        {
            string query = @$"
                SELECT EXISTS (
                    SELECT n.id FROM nodes n
                    INNER JOIN node_types nt ON n.type_id = nt.id
                    WHERE nt.id = {(int)NodeTypeEnum.Country}
                    LIMIT 1
                );
            ";

            var isCountriesLoaded = await connection.QuerySingleAsync<bool>(query);
            if (isCountriesLoaded)
                return;

            using var fileReader = File.OpenRead(@".\Data\Migrations\Countries\countries.json");
            //using var streamReader = new StreamReader(fileReader);

            //var json = await streamReader.ReadToEndAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var countries = await JsonSerializer.DeserializeAsync<List<CountryModel>>(fileReader, options);

            var typeId = (int)NodeTypeEnum.Country;
            var userId = SystemUser.Id;
            var now = DateTimeOffset.UtcNow;

            foreach (var country in countries)
            {
                var name = $"{country.Name}, {country.Code}";

                const string populateQuery = @"
                    INSERT INTO nodes (name, type_id, created_by, created_at, updated_by, updated_at)
                    VALUES (@name, @typeId, @userId, @now, @userId, @now)
                    ON CONFLICT DO NOTHING
                    ;";

                var builder = new SqlBuilder();
                var template = builder.AddTemplate(
                    populateQuery,
                    new { name, typeId, userId, now });
                

                await connection.ExecuteAsync(template.RawSql, template.Parameters);
            }
        }
    }
}