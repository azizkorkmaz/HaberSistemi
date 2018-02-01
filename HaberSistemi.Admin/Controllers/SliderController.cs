using HaberSistemi.Admin.Class;
using HaberSistemi.Admin.CustomFilter;
using HaberSistemi.Core.Dto;
using HaberSistemi.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.IO;
using System.Linq;
using System.Text;
using HaberSistemi.Data.DataContext;
using HaberSistemi.Admin.Helper;

namespace HaberSistemi.Admin.Controllers
{
    public class SliderController : Controller
    {
        private readonly HaberContext _context = new HaberContext();
        private readonly ISliderRepository _sliderRepository;

        public SliderController(ISliderRepository sliderRepository)
        {
            _sliderRepository = sliderRepository;
        }
        // GET: Slider
        public ActionResult Index( int sayfa=1)
        {
            return View(_sliderRepository.GetSliderList().Data.ToPagedList(sayfa, 10));
        }

        #region Slider Ekle
        [HttpGet]
        [LoginFilter]
        public ActionResult Ekle()
        {
            return View();
        }

        [HttpPost]
        [LoginFilter]
        public JsonResult Ekle(SliderDTO slider, HttpPostedFileBase ResimURL)
        {
            if (ModelState.IsValid)
            {
                if (ResimURL.ContentLength > 0)
                {
                    string dosya = Guid.NewGuid().ToString().Replace("-", "");
                    string uzanti = Path.GetExtension(Request.Files[0].FileName);
                    string resimYolu = "/External/Slider/" + dosya + uzanti;

                    ResimURL.SaveAs(Server.MapPath(resimYolu));
                    slider.ResimURL = resimYolu;
                }
                var res = _sliderRepository.Insert(slider);
                try
                {
                    
                    return Json(new ResultJson { Success = res.IsSuccess, Message = res.Message == null ? "Slider Başarıyla Eklendi" : res.Message });
                }
                catch (Exception ex)
                {

                    return Json(new ResultJson { Success = res.IsSuccess, Message = res.Message == null ? "Slider Eklenemedi!" : res.Message });
                }

            }
            return Json(new ResultJson { Success=false,Message="Slider ekleme işlemi başarısız!"});
        }
        #endregion

        #region Slider Düzenle
        [HttpGet]
        [LoginFilter]
        public ActionResult Duzenle(int id)
        {
            var sliderVarmi = _sliderRepository.GetById(id);

            if (sliderVarmi != null)
            {
                return View(sliderVarmi);
            }
            return View();
        }

        [HttpPost]
        [LoginFilter]
        public JsonResult Duzenle(SliderDTO slider, HttpPostedFileBase ResimURL)
        {
          
            if (ModelState.IsValid)
            {
                var dbSlider = _sliderRepository.GetById(slider.Id);
                dbSlider.Data.Baslik = slider.Baslik;
                dbSlider.Data.Aciklama = slider.Aciklama;
                dbSlider.Data.AktifMi = slider.AktifMi;
                dbSlider.Data.URL = slider.URL;
                if (ResimURL != null && ResimURL.ContentLength > 0)
                {
                    if (dbSlider.Data.ResimURL != null)
                    {
                        string url = dbSlider.Data.ResimURL;
                        string resimPath = Server.MapPath(url);
                        FileInfo files = new FileInfo(resimPath);
                        if (files.Exists)
                        {
                            files.Delete();
                        }

                    }
                    ResimYukle.Resim(slider, ResimURL);
                    dbSlider.Data.ResimURL = slider.ResimURL;
                }

                try
                {
                   var res= _sliderRepository.Update(slider);
                    return Json(new ResultJson { Success = res.IsSuccess, Message = res.Message == null ? "Slider Başarıyla Güncellenmiştir" : res.Message });

                }
                catch (Exception ex)
                {
                    //logyaz();
                    return Json(new ResultJson { Success = false, Message = "Slider Güncellenemedi!" });
                }
            }
            return Json(new ResultJson { Success = false, Message = "Güncelleme esnasında beklenmeyen bir hata oluştu!" });
        }
        #endregion

        #region Slider Sil
        public JsonResult Sil( SliderDTO slider)
        {
            var dbSlider = _sliderRepository.GetById(slider.Id);
            if (dbSlider == null)
            {
                return Json(new ResultJson { Success = false, Message = "Slider bulunamadı!" });
            }
            try
            {

                if (dbSlider.Data.ResimURL != null)
                {
                    string resimUrl = dbSlider.Data.ResimURL;
                    string resimPath = Server.MapPath(resimUrl);
                    FileInfo files = new FileInfo(resimPath);
                    if (files.Exists)
                    {
                        files.Delete();
                    }
                }
                var res = _sliderRepository.Delete(slider.Id);
                return Json(new ResultJson { Success = res.IsSuccess, Message = res.Message == null ? "Slider silme işlemi başarılı" : res.Message });
            }
            catch (Exception ex)
            {

                return Json(new ResultJson { Success = false, Message="Slider silme işlemi başarısız!" });
            }
            
        }
        #endregion
    }
}