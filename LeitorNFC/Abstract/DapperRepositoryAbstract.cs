namespace LeitorNFC.Abstract;

public abstract class DapperRepositoryAbstract
{
    protected readonly IDbConnection Connection;

    protected DapperRepositoryAbstract(IDbConnection connection)
    {
        this.Connection = connection;
    }

    protected Task ExecuteAsync(string sql, object? param = null) => Connection.ExecuteAsync(sql, param);
    protected Task ExecuteAsync(string sql, object? param = null, IDbTransaction? transaction = null) => Connection.ExecuteAsync(sql, param, transaction);
    protected Task<T> QuerySingleAsync<T>(string sql, object? param = null, IDbTransaction? transaction = null) => Connection.QuerySingleAsync<T>(sql, param, transaction);
    protected Task<dynamic> QuerySingleAsync(string sql, object? param = null, IDbTransaction? transaction = null) => Connection.QuerySingleAsync(sql, param, transaction);
    protected Task<IEnumerable<T>> QueryAsync<T>(string sql, object? param = null, IDbTransaction? transaction = null) => Connection.QueryAsync<T>(sql, param, transaction);
    protected Task<IEnumerable<dynamic>> QueryAsync(string sql, object? param = null, IDbTransaction? transaction = null) => Connection.QueryAsync(sql, param, transaction);
}
