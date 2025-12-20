namespace LeitorNFC.Interfaces;

public interface IRepository<TypeT> where TypeT : IEntity
{
    public TypeT GetById(int id);
    public IEnumerable<TypeT> GetAll(Func<TypeT, bool>? predicate = null);
    public void Add(TypeT entity);
    public void Update(TypeT entity);
    public void Delete(int id);
}
