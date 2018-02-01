using HaberSistemi.Core.Dto;
using HaberSistemi.Core.Infrastructure;
using System.Web.Mvc;

namespace HaberSistemi.Admin.Controllers
{
    public class AccountController : Controller
    {
        #region kullanıcı
        private readonly IKullaniciRepository _kullaniciRepository;
        public AccountController(IKullaniciRepository kullaniciRepository)
        {
            _kullaniciRepository = kullaniciRepository;
        }
        #endregion
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(KullaniciDTO kullanici)
        {
            var kullaniciVarmi = _kullaniciRepository.LoginKullanici(new KullaniciDTO
            {
                Email = kullanici.Email,
                Sifre = kullanici.Sifre
            });

            if (kullaniciVarmi != null && kullaniciVarmi.IsSuccess )
            {
                var kullaniciDto = kullaniciVarmi.Data;
                if (kullaniciDto.Rol.RolAdi == "Admin")
                {
                    Session["KullaniciEmail"] = kullaniciDto.Id;
                    Session["KullaniciSifre"] = kullaniciDto.Sifre;
                    return RedirectToAction("Index", "Home");
                }
                ViewBag.Mesaj = "Yetkisiz kullanıcı";
                return View();
            }
            ViewBag.Mesaj = kullaniciVarmi.Message;
            return View();
        }
    }
}