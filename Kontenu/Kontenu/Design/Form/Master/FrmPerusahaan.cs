using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraBars;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid;
using DevExpress.XtraPrinting;
using DevExpress.XtraPrintingLinks;
using DevExpress.XtraReports.Parameters;
using DevExpress.XtraReports.UI;
using DevExpress.XtraSplashScreen;
using DevExpress.XtraTabbedMdi;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System;
using OswLib;
using Kontenu.Master;
using DevExpress.XtraEditors.DXErrorProvider;
using System.Net;
using Kontenu.Umum;
using Kontenu.Sistem;
using Kontenu.OswLib;

namespace Kontenu.Master {
    public partial class FrmPerusahaan : DevExpress.XtraEditors.XtraForm {
        private String kode = Constants.PERUSAHAAN_KONTENU;
        private byte[] arrIcon;

        public FrmPerusahaan() {
            InitializeComponent();
        }

        private void FrmPerusahaan_Load(object sender, EventArgs e) {
            MySqlConnection con = new MySqlConnection(OswConfig.KONEKSI);
            MySqlCommand command = con.CreateCommand();
            MySqlTransaction trans;

            try {
                // buka koneksi
                con.Open();

                // set transaction
                trans = con.BeginTransaction();
                command.Transaction = trans;

                // Function Code
                OswControlDefaultProperties.setInfo(this, command);

                DataPerusahaan dPerusahaan = new DataPerusahaan(command, this.kode);
                txtNama.EditValue = dPerusahaan.nama;
                txtAlamat.Text = dPerusahaan.alamat;
                txtKota.Text = dPerusahaan.kota;
                txtEmail.Text = dPerusahaan.email;
                txtWebsite.Text = dPerusahaan.website;

                if (dPerusahaan.logo != null)
                {
                    string strfn = Convert.ToString(DateTime.Now.ToFileTime());
                    FileStream fs = new FileStream(strfn,
                                      FileMode.CreateNew, FileAccess.Write);
                    fs.Write(dPerusahaan.logo, 0, dPerusahaan.logo.Length);
                    fs.Flush();
                    fs.Close();
                    picGambar.Image = Image.FromFile(strfn);

                    arrIcon = dPerusahaan.logo;
                }

                // Commit Transaction
                command.Transaction.Commit();
            } catch(MySqlException ex) {
                OswPesan.pesanErrorCatch(ex, command, "PERUSAHAAN");
            } catch(Exception ex) {
                OswPesan.pesanErrorCatch(ex, command, "PERUSAHAAN");
            } finally {
                con.Close();
                try {
                    SplashScreenManager.CloseForm();
                } catch(Exception ex) {
                }
            }
        }

        private void btnSimpan_Click(object sender, EventArgs e) {
            SplashScreenManager.ShowForm(typeof(SplashUtama));
            MySqlConnection con = new MySqlConnection(OswConfig.KONEKSI);
            MySqlCommand command = con.CreateCommand();
            MySqlTransaction trans;

            try {
                // buka koneksi
                con.Open();

                // set transaction
                trans = con.BeginTransaction();
                command.Transaction = trans;

                // Function Code
                // validation
                dxValidationProvider1.SetValidationRule(txtNama, OswValidation.IsNotBlank());
                dxValidationProvider1.SetValidationRule(txtAlamat, OswValidation.IsNotBlank());

                if(!dxValidationProvider1.Validate()) {
                    foreach(Control x in dxValidationProvider1.GetInvalidControls()) {
                        dxValidationProvider1.SetIconAlignment(x, ErrorIconAlignment.MiddleRight);
                    }
                    return;
                }

                String strngNama = txtNama.Text;
                String strngAlamat = txtAlamat.Text;
                String strngTelp = txtKota.Text;
                String strngEmail = txtEmail.Text;
                String strngWebsite = txtWebsite.Text;
                byte[] logo = this.arrIcon;

                DataPerusahaan dPerusahaan = new DataPerusahaan(command, this.kode);
                dPerusahaan.nama = strngNama;
                dPerusahaan.alamat = strngAlamat;
                dPerusahaan.kota = strngTelp;
                dPerusahaan.email = strngEmail;
                dPerusahaan.website = strngWebsite;
                dPerusahaan.logo = logo;


                if(dPerusahaan.isExist) {
                    dPerusahaan.ubah();
                } else {
                    throw new Exception("Data Perusahaan tidak ditemukan");
                }

                // Commit Transaction
                command.Transaction.Commit();

                OswPesan.pesanInfo("Proses penggantian data perusahaan berhasil.");

                this.Close();
            } catch(MySqlException ex) {
                OswPesan.pesanErrorCatch(ex, command, "PERUSAHAAN");
            } catch(Exception ex) {
                OswPesan.pesanErrorCatch(ex, command, "PERUSAHAAN");
            } finally {
                con.Close();
                try {
                    SplashScreenManager.CloseForm();
                } catch(Exception ex) {
                }
            }
        }

        private void btnAmbilGambar_Click(object sender, EventArgs e) {
            try {
                if(this.openFileDialog1.ShowDialog(this) == System.Windows.Forms.DialogResult.OK) {
                    string strFn = this.openFileDialog1.FileName;
                    picGambar.Image = Image.FromFile(strFn);
                    FileInfo fiImage = new FileInfo(strFn);
                    long ImageFileLength = fiImage.Length;
                    FileStream fs = new FileStream(strFn, FileMode.Open,
                                      FileAccess.Read, FileShare.Read);
                    arrIcon = new byte[Convert.ToInt32(ImageFileLength)];
                    int iBytesRead = fs.Read(arrIcon, 0, Convert.ToInt32(ImageFileLength));
                    fs.Close();
                }
            } catch(Exception ex) {
                OswPesan.pesanError(ex.Message);
            }
        }

        private void btnHapusGambar_Click(object sender, EventArgs e) {
            arrIcon = null;
            picGambar.Image = null;
        }
    }
}