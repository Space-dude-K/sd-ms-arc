using Entities;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace ForumTest
{
    public abstract class TestWithSqlite : IDisposable
    {
        protected const string inMemoryConnectionString = "DataSource=:memory:";
        private readonly SqliteConnection _connection;

        protected readonly ForumContext DbContext;

        protected TestWithSqlite()
        {

            _connection = new SqliteConnection(inMemoryConnectionString);
            _connection.Open();
            var options = new DbContextOptionsBuilder<ForumContext>()
                    .UseSqlite(_connection)
                    .Options;
            DbContext = new ForumContext(options);
            DbContext.Database.EnsureCreated();
        }

        public void Dispose()
        {
            _connection.Close();
        }
    }
}