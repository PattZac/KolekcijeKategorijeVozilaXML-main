using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace KolekcijeKategorijeVozila
{
    internal class Vozilo
    {
        string model, kategorija;
        int godinaProizvodnje, brojKotaca;

        public string Model { get => model; set => model = value; }
        public int GodinaProizvodnje { get => godinaProizvodnje; set => godinaProizvodnje = value; }
        public string Kategorija { get => kategorija; set => kategorija = value; }

        public int BrojKotaca { get => brojKotaca; set => brojKotaca = value; }

        public Vozilo(string m, int gP, int bK) {
            Model = m;
            godinaProizvodnje = gP;
            BrojKotaca = bK;
            Kategorija = "Vozilo";
        }
        public Vozilo() { }

        public void Obradi()
        {
            if (BrojKotaca == 2)
            {
                Kategorija = "Motocikl";
            } 
            else if (BrojKotaca == 4)
            {
                Kategorija = "Automobil";
            }
            else
            {
                Kategorija = "Kamion";
            }
        }

        public string ToXml()
        {
            var xml = new XElement("vozilo",
                    new XElement("model", Model),
                    new XElement("godinaProizvodnje", GodinaProizvodnje),
                    new XElement("brojKotaca", BrojKotaca),
                    new XAttribute("kategorije", Kategorija));
            return xml.ToString();
        }

        public override string ToString()
        {
            return Kategorija + " - Model: " + Model + " | Godina Proizvodanje: " + GodinaProizvodnje + " | Broj Kotača: "+ BrojKotaca;
        }
    }
}
