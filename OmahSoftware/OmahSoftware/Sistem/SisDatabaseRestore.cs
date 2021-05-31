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
    public partial class SisDatabaseRestore : DevExpress.XtraEditors.XtraForm {
        private String dokumen = "Restore Database";

        public SisDatabaseRestore() {
            InitializeComponent();
        }

        private void SisDatabaseRestore_Load(object sender, EventArgs e) {
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
            } catch (MySqlException ex) {
                OswPesan.pesanErrorCatch(ex, command, dokumen);
            } catch (Exception ex) {
                OswPesan.pesanErrorCatch(ex, command, dokumen);
            } finally {
                con.Close();
                try {
                    SplashScreenManager.CloseForm();
                } catch(Exception ex) {
                }
            }
        }

        private void btnRestore_Click(object sender, EventArgs e) {
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
                if(!File.Exists(beFile.Text)) {
                    OswPesan.pesanError("File tidak ditemukan");
                    return;
                }

                // proses backup
                MySqlBackup mb = new MySqlBackup(command);
                mb.ImportFromFile(beFile.Text);

                OswLog.setTransaksi(command, dokumen, "Restore Database");

                OswPesan.pesanInfo("Proses Restore Berhasil");


                SisLogin.IsLogin = 0;
                SisUtama.IsLogout = 1;
                Application.Exit();

                // Commit Transaction
                command.Transaction.Commit();
            } catch (MySqlException ex) {
                OswPesan.pesanErrorCatch(ex, command, dokumen);
            } catch (Exception ex) {
                OswPesan.pesanErrorCatch(ex, command, dokumen);
            } finally {
                con.Close();
                try {
                    SplashScreenManager.CloseForm();
                } catch(Exception ex) {
                }
            }
            this.Close();
        }

        private void beFile_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e) {
            if(openFileDialog.ShowDialog() == DialogResult.OK) {
                beFile.Text = openFileDialog.FileName;
            }
        }
    }
}