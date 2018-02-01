using HaberSistemi.Core.Dto;
using System.Collections.Generic;

namespace HaberSistemi.Core.Infrastructure
{
    public interface IKullaniciRepository
    {
        ServiceResult<KullaniciDTO> GetById(int id);

        ServiceResult<bool> Delete(int id);

        ServiceResult<KullaniciDTO> LoginKullanici(KullaniciDTO kullanici);

        ServiceResult<List<KullaniciDTO>> GetAll();

        ServiceResult<List<KullaniciDTO>> GetMany(KullaniciDTO kullanici);

        ServiceResult<KullaniciDTO> Insert(KullaniciDTO kullanici);

        ServiceResult<bool> Update(KullaniciDTO kullanici);
    }
}
