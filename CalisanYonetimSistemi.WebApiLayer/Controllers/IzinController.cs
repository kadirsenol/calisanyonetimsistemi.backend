using AutoMapper;
using CalisanYonetimSistemi.BussinesLayer.Abstract;
using CalisanYonetimSistemi.EntityLayer.Concrete;
using CalisanYonetimSistemi.EntityLayer.Concrete.Support;
using CalisanYonetimSistemi.EntityLayer.Concrete.VMs.IzinVm;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CalisanYonetimSistemi.WebApiLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IzinController(IIzinManager izinManager, IMapper mapper, IRaporManager raporManager, IHttpContextAccessor httpContextAccessor) : ControllerBase
    {
        private readonly IIzinManager izinManager = izinManager;
        private readonly IMapper mapper = mapper;
        private readonly IRaporManager raporManager = raporManager;
        private readonly IHttpContextAccessor httpContextAccessor = httpContextAccessor;


        [HttpGet("[action]")]
        public async Task<IActionResult> GetIzin()
        {
            if (!ModelState.IsValid)
            {
                var errorMessage = ModelState.Values.SelectMany(p => p.Errors.Select(e => e.ErrorMessage));
                return BadRequest(errorMessage);
            }
            try
            {
                int userId = int.Parse(httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(p => p.Type == "UserId").Value);

                Izinler izinler = await izinManager.GetIzinler(userId);

                if (izinler == null)
                {
                    return Problem("İlgili çalışanın izinleri yüklenirken bir hata meydana geldi !");
                }
                return Ok(izinler);

            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Insert(IzinTalepInsertVm izinTalepInsertVm)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errorMessage = ModelState.Values.SelectMany(p => p.Errors.Select(e => e.ErrorMessage));
                    return BadRequest(errorMessage);
                }

                int userId = int.Parse(httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(p => p.Type == "UserId").Value);

                IzinTuru izinTuru = (IzinTuru)izinTalepInsertVm.IzinTuru;

                User user = await izinManager._repo.dbContext.Users
                    .Where(p => p.Id == userId)
                    .FirstOrDefaultAsync();

                int bakiye = izinTuru switch
                {
                    IzinTuru.Hastalik => user.HastalikIzinBakiye,
                    IzinTuru.Tatil => user.TatilIzinBakiye,
                    IzinTuru.Ozel => user.OzelIzinBakiye,
                    _ => 0 // Varsayılan değer
                };

                if (!(bakiye > 0 && bakiye < 16))
                {
                    return Problem("İzin bakiyeniz bulunmamaktadır.");
                }

                IzinTalep izinTalep = mapper.Map<IzinTalep>(izinTalepInsertVm);
                izinTalep.UserId = userId;
                izinTalep.IzinTuru = izinTuru;

                await izinManager.CheckIzinTarihi(izinTalepInsertVm, userId);

                if (await izinManager.Insert(izinTalep) > 0)
                {
                    return Ok("İzin talebiniz başarıyla oluşturuldu.");
                }

                return Problem("İzin talebi oluşturulurken bir hata meydana geldi.");

            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

    }
}
