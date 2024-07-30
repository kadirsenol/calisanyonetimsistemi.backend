using CalisanYonetimSistemi.EntityLayer.Concrete;
using CalisanYonetimSistemi.EntityLayer.EntityConfig.Abstract;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CalisanYonetimSistemi.EntityLayer.EntityConfig.Concrete
{
    public class RaporConfig : BaseEntityConfig<Rapor, int>
    {
        public override void Configure(EntityTypeBuilder<Rapor> builder)
        {
            base.Configure(builder);
            builder.Property(p => p.Aciklama).HasMaxLength(50);
        }
    }
}
