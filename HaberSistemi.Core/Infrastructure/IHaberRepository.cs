using HaberSistemi.Core.Dto;
using System.Collections.Generic;

namespace HaberSistemi.Core.Infrastructure
{
    public interface IHaberRepository
    {
        ServiceResult<HaberDTO> GetById(int id);

        ServiceResult<bool> Delete(int id);

        ServiceResult<HaberDTO> Get(HaberDTO haber);

        ServiceResult<List<HaberDTO>> GetHaberList();

        ServiceResult<List<HaberDTO>> GetMany(HaberDTO haber);

        ServiceResult<HaberDTO> Insert(HaberDTO haber);

        ServiceResult<bool> Update(HaberDTO haber);
    }
}
