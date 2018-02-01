using HaberSistemi.Core.Dto;
using System.Collections.Generic;

namespace HaberSistemi.Core.Infrastructure
{
    public interface IKategoriRepository
    {
        ServiceResult<KategoriDTO> GetKategoriById(int id);

        ServiceResult<bool> Delete(int id);

        ServiceResult<KategoriDTO> Get(KategoriDTO kategori);

        ServiceResult<List<KategoriDTO>> GetKategoriList();

        ServiceResult<List<KategoriDTO>> SearchKategori(KategoriDTO kategori);

        ServiceResult<KategoriDTO> Insert(KategoriDTO kategori);

        ServiceResult<bool> Update(KategoriDTO kategori);
    }
}
