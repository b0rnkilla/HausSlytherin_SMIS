namespace HausSlytherin_SMIS.Interfaces
{
    public interface IRepository<T>
    {
        void Add(T item);
        List<T> GetAll();
        T? GetById(int id);
        void Remove(int id);
    }
}