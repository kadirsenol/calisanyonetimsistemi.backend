using CalisanYonetimSistemi.EntityLayer.Abstract;

namespace CalisanYonetimSistemi.EntityLayer.Concrete
{
    public enum IzinTuru
    {
        Hastalik = 0,
        Tatil = 1,
        Ozel = 2
    }

    public class IzinTalep : BaseEntity<int>
    {
        public IzinTuru IzinTuru { get; set; }
        public DateOnly BaslangicTarih { get; set; }
        public DateOnly BitisTarih { get; set; }
        public bool Onay { get; set; } = false;
        public int UserId { get; set; }
        public virtual User User { get; set; }
    }
}
