using AutoMapper;
using CalisanYonetimSistemi.BussinesLayer.Abstract;
using CalisanYonetimSistemi.EntityLayer.Concrete;
using CalisanYonetimSistemi.EntityLayer.Concrete.VMs.UserVm;
using CalisanYonetimSistemi.WebApiLayer.MyExtensions.Email;
using CalisanYonetimSistemi.WebApiLayer.MyExtensions.Tokens;
using Microsoft.AspNetCore.Mvc;

namespace CalisanYonetimSistemi.WebApiLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserManager userManager;
        private readonly IMapper mapper;
        private readonly IConfiguration configuration;

        public AccountController(IUserManager userManager, IMapper mapper, IConfiguration configuration) // Key degerini secrets dosyasina gecebilmek adini IConfigi istedik
        {
            this.userManager = userManager;
            this.mapper = mapper;
            this.configuration = configuration;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Login(UserLoginVm userLoginVm) // IActionResult olarak dönüp return kisminda Ok(userWithToken.AccessToke) olarak donebilirsin. Else kısmına da Problem("Kullanici adi sifre hatali")
        {
            if (!ModelState.IsValid)
            {
                var errorMessage = ModelState.Values.SelectMany(p => p.Errors.Select(e => e.ErrorMessage));
                return BadRequest(errorMessage);
            }
            try
            {
                User user = mapper.Map<User>(userLoginVm);
                User user1 = await userManager.ChackUserLogin(user);
                bool chackMailConfirm = await userManager.ChackUserEmailConfirm(user1);
                bool checkUyelikOnay = await userManager.CheckUserUyelikOnay(user1);
                if (user1 != null && chackMailConfirm && checkUyelikOnay)
                {

                    TokenManager tokenManager = new TokenManager();
                    User userwithToken = await tokenManager.CreateToken(user1, configuration);
                    await userManager.update(userwithToken);
                    return Ok(new { accessToken = userwithToken.AccessToken, message = $"Hoşgeldin {(userwithToken.Ad).ToUpper()} !" });
                }
                return Problem("Opss! Giriş sırasında beklenedik bir hata meydana geldi.");
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }

        }


        [HttpPost("[action]")]
        public async Task<IActionResult> Register(UserRegisterVm userRegisterVm) // IActionResult olarak dönüp return kisminda Ok(userWithToken.AccessToke) olarak donebilirsin. Else kısmına da Problem("Kullanici adi sifre hatali")
        {

            if (!ModelState.IsValid) // Eger browserde js kapatilmissa buraya takilip yine validleri gonderecek.
            {
                var errorMessage = ModelState.Values.SelectMany(p => p.Errors.Select(e => e.ErrorMessage));
                return BadRequest(errorMessage);
            }
            try
            {
                User user = mapper.Map<User>(userRegisterVm);
                bool result = await userManager.ChackUserRegister(user);
                bool result1 = await userManager.ChackConfirmPassword(userRegisterVm.Password, userRegisterVm.ConfirmPassword);
                if (result && result1)
                {
                    user.Rol = "Calisan";

                    await userManager.Insert(user);
                    User withiduser = await userManager.GetByEmailUser(user.Email);
                    string code = await userManager.CreateEmailConfirmGuidCode(withiduser);

                    RegisterMailContent registerMailContent = new RegisterMailContent();
                    string mailcontent = await registerMailContent.AddMailContent(withiduser.Id, code, withiduser.Ad);

                    EmailHelper emailHelper = new EmailHelper();
                    bool sonuc = await emailHelper.SendEmail(user.Email, mailcontent, "INITY Software Mail Onaylama");
                    if (sonuc)
                    {
                        return Ok($"{user.Email} adresine bir doğrulama linki gönderdik, üyeliğinin tamamlanması için mail içerisinde ki linke tıklamalısın.");
                    }
                    else
                    {
                        return Problem("Opss! Mail gönderimi sırasında beklenmedik bir hata meydana geldi.");
                    }
                }
                else
                {
                    return Problem("Opss! Kayıt sırasında beklenmedik bir hata meydana geldi.");
                }

            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }



        [HttpGet("[action]")]
        public async Task<IActionResult> ConfirmEmail(string uid, string code)
        {

            if (!string.IsNullOrEmpty(uid) && !string.IsNullOrEmpty(code))
            {
                var result = await userManager.ConfirmEmailAsync(uid, code);

                if (result)
                {
                    return Redirect("http://localhost:3000");
                }
                else
                {
                    return Problem("Mail onaylama işlemi başarısız, lütfen tekrar deneyin. !");
                }
            }
            else
            {
                return Problem("Mail onaylama sırasında bir hata oluştu, lütfen tekrar deneyiniz. !");
            }
        }

    }
}
