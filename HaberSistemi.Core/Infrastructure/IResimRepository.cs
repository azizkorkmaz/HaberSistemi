using HaberSistemi.Core.Dto;
using System.Collections.Generic;

namespace HaberSistemi.Core.Infrastructure
{
    public interface IResimRepository
    {
        ServiceResult<ResimDTO> GetById(int id);

        ServiceResult<bool> Delete(int id);

        ServiceResult<ResimDTO> Get(ResimDTO resim);

        ServiceResult<List<ResimDTO>> GetAll();

        ServiceResult<List<ResimDTO>> GetMany(ResimDTO resim);

        ServiceResult<ResimDTO> Insert(ResimDTO resim);

        ServiceResult<bool> Update(ResimDTO resim);
    }
}
