using CalisanYonetimSistemi.BussinesLayer.Abstract;
using CalisanYonetimSistemi.EntityLayer.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CalisanYonetimSistemi.WebApiLayer.Controllers
{
    [Authorize(Roles = "yonetici,calisan")]
    [Route("api/[controller]")]
    [ApiController]
    public class DashBoardController(IHttpContextAccessor httpContextAccessor, IUserManager userManager) : ControllerBase
    {
        private readonly IHttpContextAccessor httpContextAccessor = httpContextAccessor;
        private readonly IUserManager userManager = userManager;

        [HttpGet("[action]")]
        public async Task<IActionResult> GetUser()
        {
            if (!ModelState.IsValid)
            {
                var errorMessage = ModelState.Values.SelectMany(p => p.Errors.Select(e => e.ErrorMessage));
                return BadRequest(errorMessage);
            }
            try
            {
                int userId = int.Parse(httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(p => p.Type == "UserId").Value);
                User user = await userManager.GetByPK(userId);
                if (user == null)
                {
                    return Problem("Olağan dışı bir durum algılandı lütfen çıkış yapıp tekrar giriniz.");
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }

}
