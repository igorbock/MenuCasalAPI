namespace LeitorNFC.Abstract;

public abstract class DapperRepositoryAbstract
{
    protected readonly IDbConnection Connection;

    protected DapperRepositoryAbstract(IDbConnection connection)
    {
        this.Connection = connection;
    }

    protected Task ExecuteAsync(string sql, object? param = null) => Connection.ExecuteAsync(sql, param);
    protected Task<T> QuerySingleAsync<T>(string sql, object? param = null) => Connection.QuerySingleAsync<T>(sql, param);
    protected Task<dynamic> QuerySingleAsync(string sql, object? param = null) => Connection.QuerySingleAsync(sql, param);
    protected Task<IEnumerable<T>> QueryAsync<T>(string sql, object? param = null) => Connection.QueryAsync<T>(sql, param);
    protected Task<IEnumerable<dynamic>> QueryAsync(string sql, object? param = null) => Connection.QueryAsync(sql, param);
}
