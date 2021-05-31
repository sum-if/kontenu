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
    public partial class FrmLaporanPendingOrderBarang : DevExpress.XtraEditors.XtraForm {
        private String id = "LAPORANPENDINGORDERBARANG";
        private String dokumen = "LAPORANPENDINGORDERBARANG";
        private String strngKodeBarang = "%";
        private String strngNamaBarang = "Semua";

        public FrmLaporanPendingOrderBarang() {
            InitializeComponent();
        }

        private void FrmLaporanPendingOrderBarang_Load(object sender, EventArgs e) {
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
            String query = @"SELECT A.tanggal AS Tanggal, A.kode AS 'Pesanan Penjualan', E.nama AS Customer, F.nama AS Kota,
                                    B.barang AS 'Kode Barang', C.nama AS 'Nama Barang',B.jumlahpesan AS 'Pesan', B.jumlahfaktur AS 'Faktur', B.jumlahpesan-B.jumlahfaktur AS 'Sisa', 
                                    D.nama AS Satuan, 
                                    B.hargajual AS 'Harga Jual',B.diskonitem + B.diskonfaktur AS Diskon
                            FROM pesananpenjualan A
                            INNER JOIN pesananpenjualandetail B ON A.kode = B.pesananpenjualan
                            INNER JOIN barang C ON B.barang = C.kode
                            INNER JOIN barangsatuan D ON B.satuan = D.kode
                            INNER JOIN customer E ON A.customer = E.kode
                            INNER JOIN kota F ON E.kota = F.kode
                            WHERE B.barang LIKE @barang AND B.jumlahpesan > B.jumlahfaktur
                            ORDER BY toDate(A.tanggal) DESC, B.barang, A.kode DESC";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("barang", awal ? Constants.TIDAK_MUNGKIN : strngKodeBarang);

            gridControl = OswGrid.getGridBrowse(gridControl, command, query, parameters,
                                                new String[] { },
                                                new String[] { "Pesan", "Faktur", "Sisa", "Harga Jual", "Diskon" });

            gridView.OptionsView.ShowFooter = true;
            gridView.Columns["Pesan"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            gridView.Columns["Pesan"].SummaryItem.DisplayFormat = "{0:N2}";
            gridView.Columns["Faktur"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            gridView.Columns["Faktur"].SummaryItem.DisplayFormat = "{0:N2}";
            gridView.Columns["Sisa"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            gridView.Columns["Sisa"].SummaryItem.DisplayFormat = "{0:N2}";
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
                String query = @"SELECT A.kode AS Kode, A.nama AS Nama, C.nama AS Kategori, A.nopart AS 'No Part', D.nama AS Rak, B.kode AS 'Kode Satuan', B.nama AS Satuan
                                FROM barang A
                                INNER JOIN barangsatuan B ON A.standarsatuan = B.kode
                                INNER JOIN barangkategori C ON A.barangkategori = C.kode
                                INNER JOIN barangrak D ON A.rak = D.kode
                                WHERE A.status = @status
                                ORDER BY C.nama,A.nama,B.nama";

                Dictionary<String, String> parameters = new Dictionary<String, String>();
                parameters.Add("status", Constants.STATUS_AKTIF);

                InfUtamaDictionary form = new InfUtamaDictionary("Info Barang", query, parameters,
                                                                new String[] { "Kode", "Nama" },
                                                                new String[] { },
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

            Dictionary<string, string> filter = new Dictionary<string, string>();
            filter.Add("Kode Barang", strngKodeBarang);
            filter.Add("Nama Barang", strngNamaBarang);

            Dictionary<string, int> lebar = new Dictionary<string, int>();
            lebar.Add("Tanggal",80);
            lebar.Add("Pesan", 70);
            lebar.Add("Faktur", 70);
            lebar.Add("Sisa", 70);
            lebar.Add("Satuan", 60);
            lebar.Add("Harga Jual", 95);
            lebar.Add("Diskon", 80);

            Tools.cetakLaporan(gridControl, dokumen, filter, lebar, true, 85, 
                               new System.Drawing.Printing.Margins(25, 25, 25, 25));
        }
    }
}