using System.ComponentModel.DataAnnotations;

namespace CalisanYonetimSistemi.EntityLayer.Concrete.VMs.IzinVm
{
    public class IzinTalepInsertVm
    {
        [Required(ErrorMessage = "İzin Türü Boş Bıraklımaz.")]
        public int IzinTuru { get; set; }

        [Required(ErrorMessage = "İzin Baslangic Günü Boş Geçilemez")]
        [RegularExpression(@"^\d{2}\.\d{2}\.\d{4}$", ErrorMessage = "Başlama tarihi 'dd.MM.yyyy' formatında olmalıdır.")]
        public string BaslangicTarih { get; set; }

        [Required(ErrorMessage = "İzin Bitis Günü Boş Geçilemez")]
        [RegularExpression(@"^\d{2}\.\d{2}\.\d{4}$", ErrorMessage = "Başlama tarihi 'dd.MM.yyyy' formatında olmalıdır.")]
        public string BitisTarih { get; set; }
    }
}
