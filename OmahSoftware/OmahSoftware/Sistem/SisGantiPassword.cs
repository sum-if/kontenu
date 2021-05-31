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
    public partial class SisGantiPassword : DevExpress.XtraEditors.XtraForm {
        private String dokumen = "Ganti Password";

        public SisGantiPassword() {
            InitializeComponent();
        }

        private void SisGantiPassword_Load(object sender, EventArgs e) {
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
                dxValidationProvider1.SetValidationRule(txtPasswordLama, OswValidation.isMinimalString(5));
                dxValidationProvider1.SetValidationRule(txtPasswordBaru, OswValidation.isMinimalString(5));
                dxValidationProvider1.SetValidationRule(txtPasswordBaruKonfirmasi, OswValidation.isMinimalString(5));

                if(!dxValidationProvider1.Validate()) {
                    foreach(Control x in dxValidationProvider1.GetInvalidControls()) {
                        dxValidationProvider1.SetIconAlignment(x, ErrorIconAlignment.MiddleRight);
                    }
                    return;
                }

                string strngPasswordLama = txtPasswordLama.Text;
                string strngPasswordBaru = txtPasswordBaru.Text;
                string strngPasswordBaruKonfirmasi = txtPasswordBaruKonfirmasi.Text;

                // cek apakah konfirmasi password baru = password baru
                if(strngPasswordBaru != strngPasswordBaruKonfirmasi) {
                    throw new Exception("Password Baru dan Konfirmasi Password Baru harus sama");
                }

                // cek apakah password lama yang di database = yang di input
                DataOswUser dOswUser = new DataOswUser(command, OswConstants.KODEUSER);
                if(!OswConvert.checkBCrypt(strngPasswordLama, dOswUser.password)) {
                    throw new Exception("Password Lama tidak sesuai");
                }

                dOswUser.password = OswConvert.convertToBCrypt(strngPasswordBaru);
                dOswUser.ubah();

                // Commit Transaction
                command.Transaction.Commit();

                OswPesan.pesanInfo("Proses penggantian password berhasil.");

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
    }
}