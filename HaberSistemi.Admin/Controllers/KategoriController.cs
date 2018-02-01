using HaberSistemi.Admin.Class;
using HaberSistemi.Core.Dto;
using HaberSistemi.Core.Infrastructure;
using System;
using System.Web.Mvc;
using PagedList;
using System.Collections.Generic;

namespace HaberSistemi.Admin.Controllers
{
    public class KategoriController : Controller
    {
        #region Kategori
        private readonly IKategoriRepository _kategoriRepository;
        public KategoriController(IKategoriRepository kategoriRepository)
        {
            _kategoriRepository = kategoriRepository;
        }
        #endregion

        [HttpGet]
        public ActionResult Index(int Sayfa = 1)
        {
            return View(_kategoriRepository.GetKategoriList().Data.ToPagedList(Sayfa, 10));
        }

        [HttpGet]
        public ActionResult Ekle()
        {
            SetKategoriListele();
            return View(new KategoriDTO());
        }

        [HttpPost]
        public JsonResult Ekle(KategoriDTO kategori)
        {
            try
            {
                var res = _kategoriRepository.Insert(kategori);
                return Json(new ResultJson { Success = res.IsSuccess, Message = res.Message == null ? "Kategori Başarıyla Eklendi" : res.Message });
            }
            catch (Exception ex)
            {
                //loglama yapabiliriz
                return Json(new ResultJson { Success = false, Message = "Kategori eklerken hata oluştu!" });
            }
        }

        [HttpGet]
        public ActionResult Duzenle(int id)
        {
            var dbKategori = _kategoriRepository.GetKategoriById(id);
            if (dbKategori == null || dbKategori.IsSuccess == false)
            {
                throw new Exception(dbKategori != null ? dbKategori.Message : "Kategori bulunamadı.");
            }
            SetKategoriListele();
            return View(dbKategori.Data);
        }

        [HttpPost]
        public JsonResult Duzenle(KategoriDTO dto)
        {
            if (ModelState.IsValid)
            {
                _kategoriRepository.Update(dto);
                return Json(new ResultJson { Success = true, Message = "Düzenleme işlemi başarılı" });
            }
            return Json(new ResultJson { Success = false, Message = "Düzenleme sırasında bir hata oluştu!" });
        }

        public JsonResult Sil(int ID)
        {

            var res = _kategoriRepository.Delete(ID);
            if (res != null && res.IsSuccess)
            {
                return Json(new ResultJson { Success = true, Message = "Kategori silme işlemi başarılı" });
            }
            else
            {
                return Json(new ResultJson { Success = false, Message = "Kategori silme işlemi başarılı değil!" });
            }
        }

        public void SetKategoriListele()
        {
            var kategoriList = _kategoriRepository.GetKategoriList();
            if (kategoriList.IsSuccess)
            {

                ViewBag.Kategori = kategoriList.Data;
            }
            else
            {
                ViewBag.Kategori = new List<KategoriDTO>();
            }
        }
    }
}