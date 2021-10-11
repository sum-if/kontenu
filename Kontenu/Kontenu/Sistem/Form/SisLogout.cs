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

namespace Kontenu.Sistem {
    public partial class SisLogout : DevExpress.XtraEditors.XtraForm {
        private String dokumen = "Form Logout";

        public SisLogout() {
            InitializeComponent();
        }

        private void picBtnYa_Click(object sender, EventArgs e)
        {
            SplashScreenManager.ShowForm(typeof(SplashUtama));
            MySqlConnection con = new MySqlConnection(OswConfig.KONEKSI);
            MySqlCommand command = con.CreateCommand();
            MySqlTransaction trans;

            try
            {
                // buka koneksi
                con.Open();

                // set transaction
                trans = con.BeginTransaction();
                command.Transaction = trans;

                // Function Code
                OswLog.setLogout(command);

                SisLogin.IsLogin = 0;
                SisUtama.IsLogout = 1;
                Application.Exit();

                // Commit Transaction
                command.Transaction.Commit();
            }
            catch (MySqlException ex)
            {
                OswPesan.pesanErrorCatch(ex, command, dokumen);
            }
            catch (Exception ex)
            {
                OswPesan.pesanErrorCatch(ex, command, dokumen);
            }
            finally
            {
                con.Close();
                try
                {
                    SplashScreenManager.CloseForm();
                }
                catch (Exception ex)
                {
                }
            }
        }

        private void picBtnTidak_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}