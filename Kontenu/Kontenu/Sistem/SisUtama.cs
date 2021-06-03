using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using MySql.Data.MySqlClient;
using OswLib;
using DevExpress.XtraEditors.DXErrorProvider;
using DevExpress.XtraSplashScreen;
using Kontenu.OswLib;
using DevExpress.XtraGrid;
using Kontenu.Umum;
using DevExpress.XtraGrid.Views.Grid;

namespace Kontenu.Sistem {
    public partial class SisUtama : DevExpress.XtraEditors.XtraForm {
        public static int IsLogout;
        private String dokumen = "Form Utama";
        public SisUtama() {
            InitializeComponent();
        }


        private void SisUtama_Load(object sender, EventArgs e) {
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
                OswMenu.setMenuUtama(this, Application.OpenForms, typeof(Program).Assembly);

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