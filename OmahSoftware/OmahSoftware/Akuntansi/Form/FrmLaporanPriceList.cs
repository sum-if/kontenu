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
    public partial class FrmLaporanPriceList : DevExpress.XtraEditors.XtraForm {
        private String id = "LAPORANPRICELIST";
        private String dokumen = "LAPORANPRICELIST";

        public FrmLaporanPriceList() {
            InitializeComponent();
        }

        private void FrmLaporanPriceList_Load(object sender, EventArgs e) {
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
                cmbKategori = ComboQueryUmum.getBarangKategori(cmbKategori, command, true);

                OswControlDefaultProperties.resetAllInput(this);

                cmbKategori.EditValue = OswCombo.getFirstEditValue(cmbKategori);

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
            String strngKategoriBarang = cmbKategori.EditValue.ToString();
            String strngKodeBarang = txtKodeBarang.Text;
            String strngNamaBarang = txtNamaBarang.Text;

            String query = @"SELECT B.kode AS 'Kode Kategori', B.nama AS Kategori, A.kode AS 'Kode Barang', A.nama AS Barang, A.nopart AS 'No Part', C.nama AS Satuan, 
                                    A.hargajual1 AS 'Harga Jual', A.hargajual2 AS 'Harga Pokok'
                            FROM barang A
                            INNER JOIN barangkategori B ON A.barangkategori = B.kode
                            INNER JOIN barangsatuan C ON A.standarsatuan = C.kode
                            WHERE A.status = 'Aktif' AND A.barangkategori LIKE @kategori AND A.kode LIKE @kodebarang AND A.nama LIKE @namabarang
                            ORDER BY B.nama, A.nama";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("kategori", awal ? Constants.TIDAK_MUNGKIN : strngKategoriBarang);
            parameters.Add("kodebarang", awal ? Constants.TIDAK_MUNGKIN : "%" + strngKodeBarang + "%");
            parameters.Add("namabarang", awal ? Constants.TIDAK_MUNGKIN : "%" + strngNamaBarang + "%");

            gridControl = OswGrid.getGridBrowse(gridControl, command, query, parameters,
                                                new String[] { "Kode Kategori", "Kode Barang" },
                                                new String[] { "Harga Jual", "Harga Pokok" });
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
            String strngKategoriBarang = cmbKategori.EditValue.ToString();
            String strngKodeBarang = txtKodeBarang.Text;
            String strngNamaBarang = txtNamaBarang.Text;

            Dictionary<string, string> filter = new Dictionary<string, string>();
            filter.Add("Kategori Barang", strngKategoriBarang);
            filter.Add("Kode Barang", strngKodeBarang);
            filter.Add("Nama Barang", strngNamaBarang);

            Dictionary<string, int> lebar = new Dictionary<string, int>();
            lebar.Add("Kategori", 95);
            lebar.Add("No Part", 65);
            lebar.Add("Satuan", 45);
            lebar.Add("Harga Jual", 75);
            lebar.Add("Harga Pokok", 75);

            Tools.cetakLaporan(gridControl, dokumen, filter, lebar, false, 85,
                               new System.Drawing.Printing.Margins(25, 25, 25, 25));
        }

        private void btnCetakPriceList_Click(object sender, EventArgs e) {
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
                String strngKodeKategori = cmbKategori.EditValue.ToString();

                RptLaporanPriceList report = new RptLaporanPriceList();
                report.Parameters["kodekategori"].Value = strngKodeKategori;
                report.Parameters["waktucetak"].Value = "Tanggal Cetak: " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");

                // Assign the printing system to the document viewer.
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