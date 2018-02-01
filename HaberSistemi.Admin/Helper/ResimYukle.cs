using HaberSistemi.Core.Dto;
using System;
using System.Web;

namespace HaberSistemi.Admin.Helper
{
    public static class ResimYukle
    {
        public static string Resim(SliderDTO slider, HttpPostedFileBase ResimURL)
        {
            string dosyaAdi = Guid.NewGuid().ToString().Replace("-", "");
            string[] uzanti = ResimURL.ContentType.Split('/');
            string tamYol = "/External/Slider/" + dosyaAdi + "." + uzanti[1];
            ResimURL.SaveAs(HttpContext.Current.Server.MapPath(tamYol));
            slider.ResimURL = tamYol;

            return slider.ResimURL;
        }
    }
}