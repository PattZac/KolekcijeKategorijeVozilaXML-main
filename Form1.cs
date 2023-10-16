using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace KolekcijeKategorijeVozila
{
    public partial class Form1 : Form
    {
        List<Vozilo> voziloList = new List<Vozilo>();
        public Form1()
        {
            InitializeComponent();
            for (int i = 1900; i <= DateTime.Now.Year; i++)
            {
                txtGodinaProizvodnje.Items.Add(i);
            }
        }

        private void btnUnesi_Click(object sender, EventArgs e)
        {
            try {
                if (Convert.ToInt16(txtBrojKotaca.Text) % 2 == 1)
                {
                    MessageBox.Show("Broj kotača nije paran! Unesi paran broj kotača", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else {
                    Vozilo vz = new Vozilo(txtModel.Text, Convert.ToInt16(txtGodinaProizvodnje.Text), Convert.ToInt16( txtBrojKotaca.Text ));
                    voziloList.Add(vz);
                    txtModel.Clear();
                    txtGodinaProizvodnje.Text = "";
                    txtBrojKotaca.Text = "";
                    txtModel.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Greška\r\n"+ex, "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnObradi_Click(object sender, EventArgs e)
        {
            foreach(Vozilo i in voziloList) {
                i.Obradi();
            }
        }

        private void btnIspis_Click(object sender, EventArgs e)
        {
            string n = "";
            foreach (Vozilo i in voziloList)
            {
                n += i.ToString() + "\r\n";
            }
            txtIspis.Text = n;
        }

        private void bntSpremi_Click(object sender, EventArgs e)
        {
            XDocument newXML = new XDocument(new XElement("Vozila",
                    from xml in voziloList
                    select new XElement("Vozilo",
                        new XAttribute("Model", xml.Model),
                        new XAttribute("GodinaProizvodnje", xml.GodinaProizvodnje),
                        new XAttribute("BrojKotaca", xml.BrojKotaca),
                        new XAttribute("Kategorije", xml.Kategorija))));
            //txtIspis.Text = newXML.ToString();

            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "XML file (*.xml)|*.xml";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    File.AppendAllText(saveFileDialog1.FileName, newXML.ToString());
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Pogrešan u spremanje datoteka!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
        }

        private void bntUcit_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "XML file (*.xml)|*.xml";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;
                    var fileStream = openFileDialog.OpenFile();

                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        XElement newXML = XElement.Load(reader);
                        //txtIspis.Text = newXML.ToString();
                        foreach (XElement element in newXML.Elements())
                        {
                            Vozilo vz = new Vozilo(element.Attribute("Model").Value, Convert.ToInt16(element.Attribute("GodinaProizvodnje").Value), Convert.ToInt16(element.Attribute("BrojKotaca").Value));
                            if (element.Attribute("Kategorije").Value != "Vozilo")
                            {
                                vz.Obradi();
                            }
                            voziloList.Add(vz);
                        }
                    }
                }
            }
        }
    }
}
