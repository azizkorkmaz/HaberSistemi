using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HaberSistemi.Core.Dto
{
    public class KategoriDTO
    {
        
        public int Id { get; set; }

        [MinLength(2, ErrorMessage = "{0} karekterden az olamaz."), MaxLength(150, ErrorMessage = "150 karekterden fazla olamaz")]
        [Required]
        public string KategoriAdi { get; set; }

        public int ParentID { get; set; }

        public string ParentAdi { get; set; }

        [MinLength(2, ErrorMessage = "{0} karekterden az olamaz."), MaxLength(150, ErrorMessage = "150 karekterden fazla olamaz")]
        public string URL { get; set; }

        public bool AktifMi { get; set; }

    }
}