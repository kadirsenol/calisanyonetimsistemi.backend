using System.ComponentModel.DataAnnotations;

namespace CalisanYonetimSistemi.EntityLayer.Concrete.VMs.YoneticiVms.RaporVm
{
    public class RaporInsertVm
    {
        [Required(ErrorMessage = "Başlangıç Dönem Alanı Boş Geçilemez")]
        [RegularExpression(@"\d{2}\.\d{2}\.\d{4}", ErrorMessage = "Tarih formatı dd.MM.yyyy olmalıdır.")]
        public string BaslangicTarih { get; set; }  //AutoMapper bunları map ettmez ise burayı dateonly olarak ayarla bu şekilde istek geliyor mu kontrole t

        [Required(ErrorMessage = "Bitiş Dönem Alanı Boş Geçilemez")]
        [RegularExpression(@"\d{2}\.\d{2}\.\d{4}", ErrorMessage = "Tarih formatı dd.MM.yyyy olmalıdır.")]
        public string BitisTarih { get; set; }

        [Required(ErrorMessage = "Rapor Türü Boş Geçilemez")]
        public string RaporTuru { get; set; }

        [Required(ErrorMessage = "Açıklama Alanı Boş Geçilemez")]
        [MinLength(3, ErrorMessage = "Açıklama alanına en az 3 karakter girmelisiniz")]
        public string Aciklama { get; set; }

        [Required(ErrorMessage = "Çalışan Boş Geçilemez")]
        public int UserId { get; set; }
    }
}
