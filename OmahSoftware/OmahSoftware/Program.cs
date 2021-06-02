using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DevExpress.LookAndFeel;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Localization;
using System.Globalization;
using System.Threading;
using OswLib;
using OmahSoftware.Sistem;
using System.Configuration;

namespace OmahSoftware {
    static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            // setting skin yang akan di gunakan
            UserLookAndFeel.Default.SkinName = "Office 2013";

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // untuk number/date
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentCulture.DateTimeFormat.DateSeparator = "/";
            Thread.CurrentThread.CurrentCulture.DateTimeFormat.TimeSeparator = ":";
            OswConfig.CURRENT_CULTURE = Thread.CurrentThread.CurrentCulture;

            // untuk bahasa
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("id-ID");
            Thread.CurrentThread.CurrentUICulture.DateTimeFormat.DateSeparator = "/";
            Thread.CurrentThread.CurrentUICulture.DateTimeFormat.TimeSeparator = ":";
            OswConfig.CURRENT_UI_CULTURE = Thread.CurrentThread.CurrentUICulture;
            
            // setting config
            OswConfig.DEBUG = false;

            OswConfig.NUMBER_BISA_TANPA_ANGKA_BELAKANG_KOMA = false;
            
            OswConfig.KONEKSI = ConfigurationManager.ConnectionStrings["databaseosw"].ConnectionString;

            // Jangan lupa ganti untuk Function toPeriode di database (normalnya: dd/MM/yyyy)
            OswConfig.FORMAT_TANGGAL = "dd/MM/yyyy";
            OswConfig.FORMAT_TANGGAL_MYSQL = "%d/%m/%Y";
            OswConfig.FORMAT_JAM = "";
            OswConfig.CREATOR = "Developed by: SUMiF - www.sumif.id";

            // jalankan program
            Application.Run(new SisLogin());
            
            if (SisLogin.IsLogin == 1) {
                SisUtama.IsLogout = 0;
                Application.Run(new SisUtama());
            } else {
                SisUtama.IsLogout = 0;
                Application.Exit();
            }

            if (SisUtama.IsLogout == 1) {
                Application.Restart();
            }
        }
    }
}
