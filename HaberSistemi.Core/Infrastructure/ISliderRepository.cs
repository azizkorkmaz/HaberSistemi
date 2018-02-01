using HaberSistemi.Core.Dto;
using System.Collections.Generic;

namespace HaberSistemi.Core.Infrastructure
{
    public interface ISliderRepository
    {
        ServiceResult<SliderDTO> GetById(int id);

        ServiceResult<SliderDTO> Get(SliderDTO dto);

        ServiceResult<List<SliderDTO>> GetSliderList();

        ServiceResult<List<SliderDTO>> GetMany(SliderDTO dto);

        ServiceResult<SliderDTO> Insert(SliderDTO data);

        ServiceResult<bool> Update(SliderDTO obj);

        ServiceResult<bool> Delete(int id);
    }
}
