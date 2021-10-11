using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DevExpress.LookAndFeel;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Localization;
using System.Globalization;
using System.Threading;
using OswLib;
using Kontenu.Sistem;
using System.Configuration;

namespace Kontenu {
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

            // setting config
            OswConfig.KONEKSI = ConfigurationManager.ConnectionStrings["database"].ConnectionString;
            OswConfig.DEBUG = false;
            OswConfig.CREATOR = "Developed by: SUMiF - www.sumif.id";
            OswConfig.RESOURCE = "Kontenu.Resources.";

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

            OswConfig.NUMBER_BISA_TANPA_ANGKA_BELAKANG_KOMA = false;
            OswConfig.FORMAT_NUMBER = "N2";

            // Jangan lupa ganti untuk Function toPeriode di database (normalnya: dd/MM/yyyy)
            OswConfig.FORMAT_TANGGAL = "dd/MM/yyyy";
            OswConfig.FORMAT_TANGGAL_MYSQL = "%d/%m/%Y";
            OswConfig.FORMAT_JAM = "";
            

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
