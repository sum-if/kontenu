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
using OmahSoftware.OswLib;
using System.Configuration;
using System.Deployment.Application;

namespace OmahSoftware.Sistem {
    public partial class SisLogin : DevExpress.XtraEditors.XtraForm {
        public static int IsLogin;
        public static int kodeUser;
        public static int namaUser;
        public static int usergroupUser;
        private String dokumen = "Form Login";

        public SisLogin() {
            InitializeComponent();
        }

        private void SisLogin_Load(object sender, EventArgs e) {
            dxValidationProvider1.RemoveControlError(txtPassword);
        }

        private void btnLogin_Click(object sender, EventArgs e) {
            SplashScreenManager.ShowForm(typeof(SplashUtama));
            MySqlConnection con = new MySqlConnection(OswConfig.KONEKSI);
            MySqlCommand command = con.CreateCommand();
            MySqlTransaction trans;
            MySqlDataReader reader;

            try {
                // buka koneksi
                con.Open();

                // set transaction
                trans = con.BeginTransaction();
                command.Transaction = trans;

                // Function Code
                // validation
                dxValidationProvider1.SetValidationRule(txtUsername, OswValidation.IsNotBlank());
                dxValidationProvider1.SetValidationRule(txtPassword, OswValidation.IsNotBlank());

                if(!dxValidationProvider1.Validate()) {
                    foreach(Control x in dxValidationProvider1.GetInvalidControls()) {
                        dxValidationProvider1.SetIconAlignment(x, ErrorIconAlignment.MiddleRight);
                    }
                    return;
                }

                IsLogin = 0;

                // proses pengecekkan data user
                command.CommandText = @"SELECT A.kode,A.nama,A.username,A.password,B.kode AS kodeusergroup, B.nama AS usergroup
                                        FROM oswuser A
                                        INNER JOIN oswusergroup B ON A.usergroup = B.kode
                                        WHERE A.username = @user AND A.status=1";
                command.Parameters.AddWithValue("user", txtUsername.Text);
                reader = command.ExecuteReader();

                if(reader.Read()) {
                    // cek bcrypt
                    if(OswConvert.checkBCrypt(txtPassword.Text, reader["password"].ToString())) {
                        IsLogin = 1;
                        OswConstants.KODEUSER = reader["kode"].ToString();
                        OswConstants.USERNAME = reader["username"].ToString();
                        OswConstants.NAMAUSER = reader["nama"].ToString();
                        OswConstants.USERGROUP = reader["usergroup"].ToString();
                        OswConstants.KODEUSERGROUP = reader["kodeusergroup"].ToString();
                        OswConstants.GUID = OswConvert.generateGUID();
                    }
                }

                reader.Close();

                if(IsLogin == 1) {
                    // tulis log
                    OswLog.setLogin(command);

                    this.Close();
                } else {
                    txtUsername.Focus();
                    //OswPesan.pesanError("Data yang Anda masukkan tidak valid.", command, dokumen);
                    throw new Exception("Data yang Anda masukkan tidak valid");
                }

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