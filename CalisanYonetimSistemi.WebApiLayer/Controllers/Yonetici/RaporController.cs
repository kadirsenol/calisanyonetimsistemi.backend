using CalisanYonetimSistemi.BussinesLayer.Abstract;
using CalisanYonetimSistemi.EntityLayer.Concrete;
using CalisanYonetimSistemi.EntityLayer.Concrete.VMs.YoneticiVms.RaporVm;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CalisanYonetimSistemi.WebApiLayer.Controllers.Yonetici
{
    [Authorize(Roles = "yonetici")]
    [Route("api/yonetici/[controller]")]
    [ApiController]
    public class RaporController(IRaporManager raporManager) : ControllerBase
    {
        private readonly IRaporManager raporManager = raporManager;

        [HttpPost("[action]")]
        public async Task<IActionResult> CreateRapor(RaporInsertVm performansRaporInsertVm)
        {
            if (!ModelState.IsValid)
            {
                var errorMessage = ModelState.Values.SelectMany(p => p.Errors.Select(e => e.ErrorMessage));
                return BadRequest(errorMessage);
            }
            try
            {
                RaporTuru raporTuru = (RaporTuru)Enum.Parse(typeof(RaporTuru), performansRaporInsertVm.RaporTuru.ToString());

                Rapor rapor = new Rapor()
                {
                    BaslangicTarih = DateOnly.Parse(performansRaporInsertVm.BaslangicTarih),
                    BitisTarih = DateOnly.Parse(performansRaporInsertVm.BitisTarih),
                    UserId = performansRaporInsertVm.UserId,
                    RaporTuru = raporTuru,
                    Aciklama = performansRaporInsertVm.Aciklama

                };

                if (await raporManager.Insert(rapor) > 0)
                {
                    return Ok("İlgili çalışan için belirli bir döneme ait performans raporu kaydedilmiştir.");
                }
                return Problem("Performans raporu kayıt sırasında beklenmedik bir hata meydana geldi.");
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}
