using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CalisanYonetimSistemi.DataAccessLayer.Abstract
{
    public interface IRepository<T, TId, TDbContext> where TDbContext : DbContext
    {
        public TDbContext dbContext { get; } // Bussines layer de kompleks is kurallari ve api projesinde ki put isleminde dbcontexte ihtiyac duydugum icin, dbcontext i IManager'a ulastirabilmek adina ekledim.

        //CRUD
        public Task<int> Insert(T entity);
        public Task<int> update(T entity);
        public Task<int> Delete(T entity);
        public Task<int> DeleteByPK(TId pk);
        public Task<int> DeleteAll(Expression<Func<T, bool>> expression = null);

        //QUERY
        public Task<bool> Any(Expression<Func<T, bool>> expression);
        public Task<T> GetByPK(TId pk);
        public Task<ICollection<T>> GetAll(Expression<Func<T, bool>> expression = null);
        public Task<IEnumerable<T>?> GetAllInclude(Expression<Func<T, bool>>? expression, params Expression<Func<T, object>>[] include);
        public Task<T?> FirstOrDefault(Expression<Func<T, bool>> expression);
        public Task<ICollection<string>> GetAllTableNamesAsync();
        public Task<ICollection<string>> GelAllTablePropsNamesAndTypes();
    }
}
