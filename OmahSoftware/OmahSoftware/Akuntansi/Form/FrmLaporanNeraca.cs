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
using OmahSoftware.Akuntansi.Laporan;
using OmahSoftware.Umum.Laporan;

namespace OmahSoftware.Akuntansi {
    public partial class FrmLaporanNeraca : DevExpress.XtraEditors.XtraForm {
        private String id = "LAPORANNERACA";
        private String dokumen = "LAPORANNERACA";

        public FrmLaporanNeraca() {
            InitializeComponent();
        }

        private void FrmLaporanNeraca_Load(object sender, EventArgs e) {
            MySqlConnection con = new MySqlConnection(OswConfig.KONEKSI);
            MySqlCommand command = con.CreateCommand();
            MySqlTransaction trans;

            try {
                // buka koneksi6
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

                // validasi harus sudah tutup periode
                DataAdmin dAdmin = new DataAdmin(command, strngTahun + strngBulan, Constants.PROSES_TUTUP_PERIODE);
                //if(!dAdmin.isExist) {
                //    throw new Exception("Bulan belum ditutup");
                //}

                String strngTanggal = OswDate.getTanggal(OswDate.getStringTanggalAkhirBulan(strngTahun+strngBulan));
                String strngSubJudul = "per " + strngTanggal + " " + cmbBulan.Text + " " + strngTahun;

                String totalAset = getNilaiAset(command, strngTahun + strngBulan);
                String totalNonAset = getNilaiNonAset(command, strngTahun + strngBulan);

                RptNeraca report = new RptNeraca();

                DataOswPerusahaan dPerusahaan = new DataOswPerusahaan(command);
                report.Parameters["GUID"].Value = OswConstants.GUID;
                report.Parameters["periode"].Value = strngTahun + strngBulan;
                report.Parameters["namaPerusahaan"].Value = dPerusahaan.nama.ToUpper();
                report.Parameters["judulReport"].Value = "LAPORAN NERACA";
                report.Parameters["subJudul"].Value = strngSubJudul;
                report.Parameters["totalaset"].Value = totalAset;
                report.Parameters["totalnonaset"].Value = totalNonAset;

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

        private string getNilaiAset(MySqlCommand command, String periode) {
            String query = @"SELECT SUM(COALESCE(F.akhirdebit,0) - COALESCE(F.akhirkredit,0)) AS saldo
                            FROM akun A
                            INNER JOIN akunkategori B ON A.akunkategori = B.kode
                            LEFT JOIN saldoakunbulanan F ON A.kode = F.akun AND F.periode = @periode
                            WHERE B.kode IN ('1000.00')";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("periode", periode);

            return OswDataAccess.executeScalarQuery(query, parameters, command);
        }

        private string getNilaiNonAset(MySqlCommand command, String periode) {
            String query = @"SELECT SUM(CASE WHEN A.saldonormalneraca = 'Debit' THEN COALESCE(F.akhirdebit,0) - COALESCE(F.akhirkredit,0) ELSE COALESCE(F.akhirkredit,0) - COALESCE(F.akhirdebit,0) END) AS saldo
                            FROM akun A
                            INNER JOIN akunkategori B ON A.akunkategori = B.kode
                            LEFT JOIN saldoakunbulanan F ON A.kode = F.akun AND F.periode = @periode
                            WHERE B.kode IN ('2000.00', '3000.00')";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("periode", periode);

            return OswDataAccess.executeScalarQuery(query, parameters, command);
        }

    }
}