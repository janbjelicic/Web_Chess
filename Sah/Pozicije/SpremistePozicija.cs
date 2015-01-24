using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.IO;
using ImageResizer;

namespace Sah.Pozicije
{
    public enum VelicinaPozicije{
        mala,
        srednja,
        velika
    }

    public class SpremistePozicija
    {
        private const string uploadFolder = "~/Skladiste_Data/";
        private IDictionary<VelicinaPozicije, String> velicine;

        public SpremistePozicija() {
            DodajVelicine();
        }

        private void DodajVelicine() {
            velicine = new Dictionary<VelicinaPozicije, String>();
            velicine.Add(VelicinaPozicije.mala, "width=180&height=180&crop=auto&format=jpg");
            velicine.Add(VelicinaPozicije.srednja, "width=630&format=jpg");
            velicine.Add(VelicinaPozicije.velika, "width=900&format=jpg");
        }
        
        private string DohvatiPrefiksImena(int idDioIgre, VelicinaPozicije velicina) {
            return idDioIgre.ToString() + "_" + velicina.ToString().ToLower();
        }

        public string DohvatiPoziciju(int idDioIgre, VelicinaPozicije velicina)
        {

            string filename = Path.Combine(uploadFolder, DohvatiPrefiksImena(idDioIgre, velicina) + ".jpg");
            if (File.Exists(HttpContext.Current.Server.MapPath(filename)))
            {
                return filename;
            }

            return Path.Combine("~/Images/", "default_" + velicina.ToString().ToLower() + ".jpg");
        }

        public bool SpremiPoziciju(int idDioIgre, HttpPostedFileBase file)
        {

            string fileName;
            foreach (var velicina in velicine)
            {
                fileName = Path.Combine(uploadFolder, DohvatiPrefiksImena(idDioIgre, velicina.Key));
                fileName = ImageBuilder.Current.Build(file, fileName, new ResizeSettings(velicina.Value), false, true);
            }

            // spremi original
            fileName = Path.Combine(uploadFolder, DohvatiPrefiksImena(idDioIgre, VelicinaPozicije.velika));
            ImageBuilder.Current.Build(file, fileName, new ResizeSettings("format=jpg"), false, true);
            
            return true;
        }
    }
}