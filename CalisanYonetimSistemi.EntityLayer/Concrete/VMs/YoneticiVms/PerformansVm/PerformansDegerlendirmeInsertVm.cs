using System.ComponentModel.DataAnnotations;

namespace CalisanYonetimSistemi.EntityLayer.Concrete.VMs.YoneticiVms.PerformansVm
{
    public class PerformansDegerlendirmeInsertVm
    {
        [Required(ErrorMessage = "Değerlendirilecek Çalışan Boş Geçilemez")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Performans Puan Alanı Boş Geçilemez")]
        [RegularExpression(@"^(0|[1-9]|10)$", ErrorMessage = "Performans Puanı 0 ile 10 arasında olmalıdır.")]
        public byte PerformansPuani { get; set; }

        [Required(ErrorMessage = "Yorum Alanı Boş Geçilemez")]
        public string Yorumlar { get; set; }

        public int PerformansTipi { get; set; }
    }
}
