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

namespace OmahSoftware.Akuntansi {
    public partial class FrmLaporanOmsetSales : DevExpress.XtraEditors.XtraForm {
        private String id = "LAPORANOMSETSALES";
        private String dokumen = "LAPORANOMSETSALES";

        public FrmLaporanOmsetSales() {
            InitializeComponent();
        }

        private void FrmLaporanOmsetSales_Load(object sender, EventArgs e) {
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

                OswControlDefaultProperties.setBrowse(this, id, command);
                OswControlDefaultProperties.setTanggal(deTanggalAwal);
                OswControlDefaultProperties.setTanggal(deTanggalAkhir);
                deTanggalAwal.DateTime = OswDate.getDateTimeTanggalAwalBulan();
                deTanggalAkhir.DateTime = OswDate.getDateTimeTanggalHariIni();

                cmbSales = ComboQueryUmum.getSales(cmbSales, command);
                cmbSales.EditValue = OswCombo.getFirstEditValue(cmbSales);

                OswControlDefaultProperties.setAngka(txtKomisi);
                txtKomisi.EditValue = "0.5";

                this.setGrid(command, true);

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

        public void setGrid(MySqlCommand command, bool awal = false) {
            String strngTanggalAwal = deTanggalAwal.Text;
            String strngTanggalAkhir = deTanggalAkhir.Text;
            String strngKomisi = txtKomisi.EditValue.ToString();
            String strngSales = cmbSales.EditValue.ToString();

            String query = @"SELECT A.tanggal AS Tanggal, A.kode AS 'Faktur', CONCAT(D.nama,' - ', E.nama) AS Customer, A.grandtotal AS 'Grand Total', (A.grandtotal * @komisi) / 100 AS Komisi
                            FROM fakturpenjualan A
                            INNER JOIN pesananpenjualan B ON A.pesananpenjualan = B.kode
                            INNER JOIN customer D ON A.customer = D.kode
                            INNER JOIN kota E ON D.kota = E.kode
                            WHERE A.jenispesananpenjualan = @jenisPesananPenjualan AND A.status = @statusLunas AND B.sales = @sales AND toDate(A.tanggal) BETWEEN toDate(@tanggalAwal) AND toDate(@tanggalAkhir)
                            ORDER BY toDate(A.tanggal) DESC, A.kode DESC";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("tanggalawal", strngTanggalAwal);
            parameters.Add("tanggalakhir", strngTanggalAkhir);
            parameters.Add("sales", awal ? Constants.TIDAK_MUNGKIN : strngSales);
            parameters.Add("komisi", strngKomisi);
            parameters.Add("jenisPesananPenjualan", Constants.JENIS_PESANAN_PENJUALAN_PESANAN_PENJUALAN);
            parameters.Add("statusLunas", Constants.STATUS_FAKTUR_PENJUALAN_LUNAS);

            gridControl = OswGrid.getGridBrowse(gridControl, command, query, parameters,
                                                new String[] { },
                                                new String[] { "Grand Total", "Komisi" });

            gridView.OptionsView.ShowFooter = true;
            gridView.Columns["Grand Total"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            gridView.Columns["Grand Total"].SummaryItem.DisplayFormat = "{0:N2}";
            gridView.Columns["Komisi"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            gridView.Columns["Komisi"].SummaryItem.DisplayFormat = "{0:N2}";
        }

        private void btnFilter_Click(object sender, EventArgs e) {
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
            String strngTanggalAwal = deTanggalAwal.Text;
            String strngTanggalAkhir = deTanggalAkhir.Text;
            String strngKomisi = txtKomisi.EditValue.ToString();
            String strngSales = cmbSales.EditValue.ToString() + " - " + cmbSales.Text;

            Dictionary<string, string> filter = new Dictionary<string, string>();
            filter.Add("Tanggal Awal", strngTanggalAwal);
            filter.Add("Tanggal Akhir", strngTanggalAkhir);
            filter.Add("Sales", strngSales);
            filter.Add("Komisi", strngKomisi);

            Dictionary<string, int> lebar = new Dictionary<string, int>();
            lebar.Add("Tanggal", 100);
            lebar.Add("Faktur", 140);
            lebar.Add("Grand Total", 160);
            lebar.Add("Komisi", 150);

            Tools.cetakLaporan(gridControl, dokumen, filter, lebar, false, 85, 
                               new System.Drawing.Printing.Margins(25, 25, 25, 25));
        }
    }
}