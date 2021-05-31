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
    public partial class FrmLaporanPenagihan : DevExpress.XtraEditors.XtraForm {
        private String id = "LAPORANPENAGIHAN";
        private String dokumen = "LAPORANPENAGIHAN";

        public FrmLaporanPenagihan() {
            InitializeComponent();
        }

        private void FrmLaporanPenagihan_Load(object sender, EventArgs e) {
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
                cmbKotaGroup = ComboQueryUmum.getKotaGroup(cmbKotaGroup, command, true);

                OswControlDefaultProperties.resetAllInput(this);

                cmbKotaGroup.EditValue = OswCombo.getFirstEditValue(cmbKotaGroup);

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
            String strngKotaGroup = cmbKotaGroup.EditValue.ToString();
            String strngKodeCustomer = txtKodeCustomer.Text;
            String strngNamaCustomer = txtNamaCustomer.Text;

            String query = @"SELECT C.nama AS Kota, B.nama AS Customer, A.kode AS Faktur, A.tanggal AS Tanggal, A.tanggaljatuhtempo AS 'Jatuh Tempo', A.grandtotal AS 'Grand Total', A.totalbayar AS 'Total Bayar', A.totalretur AS 'Total Retur', 
                                    CASE WHEN (A.grandtotal - A.totalbayar - A.totalretur) < 0 THEN 0 ELSE (A.grandtotal - A.totalbayar - A.totalretur) END AS 'Sisa Bayar'
                            FROM fakturpenjualan A
                            INNER JOIN customer B ON A.customer = B.kode
                            INNER JOIN kota C ON B.kota = C.kode
                            WHERE C.kotagroup LIKE @kotagroup AND B.kode LIKE @kodecustomer AND B.nama LIKE @namacustomer AND A.status = @status AND (A.grandtotal - A.totalbayar - A.totalretur) > 0
                            ORDER BY C.nama, B.nama";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("status", Constants.STATUS_FAKTUR_PENJUALAN_BELUM_LUNAS);
            parameters.Add("kotagroup", awal ? Constants.TIDAK_MUNGKIN : strngKotaGroup);
            parameters.Add("kodecustomer", awal ? Constants.TIDAK_MUNGKIN : "%" + strngKodeCustomer + "%");
            parameters.Add("namacustomer", awal ? Constants.TIDAK_MUNGKIN : "%" + strngNamaCustomer + "%");

            gridControl = OswGrid.getGridBrowse(gridControl, command, query, parameters,
                                                new String[] { },
                                                new String[] { "Grand Total", "Total Bayar", "Total Retur", "Sisa Bayar" });


            // tambahkan kolom di paling kiri
            gridView.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CheckBoxRowSelect;
            gridView.OptionsSelection.MultiSelect = true;

            gridView.OptionsView.ShowFooter = true;
            gridView.Columns["Grand Total"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            gridView.Columns["Grand Total"].SummaryItem.DisplayFormat = "{0:N2}";
            gridView.Columns["Total Bayar"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            gridView.Columns["Total Bayar"].SummaryItem.DisplayFormat = "{0:N2}";
            gridView.Columns["Total Retur"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            gridView.Columns["Total Retur"].SummaryItem.DisplayFormat = "{0:N2}";
            gridView.Columns["Sisa Bayar"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            gridView.Columns["Sisa Bayar"].SummaryItem.DisplayFormat = "{0:N2}";
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

        private void btnCetakPenagihan_Click(object sender, EventArgs e) {
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
                String strngKotaGroup = cmbKotaGroup.EditValue.ToString();
                String strngKodeCustomer = txtKodeCustomer.Text;
                String strngNamaCustomer = txtNamaCustomer.Text;

                // ambil isi yang dicentang
                List<string> hasil = new List<string>();
                if(gridView.SelectedRowsCount > 0) {
                    foreach(int rowHandle in gridView.GetSelectedRows()) {
                        DataRow tamp = gridView.GetDataRow(rowHandle);
                        String faktur = tamp.ItemArray[2].ToString();
                        hasil.Add(faktur);
                    }
                }

                RptLaporanPenagihan report = new RptLaporanPenagihan();
                report.Parameters["faktur"].Value = hasil.ToArray();
                report.Parameters["kotagroup"].Value = strngKotaGroup;
                report.Parameters["kodecustomer"].Value = "%" + strngKodeCustomer + "%";
                report.Parameters["namacustomer"].Value = "%" + strngNamaCustomer + "%";

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