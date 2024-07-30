using CalisanYonetimSistemi.DataAccessLayer.Abstract;
using CalisanYonetimSistemi.EntityLayer.Abstract;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CalisanYonetimSistemi.DataAccessLayer.Concrete
{
    public class Repository<T, TId, TDbContext> : IRepository<T, TId, TDbContext> where T : BaseEntity<TId> where TDbContext : DbContext, new()
    {
        public TDbContext dbContext { get; }
        public Repository(TDbContext dbcon) //TDbContext dbcon   ///BURAYI SERVİSLERDE INJECT EDEBİLİRSİN.
        {
            //dbContext = new TDbContext();
            dbContext = dbcon;
        }
        public async Task<int> Insert(T entity)
        {
            await dbContext.Set<T>().AddAsync(entity);
            return await dbContext.SaveChangesAsync();
        }

        public async Task<int> update(T entity)
        {
            entity.UpdateDate = DateTime.UtcNow.AddHours(3); // Authomapper ile mapping yapmadigim zaman icin.

            var entry = dbContext.Entry(entity);
            entry.State = EntityState.Modified;

            foreach (var property in entry.CurrentValues.Properties)
            {
                var currentValue = entry.CurrentValues[property];

                if (currentValue == null)
                {
                    entry.Property(property.Name).IsModified = false;
                }
            }

            entry.Property(p => p.CreateDate).IsModified = false;
            return await dbContext.SaveChangesAsync();
        }
        public async Task<int> Delete(T entity)
        {
            dbContext.Set<T>().Remove(entity);
            return await dbContext.SaveChangesAsync();
        }

        public async Task<int> DeleteAll(Expression<Func<T, bool>> expression)
        {
            IEnumerable<T> findentities = await dbContext.Set<T>().Where(expression).ToListAsync();
            dbContext.Set<T>().RemoveRange(findentities);
            return await dbContext.SaveChangesAsync();
        }

        public async Task<int> DeleteByPK(TId pk)
        {
            T findentity = await dbContext.Set<T>().FindAsync(pk);
            dbContext.Set<T>().Remove(findentity);
            return await dbContext.SaveChangesAsync();
        }

        public async Task<ICollection<T>> GetAll(Expression<Func<T, bool>> expression = null)
        {
            if (expression != null)
            {
                return await dbContext.Set<T>().Where(expression).ToListAsync();
            }
            else
            {
                return await dbContext.Set<T>().ToListAsync();
            }
        }

        public async Task<T> GetByPK(TId pk)
        {
            return await dbContext.Set<T>().FindAsync(pk);
        }

        public async Task<bool> Any(Expression<Func<T, bool>> expression)
        {
            return await dbContext.Set<T>().AnyAsync(expression);
        }

        public async Task<IEnumerable<T>?> GetAllInclude(Expression<Func<T, bool>>? expression, params Expression<Func<T, object>>[] include)
        {
            IQueryable<T> query;
            if (expression != null)
            {
                query = dbContext.Set<T>().Where(expression);
            }
            else
            {
                query = dbContext.Set<T>();
            }

            return include.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        }

        public async Task<T?> FirstOrDefault(Expression<Func<T, bool>> expression)
        {
            return await dbContext.Set<T>().FirstOrDefaultAsync(expression);
        }

        public async Task<ICollection<string>> GetAllTableNamesAsync()
        {
            var tableNames = new List<string>();
            var entityTypes = dbContext.Model.GetEntityTypes();
            foreach (var entityType in entityTypes)
            {
                var tableName = entityType.GetTableName();
                tableNames.Add(tableName);
            }
            return tableNames;
        }

        public async Task<ICollection<string>> GelAllTablePropsNamesAndTypes()
        {
            var propertyNames = new List<string>();
            var entity = dbContext.Model.FindEntityType(typeof(T)).GetProperties().Select(p => new { name = p.Name, proptype = p.ClrType.Name });
            foreach (var propname in entity)
            {
                propertyNames.Add(propname.name + "," + propname.proptype);
            }
            return propertyNames;
        }

    }
}
