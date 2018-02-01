using HaberSistemi.Core.Dto;
using HaberSistemi.Data.Model;
using System.Collections.Generic;
using System.Linq;

namespace HaberSistemi.Core.Infrastructure
{
    public interface IEtiketRepository
    {
        ServiceResult<EtiketDTO> GetById(int id);

        ServiceResult<bool> Delete(int id);

        ServiceResult<EtiketDTO> Get(EtiketDTO etiket);

        ServiceResult<List<EtiketDTO>> GetEtiketList();

        ServiceResult<List<EtiketDTO>> GetMany(EtiketDTO etiket);

        ServiceResult<EtiketDTO> Insert(EtiketDTO etiket);

        ServiceResult<bool> Update(EtiketDTO etiket);

        IQueryable<Etiket> Etiketler(string[] etiketler);

        void EtiketEkle(int HaberID, string etiket);

        void HaberEtiketEkle(int HaberID, string[] etiketler);
    }
}
