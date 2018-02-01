using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HaberSistemi.Data.Model;
using System;

namespace HaberSistemi.Core.Dto
{
    public class EtiketDTO
    {
        public int Id { get; set; }

        [Display(Name = "Etiket")]
        public string EtiketAdi { get; set; }

        [Display(Name = "Eklenme Tarihi")]
        public DateTime EklenmeTarihi { get; set; }

        [Display(Name = "Aktif")]
        public bool AktifMi { get; set; }

        public virtual ICollection<Haber> Haber { get; set; }
    }
}
