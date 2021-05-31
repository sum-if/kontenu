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
    public partial class FrmSettlementChequePembelian : DevExpress.XtraEditors.XtraForm {
        private String id = "SETTLEMENTCHEQUEPEMBELIAN";
        private String dokumen = "SETTLEMENTCHEQUEPEMBELIAN";

        public FrmSettlementChequePembelian() {
            InitializeComponent();
        }

        private void FrmSettlementChequePembelian_Load(object sender, EventArgs e) {
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

                cmbStatus = ComboConstantUmum.getJenisSettlementCheque(cmbStatus, true);
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
            String strngTanggalAwal = deTanggalAwal.Text;
            String strngTanggalAkhir = deTanggalAkhir.Text;
            String strngKode = txtKode.Text;
            String strngKodeSettlement = txtNoSettlement.Text;
            String strngSupplier = cmbSupplier.EditValue == null ? "" : cmbSupplier.EditValue.ToString();
            String strngStatus = cmbStatus.ItemIndex < 0 ? "" : cmbStatus.EditValue.ToString();

            String query = @"SELECT A.kode AS Nomor, A.tanggal AS Tanggal, C.nocek AS 'No. Cheque', B.nama AS 'Supplier', BB.nama AS Kota, B.alamat AS Alamat, C.total AS Total, A.nourut AS 'No Urut', A.jenis AS Status
                            FROM settlementchequepembelian A
                            INNER JOIN supplier B ON A.supplier = B.kode
                            INNER JOIN kota BB ON B.kota = BB.kode
                            INNER JOIN pembayaranpembelian C ON C.kode = A.pembayaranpembelian
                            WHERE toDate(C.tanggalcek) BETWEEN toDate(@tanggalawal) AND toDate(@tanggalakhir) AND 
                                  A.kode LIKE @kodesettlement AND C.nocek LIKE @kode AND A.supplier LIKE @supplier AND A.jenis LIKE @status
                            ORDER BY A.kode";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("format", OswConfig.FORMAT_TANGGAL_MYSQL);
            parameters.Add("tanggalawal", strngTanggalAwal);
            parameters.Add("tanggalakhir", strngTanggalAkhir);
            parameters.Add("kode", "%" + strngKode + "%");
            parameters.Add("kodesettlement", "%" + strngKodeSettlement + "%");
            parameters.Add("supplier", strngSupplier);
            parameters.Add("status", strngStatus);

            gridControl = OswGrid.getGridBrowse(gridControl, command, query, parameters, 
                                                new String[] { }, 
                                                new String[] { "Total" });
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
            FrmSettlementChequePembelianAdd form = new FrmSettlementChequePembelianAdd(false);
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

                DataSettlementChequePembelian dSettlementChequePembelian = new DataSettlementChequePembelian(command, strngKode);
                if(!dSettlementChequePembelian.isExist) {
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
            FrmSettlementChequePembelianAdd form = new FrmSettlementChequePembelianAdd(true);
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

                    DataSettlementChequePembelian dSettlementChequePembelian = new DataSettlementChequePembelian(command, strngKode);
                    if(!dSettlementChequePembelian.isExist) {
                        throw new Exception(dokumen + " tidak ditemukan.");
                    }

                    dSettlementChequePembelian.hapus();

                    // tulis log
                    OswLog.setTransaksi(command, "Hapus " + dokumen, dSettlementChequePembelian.ToString());

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