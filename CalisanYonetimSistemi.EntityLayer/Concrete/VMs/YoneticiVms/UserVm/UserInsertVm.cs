using System.ComponentModel.DataAnnotations;

namespace CalisanYonetimSistemi.EntityLayer.Concrete.VMs.YoneticiVms.UserVm
{
    public class UserInsertVm
    {
        [Required(ErrorMessage = "İsim Alanı Boş Geçilemez")]
        [MinLength(3, ErrorMessage = "Ad alanına en az 3 karakter girmelisiniz")]
        public string Ad { get; set; }

        [Required(ErrorMessage = "Departman Alanı Boş Geçilemez")]
        [MinLength(2, ErrorMessage = "Departman alanına en az 2 karakter girmelisiniz")]
        public string Departman { get; set; }

        [Required(ErrorMessage = "Pozisyon Alanı Boş Geçilemez")]
        [MinLength(3, ErrorMessage = "Pozisyon alanına en az 3 karakter girmelisiniz")]
        public string Pozisyon { get; set; }

        [Required(ErrorMessage = "İşe Başlama Alanı Boş Geçilemez")]
        [RegularExpression(@"\d{2}\.\d{2}\.\d{4}", ErrorMessage = "Tarih formatı dd.MM.yyyy olmalıdır.")]
        public string BaslamaTarihi { get; set; }

        [Required(ErrorMessage = "Email Alanı Boş Geçilemez")]
        [RegularExpression(@"^$|^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Geçersiz e-posta adresi.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Parola Alanı Boş Geçilemez")]
        [MinLength(4, ErrorMessage = "Password alanına en az 4 karakter girmelisiniz")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Rol Alanı Boş Geçilemez")]
        [MinLength(3, ErrorMessage = "Rol alanına en az 3 karakter girmelisiniz")]
        public string Rol { get; set; }

        [RegularExpression("^(true|false)$", ErrorMessage = "Lütfen sadece 'true' veya 'false' olacak şekilde isConfirmEmail değerini giriniz.")]
        public string? isConfirmEmail { get; set; }

        [RegularExpression("^(true|false)$", ErrorMessage = "Lütfen sadece 'true' veya 'false' olacak şekilde Üyelik onay değerini giriniz.")]
        public string? UyelikOnay { get; set; }

        [RegularExpression(@"^\d+$", ErrorMessage = "Hastalık izin bakiyesi sadece rakamlardan oluşmalıdır.")]
        public string? HastalikIzinBakiye { get; set; }

        [RegularExpression(@"^\d+$", ErrorMessage = "Tatil izin bakiyesi sadece rakamlardan oluşmalıdır.")]
        public string? TatilIzinBakiye { get; set; }

        [RegularExpression(@"^\d+$", ErrorMessage = "Ozel Izin Bakiye sadece rakamlardan oluşmalıdır.")]
        public string? OzelIzinBakiye { get; set; }

        [RegularExpression(@"^\d+$", ErrorMessage = "Toplam üretkenlik puanı sadece rakamlardan oluşmalıdır.")]
        public string? ToplamUretkenlikPuan { get; set; }

        [RegularExpression(@"^\d+$", ErrorMessage = "Toplam takım çalışması puanı sadece rakamlardan oluşmalıdır.")]
        public string? ToplamTakimCalismasiPuan { get; set; }

        [RegularExpression(@"^\d+$", ErrorMessage = "Toplam analitiklik puanı sadece rakamlardan oluşmalıdır.")]
        public string? ToplamAnalitiklikPuan { get; set; }
        public string? ConfirmEmailGuid { get; set; }
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }

        [RegularExpression("^(true|false)$", ErrorMessage = "Lütfen sadece 'true' veya 'false' olacak şekilde IsDelete değerini giriniz.")]
        public string? IsDelete { get; set; }
        public string? ExprationToken { get; set; }
    }
}
