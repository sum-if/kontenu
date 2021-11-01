using Kontenu.Umum.Laporan;
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
using Kontenu.Sistem;
using OswLib;
using MySql.Data.MySqlClient;
using Kontenu.Akuntansi;

namespace Kontenu.OswLib {
    class Tools {

        /// <summary>
        /// Cetak halaman browse
        /// </summary>
        /// <param name="gridControl"></param>
        /// <param name="dokumen"></param>
        public static void cetakBrowse(GridControl gridControl, string dokumen, bool landscape = true, System.Drawing.Printing.Margins margin = null, System.Drawing.Printing.PaperKind paperKind = System.Drawing.Printing.PaperKind.A4) {
            MySqlConnection con = new MySqlConnection(OswConfig.KONEKSI);
            MySqlCommand command = con.CreateCommand();

            try {
                // buka koneksi
                con.Open();

                // Function Code
                SplashScreenManager.ShowForm(typeof(SplashUtama));

                Dictionary<String, String> filter = new Dictionary<string, string>();
                Dictionary<String, int> lebarKolom = new Dictionary<string, int>();

                DateTime tanggal = DateTime.Now;
                filter.Add("Dicetak Oleh", OswConstants.NAMAUSER + ", " + tanggal.Day.ToString() + " " + OswDate.getNamaBulan(tanggal) + " " + tanggal.Year.ToString());

                LaporanPrintPreview laporan = new LaporanPrintPreview();
                laporan.documentViewer1.PrintingSystem = OswReport.cetakLaporan(command, gridControl, dokumen, filter, lebarKolom, paperKind, margin, landscape, 80);
                laporan.Show();
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

        public static void cetakLaporan(GridControl gridControl, string headerLaporan, Dictionary<string, string> filter, Dictionary<string, int> lebarKolom, bool landscape = false, int hor = 120, System.Drawing.Printing.Margins margin = null, System.Drawing.Printing.PaperKind paperKind = System.Drawing.Printing.PaperKind.A4) {
            MySqlConnection con = new MySqlConnection(OswConfig.KONEKSI);
            MySqlCommand command = con.CreateCommand();

            try {
                // buka koneksi
                con.Open();

                // Function Code
                SplashScreenManager.ShowForm(typeof(SplashUtama));

                // tambah dicetak oleh
                DateTime tanggal = DateTime.Now;
                filter.Add("Dicetak Oleh", OswConstants.NAMAUSER + ", " + tanggal.Day.ToString() + " " + OswDate.getNamaBulan(tanggal) + " " + tanggal.Year.ToString());

                LaporanPrintPreview laporan = new LaporanPrintPreview();
                laporan.documentViewer1.PrintingSystem = OswReport.cetakLaporan(command, gridControl, headerLaporan, filter, lebarKolom, paperKind, margin, landscape, hor);
                laporan.Show();

                //gridControl.ShowRibbonPrintPreview();

                try {
                    SplashScreenManager.CloseForm();
                } catch(Exception ex) {
                }
            } catch(MySqlException ex) {
                OswPesan.pesanErrorCatch(ex, command, headerLaporan);
            } catch(Exception ex) {
                OswPesan.pesanErrorCatch(ex, command, headerLaporan);
            } finally {
                con.Close();
                try {
                    SplashScreenManager.CloseForm();
                } catch(Exception ex) {
                }
            }
        }

        public static bool isHakAksesTambah(MySqlCommand command, String menu) {
            String query = @"SELECT A.tambah
                             FROM oswhakakses A
                             WHERE A.usergroup = @usergroup AND A.menu = @menu";

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("usergroup", OswConstants.KODEUSERGROUP);
            parameters.Add("menu", menu);

            string tambah = OswDataAccess.executeScalarQuery(query, parameters, command);

            return tambah == "1";
        }

        public static decimal getRoundCalc(decimal nilai) {
            return Math.Round(nilai, 2);
        }

        public static string getRoundCalc(string nilai) {
            return Math.Round(decimal.Parse(nilai), 2).ToString();
        }

        public static decimal getRoundMoney(decimal nilai) {
            return Math.Round(nilai, 2);
        }

        public static string getRoundMoney(string nilai) {
            return Math.Round(decimal.Parse(nilai), 2).ToString();
        }

        public static void cekJurnalBalance(MySqlCommand command, String oswjenisdokumen, String noreferensi) {
            String query = @"SELECT COALESCE(SUM(debit),0)
                            FROM jurnal 
                            WHERE oswjenisdokumen = @jenisdokumn AND noreferensi = @referensi AND status = 'Aktif'";

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("jenisdokumn", oswjenisdokumen);
            parameters.Add("referensi", noreferensi);

            decimal debit = Tools.getRoundMoney(decimal.Parse(OswDataAccess.executeScalarQuery(query, parameters, command)));

            query = @"SELECT COALESCE(SUM(kredit),0)
                            FROM jurnal 
                            WHERE oswjenisdokumen = @jenisdokumn AND noreferensi = @referensi AND status = 'Aktif'";

            parameters = new Dictionary<string, string>();
            parameters.Add("jenisdokumn", oswjenisdokumen);
            parameters.Add("referensi", noreferensi);

            decimal kredit = Tools.getRoundMoney(decimal.Parse(OswDataAccess.executeScalarQuery(query, parameters, command)));

            if(debit != kredit) {
                throw new Exception("Jurnal [" + oswjenisdokumen + "] [" + noreferensi + "] tidak balance.");
            }
        }

        public static bool isKelompokAkun(MySqlCommand command, String akun, String kelompokAkun)
        {
            String query = @"SELECT COUNT(*)
                             FROM akun A
                             INNER JOIN kelompokakunsetting B ON A.kode LIKE B.akun AND A.akunkategori LIKE B.kategori AND A.akunsubkategori LIKE B.subkategori AND A.akungroup LIKE B.group AND A.akunsubgroup LIKE B.subgroup
                             WHERE A.kode = @akun AND B.kelompokakun = @kelompokakun";

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("akun", akun);
            parameters.Add("kelompokakun", kelompokAkun);

            int jumlah = int.Parse(OswDataAccess.executeScalarQuery(query, parameters, command));

            return jumlah > 0;
        }

        public static void valAdmin(MySqlCommand command, String tanggal)
        {
            DataAdmin dAdmin = new DataAdmin(command, OswDate.getTahunBulan(tanggal), Constants.PROSES_TUTUP_PERIODE);
            if (dAdmin.isProcessed())
            {
                throw new Exception("Proses " + Constants.PROSES_TUTUP_PERIODE + " untuk periode '" + OswConvert.toNamaPeriode(OswDate.getTahunBulan(tanggal)) + "' sudah dilakukan.");
            }
        }
    }
}
