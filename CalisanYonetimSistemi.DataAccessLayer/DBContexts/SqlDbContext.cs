using CalisanYonetimSistemi.EntityLayer.Abstract;
using CalisanYonetimSistemi.EntityLayer.Concrete;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace CalisanYonetimSistemi.DataAccessLayer.DBContexts
{
    public class SqlDbContext : DbContext
    {
        public SqlDbContext() { }
        public SqlDbContext(DbContextOptions<SqlDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<IzinTalep> IzinTalepleri { get; set; }
        public DbSet<PerformansDegerlendirme> PerformansDegerlendirmeleri { get; set; }
        public DbSet<Rapor> Raporlar { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.Load("CalisanYonetimSistemi.EntityLayer"));
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=.;Database=CalisanYonetimSistemi;Trusted_Connection=True; Trust Server Certificate=true");
        }
        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {

            var changes = ChangeTracker.Entries().Where(p => p.State == EntityState.Deleted).ToList();

            foreach (var item in changes)
            {
                item.State = EntityState.Modified;
                BaseEntity<int> baseEntity = item.Entity as BaseEntity<int>;
                baseEntity.IsDelete = true;
                baseEntity.UpdateDate = DateTime.UtcNow.AddHours(3);
            }

            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
    }
}
