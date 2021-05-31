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
using OmahSoftware.OswLib;
using OmahSoftware.Umum;
using OmahSoftware.Pembelian;
using OmahSoftware.Akuntansi.Laporan;
using OmahSoftware.Umum.Laporan;
using OmahSoftware.Penjualan;

namespace OmahSoftware.Akuntansi {
    public partial class FrmLaporanArusKas : DevExpress.XtraEditors.XtraForm {
        private String id = "LAPORANARUSKAS";
        private String dokumen = "LAPORANARUSKAS";

        public FrmLaporanArusKas() {
            InitializeComponent();
        }

        private void FrmLaporanArusKas_Load(object sender, EventArgs e) {
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
                DataOswJenisDokumen dOswJenisDokumen = new DataOswJenisDokumen(command, id);
                this.dokumen = dOswJenisDokumen.nama;
                this.Text = this.dokumen;

                OswControlDefaultProperties.setInput(this, id, command);

                cmbBulan = ComboConstantUmum.getBulan(cmbBulan);
                cmbBulan.EditValue = OswDate.getBulan(OswDate.getStringTanggalHariIni());

                cmbTahun = ComboConstantUmum.getTahun(cmbTahun, 1, 0);
                cmbTahun.EditValue = OswDate.getTahun(OswDate.getStringTanggalHariIni());

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
                String strngBulan = cmbBulan.EditValue.ToString();
                String strngTahun = cmbTahun.EditValue.ToString();
                String strngTanggal = "01/" + strngBulan + "/" + strngTahun;
                DateTime dtTanggal = OswDate.getDateTimeFromStringTanggal(strngTanggal);
                String strngNamaBulan = OswDate.getNamaBulan(dtTanggal);

                RptArusKas report = new RptArusKas();
                report.Parameters["GUID"].Value = OswConstants.GUID;
                report.Parameters["judulReport"].Value = "LAPORAN ARUS KAS";
                report.Parameters["Periode"].Value = strngTahun + strngBulan;
                report.Parameters["tanggalAwalAkhir"].Value = "PERIODE " + strngNamaBulan + " " + strngTahun;

                //// Assign the printing system to the document viewer.
                LaporanPrintPreview laporan = new LaporanPrintPreview();
                laporan.documentViewer1.DocumentSource = report;

                OswLog.setLaporan(command, dokumen);

                laporan.Show();

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
    }
}