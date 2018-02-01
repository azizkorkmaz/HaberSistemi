using System.ComponentModel.DataAnnotations;

namespace HaberSistemi.Core.Dto
{
    public class RolDTO
    {
        public int Id { get; set; }

        [Display(Name = "Rol Adı :")]
        [MinLength(3, ErrorMessage = "Lütfen 3 karekterden fazla değer giriniz!"), MaxLength(150, ErrorMessage = " Lütfen 150 karekterden fazla girmeyiniz")]
        public string RolAdi { get; set; }

        [Display(Name = "Aktif")]
        public bool Aktif { get; set; }
    }
}
