using CalisanYonetimSistemi.BussinesLayer.Abstract;
using CalisanYonetimSistemi.EntityLayer.Concrete;
using CalisanYonetimSistemi.EntityLayer.Concrete.VMs.YoneticiVms.IzinVn;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CalisanYonetimSistemi.WebApiLayer.Controllers.Yonetici
{
    [Authorize(Roles = "yonetici")]
    [Route("api/yonetici/[controller]")]
    [ApiController]
    public class IzinController(IIzinManager izinManager, IUserManager userManager) : ControllerBase
    {
        private readonly IIzinManager izinManager = izinManager;
        private readonly IUserManager userManager = userManager;

        [HttpGet("[action]")]
        public async Task<IActionResult> IzinTalepler()
        {
            if (!ModelState.IsValid)
            {
                var errorMessage = ModelState.Values.SelectMany(p => p.Errors.Select(e => e.ErrorMessage));
                return BadRequest(errorMessage);
            }
            try
            {
                ICollection<IzinTalep> izinTaleps = new List<IzinTalep>();

                izinTaleps = await izinManager.GetAll();

                return Ok(izinTaleps);

            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }

        }

        [HttpPost("[action]")]
        public async Task<IActionResult> IzinOnay(IzinOnayVm onayVm)
        {
            if (!ModelState.IsValid)
            {
                var errorMessage = ModelState.Values.SelectMany(p => p.Errors.Select(e => e.ErrorMessage));
                return BadRequest(errorMessage);
            }
            try
            {
                IzinTalep izinTalep = await izinManager.GetByPK(onayVm.IzinId);

                if (izinTalep == null)
                {
                    return Problem("İlgili izin kaydı bulunamadı.");
                }

                if (izinTalep.Onay == true)
                {
                    return Problem("İlgili izin kaydı zaten onaylı.");
                }

                izinTalep.Onay = true;
                int userId = await izinManager._repo.dbContext.IzinTalepleri
                    .Where(p => p.Id == onayVm.IzinId)
                    .Select(r => r.UserId)
                    .FirstOrDefaultAsync();
                User user = await userManager.GetByPK(userId);

                int GunSayisi = await izinManager.IzinGunSayisi(izinTalep);

                var bakiye = izinTalep.IzinTuru switch
                {
                    IzinTuru.Hastalik => user.HastalikIzinBakiye = user.HastalikIzinBakiye - GunSayisi,
                    IzinTuru.Tatil => user.TatilIzinBakiye = user.TatilIzinBakiye - GunSayisi,
                    IzinTuru.Ozel => user.OzelIzinBakiye = user.OzelIzinBakiye - GunSayisi,
                    _ => 0 // Varsayılan değer
                };

                int sonuc = await userManager.update(user);
                int sonuc2 = await izinManager.update(izinTalep);
                if (sonuc > 0 && sonuc2 > 0)
                {
                    return Ok("İzin onaylama başarılı.");
                }
                return Problem("İlgili izin onaylama sırasında bir hata meydana geldi");
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }

        }

        [HttpGet("[action]")]
        public async Task<IActionResult> DepartmanBazliKullanilanIzinler()
        {
            if (!ModelState.IsValid)
            {
                var errorMessage = ModelState.Values.SelectMany(p => p.Errors.Select(e => e.ErrorMessage));
                return BadRequest(errorMessage);
            }
            try
            {
                var kullanilanIzinler = await izinManager.DepartmanBazliIzinKullanimi();

                return Ok(kullanilanIzinler);

            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }

        }


        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> IzinRed(int id)
        {
            if (!ModelState.IsValid)
            {
                var errorMessage = ModelState.Values.SelectMany(p => p.Errors.Select(e => e.ErrorMessage));
                return BadRequest(errorMessage);
            }
            try
            {
                int sonuc = await izinManager.DeleteByPK(id);

                if (sonuc > 0)
                {
                    return Ok("İlgili izin reddedildi.");
                }
                return Problem("İlgili izin reddedilirken bir hata meydana geldi.");
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }

        }


        [HttpGet("[action]")]
        public async Task<IActionResult> IzinAnomali()
        {
            if (!ModelState.IsValid)
            {
                var errorMessage = ModelState.Values.SelectMany(p => p.Errors.Select(e => e.ErrorMessage));
                return BadRequest(errorMessage);
            }
            try
            {
                var sonuc = await izinManager.IzinAnamoli();
                if (sonuc == null)
                {
                    return Problem("İzin taleplerinde olağan dışı bir durum yok.");
                }

                return Ok(sonuc);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}
