using HaberSistemi.Data.Model;
using System;
using System.ComponentModel.DataAnnotations;

namespace HaberSistemi.Core.Dto
{
    public class ResimDTO
    {
        
        public int Id { get; set; }

        public string ResimUrl { get; set; }

        public int HaberID { get; set; }

        public HaberDTO Haber { get; set; }

        [Display(Name = "Eklenme Tarihi")]
        public DateTime EklenmeTarihi { get; set; }

        [Display(Name = "AktifMi")]
        public bool AktifMi { get; set; }
    }
}
