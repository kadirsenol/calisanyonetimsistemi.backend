using CalisanYonetimSistemi.EntityLayer.Abstract;

namespace CalisanYonetimSistemi.EntityLayer.Concrete
{
    public enum PerformanTipi
    {
        Uretkenlik = 0,
        TakimCalismasi,
        Analitiklik
    }

    public class PerformansDegerlendirme : BaseEntity<int>
    {
        public PerformanTipi PerformansTipi { get; set; }
        public byte PerformansPuani { get; set; }
        public string Yorumlar { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
    }
}
