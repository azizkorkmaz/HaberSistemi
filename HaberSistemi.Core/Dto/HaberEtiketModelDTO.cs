using System.Collections.Generic;

namespace HaberSistemi.Core.Dto
{
    public class HaberEtiketModelDTO
    {
        public HaberDTO Haber { get; set; }

        public string[] GelenEtiketler { get; set; }

        public IEnumerable<EtiketDTO> Etiketler { get; set; }

        public string EtiketAdi { get; set; }
    }
}
