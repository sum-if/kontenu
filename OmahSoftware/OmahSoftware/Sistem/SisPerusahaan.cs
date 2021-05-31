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
using OmahSoftware.Sistem;
using DevExpress.XtraEditors.DXErrorProvider;
using System.Net;
using OmahSoftware.Umum;

namespace OmahSoftware.Sistem {
    public partial class SisPerusahaan : DevExpress.XtraEditors.XtraForm {
        private String dokumen = "Perusahaan";
        private byte[] arrIcon;

        public SisPerusahaan() {
            InitializeComponent();
        }

        private void SisPerusahaan_Load(object sender, EventArgs e) {
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

                DataOswPerusahaan dOswPerusahaan = new DataOswPerusahaan(command);
                DataOswSetting dOswSetting = new DataOswSetting(command, "footernorek");

                txtNama.EditValue = dOswPerusahaan.nama;
                txtAlamat.Text = dOswPerusahaan.alamat;
                txtTelepon.Text = dOswPerusahaan.telp;
                txtEmail.Text = dOswPerusahaan.email;
                txtNPWP.Text = dOswPerusahaan.npwp;
                txtAlamatPajak.Text = dOswPerusahaan.alamatpajak;
                txtTipePajak.Text = dOswPerusahaan.tipepajak;
                txtFooterRekening.Text = dOswSetting.isi;

                if(dOswPerusahaan.icon != null) {
                    string strfn = Convert.ToString(DateTime.Now.ToFileTime());
                    FileStream fs = new FileStream(strfn,
                                      FileMode.CreateNew, FileAccess.Write);
                    fs.Write(dOswPerusahaan.icon, 0, dOswPerusahaan.icon.Length);
                    fs.Flush();
                    fs.Close();
                    picGambar.Image = Image.FromFile(strfn);

                    arrIcon = dOswPerusahaan.icon;
                }

                // Commit Transaction
                command.Transaction.Commit();
            } catch(MySqlException ex) {
                OswPesan.pesanErrorCatch(ex, command, dokumen);
            } catch(Exception ex) {
                OswPesan.pesanErrorCatch(ex, command, dokumen);
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
                String strngTelp = txtTelepon.Text;
                String strngEmail = txtEmail.Text;
                String strngNPWP = txtNPWP.Text;
                String strngAlamatPajak = txtAlamatPajak.Text;
                String strngTipePajak = txtTipePajak.Text;
                String strngFooterRekening = txtFooterRekening.Text;
                byte[] icon = this.arrIcon;

                DataOswPerusahaan dOswPerusahaan = new DataOswPerusahaan(command);
                dOswPerusahaan.nama = strngNama;
                dOswPerusahaan.alamat = strngAlamat;
                dOswPerusahaan.telp = strngTelp;
                dOswPerusahaan.email = strngEmail;
                dOswPerusahaan.npwp = strngNPWP;
                dOswPerusahaan.alamatpajak = strngAlamatPajak;
                dOswPerusahaan.tipepajak = strngTipePajak;
                dOswPerusahaan.icon = icon;

                DataOswSetting dOswSetting = new DataOswSetting(command, "footernorek");
                dOswSetting.isi = strngFooterRekening;
                if(dOswSetting.isExist) {
                    dOswSetting.ubah();
                } else {
                    dOswSetting.tambah();
                }

                if(dOswPerusahaan.isExist) {
                    dOswPerusahaan.ubah();
                } else {
                    throw new Exception("Data Perusahaan tidak ditemukan");
                }

                // Commit Transaction
                command.Transaction.Commit();

                OswPesan.pesanInfo("Proses penggantian data perusahaan berhasil.");

                this.Close();
            } catch(MySqlException ex) {
                OswPesan.pesanErrorCatch(ex, command, dokumen);
            } catch(Exception ex) {
                OswPesan.pesanErrorCatch(ex, command, dokumen);
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