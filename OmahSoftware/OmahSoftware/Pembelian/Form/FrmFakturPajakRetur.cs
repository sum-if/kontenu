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
using OmahSoftware.Umum.Laporan;
using OmahSoftware.Akuntansi;
using DevExpress.XtraEditors.Controls;

namespace OmahSoftware.Pembelian {
    public partial class FrmFakturPajakRetur : DevExpress.XtraEditors.XtraForm {
        private String id = "FAKTURPAJAKRETUR";
        private String dokumen = "FAKTURPAJAKRETUR";

        public FrmFakturPajakRetur() {
            InitializeComponent();
        }

        private void FrmFakturPajakRetur_Load(object sender, EventArgs e) {
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
                DataOswJenisDokumen dOswJenisDokumen = new DataOswJenisDokumen(command, id);
                this.dokumen = dOswJenisDokumen.nama;

                this.Text = this.dokumen;

                OswControlDefaultProperties.setInput(this, id, command);
                OswControlDefaultProperties.setTanggal(deTanggalAwal);
                OswControlDefaultProperties.setTanggal(deTanggalAkhir);
                cmbSupplier = ComboQueryUmum.getSupplier(cmbSupplier, command, true);
                cmbStatus = ComboConstantUmum.getStatusFakturPembelian(cmbStatus, true);

                setDefaultInput(command);

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

        private void setDefaultInput(MySqlCommand command) {
            deTanggalAwal.DateTime = OswDate.getDateTimeTanggalAwalBulan();
            deTanggalAkhir.DateTime = OswDate.getDateTimeTanggalHariIni();
            txtNoReturFakturPembelian.Text = "";
            cmbSupplier.EditValue = OswCombo.getFirstEditValue(cmbSupplier);
            cmbStatus.ItemIndex = 0;

            this.setGrid(command);
        }

        public void setGrid(MySqlCommand command) {
            String strngTanggalAwal = deTanggalAwal.Text;
            String strngTanggalAkhir = deTanggalAkhir.Text;
            String strngNoReturFakturPembelian = txtNoReturFakturPembelian.Text;
            String strngSupplier = cmbSupplier.EditValue == null ? "" : cmbSupplier.EditValue.ToString();
            String strngStatus = cmbStatus.ItemIndex < 0 ? "" : cmbStatus.EditValue.ToString();
            String strngFakturBelumUpload = chkUpload.Checked ? "1" : "0";

            String query = @"SELECT A.tanggal AS 'Tgl Faktur', A.kode AS 'No Retur Faktur Pembelian', B.nama AS Supplier, C.nama AS Kota, A.grandtotal AS 'Total Faktur', 
                                    A.nofakturpajak AS 'No Faktur Pajak', A.tanggalfakturpajak AS 'Tgl Faktur Pajak', A.masapajak AS 'Masa Pajak', A.tanggalupload AS 'Tgl Upload'
                            FROM returpembelian A
                            INNER JOIN supplier B ON A.supplier = B.kode
                            INNER JOIN kota C ON B.kota = C.kode
                            WHERE toDate(A.tanggal) BETWEEN toDate(@tanggalawal) AND toDate(@tanggalakhir) AND A.kode LIKE @kode AND A.supplier LIKE @supplier AND IF(@belumupload = '1',A.tanggalupload = '',A.tanggalupload != '')
                            ORDER BY A.kode";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("tanggalawal", strngTanggalAwal);
            parameters.Add("tanggalakhir", strngTanggalAkhir);
            parameters.Add("kode", "%" + strngNoReturFakturPembelian + "%");
            parameters.Add("supplier", strngSupplier);
            parameters.Add("status", strngStatus);
            parameters.Add("belumupload", strngFakturBelumUpload);

            Dictionary<String, int> widths = new Dictionary<String, int>();
            // 854 - 21 (kiri) - 17 (vertikal lines) - 35 (No) = 847
            widths.Add("Tgl Faktur", 70);
            widths.Add("No Retur Faktur Pembelian", 130);
            widths.Add("Supplier", 170);
            widths.Add("Kota", 90);
            widths.Add("Total Faktur", 90);
            widths.Add("No Faktur Pajak", 125);
            widths.Add("Tgl Faktur Pajak", 100);
            widths.Add("Masa Pajak", 90);
            widths.Add("Tgl Upload", 100);

            Dictionary<String, String> inputType = new Dictionary<string, string>();
            inputType.Add("Total Faktur", OswInputType.NUMBER);

            OswGrid.getGridInput(gridControl1, command, query, parameters, widths, inputType,
                                 new String[] { },
                                 new String[] { "Tgl Faktur", "No Retur Faktur Pembelian", "Supplier", "Total Faktur", "Kota" },
                                 false);

            GridView gridView = gridView1;
            gridView.Columns["Tgl Faktur"].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            gridView.Columns["No Retur Faktur Pembelian"].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            gridView.Columns["Supplier"].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
        }

        private void btnSimpan_Click(object sender, EventArgs e) {
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
                GridView gridView = gridView1;
                for(int i = 0; i < gridView.DataRowCount; i++) {
                    if(gridView.GetRowCellValue(i, "No Retur Faktur Pembelian").ToString() == "") {
                        continue;
                    }

                    String strngNoReturFakturPembelian = gridView.GetRowCellValue(i, "No Retur Faktur Pembelian").ToString();
                    String strngNomorFaktur = gridView.GetRowCellValue(i, "No Faktur Pajak").ToString();
                    String strngTglFaktur = gridView.GetRowCellDisplayText(i, "Tgl Faktur Pajak").ToString();
                    String strngMasaFaktur = gridView.GetRowCellDisplayText(i, "Masa Pajak").ToString();
                    String strngTglUpload = gridView.GetRowCellDisplayText(i, "Tgl Upload").ToString();

                    DataReturPembelian dReturPembelian = new DataReturPembelian(command, strngNoReturFakturPembelian);
                    dReturPembelian.nofakturpajak = strngNomorFaktur;
                    dReturPembelian.tanggalfakturpajak = strngTglFaktur;
                    dReturPembelian.masapajak = strngMasaFaktur;
                    dReturPembelian.tanggalupload = strngTglUpload;
                    dReturPembelian.ubahFakturPajak();

                    // tulis log
                    OswLog.setTransaksi(command, dokumen, dReturPembelian.ToString());
                }

                // Commit Transaction
                command.Transaction.Commit();

                OswPesan.pesanInfo("Proses " + dokumen + " berhasil.");

                setDefaultInput(command);
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
            Tools.cetakBrowse(gridControl1, dokumen);
        }

        private void chkUpload_CheckedChanged(object sender, EventArgs e) {
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
    }
}