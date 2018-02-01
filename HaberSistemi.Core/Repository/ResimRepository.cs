using HaberSistemi.Core.Infrastructure;
using System.Collections.Generic;
using HaberSistemi.Data.DataContext;
using HaberSistemi.Core.Dto;
using System.Linq;
using HaberSistemi.Data.Model;
using System.Data.Entity.Migrations;

namespace HaberSistemi.Core.Repository
{
    public class ResimRepository : IResimRepository
    {

        private readonly HaberContext _context = new HaberContext();

        public ServiceResult<ResimDTO> GetById(int id)
        {
            var resim = _context.Resim.FirstOrDefault(x => x.Id == id);
            if (resim != null)
            {
                return ServiceResult<ResimDTO>.Success(ModelToResimDTO(resim));
            }
            return ServiceResult<ResimDTO>.Fail("Kayıt bulunamadı!");
        }

        public ServiceResult<ResimDTO> Get(ResimDTO resim)
        {
            var resimList = _context.Resim.Where(x => x.AktifMi);
            if (string.IsNullOrEmpty(resim.ResimUrl))
            {
                resimList.Where(x => x.ResimUrl == resim.ResimUrl);
            }
            if (resimList.Any())
            {
                return ServiceResult<ResimDTO>.Success(ModelToResimDTO(resimList.SingleOrDefault()));
            }
            return ServiceResult<ResimDTO>.Fail("Aranan kriterlere uygun kayıtlı resim bulunamadı!");
        }

        public ServiceResult<List<ResimDTO>> GetAll()
        {
            var resimler = _context.Resim.Where(x => x.AktifMi == true).ToList();
            if (resimler != null && resimler.Any())
            {
                var resResimList = new List<ResimDTO>();
                foreach (var item in resimler)
                {
                    resResimList.Add(ModelToResimDTO(item));
                }
                return ServiceResult<List<ResimDTO>>.Success(resResimList);
            }
            return ServiceResult<List<ResimDTO>>.Fail("Listenecek resim bulunamadı");
        }

        public ServiceResult<List<ResimDTO>> GetMany(ResimDTO resim)
        {
            var resimList = _context.Resim.ToList();
            if (!string.IsNullOrEmpty(resim.ResimUrl))
            {
                resimList.Where(x => x.ResimUrl == resim.ResimUrl);
            }
            if (resimList.Any())
            {
                var resList = new List<ResimDTO>();
                foreach (var item in resimList)
                {
                    resList.Add(ModelToResimDTO(item));
                }
                return ServiceResult<List<ResimDTO>>.Success(resList);
            }
            return ServiceResult<List<ResimDTO>>.Fail("Aranan kritere uygun resim buunamadı!");
        }

        public ServiceResult<ResimDTO> Insert(ResimDTO dto)
        {
            var yeniResim = DTOToResimModel(dto);
            if (yeniResim != null)
            {
                _context.Resim.Add(yeniResim);
                _context.SaveChanges();
                return ServiceResult<ResimDTO>.Success(ModelToResimDTO(yeniResim));
            }
            return ServiceResult<ResimDTO>.Fail("Kayıt eklenemedi!");
        }

        public ServiceResult<bool> Update(ResimDTO obj)
        {
            if (obj != null && obj.Id > 0)
            {
                var resim = _context.Resim.FirstOrDefault(x => x.Id == obj.Id);
                if (resim != null)
                {
                    ModelToResimDTO(resim);
                    _context.Resim.AddOrUpdate(resim);
                    _context.SaveChanges();
                    return ServiceResult<bool>.Success(true);
                }
                return ServiceResult<bool>.Fail("Kayıt güncellenemedi!");
            }
            return ServiceResult<bool>.Fail("Kayıt bulunamadığından güncellenemedi");
        }

        public ServiceResult<bool> Delete(int id)
        {
            var resim = _context.Resim.FirstOrDefault(x => x.Id == id);
            if (resim != null)
            {
                _context.Resim.Remove(resim);
                var res = _context.SaveChanges();
                return res > 0 ? ServiceResult<bool>.Success(true) : ServiceResult<bool>.Fail("Kayıt silinmeedi!");
            }
            return ServiceResult<bool>.Fail("Kayıt bulunamadığından silinemedi!");
        }

        public ResimDTO ModelToResimDTO(Resim resim)
        {
            if (resim != null)
            {
                var resimDto = new ResimDTO()
                {
                    Id = resim.Id,
                    ResimUrl = resim.ResimUrl,
                    AktifMi = resim.AktifMi,
                    HaberID = resim.HaberID
                };
                return resimDto;
            }
            return null;
        }

        public Resim DTOToResimModel(ResimDTO dto)
        {
            if (dto != null)
            {
                var resim = new Resim()
                {
                    Id = dto.Id,
                    ResimUrl = dto.ResimUrl,
                    AktifMi = dto.AktifMi,
                    HaberID = dto.HaberID,
                 
                };
                return resim;
            }
            return null;
        }
    }
}
