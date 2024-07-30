using CalisanYonetimSistemi.EntityLayer.Abstract;

namespace CalisanYonetimSistemi.EntityLayer.Concrete
{
    public class User : BaseEntity<int>
    {
        public string Ad { get; set; }
        public string Pozisyon { get; set; }
        public string Departman { get; set; }
        public DateOnly BaslamaTarihi { get; set; }
        public int HastalikIzinBakiye { get; set; } = 5;
        public int TatilIzinBakiye { get; set; } = 5;
        public int OzelIzinBakiye { get; set; } = 5;
        public int ToplamUretkenlikPuan { get; set; } = 0;
        public int ToplamTakimCalismasiPuan { get; set; } = 0;
        public int ToplamAnalitiklikPuan { get; set; } = 0;
        public string Email { get; set; }
        public string Password { get; set; }
        public string Rol { get; set; }
        public bool UyelikOnay { get; set; } = false;
        public bool isConfirmEmail { get; set; } = false;
        public string? ConfirmEmailGuid { get; set; }
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? ExprationToken { get; set; }
        public virtual ICollection<IzinTalep> IzinTalepler { get; set; }
        public virtual ICollection<PerformansDegerlendirme> PerformansDegerlendirmeler { get; set; }
        public virtual ICollection<Rapor> Raporlar { get; set; }
    }
}
