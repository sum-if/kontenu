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
    public partial class FrmFakturPenjualan : DevExpress.XtraEditors.XtraForm {
        private String id = "FAKTURPENJUALAN";
        private String dokumen = "FAKTURPENJUALAN";

        public FrmFakturPenjualan() {
            InitializeComponent();
        }

        private void FrmFakturPenjualan_Load(object sender, EventArgs e) {
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

                cmbCustomer = ComboQueryUmum.getCustomer(cmbCustomer, command, true);
                cmbCustomer.EditValue = OswCombo.getFirstEditValue(cmbCustomer);

                cmbStatus = ComboConstantUmum.getStatusFakturPenjualan(cmbStatus, true);
                cmbStatus.ItemIndex = 0;

                cmbFakturPajak = ComboConstantUmum.getStatusFakturPajak(cmbFakturPajak, true);
                cmbFakturPajak.ItemIndex = 0;

                cmbGunggung = ComboConstantUmum.getStatusYa(cmbGunggung, true);
                cmbGunggung.ItemIndex = 0;

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
            String strngCustomer = cmbCustomer.EditValue == null ? "" : cmbCustomer.EditValue.ToString();
            String strngStatus = cmbStatus.ItemIndex < 0 ? "" : cmbStatus.EditValue.ToString();
            String strngStatusPajak = cmbFakturPajak.ItemIndex < 0 ? "" : cmbFakturPajak.EditValue.ToString();
            String strngGunggung = cmbGunggung.ItemIndex < 0 ? "" : cmbGunggung.EditValue.ToString();

            String query = @"SELECT A.kode AS Nomor, A.tanggal AS Tanggal, B.nama AS Customer, H.nama AS Kota, A.tanggaljatuhtempo AS 'Jatuh Tempo', A.nofakturpajak AS 'No Faktur Pajak', B.npwpkode AS NPWP, COALESCE(D.nama,F.nama) AS 'Sales',
                                    A.pesananpenjualan AS 'Pesanan Penjualan', G.nama AS Ekspedisi,A.grandtotal AS Total, A.totalbayar AS 'Total Bayar', A.totalretur AS 'Total Retur', A.status AS Status
                            FROM fakturpenjualan A
                            INNER JOIN customer B ON A.customer = B.kode
                            INNER JOIN ekspedisi G ON A.ekspedisi = G.kode
                            INNER JOIN kota H ON B.kota = H.kode
                            LEFT JOIN pesananpenjualan C ON A.pesananpenjualan = C.kode AND A.jenispesananpenjualan = @jenisPesananPenjualan
                            LEFT JOIN sales D ON C.sales = D.kode
                            LEFT JOIN backorder E ON A.pesananpenjualan = E.kode AND A.jenispesananpenjualan = 'Back Order'
                            LEFT JOIN sales F ON E.sales = F.kode
                            WHERE toDate(A.tanggal) BETWEEN toDate(@tanggalawal) AND toDate(@tanggalakhir) AND A.gunggung LIKE @gunggung AND
                                  A.kode LIKE @kode AND A.customer LIKE @customer AND A.status LIKE @status AND
                                  CASE @statuspajak WHEN 'Sudah' THEN A.nofakturpajak != '' WHEN 'Belum' THEN A.nofakturpajak = '' ELSE TRUE END
                            ORDER BY A.kode";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("tanggalawal", strngTanggalAwal);
            parameters.Add("tanggalakhir", strngTanggalAkhir);
            parameters.Add("kode", "%" + strngKode + "%");
            parameters.Add("customer", strngCustomer);
            parameters.Add("status", strngStatus);
            parameters.Add("gunggung", strngGunggung);
            parameters.Add("statuspajak", strngStatusPajak);
            parameters.Add("jenisPesananPenjualan", Constants.JENIS_PESANAN_PENJUALAN_PESANAN_PENJUALAN);

            gridControl = OswGrid.getGridBrowse(gridControl, command, query, parameters, 
                                                new String[] { },
                                                new String[] { "Total", "Total Bayar", "Total Retur" });
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
            FrmFakturPenjualanAdd form = new FrmFakturPenjualanAdd(false);
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

                DataFakturPenjualan dFakturPenjualan = new DataFakturPenjualan(command, strngKode);
                if(!dFakturPenjualan.isExist) {
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
            FrmFakturPenjualanAdd form = new FrmFakturPenjualanAdd(true);
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

                    DataFakturPenjualan dFakturPenjualan = new DataFakturPenjualan(command, strngKode);
                    if(!dFakturPenjualan.isExist) {
                        throw new Exception(dokumen + " tidak ditemukan.");
                    }

                    dFakturPenjualan.hapus();

                    // tulis log
                    OswLog.setTransaksi(command, "Hapus " + dokumen, dFakturPenjualan.ToString());

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