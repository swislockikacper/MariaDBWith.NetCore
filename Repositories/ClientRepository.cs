using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using MariaDBWith.NetCore.Models;
using MySql.Data.MySqlClient;

namespace MariaDBWith.NetCore.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private string connectionString = "Server=PLACEHOLDER.mariadb.database.azure.com; Port=3306; Database=PLACEHOLDER; Uid=PLACEHOLDER@PLACEHOLDER; Pwd=PLACEHOLDER; SslMode=Preferred;";

        private MySqlConnection Connection() => new MySqlConnection(connectionString);

        public async Task DeleteAsync(string id)
        {
            using (var connection = Connection())
            {
                connection.Open();
                var commandText = "DELETE FROM Client Where Id = @Id;";

                var command = new MySqlCommand(commandText, connection);

                command.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@Id",
                    DbType = DbType.String,
                    Value = id,
                });

                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task<List<Client>> GetAllAsync()
        {
            var clients = new List<Client>();

            using (var connection = Connection())
            {
                connection.Open();

                var query = "SELECT Id, FullName, Age FROM Client;";
                var command = new MySqlCommand(query, connection);

                using (var reader = command.ExecuteReader())
                {
                    while(await reader.ReadAsync())
                    {
                        clients.Add(new Client
                        {
                            Id = await reader.GetFieldValueAsync<string>(0),
                            FullName = await reader.GetFieldValueAsync<string>(1),
                            Age = await reader.GetFieldValueAsync<short>(2)
                        });
                    }
                }
            }

            return clients;
        }

        public async Task<Client> GetByIdAsync(string id)
        {
            using (var connection = Connection())
            {
                connection.Open();

                var query = "SELECT Id, FullName, Age FROM Client WHERE Id = @Id";
                var command = new MySqlCommand(query, connection);

                command.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@Id",
                    DbType = DbType.String,
                    Value = id
                });

                using (var reader = command.ExecuteReader())
                {
                    if (await reader.ReadAsync())
                    {
                        return new Client
                        {
                            Id = await reader.GetFieldValueAsync<string>(0),
                            FullName = await reader.GetFieldValueAsync<string>(1),
                            Age = await reader.GetFieldValueAsync<short>(2)
                        };
                    }
                }
            }

            throw new Exception($"User with Id = {id} does not exists");
        }

        public async Task InsertAsync(Client client)
        {
            client.Id = Guid.NewGuid().ToString();

            using (var connection = Connection())
            {
                connection.Open();
                var commandText = "INSERT INTO Client (Id, FullName, Age) VALUES (@Id, @FullName, @Age);";

                var command = new MySqlCommand(commandText, connection);

                command.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@Id",
                    DbType = DbType.String,
                    Value = client.Id,
                });

                command.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@FullName",
                    DbType = DbType.String,
                    Value = client.FullName,
                });

                command.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@Age",
                    DbType = DbType.Int16,
                    Value = client.Age,
                });

                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task UpdateAsync(Client client)
        {
            using (var connection = Connection())
            {
                connection.Open();
                var commandText = @"UPDATE Client SET FullName = @FullName, Age = @Age WHERE Id = @Id;";

                var command = new MySqlCommand(commandText, connection);

                command.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@Id",
                    DbType = DbType.String,
                    Value = client.Id,
                });

                command.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@FullName",
                    DbType = DbType.String,
                    Value = client.FullName,
                });

                command.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@Age",
                    DbType = DbType.Int16,
                    Value = client.Age,
                });

                await command.ExecuteNonQueryAsync();
            }
        }
    }
}
