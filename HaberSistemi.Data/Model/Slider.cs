using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HaberSistemi.Data.Model
{
    [Table("Silder")]
   public class Slider:BaseEntity
    {
        [Display(Name = " Başlık")]
        [MinLength(3, ErrorMessage = "En az {0} karekter olabilir!"), MaxLength(255, ErrorMessage = "En çok {1} karekter olabilir!")]
        public string  Baslik { get; set; }

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
    }
}
