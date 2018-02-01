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

namespace HaberSistemi.Admin.Controllers
{
    public class HaberController : Controller
    {
        // GET: Haber

        #region Veritabanı
        private readonly IKategoriRepository _kategoriRepository;
        private readonly IHaberRepository _haberRepository;
        private readonly IKullaniciRepository _kullaniciRepository;
        private readonly IResimRepository _resimRepository;
        private readonly IEtiketRepository _etiketRepository;
        #endregion

        public HaberController(IHaberRepository haberRepository, IKategoriRepository kategorirepository, IKullaniciRepository kullaniciRepository, IResimRepository resimRepository, IEtiketRepository etiketRepository)
        {
            _haberRepository = haberRepository;
            _kategoriRepository = kategorirepository;
            _kullaniciRepository = kullaniciRepository;
            _resimRepository = resimRepository;
            _etiketRepository = etiketRepository;
        }

        [LoginFilter]
        public ActionResult Index(int sayfa = 1)
        {
            return View(_haberRepository.GetHaberList().Data.ToPagedList(sayfa, 10));
        }

        [HttpGet]
        [LoginFilter]
        public ActionResult Ekle()
        {
            SetKategoriListele();
            return View();
        }

        #region Haber Düzenle
        [HttpGet]
        [LoginFilter]
        public ActionResult Duzenle(int id)
        {
            var gelenHaber = _haberRepository.GetById(id);

            #region Etiket Kısmı
            var gelenEtiket = gelenHaber.Data.Etiket.Select(x => x.EtiketAdi).ToArray();
            var haberEtiketModel = new HaberEtiketModelDTO
            {
                Haber = gelenHaber.Data,
                GelenEtiketler = gelenEtiket,
                Etiketler = _etiketRepository.GetEtiketList().Data
            };
            StringBuilder etiketBirlestir = new StringBuilder();
            foreach (var etiket in haberEtiketModel.GelenEtiketler)
            {
                etiketBirlestir.Append(etiket.ToString());
                etiketBirlestir.Append(",");
            }
            haberEtiketModel.EtiketAdi = etiketBirlestir.ToString();
            #endregion

            if (gelenHaber == null)
            {
                return ViewBag.Mesaj(gelenHaber.Message);
            }
            else
            {
                SetKategoriListele();
                return View(haberEtiketModel);
            }
        }

        [HttpPost]
        [LoginFilter]
        public ActionResult Duzenle(HaberDTO haber, HttpPostedFileBase VitrinResmi, IEnumerable<HttpPostedFileBase> DetayResim,string EtiketAd)
        {
            var gelenHaber = _haberRepository.GetById(haber.Id);
            gelenHaber.Data.KategoriID = haber.KategoriID;
            gelenHaber.Data.KisaAciklama = haber.KisaAciklama;
            gelenHaber.Data.Aciklama = haber.Aciklama;
            gelenHaber.Data.Baslik = haber.Baslik;
            gelenHaber.Data.AktifMi = haber.AktifMi;

            if (VitrinResmi != null)
            {
                string dosyaAdi = gelenHaber.Data.Resim;
                string dosyaYolu = Server.MapPath(dosyaAdi);
                FileInfo dosya = new FileInfo(dosyaYolu);
                if (dosya.Exists)
                {
                    dosya.Delete();
                }
                string file_name = Guid.NewGuid().ToString().Replace("-","");
                string uzanti = Path.GetExtension(Request.Files[0].FileName);
                string tamYol = "/External/Haber/" + file_name + uzanti;
                Request.Files[0].SaveAs(Server.MapPath(tamYol));
                gelenHaber.Data.Resim = tamYol;
            }

            string cokluResim = Path.GetExtension(Request.Files[1].FileName);
            if (cokluResim != "")
            {
                foreach (var dosya in DetayResim)
                {
                    string file_name = Guid.NewGuid().ToString().Replace("-", "");
                    string uzanti = Path.GetExtension(Request.Files[1].FileName);
                    string tamYol = "/External/Haber/" + file_name + uzanti;
                    dosya.SaveAs(Server.MapPath(tamYol));
                    var img = new ResimDTO
                    {
                        ResimUrl = tamYol
                    };
                    img.HaberID = gelenHaber.Data.Id;
                    img.EklenmeTarihi = DateTime.Now;
                    _resimRepository.Insert(img);
                }
                
            }
            _etiketRepository.EtiketEkle(haber.Id, EtiketAd);
            _haberRepository.Update(gelenHaber.Data);
            TempData["Bilgi"] = "Güncelleme İşleminiz Başarılı";
            return RedirectToAction("Index","Haber");
        }
        #endregion

        #region Haber Ekle
        [HttpPost]
        [LoginFilter]
        public ActionResult Ekle(HaberDTO haber, int kategoriID, HttpPostedFileBase vitrinResmi, IEnumerable<HttpPostedFileBase> DetayResim, string Etiket)
        {
            var sessionControl = HttpContext.Session["KullaniciEmail"];
            
            //if (ModelState.IsValid)
            //{
                var kullanici = _kullaniciRepository.GetById(Int32.Parse(sessionControl.ToString()));
                haber.KullaniciID = kullanici.Data.Id;
                haber.KategoriID = kategoriID;
                if (vitrinResmi != null)
                {
                    string dosyaAdi = Guid.NewGuid().ToString().Replace("-", "");
                    string uzanti = Path.GetExtension(Request.Files[0].FileName);
                    string tamYol = "/External/Haber/" + dosyaAdi + uzanti;
                    Request.Files[0].SaveAs(Server.MapPath(tamYol));
                    haber.Resim = tamYol;
                }

                var resHaber = _haberRepository.Insert(haber);
                if(resHaber != null && resHaber.IsSuccess)
                {
                    _etiketRepository.EtiketEkle(resHaber.Data.Id, Etiket);

                    string cokluResimler = Path.GetExtension(Request.Files[1].FileName);
                    if (cokluResimler != "")
                    {
                        foreach (var file in DetayResim)
                        {
                            if (file.ContentLength > 0)
                            {
                                string dosyaAdi = Guid.NewGuid().ToString().Replace("-", "");
                                string uzanti = Path.GetExtension(Request.Files[1].FileName);
                                string tamYol = "/External/Haber/" + dosyaAdi + uzanti;
                                file.SaveAs(Server.MapPath(tamYol));

                                var resim = new ResimDTO
                                {
                                    ResimUrl = tamYol
                                };

                                if (resHaber.Data != null)
                                {
                                    resim.HaberID = resHaber.Data.Id;
                                    _resimRepository.Insert(resim);
                                }
                            }
                        }

                    }
                    TempData["Bilgi"] = "Haber ekleme işleminiz başarılı";
                    return RedirectToAction("Index", "Haber");

                }

                TempData["Bilgi"] = "Haber ekleme işleminiz başarısız";
            //}
            return View();
        }
        #endregion

        #region Haber Sil
        public ActionResult Sil(int id)
        {
            var dbHaber = _haberRepository.GetById(id);
            //var dbDetayResim = _haberRepository.GetMany(x=>x.HaberID=id);
            var dbDetayResim = _resimRepository.GetMany(new ResimDTO
            {
                HaberID = dbHaber.Data.Id
                  
            });
            if (dbHaber == null)
            {
                throw new Exception("Haber Bulunamadı!");
            }
            string fileName = dbHaber.Data.Resim;
            string path = Server.MapPath(fileName);
            FileInfo file = new FileInfo(path);
            if (file.Exists)//dosyanın fiziksel varlığı kontrol ediliyor varsa sil
            {
                file.Delete();
            }
            if (dbDetayResim != null)
            {
                foreach (var item in dbDetayResim.Data)
                {
                    string detayPath = Server.MapPath(item.ResimUrl);
                    FileInfo files = new FileInfo(detayPath);
                    if (files.Exists)
                    {
                        files.Delete();
                    }
                }
            }
            _haberRepository.Delete(id);
            TempData["Bilgi"] = "Haber başarılı bir şekilde silindi";
            return RedirectToAction("Index", "Haber");
        }
        #endregion

        #region Resim Sil
        public ActionResult ResimSil(int id)
        {
            var dbResim = _resimRepository.GetById(id);
            if (dbResim == null)
            {
                throw new Exception("Resim bulunamadı!");
            }
            string file_name = dbResim.Data.ResimUrl;
            string path = Server.MapPath(file_name);
            FileInfo file = new FileInfo(path);
            if (file.Exists)
            {
                file.Delete();
            }
            _resimRepository.Delete(id);
            TempData["Bilgi"] = "Resim silme İşlemi Başarılı";
            return RedirectToAction("Index", "Haber");
        }
        #endregion

        #region Haber Aktif / Pasif Yapar
        public ActionResult Onay(int id)
        {
            var gelenHaber = _haberRepository.GetById(id);
            if (gelenHaber != null && gelenHaber.Data.AktifMi == true)
            {
                gelenHaber.Data.AktifMi = false;
                _haberRepository.Update(gelenHaber.Data);
                TempData["Bilgi"] = "Onay işlemi başarılı";
                return RedirectToAction("Index", "Haber");
            }
            else if (gelenHaber != null && gelenHaber.Data.AktifMi == false)
            {
                gelenHaber.Data.AktifMi = true;
                _haberRepository.Update(gelenHaber.Data);
                TempData["Bilgi"] = "Onay işlemi başarılı";
                return RedirectToAction("Index", "Haber");
            }
            return View();
        }
        #endregion

        #region Set Kategori Listele
        public void SetKategoriListele(object kategori = null)
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
        #endregion
    }
}