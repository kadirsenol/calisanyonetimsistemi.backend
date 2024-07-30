namespace CalisanYonetimSistemi.EntityLayer.Abstract
{
    public class BaseEntity<TId>
    {
        public TId Id { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.UtcNow.AddHours(3); // Sqlden baska bir db ye geciste config islemine gerek kalmamasini saglar.
        public DateTime UpdateDate { get; set; } = DateTime.UtcNow.AddHours(3);
        public bool IsDelete { get; set; } = false;
    }
}
