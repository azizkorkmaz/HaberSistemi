using HaberSistemi.Core.Infrastructure;
using HaberSistemi.Data.DataContext;
using HaberSistemi.Data.Model;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity.Migrations;//AddOrUpdate için gerekli
using HaberSistemi.Core.Dto;

namespace HaberSistemi.Core.Repository
{
    public class KullaniciRepository : IKullaniciRepository
    {
        private readonly HaberContext _context = new HaberContext();

        public ServiceResult<KullaniciDTO> GetById(int id)
        {
            var kullanici = _context.Kullanici.FirstOrDefault(x => x.Id == id);

            if (kullanici != null)
            {
                return ServiceResult<KullaniciDTO>.Success(ModelToDTOKullanici(kullanici));
            }
            return ServiceResult<KullaniciDTO>.Fail("Kullanici Bulunamadı");
        }

        public ServiceResult<KullaniciDTO> LoginKullanici(KullaniciDTO kullanicidto)
        {
            var kullanici = _context.Kullanici.Where(x => x.AktifMi && x.Email == kullanicidto.Email && x.Sifre == kullanicidto.Sifre).FirstOrDefault();
            
            if (kullanici != null)
            {
                return ServiceResult<KullaniciDTO>.Success(ModelToDTOKullanici(kullanici));
            }
            return ServiceResult<KullaniciDTO>.Fail("Şifre veya E-mail adresi yanlış!");
        }

        public ServiceResult<List<KullaniciDTO>> GetAll()
        {
            var kullanıcılar = _context.Kullanici.Where(x => x.AktifMi==true).ToList();
            if (kullanıcılar!=null && kullanıcılar.Any())
            {
                var resList = new List<KullaniciDTO>();
                foreach (var item in kullanıcılar)
                {
                    resList.Add(ModelToDTOKullanici(item));
                }
                return ServiceResult<List<KullaniciDTO>>.Success(resList);
            }
            return ServiceResult<List<KullaniciDTO>>.Fail("Listenecek kullanıcı bulunamadı!");
        }

        public ServiceResult<List<KullaniciDTO>> GetMany(KullaniciDTO dto)
        {
            var kullaniciList = _context.Kullanici.Where(x => x.AktifMi);
            if (!string.IsNullOrEmpty(dto.AdSoyad))
            {
                kullaniciList.Where(x => x.AdSoyad == dto.AdSoyad);
            }
            if (!string.IsNullOrEmpty(dto.Email))
            {
                kullaniciList.Where(x => x.Email == dto.Email);
            }
            if (kullaniciList.Any())
            {
                var resList = new List<KullaniciDTO>();
                foreach (var item in kullaniciList)
                {
                    resList.Add(ModelToDTOKullanici(item));
                }
                return ServiceResult<List<KullaniciDTO>>.Success(resList);
            }

            return ServiceResult<List<KullaniciDTO>>.Fail("Aranan kriterlere uygun kullanici bulunamadı!");
        }

        public ServiceResult<KullaniciDTO> Insert(KullaniciDTO data)
        {
            var kullanici = DTOToModelKullanici(data);
            if (kullanici!=null)
            {
                _context.Kullanici.Add(kullanici);
                _context.SaveChanges();
                return ServiceResult<KullaniciDTO>.Success(ModelToDTOKullanici(kullanici));
            }
            return ServiceResult<KullaniciDTO>.Fail("Kayıt eklenmedi");
        }

        public ServiceResult<bool> Update(KullaniciDTO obj)
        {
            if (obj != null && obj.Id>0)
            {
                var kullanici = _context.Kullanici.FirstOrDefault(x => x.Id == obj.Id);
                if (kullanici!=null)
                {
                    kullanici.AdSoyad = obj.AdSoyad;
                    kullanici.Email = obj.Email;
                    kullanici.Sifre = obj.Sifre;
                    kullanici.Rol.RolAdi = obj.Rol.RolAdi;
                    kullanici.Rol.Id = obj.Rol.Id;
                    _context.Kullanici.AddOrUpdate(kullanici);
                    _context.SaveChanges();
                    return ServiceResult<bool>.Success(true);
                }
                return ServiceResult<bool>.Fail("Güncellenecek haber bulunamadı!");
            }
            return ServiceResult<bool>.Fail("Böyle bir kullanıcı kayıtlı değil!");
        }

        public ServiceResult<bool> Delete(int id)
        {
            var kullanici = _context.Kullanici.FirstOrDefault(x => x.Id == id);
            if (kullanici != null)
            {
                _context.Kullanici.Remove(kullanici);
                var res = _context.SaveChanges();
                return res > 0 ? ServiceResult<bool>.Success(true) : ServiceResult<bool>.Fail("Kayıt silinemedi");
            }
            return ServiceResult<bool>.Fail("Kayıt bulunamadığından silinemedi");
        }

        public KullaniciDTO ModelToDTOKullanici(Kullanici kullanici)
        {
            if (kullanici != null)
            {
                var KullaniciDto = new KullaniciDTO
                {
                    Id = kullanici.Id,
                    AdSoyad = kullanici.AdSoyad,
                    AktifMi = kullanici.AktifMi,
                    Email = kullanici.Email,
                    Sifre = kullanici.Sifre,
                    Rol = kullanici.Rol

                };

                return KullaniciDto;
            }

            return null;
        }

        public Kullanici DTOToModelKullanici(KullaniciDTO dto)
        {
            var kullanici = new Kullanici
            {
                Id = dto.Id,
                AdSoyad = dto.AdSoyad,
                Email = dto.Email,
                Sifre = dto.Sifre,
                Rol = dto.Rol,
                AktifMi = dto.AktifMi

            };

            return kullanici;
        }
    }
}
