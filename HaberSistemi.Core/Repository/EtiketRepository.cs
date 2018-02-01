using HaberSistemi.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using HaberSistemi.Core.Dto;
using HaberSistemi.Data.DataContext;
using HaberSistemi.Data.Model;
using System.Data.Entity.Migrations;

namespace HaberSistemi.Core.Repository
{
    public class EtiketRepository : IEtiketRepository
    {
        private readonly HaberContext _context = new HaberContext();

        public ServiceResult<EtiketDTO> GetById(int id)
        {
            var etiket = _context.Etiket.FirstOrDefault(x => x.Id == id);
            if (etiket != null)
            {
                return ServiceResult<EtiketDTO>.Success(ModelToDtoEtiket(etiket));
            }
            return ServiceResult<EtiketDTO>.Fail("Kayıt bulunamadı");
        }

        public ServiceResult<EtiketDTO> Get(EtiketDTO dto)
        {
            var etiketler = _context.Etiket.Where(x => x.AktifMi);
            if (!string.IsNullOrEmpty(dto.EtiketAdi))
            {
                etiketler.Where(x => x.EtiketAdi == dto.EtiketAdi);
            }
            if (dto.Haber != null)
            {
                etiketler.Where(x => x.Haber == dto.Haber);
            }
            if (etiketler.Any())
            {
                return ServiceResult<EtiketDTO>.Success(ModelToDtoEtiket(etiketler.FirstOrDefault()));
            }
            return ServiceResult<EtiketDTO>.Fail("Aranan kritere uygun etiket bulunamadı");
        }

        public ServiceResult<List<EtiketDTO>> GetEtiketList()
        {
            var etiketList = _context.Etiket.ToList();
            if (etiketList != null && etiketList.Any())
            {
                var resList = new List<EtiketDTO>();
                foreach (var item in etiketList)
                {
                    resList.Add(ModelToDtoEtiket(item));
                }
                return ServiceResult<List<EtiketDTO>>.Success(resList);
            }
            return ServiceResult<List<EtiketDTO>>.Fail("Listelenecek etiket bulunamadı!");
        }

        public ServiceResult<List<EtiketDTO>> GetMany(EtiketDTO dto)
        {
            var etiketList = _context.Etiket.ToList();
            if (!string.IsNullOrEmpty(dto.EtiketAdi))
            {
                etiketList.Where(x => x.EtiketAdi == dto.EtiketAdi);
            }
            if (dto.Haber != null)
            {
                etiketList.Where(x => x.Haber == dto.Haber);
            }
            if (etiketList.Any())
            {
                var resList = new List<EtiketDTO>();
                foreach (var item in etiketList)
                {
                    resList.Add(ModelToDtoEtiket(item));
                }
                return ServiceResult<List<EtiketDTO>>.Success(resList);
            }
            return ServiceResult<List<EtiketDTO>>.Fail("Aranan kritere uygun etiket bulunamadı!");
        }

        public ServiceResult<EtiketDTO> Insert(EtiketDTO data)
        {
            var yeniEtiket = DtoToModelEtiket(data);
            if (yeniEtiket != null)
            {
                _context.Etiket.Add(yeniEtiket);
                _context.SaveChanges();
                return ServiceResult<EtiketDTO>.Success(ModelToDtoEtiket(yeniEtiket));
            }
            return ServiceResult<EtiketDTO>.Fail("Kayıt eklenmedi!");
        }

        public ServiceResult<bool> Update(EtiketDTO obj)
        {
            if (obj != null && obj.Id > 0)
            {
                var etiket = _context.Etiket.FirstOrDefault(x => x.Id == obj.Id);
                if (etiket != null)
                {
                    etiket.EtiketAdi = obj.EtiketAdi;
                    etiket.Haber = obj.Haber;
                    etiket.AktifMi = obj.AktifMi;
                    etiket.EklenmeTarihi = obj.EklenmeTarihi;
                    _context.Etiket.AddOrUpdate(etiket);
                    _context.SaveChanges();
                    return ServiceResult<bool>.Success(true);
                }
                return ServiceResult<bool>.Fail("Güncellenecek etiket bulunamadı!");
            }
            return ServiceResult<bool>.Fail("Böyle bir kayıt bulunamadı!");
        }

        public ServiceResult<bool> Delete(int id)
        {
            var etiket = _context.Etiket.FirstOrDefault(x => x.Id == id);
            if (etiket != null)
            {
                _context.Etiket.Remove(etiket);
                var res = _context.SaveChanges();
                return res > 0 ? ServiceResult<bool>.Success(true) : ServiceResult<bool>.Fail("Kayıt silinemedi!");
            }
            return ServiceResult<bool>.Fail("Kayıt bulunamasığından silinemedi!");
        }

        public IQueryable<Etiket> Etiketler(string[] etiketler)//etiketdto olunca hata sebep?
        {
            IQueryable<Etiket> yeniEtiket = _context.Etiket.Where(x => etiketler.Contains(x.EtiketAdi));
            return yeniEtiket;
        }

        public void EtiketEkle(int HaberID, string etiket)
        {
            if (!string.IsNullOrEmpty(etiket))
            {
                string[] etikets = etiket.Split(',');
                foreach (var tag in etikets)
                {
                    var modelEtiket = _context.Etiket.Where(x => x.EtiketAdi == tag.ToLower().Trim()).FirstOrDefault();
                    if (modelEtiket == null)
                    {
                        var newEtiket = new Etiket
                        {
                            EklenmeTarihi = DateTime.Now,
                            EtiketAdi = tag,
                            AktifMi = true
                        };

                        _context.Etiket.Add(newEtiket);
                        _context.SaveChanges();
                    }
                }
                this.HaberEtiketEkle(HaberID, etikets);
            }
        }

        public void HaberEtiketEkle(int HaberID, string[] etiketler)
        {
            var haber = _context.Haber.FirstOrDefault(x => x.Id == HaberID);
            var gelenEtiket = Etiketler(etiketler);
            haber.Etiket.Clear();
            gelenEtiket.ToList().ForEach(etiket => haber.Etiket.Add(etiket));//foreach in bu yazılımını araştır
            _context.SaveChanges();
        }

        public EtiketDTO ModelToDtoEtiket(Etiket etiket)
        {
            if (etiket != null)
            {
                var etiketDto = new EtiketDTO
                {
                    EtiketAdi = etiket.EtiketAdi,
                    Haber = etiket.Haber,
                    AktifMi = etiket.AktifMi,
                    EklenmeTarihi = etiket.EklenmeTarihi,
                    Id = etiket.Id
                };
                return etiketDto;
            }
            return null;
        }

        public Etiket DtoToModelEtiket(EtiketDTO dto)
        {
            if (dto != null)
            {
                var etiket = new Etiket
                {
                    EtiketAdi = dto.EtiketAdi,
                    Haber = dto.Haber,
                    Id = dto.Id,
                    EklenmeTarihi = dto.EklenmeTarihi,
                    AktifMi = dto.AktifMi
                };
                return etiket;
            }
            return null;
        }


    }
}
