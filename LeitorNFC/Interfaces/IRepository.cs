namespace LeitorNFC.Interfaces;

public interface IRepository<TypeT> where TypeT : IEntity
{
    public TypeT GetById(int id);
    public Task<IEnumerable<TypeT>> GetAllAsync(Func<TypeT, bool>? predicate = null);
    public Task AddAsync(TypeT entity);
    public void Update(TypeT entity);
    public void Delete(int id);
}
