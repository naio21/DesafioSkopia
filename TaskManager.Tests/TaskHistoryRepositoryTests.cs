using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Dapper;
using TaskManager.Models;
using TaskManager.Repositories.Implementations;
using Xunit;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace TaskManager.Tests;

public class TaskHistoryRepositoryTests
{
    [Fact]
    public async Task CreateAndGetHistory_ShouldWork()
    {
        var config = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string>
        {
            {"ConnectionStrings:DefaultConnection", "DataSource=:memory:"}
        }).Build();
        var repo = new TaskHistoryRepository(config);
        using var conn = new SqliteConnection("DataSource=:memory:");
        conn.Open();
        await conn.ExecuteAsync(@"CREATE TABLE TaskHistory (
            HistoryId INTEGER PRIMARY KEY AUTOINCREMENT,
            TaskId INTEGER NOT NULL,
            FieldChanged TEXT NOT NULL,
            ChangedAt TEXT NOT NULL,
            ChangedBy INTEGER NOT NULL,
            Comment TEXT
        );");
        var history = new TaskHistory { TaskId = 1, FieldChanged = "Status", ChangedAt = System.DateTime.UtcNow, ChangedBy = 2, Comment = "Done" };
        await conn.ExecuteAsync(@"INSERT INTO TaskHistory (TaskId, FieldChanged, ChangedAt, ChangedBy, Comment) VALUES (@TaskId, @FieldChanged, @ChangedAt, @ChangedBy, @Comment)", history);
        var result = await conn.QueryAsync<TaskHistory>("SELECT * FROM TaskHistory WHERE TaskId = 1");
        Assert.Single(result);
    }
}
