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
using OmahSoftware.Penjualan.Laporan;
using OmahSoftware.Umum.Laporan;

namespace OmahSoftware.Penjualan {
    public partial class FrmLaporanPenjualan : DevExpress.XtraEditors.XtraForm {
        private String id = "LAPORANPENJUALAN";
        private String dokumen = "LAPORANPENJUALAN";
        private String strngKodeCustomer = "%";
        private String strngNamaCustomer = "Semua";

        public FrmLaporanPenjualan() {
            InitializeComponent();
        }

        private void FrmLaporanPenjualan_Load(object sender, EventArgs e) {
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
                deTanggalAwal.DateTime = OswDate.getDateTimeTanggalAwalBulan();
                deTanggalAkhir.DateTime = OswDate.getDateTimeTanggalHariIni();

                txtCustomer.Text = strngNamaCustomer;

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
                String strngTanggalAwal = deTanggalAwal.Text;
                String strngTanggalAkhir = deTanggalAkhir.Text;

                RptCetakLaporanPenjualan report = new RptCetakLaporanPenjualan();
                report.Parameters["guid"].Value = OswConstants.GUID;
                report.Parameters["periode"].Value = "Periode " + strngTanggalAwal + " - " + strngTanggalAkhir;
                report.Parameters["customer"].Value = strngKodeCustomer;
                report.Parameters["tanggalAwal"].Value = strngTanggalAwal;
                report.Parameters["tanggalAkhir"].Value = strngTanggalAkhir;
                report.Parameters["keteranganCustomer"].Value = strngNamaCustomer;

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

        private void btnCariCustomer_Click(object sender, EventArgs e) {
            infoCustomer();
        }

        private void infoCustomer() {
            MySqlConnection con = new MySqlConnection(OswConfig.KONEKSI);
            MySqlCommand command = con.CreateCommand();
            MySqlTransaction trans;

            try {
                // buka koneksi
                con.Open();

                // set transaction
                trans = con.BeginTransaction();
                command.Transaction = trans;

                DataOswSetting dOswSettingCustomerGrup = new DataOswSetting(command, "grup_customer");
                String strngGrupCustomer = dOswSettingCustomerGrup.isi;

                // Function Code
                String query = @"SELECT A.kode AS 'Kode Customer', A.nama AS Customer, B.nama AS Kota, A.alamat AS Alamat, A.telp AS Telp, A.cp AS CP
                                FROM customer A 
                                INNER JOIN kota B ON A.kota = B.kode
                                WHERE A.status = @status
                                ORDER BY A.nama";

                Dictionary<String, String> parameters = new Dictionary<String, String>();
                parameters.Add("status", Constants.STATUS_AKTIF);

                InfUtamaDictionary form = new InfUtamaDictionary("Info Customer", query, parameters,
                                                                new String[] { "Kode Customer", "Customer" },
                                                                new String[] { "Kode Customer" },
                                                                new String[] { });
                this.AddOwnedForm(form);
                form.ShowDialog();
                if(!form.hasil.ContainsKey("Kode Customer")) {
                    return;
                }

                String kodeCustomer = form.hasil["Kode Customer"];
                String namaCustomer = form.hasil["Customer"];

                strngKodeCustomer = kodeCustomer;
                strngNamaCustomer = namaCustomer;

                txtCustomer.Text = strngNamaCustomer;

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