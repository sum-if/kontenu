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
using Kontenu.Sistem;
using DevExpress.XtraEditors.DXErrorProvider;
using System.Net;
using Kontenu.Umum;
using Kontenu.OswLib;

namespace Kontenu.Sistem {
    public partial class SisLogError : DevExpress.XtraEditors.XtraForm {
        private String id = "LOGERROR";
        private String dokumen = "Log Error";

        public SisLogError() {
            InitializeComponent();
        }

        private void MstUser_Load(object sender, EventArgs e) {
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
                OswControlDefaultProperties.setBrowse(this, id, command);
                OswControlDefaultProperties.setTanggal(deTanggalAwal);
                OswControlDefaultProperties.setTanggal(deTanggalAkhir);
                deTanggalAwal.DateTime = OswDate.getDateTimeTanggalAwalBulan();
                deTanggalAkhir.DateTime = DateTime.Now;
                cmbUser = ComboQueryUmum.getUser(cmbUser, command, true);
                cmbUser.EditValue = OswCombo.getFirstEditValue(cmbUser);

                this.setGrid(command);

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

        public void setGrid(MySqlCommand command) {
            String strngTanggalAwal = deTanggalAwal.Text;
            String strngTanggalAkhir = deTanggalAkhir.Text;
            String strngUser = cmbUser.EditValue.ToString();
            String strngHostname = txtHostname.Text;
            String strngIP = txtIP.Text;
            String strngDokumen = txtDokumen.Text;
            String strngMessage = txtMessage.Text;

            String query = @"SELECT A.user AS 'Kode User', B.nama AS 'Nama', C.nama AS Usergroup, A.hostname AS Hostname, A.ip AS IP, A.dokumen AS Dokumen, 
                                    A.message AS Message, A.stacktrace AS Stacktrace, CAST(A.waktu AS CHAR) AS Waktu
                            FROM oswlogerror A
                            LEFT JOIN oswuser B ON A.user = B.kode
                            LEFT JOIN oswusergroup C ON B.usergroup = C.kode
                            WHERE A.user LIKE @user AND A.hostname LIKE @hostname AND A.ip LIKE @ip AND DATE(A.waktu) BETWEEN toDate(@tanggalawal) AND toDate(@tanggalakhir) AND
                                  A.dokumen LIKE @dokumen AND A.message LIKE @message
                            ORDER BY A.waktu DESC";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("tanggalawal", strngTanggalAwal);
            parameters.Add("tanggalakhir", strngTanggalAkhir);
            parameters.Add("user", "%" + strngUser + "%");
            parameters.Add("hostname", "%" + strngHostname + "%");
            parameters.Add("ip", "%" + strngIP + "%");
            parameters.Add("dokumen", "%" + strngDokumen + "%");
            parameters.Add("message", "%" + strngMessage + "%");
            

            gridControl = OswGrid.getGridBrowse(gridControl, command, query, parameters, 
                                                new String[] { }, 
                                                new String[] { });
        }

        private void btnCari_Click(object sender, EventArgs e) {
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
                this.setGrid(command);

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

        private void btnCetak_Click(object sender, EventArgs e) {
            Tools.cetakBrowse(gridControl, dokumen);
        }
    }
}