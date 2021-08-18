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
using Kontenu.OswLib;
using Kontenu.Umum;

namespace Kontenu.Akuntansi {
    public partial class FrmAkunSubGroupAdd : DevExpress.XtraEditors.XtraForm {
        private String id = "AKUNSUBGROUP";
        private String dokumen = "AKUNSUBGROUP";
        private Boolean isAdd;

        public FrmAkunSubGroupAdd(bool pIsAdd) {
            isAdd = pIsAdd;
            InitializeComponent();
        }

        private void FrmAkunSubGroupAdd_Load(object sender, EventArgs e) {
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
                DataOswJenisDokumen dOswJenisDokumen = new DataOswJenisDokumen(command, id);
                if(!this.isAdd) {
                    this.dokumen = "Ubah " + dOswJenisDokumen.nama;
                } else {
                    this.dokumen = "Tambah " + dOswJenisDokumen.nama;
                }

                this.Text = this.dokumen;

                OswControlDefaultProperties.setInput(this, id, command);
                cmbAkunGroup = ComboQueryUmum.getAkunGroupAll(cmbAkunGroup, command);
                cmbAkunGroup.EditValue = OswCombo.getFirstEditValue(cmbAkunGroup);

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

        private void setDefaultInput(MySqlCommand command) {
            if(!this.isAdd) {
                String strngKode = txtKode.Text;
                txtKode.Enabled = false;
                cmbAkunGroup.Enabled = false;

                // data
                DataAkunSubGroup dAkunSubGroup = new DataAkunSubGroup(command, strngKode);
                cmbAkunGroup.EditValue = dAkunSubGroup.akungroup;

                txtNama.EditValue = dAkunSubGroup.nama;
            } else {
                OswControlDefaultProperties.resetAllInput(this);
                cmbAkunGroup.EditValue = OswCombo.getFirstEditValue(cmbAkunGroup);
            }

            txtNama.Focus();
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
                dxValidationProvider1.SetValidationRule(cmbAkunGroup, OswValidation.IsNotBlank());
                dxValidationProvider1.SetValidationRule(txtNama, OswValidation.IsNotBlank());

                if(!dxValidationProvider1.Validate()) {
                    foreach(Control x in dxValidationProvider1.GetInvalidControls()) {
                        dxValidationProvider1.SetIconAlignment(x, ErrorIconAlignment.MiddleRight);
                    }
                    return;
                }

                if(cmbAkunGroup.EditValue == null) {
                    throw new Exception("Silahkan pilih Group");
                }

                String strngKode = txtKode.Text;
                String strngAkunGroup = cmbAkunGroup.EditValue.ToString();
                String strngNama = txtNama.EditValue.ToString();

                DataAkunSubGroup dAkunSubGroup = new DataAkunSubGroup(command, strngKode);
                dAkunSubGroup.akungroup = strngAkunGroup;
                dAkunSubGroup.nama = strngNama;

                if(this.isAdd) {
                    dAkunSubGroup.tambah();
                } else {
                    dAkunSubGroup.ubah();
                }

                // tulis log
                OswLog.setTransaksi(command, dokumen, dAkunSubGroup.ToString());

                // reload grid di form header
                FrmAkunSubGroup frmAkunSubGroup = (FrmAkunSubGroup)this.Owner;
                frmAkunSubGroup.setGrid(command);

                // Commit Transaction
                command.Transaction.Commit();

                OswPesan.pesanInfo("Proses " + dokumen + " berhasil.");

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

    }
}