using CalisanYonetimSistemi.EntityLayer.Concrete;
using CalisanYonetimSistemi.EntityLayer.EntityConfig.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CalisanYonetimSistemi.EntityLayer.EntityConfig.Concrete
{
    public class PerformansDegerlendirmeConfig : BaseEntityConfig<PerformansDegerlendirme, int>
    {
        public override void Configure(EntityTypeBuilder<PerformansDegerlendirme> builder)
        {
            base.Configure(builder);
            builder.Property(p => p.Yorumlar).HasMaxLength(500);
            builder.HasCheckConstraint("CK_PerformansPuani_Range", "[PerformansPuani] >= 0 AND [PerformansPuani] <= 10");

        }
    }
}
