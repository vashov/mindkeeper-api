using Dapper;
using MindKeeper.Api.Data.Entities;
using MindKeeper.Api.Data.Repositories.Nodes.Models;
using MindKeeper.Shared.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MindKeeper.Api.Data.Repositories.Nodes
{
    public class NodeRepository : INodeRepository
    {
        private readonly IDbConnection _connection;

        public NodeRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<Node> Create(int userId, string name, string descritpion, int typeId, int parentId)
        {
            var now = DateTimeOffset.UtcNow;

            const string createNodeCommand = @"
                INSERT INTO nodes (name, descritpion, type_id, created_by, created_at, updated_by, updated_at)
                VALUES (@name, @descritpion, @typeId, @userId, @now, @userId, @now)
                RETURNING *;
            ";

            var node = await _connection.QuerySingleAsync<Node>(
                createNodeCommand,
                new { userId, name, descritpion, typeId, parentId, now });

            const string createParentChildCommand = @"
                INSERT INTO node_node (parent_id, child_id)
                VALUES (@parentId, @childId);
            ";

            if (parentId != default)
            {
                int childId = node.Id;
                await _connection.ExecuteAsync(
                    createParentChildCommand,
                    new { parentId, childId });
                
                node.Parents.Add(parentId);
            }
            
            return node;
        }

        public async Task<Node> Get(long id)
        {
            const string getQuery = "SELECT * FROM nodes WHERE id = @id";

            var node = await _connection.QuerySingleOrDefaultAsync<Node>(getQuery, new { id });

            // TODO: fill children and parents;

            return node;
        }

        public async Task<List<Node>> GetAll(NodeFilter filter)
        {
            const string getAllQuery = @"
                SELECT n.*, nnc.child_id, nnp.parent_id FROM nodes n 
                LEFT JOIN node_node nnc ON nnc.parent_id = n.id
                LEFT JOIN node_node nnp ON nnp.child_id = n.id
                --LEFT JOIN nodes np ON np.id = nnp.parent_id
                /**where**/
                ;";

            var builder = new SqlBuilder();

            if (filter.UserId != default)
            {
                builder.Where("n.created_by = @UserId");
            }

            if (filter.ParentId != default)
            {
                builder.Where("nnp.parent_id = @ParentId");
            }

            if (!string.IsNullOrWhiteSpace(filter.Name))
            {
                filter.Name = filter.Name.PrepareForSqlLike(putPercentToEnd: true).ToLower();
                builder.Where("n.name ILIKE @Name");
            }

            if (filter.Nodes.HasValues())
            {
                builder.Where("n.id = ANY(@Nodes)");
            }

            var template = builder.AddTemplate(getAllQuery, filter);

            // TODO: mapping action many-to-many

            var nodes = await _connection.QueryAsync<Node>(
                template.RawSql,
                template.Parameters);

            return nodes.ToList();
        }

        public async Task<bool> SetChild(int nodeId, int childNodeId)
        {
            string query = $"INSERT INTO node_node (parent_id, child_id) VALUES ({nodeId}, {childNodeId});";

            var result = await _connection.ExecuteAsync(query);
            return result > 0;
        }

        public async Task<bool> SetParent(int nodeId, int parentNodeId)
        {
            string query = $"INSERT INTO node_node (parent_id, child_id) VALUES ({parentNodeId}, {nodeId});";

            var result = await _connection.ExecuteAsync(query);
            return result > 0;
        }

        public async Task<bool> DeleteChild(int nodeId, int childNodeId)
        {
            string query = $"DELETE FROM node_node WHERE parent_id = {nodeId} AND child_id = {childNodeId};";

            var result = await _connection.ExecuteAsync(query);
            return result > 0;
        }

        public async Task<bool> DeleteParent(int nodeId, int parentNodeId)
        {
            string query = $"DELETE FROM node_node WHERE parent_id = {parentNodeId} AND child_id = {nodeId};";

            var result = await _connection.ExecuteAsync(query);
            return result > 0;
        }
    }
}
