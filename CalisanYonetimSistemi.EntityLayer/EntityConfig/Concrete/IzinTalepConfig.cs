using CalisanYonetimSistemi.EntityLayer.Concrete;
using CalisanYonetimSistemi.EntityLayer.EntityConfig.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CalisanYonetimSistemi.EntityLayer.EntityConfig.Concrete
{
    public class IzinTalepConfig : BaseEntityConfig<IzinTalep, int>
    {
        public override void Configure(EntityTypeBuilder<IzinTalep> builder)
        {
            base.Configure(builder);
            builder.Property(p => p.BaslangicTarih).HasDefaultValueSql("CAST(GETDATE() AS DATE)");
            builder.Property(p => p.BitisTarih).HasDefaultValueSql("CAST(GETDATE() AS DATE)");

        }
    }
}
