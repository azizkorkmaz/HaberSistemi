using HaberSistemi.Core.Infrastructure;
using System.Collections.Generic;
using System.Linq;
using HaberSistemi.Core.Dto;
using HaberSistemi.Data.Model;
using HaberSistemi.Data.DataContext;
using System.Data.Entity.Migrations;

namespace HaberSistemi.Core.Repository
{
    public class SliderRepository : ISliderRepository
    {
        private readonly HaberContext _context = new HaberContext();

        public ServiceResult<SliderDTO> Get(SliderDTO dto)
        {
            var sliderList = _context.Slider.Where(x => x.AktifMi);
            if (string.IsNullOrEmpty(dto.Aciklama))
            {
                sliderList.Where(x => x.Aciklama == dto.Aciklama);
            }
            if (string.IsNullOrEmpty(dto.Baslik))
            {
                sliderList.Where(x => x.Baslik == dto.Baslik);
            }
            if (sliderList.Any())
            {
                return ServiceResult<SliderDTO>.Success(ModelToDTOSlider(sliderList.FirstOrDefault()));
            }

            return ServiceResult<SliderDTO>.Fail("Aranan kriterlere uygun haber bulunamadı!");
        }

        public ServiceResult<SliderDTO> GetById(int id)
        {
            var slider = _context.Slider.FirstOrDefault(x => x.Id == id);

            if (slider != null)
            {
                var resDto = ModelToDTOSlider(slider);
                return ServiceResult<SliderDTO>.Success(resDto);
            }

            return ServiceResult<SliderDTO>.Fail("Kayıt bulunamadı.");
        }

        public ServiceResult<List<SliderDTO>> GetSliderList()
        {
            var sliderler = _context.Slider.ToList();
            if (sliderler != null && sliderler.Any())
            {

                var resList = new List<SliderDTO>();
                foreach (var item in sliderler)
                {
                    resList.Add(ModelToDTOSlider(item));
                }

                return ServiceResult<List<SliderDTO>>.Success(resList);
            }
            return ServiceResult<List<SliderDTO>>.Fail("Listelenecek Haber Bulunmamaktadır");
        }

        public ServiceResult<List<SliderDTO>> GetMany(SliderDTO dto)
        {
            var sliderList = _context.Slider.ToList();
            if (!string.IsNullOrEmpty(dto.Aciklama))
            {
                sliderList.Where(x => x.Aciklama == dto.Aciklama);
            }
            if (!string.IsNullOrEmpty(dto.Baslik))
            {
                sliderList.Where(x => x.Baslik == dto.Baslik);
            }
            if (sliderList.Any())
            {
                var resList = new List<SliderDTO>();
                foreach (var item in sliderList)
                {
                    resList.Add(ModelToDTOSlider(item));
                }
                return ServiceResult<List<SliderDTO>>.Success(resList);
            }

            return ServiceResult<List<SliderDTO>>.Fail("Aranan kriterlere uygun haber bulunamadı!");
        }

        public ServiceResult<SliderDTO> Insert(SliderDTO data)
        {
            var slider = DTOToModelSlider(data);
            if (slider != null)
            {
                _context.Slider.Add(slider);
                _context.SaveChanges();
                return ServiceResult<SliderDTO>.Success(ModelToDTOSlider(slider));
            }
            return ServiceResult<SliderDTO>.Fail("Kayıt eklenmedi.");
        }

        public ServiceResult<bool> Update(SliderDTO obj)
        {
            if (obj != null && obj.Id > 0)
            {
                var slider = _context.Slider.FirstOrDefault(x => x.Id == obj.Id);

                if (slider != null)
                {
                    slider.Baslik = obj.Baslik;
                    slider.Aciklama = obj.Aciklama;
                    slider.AktifMi = obj.AktifMi;
                    slider.ResimURL = obj.ResimURL;
                    _context.Slider.AddOrUpdate(slider);
                    _context.SaveChanges();
                    return ServiceResult<bool>.Success(true);
                }
                return ServiceResult<bool>.Fail("Güncellenecek slider bulunamadı!");
            }

            return ServiceResult<bool>.Fail("Böyle bir slider kayıtlı değil!");
        }

        public ServiceResult<bool> Delete(int id)
        {
            var slider = _context.Slider.FirstOrDefault(x => x.Id == id);

            if (slider != null)
            {
                _context.Slider.Remove(slider);
                var res = _context.SaveChanges();
                return res > 0 ? ServiceResult<bool>.Success(true) : ServiceResult<bool>.Fail("Kayıt silinemedi.");
            }

            return ServiceResult<bool>.Fail("Kayıt bulunamadığından silinemedi.");
        }

        public SliderDTO ModelToDTOSlider(Slider slider)
        {
            if (slider != null)
            {
                var silderDTO = new SliderDTO
                {
                    Id = slider.Id,
                    Baslik = slider.Baslik,
                    Aciklama = slider.Aciklama,
                    AktifMi = slider.AktifMi,
                    ResimURL = slider.ResimURL,
                    URL = slider.URL
                };

                return silderDTO;
            }

            return null;
        }

        public Slider DTOToModelSlider(SliderDTO dto)
        {
            var slider = new Slider
            {
                Id = dto.Id,
                Baslik = dto.Baslik,
                Aciklama = dto.Aciklama,
                AktifMi = dto.AktifMi,
                ResimURL = dto.ResimURL,
                URL = dto.URL
            };

            return slider;
        }

    }
}
