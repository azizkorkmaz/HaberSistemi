using HaberSistemi.Core.Infrastructure;
using System.Collections.Generic;
using System.Linq;
using HaberSistemi.Data.Model;
using HaberSistemi.Data.DataContext;
using HaberSistemi.Core.Dto;
using System.Data.Entity.Migrations;

namespace HaberSistemi.Core.Repository
{
    public class HaberRepository : IHaberRepository
    {
        private readonly HaberContext _context = new HaberContext();

        public ServiceResult<HaberDTO> GetById(int id)
        {
            var haber = _context.Haber.FirstOrDefault(x => x.Id == id);

            if (haber != null)
            {
                var resDto = ModelToDTOHaber(haber);
                var etiketList = haber.Etiket.ToList();
                if (etiketList != null && etiketList.Any())
                {
                    var etiketDtoList = new List<EtiketDTO>();

                    foreach (var item in etiketList)
                    {
                        etiketDtoList.Add(new EtiketDTO
                        {
                            AktifMi = item.AktifMi,
                            EklenmeTarihi = item.EklenmeTarihi,
                            EtiketAdi = item.EtiketAdi,
                            Id = item.Id
                        });
                    }
                    resDto.Etiket = etiketDtoList;
                }

                return ServiceResult<HaberDTO>.Success(resDto);
            }

            return ServiceResult<HaberDTO>.Fail("Kayıt bulunamadı.");
        }

        public ServiceResult<HaberDTO> Get(HaberDTO dto)
        {
            var haberList = _context.Haber.Where(x => x.AktifMi);
            if (string.IsNullOrEmpty(dto.Aciklama))
            {
                haberList.Where(x => x.Aciklama == dto.Aciklama);
            }
            if (string.IsNullOrEmpty(dto.Baslik))
            {
                haberList.Where(x => x.Baslik == dto.Baslik);
            }
            if (haberList.Any())
            {
                return ServiceResult<HaberDTO>.Success(ModelToDTOHaber(haberList.FirstOrDefault()));
            }

            return ServiceResult<HaberDTO>.Fail("Aranan kriterlere uygun haber bulunamadı!");
        }

        public ServiceResult<List<HaberDTO>> GetHaberList()
        {
            var haberler = _context.Haber.ToList();
            if (haberler != null && haberler.Any())
            {

                var resList = new List<HaberDTO>();
                foreach (var item in haberler)
                {
                    resList.Add(ModelToDTOHaber(item));
                }

                return ServiceResult<List<HaberDTO>>.Success(resList);
            }
            return ServiceResult<List<HaberDTO>>.Fail("Listelenecek Haber Bulunmamaktadır");
        }

        public ServiceResult<List<HaberDTO>> GetMany(HaberDTO dto)
        {
            var haberList = _context.Haber.ToList();
            if (!string.IsNullOrEmpty(dto.Aciklama))
            {
                haberList.Where(x => x.Aciklama == dto.Aciklama);
            }
            if (!string.IsNullOrEmpty(dto.Baslik))
            {
                haberList.Where(x => x.Baslik == dto.Baslik);
            }

            if (dto.Okunma > 0)
            {
                haberList.Where(x => x.Okunma == dto.Okunma);
            }

            if (haberList.Any())
            {
                var resList = new List<HaberDTO>();
                foreach (var item in haberList)
                {
                    resList.Add(ModelToDTOHaber(item));
                }
                return ServiceResult<List<HaberDTO>>.Success(resList);
            }

            return ServiceResult<List<HaberDTO>>.Fail("Aranan kriterlere uygun haber bulunamadı!");
        }

        public ServiceResult<HaberDTO> Insert(HaberDTO data)
        {
            var haber = DTOToModelHaber(data);
            if (haber != null)
            {
                _context.Haber.Add(haber);
                _context.SaveChanges();
                return ServiceResult<HaberDTO>.Success(ModelToDTOHaber(haber));
            }
            return ServiceResult<HaberDTO>.Fail("Kayıt eklenmedi.");
        }

        public ServiceResult<bool> Update(HaberDTO obj)
        {
            if (obj != null && obj.Id > 0)
            {
                var haber = _context.Haber.FirstOrDefault(x => x.Id == obj.Id);

                if (haber != null)
                {
                    haber.Baslik = obj.Baslik;
                    haber.KisaAciklama = obj.KisaAciklama;
                    haber.Aciklama = obj.Aciklama;
                    haber.Okunma = obj.Okunma;
                    haber.Resim = obj.Resim;
                    haber.AktifMi = obj.AktifMi;
                    _context.Haber.AddOrUpdate(haber);
                    _context.SaveChanges();
                    return ServiceResult<bool>.Success(true);
                }
                return ServiceResult<bool>.Fail("Güncellenecek haber bulunamadı!");
            }

            return ServiceResult<bool>.Fail("Böyle bir haber kayıtlı değil!");
        }

        public ServiceResult<bool> Delete(int id)
        {
            var haber = _context.Haber.FirstOrDefault(x => x.Id == id);

            if (haber != null)
            {
                _context.Haber.Remove(haber);
                var res = _context.SaveChanges();
                return res > 0 ? ServiceResult<bool>.Success(true) : ServiceResult<bool>.Fail("Kayıt silinemedi.");
            }

            return ServiceResult<bool>.Fail("Kayıt bulunamadığından silinemedi.");
        }

        public HaberDTO ModelToDTOHaber(Haber haber)
        {
            if (haber != null)
            {
                var haberDTO = new HaberDTO
                {
                    Id = haber.Id,
                    Baslik = haber.Baslik,
                    Aciklama = haber.Aciklama,
                    KisaAciklama = haber.KisaAciklama,
                    EklenmeTarihi = haber.EklenmeTarihi,
                    AktifMi = haber.AktifMi,
                    Okunma = haber.Okunma,
                    Resim = haber.Resim,
                    KategoriID = haber.KategoriID,
                    KullaniciID = haber.KullaniciID,

                };

                return haberDTO;
            }

            return null;
        }

        public Haber DTOToModelHaber(HaberDTO dto)
        {
            var haber = new Haber
            {
                Id = dto.Id,
                Baslik = dto.Baslik,
                Aciklama = dto.Aciklama,
                KisaAciklama = dto.KisaAciklama,
                AktifMi = dto.AktifMi,
                Okunma = dto.Okunma,
                Resim = dto.Resim,
                KategoriID = dto.KategoriID,
                KullaniciID = dto.KullaniciID,

            };

            return haber;
        }
    }
}