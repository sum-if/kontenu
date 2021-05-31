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

namespace OmahSoftware.Umum {
    public partial class FrmBarang : DevExpress.XtraEditors.XtraForm {
        private String id = "BARANG";
        private String dokumen = "BARANG";

        public FrmBarang() {
            InitializeComponent();
        }

        private void FrmBarang_Load(object sender, EventArgs e) {
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
                cmbKategori.ItemIndex = 0;

                cmbStatus = ComboConstantUmum.getStatusBarangAktif(cmbStatus, true);
                cmbStatus.ItemIndex = 0;

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
            String strngKode = txtKode.Text;
            String strngNama = txtNama.Text;
            String strngNoPart = txtNoPart.Text;
            String strngKategori = cmbKategori.ItemIndex < 0 ? "" : cmbKategori.EditValue.ToString();
            String strngStatus = cmbStatus.ItemIndex < 0 ? "" : cmbStatus.EditValue.ToString();
            String query = @"SELECT A.kode AS Kode, A.nama AS Nama, E.nama AS Satuan, A.nopart AS 'No Part', C.nama AS Kategori,
                                    A.stokminimum AS 'Stok Minimum', A.hargajual1 AS 'Harga Jual 1', A.hargajual2 AS 'Harga Jual 2', F.nama AS Rak, A.status AS 'Status'
                            FROM barang A
                            INNER JOIN barangkategori C ON A.barangkategori = C.kode
                            INNER JOIN barangsatuan E ON A.standarsatuan = E.kode
                            INNER JOIN barangrak F ON A.rak = F.kode
                            WHERE A.kode LIKE @kode AND A.nama LIKE @nama AND A.nopart LIKE @nopart AND A.barangkategori LIKE @kategori AND A.status LIKE @status
                            ORDER BY A.kode";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("kode", "%" + strngKode + "%");
            parameters.Add("nama", "%" + strngNama + "%");
            parameters.Add("nopart", "%" + strngNoPart + "%");
            parameters.Add("kategori", strngKategori);
            parameters.Add("status", strngStatus);

            gridControl = OswGrid.getGridBrowse(gridControl, command, query, parameters,
                                                new String[] { },
                                                new String[] { "Stok Minimum", "Harga Jual 1", "Harga Jual 2" });
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

        private void btnUbah_Click(object sender, EventArgs e) {
            FrmBarangAdd form = new FrmBarangAdd(false);
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
                if(gridView.GetSelectedRows().Length == 0) {
                    OswPesan.pesanError("Silahkan pilih " + dokumen + " yang akan diubah.");
                    return;
                }

                String strngKode = gridView.GetRowCellValue(gridView.FocusedRowHandle, "Kode").ToString();

                DataBarang dBarang = new DataBarang(command, strngKode);
                if(!dBarang.isExist) {
                    throw new Exception(dokumen + " tidak ditemukan.");
                }

                this.AddOwnedForm(form);
                form.txtKode.Text = strngKode;

                // Commit Transaction
                command.Transaction.Commit();
            } catch(MySqlException ex) {
                OswPesan.pesanErrorCatch(ex, command, dokumen);
            } catch(Exception ex) {
                OswPesan.pesanErrorCatch(ex, command, dokumen);
            } finally {
                con.Close();
            }

            form.ShowDialog();
        }

        private void btnTambah_Click(object sender, EventArgs e) {
            FrmBarangAdd form = new FrmBarangAdd(true);
            this.AddOwnedForm(form);
            form.ShowDialog();
        }

        private void btnHapus_Click(object sender, EventArgs e) {
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
                if(gridView.GetSelectedRows().Length == 0) {
                    OswPesan.pesanError("Silahkan pilih " + dokumen + " yang akan dihapus.");
                    return;
                }

                if(OswPesan.pesanKonfirmasiHapus() == DialogResult.Yes) {
                    String strngKode = gridView.GetRowCellValue(gridView.FocusedRowHandle, "Kode").ToString();

                    DataBarang dBarang = new DataBarang(command, strngKode);
                    if(!dBarang.isExist) {
                        throw new Exception(dokumen + " tidak ditemukan.");
                    }

                    dBarang.hapus();

                    // tulis log
                    OswLog.setTransaksi(command, "Hapus " + dokumen, dBarang.ToString());

                    // reload grid
                    this.setGrid(command);

                    // Commit Transaction
                    command.Transaction.Commit();

                    OswPesan.pesanInfo(dokumen + " berhasil dihapus.");
                }
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