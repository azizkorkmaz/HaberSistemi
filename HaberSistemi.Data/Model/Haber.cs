using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HaberSistemi.Data.Model
{
    [Table("Haber")]
    public class Haber:BaseEntity
    {
        

        [Display(Name ="Haber Başlık")]
        [MaxLength(255,ErrorMessage ="Çok fazla karekter girdiniz!")]
        [Required]
        public string Baslik { get; set; }

        [Display(Name = "Kısa Açıklama")]
        public string KisaAciklama { get; set; }

        [Display(Name = "Acıklama")]
        public string Aciklama { get; set; }

        public int Okunma { get; set; }

        public int KullaniciID { get; set; }

        public virtual Kullanici Kullanici { get; set; }

        [Display(Name = "Resim")]
        [MaxLength(255, ErrorMessage = "Çok fazla karekter girdiniz!")]
        public string Resim { get; set; }

        public virtual ICollection<Resim> Resimler { get; set; }

        public virtual ICollection<Etiket> Etiket { get; set; }

        public int KategoriID { get; set; }

        public virtual Kategori Kategori { get; set; }

    }
}
