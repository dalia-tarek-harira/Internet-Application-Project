namespace BookSwap.Repository
{
    public interface IRepository<T>
    {
     
        public void Add(T entity);
        public void Update(T entity);
        public T GetById(int id);
        public void Delete(int id); 
        public void Save();
    }
}
