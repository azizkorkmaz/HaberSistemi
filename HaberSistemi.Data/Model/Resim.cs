using System.ComponentModel.DataAnnotations.Schema;

namespace HaberSistemi.Data.Model
{
    [Table("Resim")]
    public class Resim:BaseEntity
    {
       
        public string ResimUrl { get; set; }

        public int HaberID { get; set; }

        public virtual Haber Haber { get; set; }

    }
}
