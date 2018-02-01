using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HaberSistemi.Data.Model
{
    [Table("Etiket")]
   public class Etiket:BaseEntity
    {
        [Display(Name ="Etiket")]
        public string EtiketAdi { get; set; }

        public virtual ICollection<Haber> Haber { get; set; }
    }
}
