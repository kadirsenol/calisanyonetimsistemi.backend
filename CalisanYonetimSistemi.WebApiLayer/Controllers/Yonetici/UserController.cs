using AutoMapper;
using CalisanYonetimSistemi.BussinesLayer.Abstract;
using CalisanYonetimSistemi.EntityLayer.Concrete;
using CalisanYonetimSistemi.EntityLayer.Concrete.VMs.YoneticiVms;
using CalisanYonetimSistemi.EntityLayer.Concrete.VMs.YoneticiVms.UserVm;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CalisanYonetimSistemi.WebApiLayer.Controllers.Yonetici
{
    [Authorize(Roles = "yonetici")]
    [Route("api/yonetici/[controller]")]
    [ApiController]
    public class UserController(IUserManager userManager, IMapper mapper) : ControllerBase
    {
        private readonly IUserManager userManager = userManager;
        private readonly IMapper mapper = mapper;

        [HttpGet("[action]")]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                return Ok(await userManager.GetAll());
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> DeleteUser(DeleteById deleteById)
        {
            try
            {
                int sonuc = await userManager.DeleteByPK(deleteById.Id);
                if (sonuc > 0)
                {
                    return Ok("İlgili kayıt başarıyla silindi");

                }
                return Problem("İlgili kayıt silinirken bir hata meydana geldi.");
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> UpdateUser(UserUpdateVm userUpdateVm)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errorMessage = ModelState.Values.SelectMany(p => p.Errors.Select(e => e.ErrorMessage));
                    return BadRequest(errorMessage);
                }

                //Tum veriler bos gelirse;
                await userManager.ChackUserUpdateNullorEmpty(userUpdateVm.Ad, userUpdateVm.Departman, userUpdateVm.Pozisyon, userUpdateVm.BaslamaTarihi, userUpdateVm.Email, userUpdateVm.Password
                     , userUpdateVm.Rol, userUpdateVm.isConfirmEmail, userUpdateVm.ConfirmEmailGuid, userUpdateVm.AccessToken, userUpdateVm.RefreshToken,
                     userUpdateVm.ExprationToken, userUpdateVm.IsDelete, userUpdateVm.TatilIzinBakiye, userUpdateVm.OzelIzinBakiye, userUpdateVm.HastalikIzinBakiye,
                     userUpdateVm.ToplamAnalitiklikPuan, userUpdateVm.ToplamTakimCalismasiPuan, userUpdateVm.ToplamUretkenlikPuan, userUpdateVm.UyelikOnay);


                //Token gecerlilik suresi uygun ise guncellenmesine izin ver
                if (!string.IsNullOrEmpty(userUpdateVm.ExprationToken))
                {
                    await userManager.ChackUserTokenExprationTimeValid(userUpdateVm.ExprationToken);
                }

                User user = await userManager.GetByPK(userUpdateVm.Id);

                mapper.Map(userUpdateVm, user);

                //null olmayan proplar map sirasinda default degeri ile degismemesi icin
                await userManager.NullByPass(user, userUpdateVm);

                int sonuc = await userManager.update(user);
                if (sonuc > 0)
                {
                    return Ok("Güncelleme İşlemi Başarılı.");
                }
                return Problem("Güncelleme sırasında bir hata meydana geldi.");

            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> InsertUser(UserInsertVm userInsertVm)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errorMessage = ModelState.Values.SelectMany(p => p.Errors.Select(e => e.ErrorMessage));
                    return BadRequest(errorMessage);
                }

                if (!string.IsNullOrEmpty(userInsertVm.ExprationToken))
                {
                    await userManager.ChackUserTokenExprationTimeValid(userInsertVm.ExprationToken);
                }

                User user = mapper.Map<User>(userInsertVm);

                await userManager.ChackUserRegister(user);

                int sonuc = await userManager.Insert(user);

                if (sonuc > 0)
                {
                    return Ok("Kullanıcı oluşturuldu.");
                }
                return Problem("Kullanıcı oluşturulurken bir hata meydana geldi.");
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

    }
}
