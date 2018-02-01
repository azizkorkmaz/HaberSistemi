using System.Collections.Generic;

namespace HaberSistemi.Data.Model
{
    public class HaberEtiketModel
    {
        public Haber Haber { get; set; }

        public string[] GelenEtiketler { get; set; }

        public IEnumerable<Etiket> Etiketler { get; set; }

        public string EtiketAdi { get; set; }
    }
}
