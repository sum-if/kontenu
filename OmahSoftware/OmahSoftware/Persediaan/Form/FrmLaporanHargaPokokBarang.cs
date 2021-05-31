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

namespace OmahSoftware.Persediaan {
    public partial class FrmLaporanHargaPokokBarang : DevExpress.XtraEditors.XtraForm {
        private String id = "LAPORANHARGAPOKOKBARANG";
        private String dokumen = "LAPORANHARGAPOKOKBARANG";

        public FrmLaporanHargaPokokBarang() {
            InitializeComponent();
        }

        private void FrmLaporanHargaPokokBarang_Load(object sender, EventArgs e) {
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
                OswControlDefaultProperties.resetAllInput(this);

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
            String strngKodeBarang = txtKodeBarang.Text;
            String strngNamaBarang = txtNamaBarang.Text;

            String query = @"SELECT A.kode AS Kode, A.nama AS Nama, A.nopart AS 'No Part', F.nama AS Rak, B.nama AS Satuan, 
                                    COALESCE(G.nilai,0) AS 'Harga Modal'
                            FROM barang A
                            INNER JOIN barangsatuan B ON A.standarsatuan = B.kode
                            INNER JOIN barangrak F ON A.rak = F.kode
                            LEFT JOIN saldopersediaanhpp G ON A.kode = G.barang
                            WHERE A.kode LIKE @kodebarang AND A.nama LIKE @namabarang
                            ORDER BY A.nama,B.nama";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("kodebarang", awal ? Constants.TIDAK_MUNGKIN : "%" + strngKodeBarang + "%");
            parameters.Add("namabarang", awal ? Constants.TIDAK_MUNGKIN : "%" + strngNamaBarang + "%");

            gridControl = OswGrid.getGridBrowse(gridControl, command, query, parameters,
                                                new String[] { },
                                                new String[] { "Harga Modal" });
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
            String strngKodeBarang = txtKodeBarang.Text;
            String strngNamaBarang = txtNamaBarang.Text;

            Dictionary<string, string> filter = new Dictionary<string, string>();
            filter.Add("Kode Barang", strngKodeBarang);
            filter.Add("Nama Barang", strngNamaBarang);

            Dictionary<string, int> lebar = new Dictionary<string, int>();
            lebar.Add("Kode",40);
            lebar.Add("No Part",40);
            lebar.Add("Rak",30);
            lebar.Add("Satuan",30);
            lebar.Add("Harga Modal",40);

            Tools.cetakLaporan(gridControl, dokumen, filter, lebar, true, 85,
                               new System.Drawing.Printing.Margins(25, 25, 25, 25));
        }
    }
}