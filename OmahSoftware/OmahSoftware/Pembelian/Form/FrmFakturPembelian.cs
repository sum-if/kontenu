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

namespace OmahSoftware.Pembelian {
    public partial class FrmFakturPembelian : DevExpress.XtraEditors.XtraForm {
        private String id = "FAKTURPEMBELIAN";
        private String dokumen = "FAKTURPEMBELIAN";

        public FrmFakturPembelian() {
            InitializeComponent();
        }

        private void FrmFakturPembelian_Load(object sender, EventArgs e) {
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

                cmbSupplier = ComboQueryUmum.getSupplier(cmbSupplier, command, true);
                cmbSupplier.EditValue = OswCombo.getFirstEditValue(cmbSupplier);

                cmbStatus = ComboConstantUmum.getStatusFakturPembelian(cmbStatus, true);
                cmbStatus.ItemIndex = 0;

                cmbFakturPajak = ComboConstantUmum.getStatusFakturPajak(cmbFakturPajak, true);
                cmbFakturPajak.ItemIndex = 0;

                cmbJenisPPN = ComboConstantUmum.getJenisPPN(cmbJenisPPN, true);
                cmbJenisPPN.ItemIndex = 0;

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
            String strngTanggalAwal = deTanggalAwal.Text;
            String strngTanggalAkhir = deTanggalAkhir.Text;
            String strngKode = txtKode.Text;
            String strngFakturSupplier = txtFakturSupplier.Text;
            String strngSupplier = cmbSupplier.EditValue == null ? "" : cmbSupplier.EditValue.ToString();
            String strngStatus = cmbStatus.ItemIndex < 0 ? "" : cmbStatus.EditValue.ToString();
            String strngStatusPajak = cmbFakturPajak.ItemIndex < 0 ? "" : cmbFakturPajak.EditValue.ToString();
            String strngJenisPPn = cmbJenisPPN.ItemIndex < 0 ? "" : cmbJenisPPN.EditValue.ToString();

            String query = @"SELECT A.kode AS Nomor, A.tanggal AS Tanggal, B.nama AS Supplier, C.nama AS Kota, A.jenisppn AS 'Jenis PPN', A.nofakturpajak AS 'No Faktur Pajak', A.pesananpembelian AS 'Pesanan Pembelian', A.faktursupplier AS 'Faktur Supplier', A.jatuhtempo AS 'Jatuh Tempo', 
                                    A.grandtotal AS 'Total Faktur', A.totalbayar AS 'Total Bayar', A.totalretur AS 'Total Retur', A.status AS Status
                            FROM fakturpembelian A
                            INNER JOIN supplier B ON A.supplier = B.kode
                            INNER JOIN kota C ON B.kota = C.kode
                            WHERE toDate(A.tanggal) BETWEEN toDate(@tanggalawal) AND toDate(@tanggalakhir) AND 
                                  A.kode LIKE @kode AND A.supplier LIKE @supplier AND A.status LIKE @status AND A.faktursupplier LIKE @faktursupplier AND A.jenisppn LIKE @jenisppn AND 
                                  CASE @statuspajak WHEN 'Sudah' THEN A.nofakturpajak != '' WHEN 'Belum' THEN A.nofakturpajak = '' ELSE TRUE END
                            ORDER BY A.kode";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("tanggalawal", strngTanggalAwal);
            parameters.Add("tanggalakhir", strngTanggalAkhir);
            parameters.Add("kode", "%" + strngKode + "%");
            parameters.Add("faktursupplier", "%" + strngFakturSupplier + "%");
            parameters.Add("supplier", strngSupplier);
            parameters.Add("status", strngStatus);
            parameters.Add("statuspajak", strngStatusPajak);
            parameters.Add("jenisppn", strngJenisPPn);

            gridControl = OswGrid.getGridBrowse(gridControl, command, query, parameters,
                                                new String[] { },
                                                new String[] { "Total Faktur", "Total Bayar", "Total Retur" });
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
            FrmFakturPembelianAdd form = new FrmFakturPembelianAdd(false);
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

                String strngKode = gridView.GetRowCellValue(gridView.FocusedRowHandle, "Nomor").ToString();

                DataFakturPembelian dFakturPembelian = new DataFakturPembelian(command, strngKode);
                if(!dFakturPembelian.isExist) {
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
            FrmFakturPembelianAdd form = new FrmFakturPembelianAdd(true);
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
                    String strngKode = gridView.GetRowCellValue(gridView.FocusedRowHandle, "Nomor").ToString();

                    DataFakturPembelian dFakturPembelian = new DataFakturPembelian(command, strngKode);
                    if(!dFakturPembelian.isExist) {
                        throw new Exception(dokumen + " tidak ditemukan.");
                    }

                    dFakturPembelian.hapus();

                    // tulis log
                    OswLog.setTransaksi(command, "Hapus " + dokumen, dFakturPembelian.ToString());

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