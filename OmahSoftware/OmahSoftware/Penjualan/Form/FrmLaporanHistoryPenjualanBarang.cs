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

namespace OmahSoftware.Penjualan {
    public partial class FrmLaporanHistoryPenjualanBarang : DevExpress.XtraEditors.XtraForm {
        private String id = "LAPORANHISTORYPENJUALANBARANG";
        private String dokumen = "LAPORANHISTORYPENJUALANBARANG";
        private String strngKodeBarang = "%";
        private String strngNamaBarang = "Semua";

        public FrmLaporanHistoryPenjualanBarang() {
            InitializeComponent();
        }

        private void FrmLaporanHistoryPenjualanBarang_Load(object sender, EventArgs e) {
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

                txtBarang.Text = strngNamaBarang;

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

            String query = @"SELECT A.tanggal AS Tanggal, A.kode AS 'Faktur Penjualan', E.nama AS customer, F.nama AS Kota,
                                    B.barang AS 'Kode Barang', C.nama AS 'Nama Barang',B.jumlahfaktur AS Jumlah, D.nama AS Satuan, 
                                    B.hargajual AS 'Harga Jual',B.diskonitem + B.diskonfaktur AS Diskon, B.dpp AS DPP, B.ppn AS PPN, ROUND(B.dpp * B.jumlahfaktur,2) AS 'Total DPP', ROUND(B.ppn * B.jumlahfaktur,2) AS 'Total PPN', B.subtotal AS Subtotal
                            FROM fakturpenjualan A
                            INNER JOIN fakturpenjualandetail B ON A.kode = B.fakturpenjualan
                            INNER JOIN barang C ON B.barang = C.kode
                            INNER JOIN barangsatuan D ON B.satuan = D.kode
                            INNER JOIN customer E ON A.customer = E.kode
                            INNER JOIN kota F ON E.kota = F.kode
                            WHERE B.barang LIKE @barang AND toDate(A.tanggal) BETWEEN toDate(@tanggalAwal) AND toDate(@tanggalAkhir)
                            ORDER BY toDate(A.tanggal) DESC, B.barang, A.kode DESC";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("tanggalawal", strngTanggalAwal);
            parameters.Add("tanggalakhir", strngTanggalAkhir);
            parameters.Add("barang", awal ? Constants.TIDAK_MUNGKIN : strngKodeBarang);

            gridControl = OswGrid.getGridBrowse(gridControl, command, query, parameters,
                                                new String[] { },
                                                new String[] { "Jumlah", "Harga Jual", "Diskon", "DPP", "PPN", "Total DPP", "Total PPN", "Subtotal" });

            gridView.OptionsView.ShowFooter = true;
            gridView.Columns["DPP"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            gridView.Columns["DPP"].SummaryItem.DisplayFormat = "{0:N2}";
            gridView.Columns["PPN"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            gridView.Columns["PPN"].SummaryItem.DisplayFormat = "{0:N2}";
            gridView.Columns["Subtotal"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            gridView.Columns["Subtotal"].SummaryItem.DisplayFormat = "{0:N2}";

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

        private void btnCari_Click(object sender, EventArgs e) {
            infoBarang();
        }

        private void infoBarang() {
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

                String query = @"SELECT *
                                FROM (
                                    SELECT 10 AS Urutan, '%' AS Kode, 'Semua' AS Nama, '' AS Kategori, '' AS 'No Part', '' AS Rak, '' AS 'Kode Satuan', '' AS Satuan
                                    UNION
                                    (SELECT 100 AS Urutan, A.kode AS Kode, A.nama AS Nama, C.nama AS Kategori, A.nopart AS 'No Part', D.nama AS Rak, B.kode AS 'Kode Satuan', B.nama AS Satuan
                                    FROM barang A
                                    INNER JOIN barangsatuan B ON A.standarsatuan = B.kode
                                    INNER JOIN barangkategori C ON A.barangkategori = C.kode
                                    INNER JOIN barangrak D ON A.rak = D.kode
                                    WHERE A.status = @status
                                    ORDER BY C.nama,A.nama,B.nama)
                                ) Z
                                ORDER BY Z.urutan, Z.kode";

                Dictionary<String, String> parameters = new Dictionary<String, String>();
                parameters.Add("status", Constants.STATUS_AKTIF);

                InfUtamaDictionary form = new InfUtamaDictionary("Info Barang", query, parameters,
                                                                new String[] { "Kode", "Nama" },
                                                                new String[] { "Urutan" },
                                                                new String[] { });
                this.AddOwnedForm(form);
                form.ShowDialog();
                if(!form.hasil.ContainsKey("Kode")) {
                    return;
                }

                strngKodeBarang = form.hasil["Kode"];
                strngNamaBarang = form.hasil["Nama"];

                txtBarang.Text = strngNamaBarang;

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
            btnFilter.PerformClick();

            String strngTanggalAwal = deTanggalAwal.Text;
            String strngTanggalAkhir = deTanggalAkhir.Text;

            Dictionary<string, string> filter = new Dictionary<string, string>();
            filter.Add("Tanggal Awal", strngTanggalAwal);
            filter.Add("Tanggal Akhir", strngTanggalAkhir);
            filter.Add("Kode Barang", strngKodeBarang);
            filter.Add("Nama Barang", strngNamaBarang);

            Dictionary<string, int> lebar = new Dictionary<string, int>();

            Tools.cetakLaporan(gridControl, dokumen, filter, lebar, true, 85,
                               new System.Drawing.Printing.Margins(25, 25, 25, 25));
        }
    }
}