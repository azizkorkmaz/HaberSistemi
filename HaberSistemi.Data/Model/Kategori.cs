using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HaberSistemi.Data.Model
{
    [Table("Kategori")]
    public class Kategori:BaseEntity
    {
        
        [MinLength(2,ErrorMessage ="{0} karekterden az olamaz."), MaxLength(150,ErrorMessage ="150 karekterden fazla olamaz")]
        [Required]
        public string KategoriAdi { get; set; }

        public int ParentID { get; set; }

        [MinLength(2, ErrorMessage = "{0} karekterden az olamaz."), MaxLength(150, ErrorMessage = "150 karekterden fazla olamaz")]
        public string URL { get; set; }

        public virtual ICollection<Haber> Haberler { get; set; }

        public virtual Haber Haber { get; set; }
    }
}
