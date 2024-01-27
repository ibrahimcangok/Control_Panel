using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Data.OleDb;
using Microsoft.VisualBasic;

namespace Programlar
{
    public partial class Form1 : Form
    {
        OleDbConnection con;
        string pcadi = Environment.MachineName;
        public Form1()
        {
            con = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=vt1.mdb");
            InitializeComponent();
            tablo_olusturma();
        }

        void tablo_olusturma()
        {
            try
            {
                OleDbConnection baglanti = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=vt1.mdb");
                baglanti.Open();
                OleDbCommand komut = new OleDbCommand("Create Table " + Environment.MachineName + " ([ID] counter primary key,[program] varchar(255),[tarih] Date/Time,[saat] Date/Time)", baglanti);
                komut.ExecuteNonQuery();
                baglanti.Close();
                MessageBox.Show("Bilgisayar Kaydı Başarıyla Oluşturuldu !!!");
            }
            catch (Exception)
            {
                MessageBox.Show("Tablonuz Oluşturuldu !!!");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            label1.Text = DateTime.Now.ToLongTimeString();
            timer1.Start();
            try
            {
                BütünProgramlar.Items.Clear();
                MicrosoftProgramları.Items.Clear();
                string prganahtar = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
                RegistryKey anahtar = Registry.LocalMachine.OpenSubKey(prganahtar);
                string[] altanahtar = anahtar.GetSubKeyNames();
                string[] programlar = new string[altanahtar.Length];
                int sonuc = -1;
                for (int i = 0; i < altanahtar.Length; i++)
                {
                    RegistryKey SubKey = anahtar.OpenSubKey(altanahtar[i]);
                    try
                    {
                        object displayNameObj = SubKey.GetValue("DisplayName");
                        if (displayNameObj != null)
                        {
                            string prgismi = displayNameObj.ToString().Trim();
                            if (!String.IsNullOrEmpty(prgismi))
                            {
                                programlar[i] = prgismi;
                                sonuc = programlar[i].IndexOf("Microsoft");
                                if (sonuc >= 0)
                                {
                                    MicrosoftProgramları.Items.Add(programlar[i]);
                                }
                                BütünProgramlar.Items.Add(programlar[i]);
                            }
                        }
                    }
                    catch
                    {
                        Application.DoEvents();
                    }
                    finally
                    {
                        SubKey?.Close();
                    }
                }

            
                anahtar.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Hata !!!");
            }
            OleDbConnection bag = new OleDbConnection();
            OleDbCommand sorgu = new OleDbCommand();
            bag.ConnectionString = ("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=vt1.mdb");
            bag.Open();
            sorgu.Connection = bag;
            for (int i = 0; i < MicrosoftProgramları.Items.Count; i++)
            {
                sorgu.CommandText = "INSERT INTO MONSTER (program,tarih,saat)" + "VALUES('" + MicrosoftProgramları.Items[i].ToString() + "','" + Convert.ToDateTime(dateTimePicker1.Text) + "','" + label1.Text + "')";
                sorgu.ExecuteNonQuery();
            }

            bag.Close();
            MessageBox.Show("Kayıt İşlemi Tamamlandı");
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label1.Text = DateTime.Now.ToLongTimeString();
        }
    }
}