using CalisanYonetimSistemi.BussinesLayer.Abstract;
using CalisanYonetimSistemi.DataAccessLayer.Abstract;
using CalisanYonetimSistemi.DataAccessLayer.Concrete;
using CalisanYonetimSistemi.DataAccessLayer.DBContexts;
using CalisanYonetimSistemi.EntityLayer.Abstract;
using System.Linq.Expressions;

namespace CalisanYonetimSistemi.BussinesLayer.Concrete
{
    public class Manager<T, TId> : IManager<T, TId> where T : BaseEntity<TId>
    {
        public IRepository<T, TId, SqlDbContext> _repo { get; } // DBContext degismesi durumunda burasi(1/3) degistirilecek. Bussines layer de kompleks is kurallari ve api projesinde ki put isleminde dbcontexte ihtiyac duydugum icin IManagere ekledigim dbcontext.
                                                                // DbContext ctordan istendiginde degismesi halinde herbir manager sinifi icin degistirilmesi gerekecek. Ama bu sekilde sadece yerden degismesi yetiyor.

        public Manager() //SqlDbContext dbContext
        {
            _repo = new Repository<T, TId, SqlDbContext>(new SqlDbContext()); // DBContext degismesi durumunda burasi(2/3) degistirilecek.
        }

        public virtual async Task<int> Delete(T entity)
        {
            return await _repo.Delete(entity);
        }

        public virtual async Task<int> DeleteAll(Expression<Func<T, bool>> expression = null)
        {
            if (expression != null)
            {
                return await _repo.DeleteAll(expression);

            }
            else
            {
                return await _repo.DeleteAll();
            }
        }

        public virtual async Task<int> DeleteByPK(TId pk)
        {
            return await _repo.DeleteByPK(pk);
        }

        public virtual async Task<ICollection<T>> GetAll(Expression<Func<T, bool>> expression = null)
        {
            if (expression != null)
            {
                return await _repo.GetAll(expression);
            }
            else
            {
                return await _repo.GetAll();
            }
        }

        public virtual async Task<IEnumerable<T>?> GetAllInclude(Expression<Func<T, bool>>? expression, params Expression<Func<T, object>>[] include)
        {
            return await _repo.GetAllInclude(expression, include);
        }

        public virtual async Task<T> GetByPK(TId pk)
        {
            return await _repo.GetByPK(pk);
        }

        public virtual async Task<int> Insert(T entity)
        {
            return await _repo.Insert(entity);
        }

        public virtual async Task<int> update(T entity)
        {
            return await _repo.update(entity);
        }

        public async virtual Task<T?> FirstOrDefault(Expression<Func<T, bool>> expression)
        {
            return await _repo.FirstOrDefault(expression);
        }
        public async virtual Task<bool> Any(Expression<Func<T, bool>> expression)
        {
            return await _repo.Any(expression);
        }

        public async virtual Task<ICollection<string>> GetAllTableNamesAsync()
        {
            return await _repo.GetAllTableNamesAsync();
        }

        public async Task<ICollection<string>> GelAllTablePropsNamesAndTypes()
        {
            return await _repo.GelAllTablePropsNamesAndTypes();
        }
    }
}
