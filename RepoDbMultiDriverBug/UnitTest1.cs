using System;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Npgsql;
using NUnit.Framework;
using RepoDb;

namespace RepoDbMultiDriverBug
{
    public class Tests
    {
        public class testdbo
        {
            public int idx { get; set; }
        }

        public string SqlServerConnStr { get; } = "Server=localhost,1439;User Id=sa;Password=s!OgzP3lx8$H;Database=dab960950270438d81b88ae5e86d127b";

        public string PostgresConnStr { get; } =
            "Server=localhost;Port=5432;Username=postgres;Password=postgres;Database=dab960950270438d81b88ae5e86d127b";


        [OneTimeSetUp]
        public void Setup()
        {
            SqlServerBootstrap.Initialize();
            PostgreSqlBootstrap.Initialize();
        }

        [Test]
        public async Task OnlySqlServer()
        {
            var sqlserverConn = new SqlConnection(SqlServerConnStr);

            var table = @"CREATE TABLE testdbo (idx int)";

            try
            {
                await sqlserverConn.ExecuteNonQueryAsync(table);
            }
            catch (Exception)
            {
            }

            var item = new testdbo();

            await sqlserverConn.InsertAsync(item);

            Assert.Pass();
        }


        [Test]
        public async Task OnlyPostgres()
        {
            var postgresConn = new NpgsqlConnection(PostgresConnStr);

            var table = @"CREATE TABLE testdbo (idx int)";

            try
            {
                await postgresConn.ExecuteNonQueryAsync(table);
            }
            catch (Exception)
            {
            }

            var item = new testdbo();

            await postgresConn.InsertAsync(item);

            Assert.Pass();
        }

        [Test]
        public async Task Both()
        {
            var sqlserverConn = new SqlConnection(SqlServerConnStr);
            var postgresConn = new NpgsqlConnection(PostgresConnStr);

            var table = @"CREATE TABLE testdbo (idx int)";

            try
            {
                await sqlserverConn.ExecuteNonQueryAsync(table);
                await postgresConn.ExecuteNonQueryAsync(table);
            }
            catch (Exception)
            {
            }

            var item = new testdbo();

            await sqlserverConn.InsertAsync(item);
            await postgresConn.InsertAsync(item);

            Assert.Pass();
        }
    }
}
