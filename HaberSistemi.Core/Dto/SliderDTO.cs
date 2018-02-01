using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaberSistemi.Core.Dto
{
   public class SliderDTO
    {
        public int Id { get; set; }

        [Display(Name = " Başlık")]
        [MinLength(3, ErrorMessage = "En az {0} karekter olabilir!"), MaxLength(255, ErrorMessage = "En çok {1} karekter olabilir!")]
        public string Baslik { get; set; }

        [Display(Name = " URL")]
        [MinLength(3, ErrorMessage = "En az {0} karekter olabilir!"), MaxLength(255, ErrorMessage = "En çok {1} karekter olabilir!")]
        public string URL { get; set; }

        [Display(Name = " Açıklama")]
        [MinLength(3, ErrorMessage = "En az {0} karekter olabilir!"), MaxLength(255, ErrorMessage = "En çok {1} karekter olabilir!")]
        public string Aciklama { get; set; }

        [Display(Name = " ResimURL")]
        [MinLength(3, ErrorMessage = "En az {0} karekter olabilir!"), MaxLength(255, ErrorMessage = "En çok {1} karekter olabilir!")]
        [Required]
        public string ResimURL { get; set; }

        [Display(Name = "Aktif")]
        public bool AktifMi { get; set; }

        [Display(Name = "Eklenme Tarihi")]
        public DateTime EklenmeTarihi { get; set; }
    }
}
