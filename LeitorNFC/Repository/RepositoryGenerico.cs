namespace LeitorNFC.Repository;

public class RepositoryGenerico<TEntity> : DapperRepositoryAbstract, IRepository<TEntity> where TEntity : class, IEntity
{
    private static readonly string InsertSql = SqlBuilderHelper.BuildInsert<TEntity>();
    private static readonly string UpdateSql = SqlBuilderHelper.BuildUpdate<TEntity>();
    private static readonly string DeleteSql = SqlBuilderHelper.BuildDelete<TEntity>();
    private static readonly string GetByIdSql = SqlBuilderHelper.BuildSelectById<TEntity>();
    private static readonly string GetAllSql = SqlBuilderHelper.BuildSelectAll<TEntity>();
    private static readonly string SequenceSql = SqlBuilderHelper.BuildSequence<TEntity>();

    public RepositoryGenerico(IDbConnection connection) : base(connection) { }

    public async Task<long> AddAsync(TEntity entity)
    {
        entity.Id = await QuerySingleAsync<long>(SequenceSql);
        await ExecuteAsync(InsertSql, entity);
        return entity.Id;
    }
    public Task UpdateAsync(TEntity entity) => ExecuteAsync(UpdateSql, entity);
    public Task DeleteAsync(long id) => ExecuteAsync(DeleteSql, new { id });
    public Task<TEntity> GetAsync(long id) => QuerySingleAsync<TEntity>(GetByIdSql, new { id });
    public Task<IEnumerable<TEntity>> GetAsync() => QueryAsync<TEntity>(GetAllSql);
}
