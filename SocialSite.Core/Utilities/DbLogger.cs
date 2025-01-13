using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using SocialSite.Domain.Utilities;

namespace SocialSite.Core.Utilities;

public class DbLogger : ILogger
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly string? _connectionString;

    public DbLogger(IDateTimeProvider dateTimeProvider, string? connectionString)
    {
        _dateTimeProvider = dateTimeProvider;
        _connectionString = connectionString;
    }

    public async Task Log(string exception, string message, string location, string? userId)
    {
        var query = "INSERT INTO dbo.AppLog (Exception, Message, Location, UserId, LogDate) " +
                    "VALUES (@Exception, @Message, @Location, @UserId, @LogDate)";
        try
        {
            using var connection = new SqlConnection(_connectionString);

            await connection.OpenAsync();

            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Exception", exception);
            command.Parameters.AddWithValue("@Message", message);
            command.Parameters.AddWithValue("@Location", location);
            command.Parameters.AddWithValue("@UserId", userId);
            command.Parameters.AddWithValue("@LogDate", _dateTimeProvider.GetDateTime());

            await command.ExecuteNonQueryAsync();
        }
        catch { };
    }
}
