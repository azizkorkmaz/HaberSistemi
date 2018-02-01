using HaberSistemi.Core.Infrastructure;
using System.Collections.Generic;
using System.Linq;
using HaberSistemi.Data.Model;
using HaberSistemi.Data.DataContext;
using System.Data.Entity.Migrations;
using HaberSistemi.Core.Dto;

namespace HaberSistemi.Core.Repository
{
    public class RolRepository : IRolRepository
    {
        private readonly HaberContext _context = new HaberContext();

        public ServiceResult<RolDTO> GetById(int id)
        {
            var rol = _context.Rol.FirstOrDefault(x => x.Id == id);
            if (rol != null)
            {
                return ServiceResult<RolDTO>.Success(ModelToRolDTO(rol));
            }
            return ServiceResult<RolDTO>.Fail("Rol bulunamadı!");
        }

        public ServiceResult<RolDTO> Get(RolDTO roldto)
        {
            var rolList = _context.Rol.Where(x => x.AktifMi);
            if (string.IsNullOrEmpty(roldto.RolAdi))
            {
                rolList.Where(x => x.RolAdi == roldto.RolAdi);
            }
            if (rolList.Any())
            {
                return ServiceResult<RolDTO>.Success(ModelToRolDTO(rolList.FirstOrDefault()));
            }
            return ServiceResult<RolDTO>.Fail("Aranan kritere uygun rol bulıunamadı!");
        }

        public ServiceResult<List<RolDTO>> GetAll()
        {
            var roller = _context.Rol.Where(x => x.AktifMi == true).ToList();
            if (roller!=null && roller.Any())
            {
                var resList = new List<RolDTO>();
                foreach (var item in roller)
                {
                    resList.Add(ModelToRolDTO(item));
                }
                return ServiceResult<List<RolDTO>>.Success(resList);
            }
            return ServiceResult<List<RolDTO>>.Fail("Listelenecek rol bulunamadı!");
        }

        public ServiceResult<List<RolDTO>> GetMany(RolDTO roldto)
        {
            var rolList = _context.Rol.Where(x => x.AktifMi);
            if (string.IsNullOrEmpty(roldto.RolAdi))
            {
                rolList.Where(x => x.RolAdi == roldto.RolAdi);
            }
            if (rolList.Any())
            {
                var resList = new List<RolDTO>();
                foreach (var item in rolList)
                {
                    resList.Add(ModelToRolDTO(item));
                }
                return ServiceResult<List<RolDTO>>.Success(resList);
            }
            return ServiceResult<List<RolDTO>>.Fail("Aranan kritere uygun rol adı bulunamadı!");

        }

        public ServiceResult<RolDTO> Insert(RolDTO data)
        {
            var rol = DTOToModelRol(data);
            if (rol!=null)
            {
                _context.Rol.Add(rol);
                _context.SaveChanges();
                return ServiceResult<RolDTO>.Success(ModelToRolDTO(rol));
            }
            return ServiceResult<RolDTO>.Fail("Rol eklenmedi!");
        }

        public ServiceResult<bool> Update(RolDTO obj)
        {
            if (obj != null && obj.Id>0)
            {
                var rol = _context.Rol.FirstOrDefault(x => x.Id == obj.Id);
                if (rol != null)
                {
                    rol.RolAdi = obj.RolAdi;
                    _context.Rol.AddOrUpdate();
                    _context.SaveChanges();
                    return ServiceResult<bool>.Success(true);
                }
                return ServiceResult<bool>.Fail("Güncellenecek rol bulunamadı!");
            }
            return ServiceResult<bool>.Fail("Böyle bir rol kayıtlı değil");
        }

        public ServiceResult<bool> Delete(int id)
        {
            var rol = _context.Rol.FirstOrDefault(x => x.Id == id);
            if (rol !=null)
            {
                _context.Rol.Remove(rol);
                var res = _context.SaveChanges();
                return res > 0 ? ServiceResult<bool>.Success(true) : ServiceResult<bool>.Fail("Kayıt silinmedi!");
            }
            return ServiceResult<bool>.Fail("Kayıt bulunamadığından silinemedi!");
        }

        public RolDTO ModelToRolDTO(Rol rol)
        {
            if (rol != null)
            {
                var rolDto = new RolDTO
                {
                    Id = rol.Id,
                    RolAdi = rol.RolAdi,
                    Aktif = rol.AktifMi
                };
                return rolDto;

            }
            return null;
        }

        public Rol DTOToModelRol(RolDTO dto)
        {
            var rol = new Rol
            {
                RolAdi = dto.RolAdi,
                Id = dto.Id,
                AktifMi = dto.Aktif

            };
            return rol;
        }
    }
}
