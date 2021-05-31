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
    public partial class FrmPersediaan : DevExpress.XtraEditors.XtraForm {
        private String id = "PERSEDIAANBARANG";
        private String dokumen = "PERSEDIAANBARANG";

        public FrmPersediaan() {
            InitializeComponent();
        }

        private void FrmPersediaan_Load(object sender, EventArgs e) {
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

                chkDiBawahStokMinimum.Checked = false;

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

        public void setGrid(MySqlCommand command, bool isLoadForm = false) {
            String query = "";
            Dictionary<String, String> parameters = new Dictionary<String, String>();

            if(isLoadForm) {
                query = @"SELECT '' AS Kode, '' AS Nama, '' AS 'No Part', '' AS Rak, '' AS Satuan, 0.0 + '0' AS 'Stok Minimum', 0.0 + '0' AS Stok";
            } else {
                String strngKode = txtKode.Text;
                String strngNama = txtNama.Text;
                String strngNoPart = txtNoPart.Text;

                if(chkDiBawahStokMinimum.Checked && chkStok.Checked) {
                    query = @"SELECT A.kode AS Kode, A.nama AS Nama, A.nopart AS 'No Part', F.nama AS Rak, G.nama AS Satuan, A.stokminimum AS 'Stok Minimum', COALESCE(D.jumlah,0.0 + '0') AS Stok
                            FROM barang A
                            INNER JOIN barangkategori B ON A.barangkategori= B.kode
                            INNER JOIN barangsatuan G ON A.standarsatuan = G.kode
                            INNER JOIN barangrak F ON A.rak = F.kode
                            LEFT JOIN saldopersediaanaktual D ON A.kode = D.barang
                            WHERE A.kode LIKE @kode AND A.nama LIKE @nama AND A.nopart LIKE @nopart AND COALESCE(D.jumlah,0) < A.stokminimum AND COALESCE(D.jumlah,0) > 0
                            ORDER BY A.kode, A.nama, G.nama,B.nama";
                } else if(chkDiBawahStokMinimum.Checked && chkStok.Checked == false) {
                    query = @"SELECT A.kode AS Kode, A.nama AS Nama, A.nopart AS 'No Part', F.nama AS Rak, G.nama AS Satuan, A.stokminimum AS 'Stok Minimum', COALESCE(D.jumlah,0.0 + '0') AS Stok
                            FROM barang A
                            INNER JOIN barangkategori B ON A.barangkategori= B.kode
                            INNER JOIN barangsatuan G ON A.standarsatuan = G.kode
                            INNER JOIN barangrak F ON A.rak = F.kode
                            LEFT JOIN saldopersediaanaktual D ON A.kode = D.barang
                            WHERE A.kode LIKE @kode AND A.nama LIKE @nama AND A.nopart LIKE @nopart AND COALESCE(D.jumlah,0) < A.stokminimum 
                            ORDER BY A.kode, A.nama, G.nama,B.nama";
                } else if(chkDiBawahStokMinimum.Checked == false && chkStok.Checked) {
                    query = @"SELECT A.kode AS Kode, A.nama AS Nama, A.nopart AS 'No Part', F.nama AS Rak, G.nama AS Satuan, A.stokminimum AS 'Stok Minimum', COALESCE(D.jumlah,0.0 + '0') AS Stok
                            FROM barang A
                            INNER JOIN barangkategori B ON A.barangkategori= B.kode
                            INNER JOIN barangsatuan G ON A.standarsatuan = G.kode
                            INNER JOIN barangrak F ON A.rak = F.kode
                            LEFT JOIN saldopersediaanaktual D ON A.kode = D.barang
                            WHERE A.kode LIKE @kode AND A.nama LIKE @nama AND A.nopart LIKE @nopart AND COALESCE(D.jumlah,0) > 0
                            ORDER BY A.kode, A.nama, G.nama,B.nama";
                } else {
                    query = @"SELECT A.kode AS Kode, A.nama AS Nama, A.nopart AS 'No Part', F.nama AS Rak, G.nama AS Satuan, A.stokminimum AS 'Stok Minimum', COALESCE(D.jumlah,0.0 + '0') AS Stok
                            FROM barang A
                            INNER JOIN barangkategori B ON A.barangkategori= B.kode
                            INNER JOIN barangsatuan G ON A.standarsatuan = G.kode
                            INNER JOIN barangrak F ON A.rak = F.kode
                            LEFT JOIN saldopersediaanaktual D ON A.kode = D.barang
                            WHERE A.kode LIKE @kode AND A.nama LIKE @nama AND A.nopart LIKE @nopart 
                            ORDER BY A.kode, A.nama, G.nama,B.nama";
                }

                parameters = new Dictionary<String, String>();
                parameters.Add("kode", "%" + strngKode + "%");
                parameters.Add("nama", "%" + strngNama + "%");
                parameters.Add("nopart", "%" + strngNoPart + "%");
            }

            gridControl = OswGrid.getGridBrowse(gridControl, command, query, parameters,
                                                new String[] { },
                                                new String[] { "Stok Minimum", "Stok" });
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