using HaberSistemi.Core.Dto;
using System.Collections.Generic;

namespace HaberSistemi.Core.Infrastructure
{
    public interface IRolRepository
    {
        ServiceResult<RolDTO> GetById(int id);

        ServiceResult<bool> Delete(int id);

        ServiceResult<RolDTO> Get(RolDTO kullanici);

        ServiceResult<List<RolDTO>> GetAll();

        ServiceResult<List<RolDTO>> GetMany(RolDTO kullanici);

        ServiceResult<RolDTO> Insert(RolDTO kullanici);

        ServiceResult<bool> Update(RolDTO kullanici);
    }
}
