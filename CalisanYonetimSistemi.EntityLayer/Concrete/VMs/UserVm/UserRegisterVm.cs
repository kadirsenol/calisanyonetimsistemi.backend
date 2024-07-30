using System.ComponentModel.DataAnnotations;

namespace CalisanYonetimSistemi.EntityLayer.Concrete.VMs.UserVm
{
    public class UserRegisterVm
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
        [RegularExpression(@"^\d{2}\.\d{2}\.\d{4}$", ErrorMessage = "Başlama tarihi 'dd.MM.yyyy' formatında olmalıdır.")]
        public string BaslamaTarihi { get; set; }

        [Required(ErrorMessage = "Email Alanı Boş Geçilemez")]
        [RegularExpression(@"^$|^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Geçersiz e-posta adresi.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Parola Alanı Boş Geçilemez")]
        [MinLength(4, ErrorMessage = "Password alanına en az 4 karakter girmelisiniz")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Parola Doğrulama Alanı Boş Geçilemez")]
        public string ConfirmPassword { get; set; }
    }
}
