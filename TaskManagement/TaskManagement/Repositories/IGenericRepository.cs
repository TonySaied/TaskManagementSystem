namespace TaskManagement.Repositories
{
    public interface IGenericRepository <T> where T : class
    {
        IQueryable<T> GetAll();
        T GetById(object id);
        void Insert(T obj);
        void Update(T obj);
        void Delete(object id);
        void DeleteRange(IEnumerable<T> entities);
        void Save();
        void ExecuteRawSql(string sql, params object[] parameters);// To Call update stored Procedure
    }
}
