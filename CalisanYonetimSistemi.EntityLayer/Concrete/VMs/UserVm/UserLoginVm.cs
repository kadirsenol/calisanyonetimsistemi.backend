using System.ComponentModel.DataAnnotations;

namespace CalisanYonetimSistemi.EntityLayer.Concrete.VMs.UserVm
{
    public class UserLoginVm
    {

        [Required(ErrorMessage = "Email Alanı Boş Bırakılamaz.")]
        [RegularExpression(@"^$|^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Geçersiz e-posta adresi.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password Alanı Boş Bırakılamaz.")]
        public string Password { get; set; }
    }
}
