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
    public partial class SisDatabaseBackup : DevExpress.XtraEditors.XtraForm {
        private String dokumen = "Backup Database";

        public SisDatabaseBackup() {
            InitializeComponent();
        }

        private void SisDatabaseBackup_Load(object sender, EventArgs e) {
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

        private void btnBackup_Click(object sender, EventArgs e) {
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
                if(beFile.Text == "") {
                    OswPesan.pesanError("Nama File Harus Diisi");
                    return;
                }

                // proses backup
                MySqlBackup mb = new MySqlBackup(command);
                //mb.ExportInfo.ScriptsDelimiter = ";;";
                mb.ExportInfo.AddCreateDatabase = true;
                mb.ExportToFile(beFile.Text);


                OswLog.setTransaksi(command, dokumen, "Backup Database");

                OswPesan.pesanInfo("Proses Backup Berhasil");

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
            if(saveFileDialog.ShowDialog() == DialogResult.OK) {
                beFile.Text = saveFileDialog.FileName;
            }
        }
    }
}