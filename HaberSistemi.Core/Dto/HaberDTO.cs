using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HaberSistemi.Core.Dto
{
    public class HaberDTO
    {
        public int Id { get; set; }

        [Display(Name = "Haber Başlık")]
        [MaxLength(255, ErrorMessage = "Çok fazla karekter girdiniz!")]
        [Required]
        public string Baslik { get; set; }

        [Display(Name = "Kısa Açıklama")]
        public string KisaAciklama { get; set; }

        [Display(Name = "Acıklama")]
        public string Aciklama { get; set; }

        public int Okunma { get; set; }

        public int KullaniciID { get; set; }

        public KullaniciDTO Kullanici { get; set; }

        [Display(Name = "Aktif")]
        public bool AktifMi { get; set; }

        [Display(Name = "Eklenme Tarihi")]
        public DateTime EklenmeTarihi { get; set; }

        [Display(Name = "Resim")]
        [MaxLength(255, ErrorMessage = "Çok fazla karekter girdiniz!")]
        public string Resim { get; set; }

        public  ICollection<ResimDTO> Resimler { get; set; }

        public  List<EtiketDTO> Etiket { get; set; }

        public int KategoriID { get; set; }

        public KategoriDTO Kategori { get; set; }

    }
}
