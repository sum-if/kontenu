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
using OmahSoftware.Pembelian.Laporan;
using OmahSoftware.Umum.Laporan;

namespace OmahSoftware.Pembelian {
    public partial class FrmPajakMasukan : DevExpress.XtraEditors.XtraForm {
        private String id = "PAJAKMASUKAN";
        private String dokumen = "PAJAKMASUKAN";

        public FrmPajakMasukan() {
            InitializeComponent();
        }

        private void FrmPajakMasukan_Load(object sender, EventArgs e) {
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

                cmbSupplier = ComboQueryUmum.getSupplier(cmbSupplier, command, true);
                cmbSupplier.EditValue = OswCombo.getFirstEditValue(cmbSupplier);

                cmbBulan = ComboConstantUmum.getBulan(cmbBulan);
                cmbBulan.EditValue = OswDate.getBulan(OswDate.getStringTanggalHariIni());

                cmbTahun = ComboConstantUmum.getTahun(cmbTahun, 2, 0);
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
                String strngPeriode = strngTahun + strngBulan;
                String strngSupplier = cmbSupplier.EditValue.ToString();
                String strngBelumUpload = chkUpload.Checked ? "1" : "0";

                RptCetakPajakMasukan report = new RptCetakPajakMasukan();
                report.Parameters["Periode"].Value = strngPeriode;
                report.Parameters["Supplier"].Value = strngSupplier;
                report.Parameters["BelumUpload"].Value = strngBelumUpload;

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