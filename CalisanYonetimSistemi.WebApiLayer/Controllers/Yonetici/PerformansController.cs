using AutoMapper;
using CalisanYonetimSistemi.BussinesLayer.Abstract;
using CalisanYonetimSistemi.EntityLayer.Concrete;
using CalisanYonetimSistemi.EntityLayer.Concrete.VMs.YoneticiVms.PerformansVm;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CalisanYonetimSistemi.WebApiLayer.Controllers.Yonetici
{
    [Authorize(Roles = "yonetici")]
    [Route("api/yonetici/[controller]")]
    [ApiController]
    public class PerformansController(IPerformansDegerlendirmeManager performansDegerlendirmeManager, IRaporManager raporManager, IUserManager userManager, IMapper mapper) : ControllerBase
    {
        private readonly IPerformansDegerlendirmeManager performansDegerlendirmeManager = performansDegerlendirmeManager;
        private readonly IRaporManager raporManager = raporManager;
        private readonly IUserManager userManager = userManager;
        private readonly IMapper mapper = mapper;

        [HttpPost("[action]")]
        public async Task<IActionResult> PerformansDegerlendirmeInsert(PerformansDegerlendirmeInsertVm performansVm)
        {
            if (!ModelState.IsValid)
            {
                var errorMessage = ModelState.Values.SelectMany(p => p.Errors.Select(e => e.ErrorMessage));
                return BadRequest(errorMessage);
            }
            try
            {
                PerformansDegerlendirme performansDegerlendirme = mapper.Map<PerformansDegerlendirme>(performansVm);
                PerformanTipi performanTipi = (PerformanTipi)performansVm.PerformansTipi;
                performansDegerlendirme.PerformansTipi = performanTipi;

                if (await performansDegerlendirmeManager.Insert(performansDegerlendirme) > 0)
                {
                    User user = await userManager.GetByPK(performansDegerlendirme.UserId);

                    var toplamPuan = performansDegerlendirme.PerformansTipi switch
                    {
                        PerformanTipi.Analitiklik => user.ToplamAnalitiklikPuan = user.ToplamAnalitiklikPuan + performansDegerlendirme.PerformansPuani,
                        PerformanTipi.TakimCalismasi => user.ToplamTakimCalismasiPuan = user.ToplamTakimCalismasiPuan + performansDegerlendirme.PerformansPuani,
                        PerformanTipi.Uretkenlik => user.ToplamUretkenlikPuan = user.ToplamUretkenlikPuan + performansDegerlendirme.PerformansPuani,
                        _ => 0 // swith in diger durumlari icin
                    };

                    int sonuc = await userManager.update(user);

                    if (sonuc > 0)
                    {
                        return Ok("Performans değerlendirme kaydı başarıyla gerçekleşti.");
                    }

                }
                return Problem("Değerlendirme sırasında bir hata oluştu.");

            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }

        }

        [HttpGet("[action]")]
        public async Task<IActionResult> PerformansRaporGet()
        {
            if (!ModelState.IsValid)
            {
                var errorMessage = ModelState.Values.SelectMany(p => p.Errors.Select(e => e.ErrorMessage));
                return BadRequest(errorMessage);
            }
            try
            {
                ICollection<Rapor> performansRaporlari = await raporManager.GetAll();

                return Ok(performansRaporlari);

            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }

        }



        [HttpGet("[action]")]
        public async Task<IActionResult> PerformansDegerlendirmeGet()
        {
            if (!ModelState.IsValid)
            {
                var errorMessage = ModelState.Values.SelectMany(p => p.Errors.Select(e => e.ErrorMessage));
                return BadRequest(errorMessage);
            }
            try
            {
                ICollection<PerformansDegerlendirme> performansDegerlendirmeler = await performansDegerlendirmeManager.GetAll();

                return Ok(performansDegerlendirmeler);

            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }

        }

        [HttpGet("[action]")]
        public async Task<IActionResult> DepartmanOrtPerPuan()
        {
            if (!ModelState.IsValid)
            {
                var errorMessage = ModelState.Values.SelectMany(p => p.Errors.Select(e => e.ErrorMessage));
                return BadRequest(errorMessage);
            }
            try
            {
                var ortPuanlar = await performansDegerlendirmeManager.DepartmanOrtPerformansPuan();

                return Ok(ortPuanlar);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpGet("[action]/{year}")]
        public async Task<IActionResult> DepartmanOrtPerTrendi(int year)
        {
            if (!ModelState.IsValid)
            {
                var errorMessage = ModelState.Values.SelectMany(p => p.Errors.Select(e => e.ErrorMessage));
                return BadRequest(errorMessage);
            }
            try
            {
                var departmanlarınAylıkPerTrendi = await performansDegerlendirmeManager.DepartmanOrtPerformansAylikTrendi(year);

                return Ok(departmanlarınAylıkPerTrendi);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpGet("[action]/{year}")]
        public async Task<IActionResult> BireyselOrtPerTrendi(int year)
        {
            if (!ModelState.IsValid)
            {
                var errorMessage = ModelState.Values.SelectMany(p => p.Errors.Select(e => e.ErrorMessage));
                return BadRequest(errorMessage);
            }
            try
            {
                var bireyeselAylıkPerTrendi = await performansDegerlendirmeManager.BireyselOrtPerformansAylikTrendi(year);

                return Ok(bireyeselAylıkPerTrendi);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpGet("[action]/{year}")]
        public async Task<IActionResult> BireyselPerformansAnomali(int year)
        {
            if (!ModelState.IsValid)
            {
                var errorMessage = ModelState.Values.SelectMany(p => p.Errors.Select(e => e.ErrorMessage));
                return BadRequest(errorMessage);
            }
            try
            {
                var bireyeselPerAnomali = await performansDegerlendirmeManager.BireyselPerformansDegisimAnomali(year);

                return Ok(bireyeselPerAnomali);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpGet("[action]/{year}")]
        public async Task<IActionResult> DepartmanPerformansAnomali(int year)
        {
            if (!ModelState.IsValid)
            {
                var errorMessage = ModelState.Values.SelectMany(p => p.Errors.Select(e => e.ErrorMessage));
                return BadRequest(errorMessage);
            }
            try
            {
                var departmanPerAnomali = await performansDegerlendirmeManager.DepartmanPerformansDegisimAnomali(year);

                return Ok(departmanPerAnomali);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}
