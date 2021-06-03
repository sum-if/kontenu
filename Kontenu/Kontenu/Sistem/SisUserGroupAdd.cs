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

namespace Kontenu.Sistem {
    public partial class SisUserGroupAdd : DevExpress.XtraEditors.XtraForm {
        private String id = "USERGROUP";
        private String dokumen = "Tambah/Edit User Group";
        private Boolean isAdd;

        public SisUserGroupAdd(bool pIsAdd) {
            isAdd = pIsAdd;
            InitializeComponent();
        }

        private void MstUserGroupAdd_Load(object sender, EventArgs e) {
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
                OswControlDefaultProperties.setInput(this,id,command);
                cmbStatus = ComboConstantUmum.getStatusAktif(cmbStatus);
                
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
                dxValidationProvider1.SetValidationRule(txtNama, OswValidation.IsNotBlank());
                dxValidationProvider1.SetValidationRule(cmbStatus, OswValidation.IsNotBlank());

                if(!dxValidationProvider1.Validate()) {
                    foreach(Control x in dxValidationProvider1.GetInvalidControls()) {
                        dxValidationProvider1.SetIconAlignment(x, ErrorIconAlignment.MiddleRight);
                    }
                    return;
                }


                string strngKode = txtKode.Text;
                string strngNama = txtNama.Text;
                string strngStatus = cmbStatus.EditValue.ToString();
                string strngKeterangan = txtKeterangan.Text;

                DataOswUserGroup dOswUserGroup = new DataOswUserGroup(command, strngKode);
                dOswUserGroup.nama = strngNama;
                dOswUserGroup.status = strngStatus;
                dOswUserGroup.keterangan = strngKeterangan;

                if(dOswUserGroup.isExist) {
                    dOswUserGroup.ubah();
                } else {
                    dOswUserGroup.tambah();
                }
                
                // tulis log
                OswLog.setTransaksi(command, dokumen, dOswUserGroup.ToString());

                // reload grid di form header
                SisUserGroup mstUserGroup = (SisUserGroup)this.Owner;
                mstUserGroup.setGrid(command);

                // Commit Transaction
                command.Transaction.Commit();

                OswPesan.pesanInfo("Proses penyimpanan User Group berhasil.");

                if(this.isAdd) {
                    setDefaultInput(command);
                } else {
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
                String strngKode = txtKode.Text;
                DataOswUserGroup dOswUserGroup = new DataOswUserGroup(command, strngKode);

                this.Text = "Ubah User Group";
                txtNama.Text = dOswUserGroup.nama;
                cmbStatus.EditValue = dOswUserGroup.status;
                txtKeterangan.Text = dOswUserGroup.keterangan;
            } else {
                OswControlDefaultProperties.resetAllInput(this);
                cmbStatus.ItemIndex = 0;
            }
            
            txtKode.Focus();
        }
    }
}