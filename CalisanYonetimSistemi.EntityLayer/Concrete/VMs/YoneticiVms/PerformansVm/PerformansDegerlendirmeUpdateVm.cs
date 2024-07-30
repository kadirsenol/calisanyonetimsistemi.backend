using System.ComponentModel.DataAnnotations;

namespace CalisanYonetimSistemi.EntityLayer.Concrete.VMs.YoneticiVms.PerformansVm
{
    public class PerformansDegerlendirmeUpdateVm
    {
        [Required(ErrorMessage = "Güncellenecek Kayıt Boş Geçilemez")]
        [RegularExpression("^[1-9][0-9]*$", ErrorMessage = "Lütfen sadece rakamlardan oluşan ve 0 ile başlamayan bir Id giriniz.")]
        public int Id { get; set; }

        [RegularExpression("^[1-9][0-9]*$", ErrorMessage = "Lütfen sadece rakamlardan oluşan ve 0 ile başlamayan bir UserId giriniz.")]
        public int? UserId { get; set; }

        [RegularExpression(@"^(0|[1-9]|10)$", ErrorMessage = "Performans Puanı 0 ile 10 arasında olmalıdır.")]
        public byte? PerformansPuani { get; set; }

        [MaxLength(500, ErrorMessage = "En fazla 500 karakterden olusacak yorum yazabilrisiniz")]
        public string? Yorumlar { get; set; }

        [RegularExpression(@"^\d+$", ErrorMessage = "Performans tipi yalnızca rakamlardan oluşmalıdır.")]
        public int? PerformansTipi { get; set; }
    }
}
