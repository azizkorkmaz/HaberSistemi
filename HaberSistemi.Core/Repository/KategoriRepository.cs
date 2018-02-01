using HaberSistemi.Core.Infrastructure;
using System.Collections.Generic;
using HaberSistemi.Core.Dto;
using HaberSistemi.Data.DataContext;
using HaberSistemi.Data.Model;
using System.Linq;
using System.Data.Entity.Migrations;

namespace HaberSistemi.Core.Repository
{
    public class KategoriRepository : IKategoriRepository
    {
        private readonly HaberContext _context = new HaberContext();

        public ServiceResult<KategoriDTO> GetKategoriById(int id)
        {
            var kategori = _context.Kategori.FirstOrDefault(x => x.Id == id);
            if (kategori != null)
            {
                return ServiceResult<KategoriDTO>.Success(ModelToDTOKategori(kategori));
            }

            return ServiceResult<KategoriDTO>.Fail("Kayıt bulunamadı.");
        }

        public ServiceResult<KategoriDTO> Get(KategoriDTO kategori)
        {
            var kategoriList = _context.Kategori.Where(x => x.AktifMi);

            if (!string.IsNullOrEmpty(kategori.KategoriAdi))
            {
                kategoriList.Where(x => x.KategoriAdi == kategori.KategoriAdi);
            }

            if (!string.IsNullOrEmpty(kategori.URL))
            {
                kategoriList.Where(x => x.URL == kategori.URL);
            }

            if (kategori.ParentID>0)
            {
                kategoriList.Where(x => x.ParentID == kategori.ParentID);
            }
            if (kategoriList.Any())
            {
                return ServiceResult<KategoriDTO>.Success(ModelToDTOKategori(kategoriList.FirstOrDefault()));
            }

            return ServiceResult<KategoriDTO>.Fail("Aranan Kriterlere Uygun Kayıtlı Kategori Bulunamadı");
        }

        public ServiceResult<List<KategoriDTO>> GetKategoriList()
        {
            var kategoriler = _context.Kategori.ToList();

            if (kategoriler!=null && kategoriler.Any())
            {
                var resListKategori = new List<KategoriDTO>();

                foreach (var item in kategoriler)
                {
                    var kategori = ModelToDTOKategori(item);
                    if (kategori.ParentID>0)
                    {
                        kategori.ParentAdi = _context.Kategori.Where(x => x.Id == kategori.ParentID).FirstOrDefault()?.KategoriAdi;
                    }
                    resListKategori.Add(kategori);

                }
                return ServiceResult<List<KategoriDTO>>.Success(resListKategori);
            }
            return ServiceResult<List<KategoriDTO>>.Fail("Listelenecek kategori bulunamadı");
        }

        public ServiceResult<List<KategoriDTO>> SearchKategori(KategoriDTO kategori)
        {
            var kategoriList = _context.Kategori.Where(x => x.AktifMi).ToList();

            if (!string.IsNullOrEmpty(kategori.KategoriAdi))
            {
                kategoriList.Where(x => x.KategoriAdi == kategori.KategoriAdi);
            }
            if (!string.IsNullOrEmpty(kategori.URL))
            {
                kategoriList.Where(x => x.URL == kategori.URL);
            }
            if (kategori.ParentID > 0)
            {
                kategoriList.Where(x => x.ParentID == kategori.ParentID);
            }
            if (kategoriList.Any())
            {
                var resList = new List<KategoriDTO>();
                foreach (var item in kategoriList)
                {
                    resList.Add(ModelToDTOKategori(item));
                }
                return ServiceResult<List<KategoriDTO>>.Success(resList);
            }
            return ServiceResult<List<KategoriDTO>>.Fail("Aranan kritere uygun kategori bulunamadı!");
        }

        public ServiceResult<KategoriDTO> Insert(KategoriDTO data)
        {
            var yeniKategori = DTOToModelKategori(data);
            if (yeniKategori!=null)
            {
                _context.Kategori.Add(yeniKategori);
                var res = _context.SaveChanges();
                return res > 0 ? ServiceResult<KategoriDTO>.Success(ModelToDTOKategori(yeniKategori)) : ServiceResult<KategoriDTO>.Fail("Kayıt Eklenmedi!");
            }

            return ServiceResult<KategoriDTO>.Fail("Eklenecek kayıt bulunamadı!");
        }

        public ServiceResult<bool> Update(KategoriDTO obj)
        {
            if (obj!=null && obj.Id>0)
            {
                var kategori = _context.Kategori.FirstOrDefault(x => x.Id == obj.Id);
                if (kategori!=null)
                {
                    var updatedModel = DTOToModelKategori(obj);
                    _context.Kategori.AddOrUpdate(updatedModel);
                   var res = _context.SaveChanges();
                    return res >= 0 ? ServiceResult<bool>.Success(true) : ServiceResult<bool>.Fail("Kategori Güncellenemedi!");
                }
                return ServiceResult<bool>.Fail("Güncellenecek kategori bulunamadı!");
            }
            return ServiceResult<bool>.Fail("Böyle bir kategori kayıtlı değil!");
        }

        public ServiceResult<bool> Delete(int id)
        {
            var kategori = _context.Kategori.FirstOrDefault(x => x.Id == id);
            if (kategori!=null)
            {
                _context.Kategori.Remove(kategori);
               var res= _context.SaveChanges();
                return res > 0 ? ServiceResult<bool>.Success(true) : ServiceResult<bool>.Fail("Kayıt Silinmedi!");
            }
            return ServiceResult<bool>.Fail("Kayıt bulunamadığından silinemedi!");
        }

        public KategoriDTO ModelToDTOKategori(Kategori kategori)
        {
            if (kategori != null)//az önce sildiğimni ben ekledim paid gelsin diye
            {
                var kategoriDTO = new KategoriDTO
                {
                    Id = kategori.Id,
                    KategoriAdi = kategori.KategoriAdi,
                    URL = kategori.URL,
                    ParentID = kategori.ParentID,
                    AktifMi = kategori.AktifMi
                };
                
                return kategoriDTO;
            }

            return null;
        }

        public Kategori DTOToModelKategori(KategoriDTO dto)
        {
            var kategori = new Kategori
            {
                Id = dto.Id,
                KategoriAdi = dto.KategoriAdi,
                URL = dto.URL,
                ParentID = dto.ParentID,
                AktifMi = dto.AktifMi
            };

            return kategori;
        }

    }
}
