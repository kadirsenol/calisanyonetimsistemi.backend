using CalisanYonetimSistemi.EntityLayer.Abstract;

namespace CalisanYonetimSistemi.EntityLayer.Concrete
{
    public enum RaporTuru
    {
        Izin = 0,
        Performans
    }
    public class Rapor : BaseEntity<int>
    {
        public DateOnly BaslangicTarih { get; set; }
        public DateOnly BitisTarih { get; set; }
        public RaporTuru RaporTuru { get; set; }
        public string Aciklama { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
