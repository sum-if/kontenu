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
using Kontenu.Sistem;
using DevExpress.XtraEditors.DXErrorProvider;
using System.Net;
using Kontenu.Umum;
using Kontenu.OswLib;

namespace Kontenu.Sistem {
    public partial class SisUserAdd : DevExpress.XtraEditors.XtraForm {
        private String id = "USER";
        private String dokumen = "Tambah/Edit User";
        private Boolean isAdd;

        public SisUserAdd(bool pIsAdd) {
            isAdd = pIsAdd;
            InitializeComponent();
        }

        private void MstUserAdd_Load(object sender, EventArgs e) {
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
                OswControlDefaultProperties.setInput(this, id, command);
                cmbStatus = ComboConstantUmum.getStatusAktif(cmbStatus);
                cmbUserGroup = ComboQueryUmum.getUserGroup(cmbUserGroup, command);

                this.setDefaultInput(command);

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

        private void btnSimpan_Click(object sender, EventArgs e) {
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
                // validation
                dxValidationProvider1.SetValidationRule(txtKode, OswValidation.IsNotBlank());
                dxValidationProvider1.SetValidationRule(txtNama, OswValidation.IsNotBlank());
                dxValidationProvider1.SetValidationRule(txtUsername, OswValidation.IsNotBlank());
                dxValidationProvider1.SetValidationRule(cmbStatus, OswValidation.IsNotBlank());
                dxValidationProvider1.SetValidationRule(cmbUserGroup, OswValidation.IsNotBlank());

                if(!dxValidationProvider1.Validate()) {
                    foreach(Control x in dxValidationProvider1.GetInvalidControls()) {
                        dxValidationProvider1.SetIconAlignment(x, ErrorIconAlignment.MiddleRight);
                    }
                    return;
                }

                DataOswUser dOswUserCek = new DataOswUser(command, txtKode.Text);
                if (dOswUserCek.isExist && this.isAdd == true)
                {
                    throw new Exception("Kode Tidak Boleh sama");
                }


                if(txtPassword.Text != "") {
                    dxValidationProvider1.SetValidationRule(txtPassword, OswValidation.isMinimalString(5));

                    if(!dxValidationProvider1.Validate()) {
                        foreach(Control x in dxValidationProvider1.GetInvalidControls()) {
                            dxValidationProvider1.SetIconAlignment(x, ErrorIconAlignment.MiddleRight);
                        }
                        return;
                    }
                }
                
                string strngKode = txtKode.Text;
                string strngNama = txtNama.Text;
                string strngUsername = txtUsername.Text;
                string strngPassword = txtPassword.Text;
                string strngUserGroup = cmbUserGroup.EditValue.ToString();
                string strngStatus = cmbStatus.EditValue.ToString();

                DataOswUser dOswUser = new DataOswUser(command, strngKode);
                dOswUser.nama = strngNama;
                dOswUser.username = strngUsername;
                // ganti password jika tidak kosong
                if(strngPassword != "") {
                    dOswUser.password = OswConvert.convertToBCrypt(strngPassword);
                }
                dOswUser.usergroup = strngUserGroup;
                dOswUser.status = strngStatus;

                if(dOswUser.isExist) {
                    dOswUser.ubah();
                } else {
                    dOswUser.tambah();
                }

                // tulis log
                OswLog.setTransaksi(command, dokumen, dOswUser.ToString());

                // reload grid di form header
                SisUser mstUser = (SisUser)this.Owner;
                mstUser.setGrid(command);

                OswPesan.pesanInfo("Proses penyimpanan User berhasil.");

                if(this.isAdd) {
                    setDefaultInput(command);

                    // Commit Transaction
                    command.Transaction.Commit();
                } else {
                    // Commit Transaction
                    command.Transaction.Commit();

                    this.Close();
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

        private void setDefaultInput(MySqlCommand command) {
            if(!this.isAdd) {
                txtKode.Enabled = false;
                string strngKode = txtKode.Text;
                DataOswUser dOswUser = new DataOswUser(command, strngKode);

                this.Text = "Ubah User";
                txtNama.Text = dOswUser.nama;
                txtUsername.Text = dOswUser.username;
                cmbUserGroup.EditValue = dOswUser.usergroup;
                cmbStatus.EditValue = dOswUser.status;

            } else {
                OswControlDefaultProperties.resetAllInput(this);
                cmbStatus.ItemIndex = 0;
                cmbUserGroup.EditValue = OswCombo.getFirstEditValue(cmbUserGroup);

            }
            txtKode.Enabled = true;
            txtKode.Focus();
        }
    }
}