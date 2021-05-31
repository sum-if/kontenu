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

namespace OmahSoftware.Akuntansi {
    public partial class FrmAkunAdd : DevExpress.XtraEditors.XtraForm {
        private String id = "AKUN";
        private String dokumen = "AKUN";
        private Boolean isAdd;

        public FrmAkunAdd(bool pIsAdd) {
            isAdd = pIsAdd;
            InitializeComponent();
        }

        private void FrmAkunAdd_Load(object sender, EventArgs e) {
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
                cmbAkunKategori = ComboQueryUmum.getAkunKategori(cmbAkunKategori, command);
                cmbAkunKategori.EditValue = OswCombo.getFirstEditValue(cmbAkunKategori);
                cmbAkunSubKategori = ComboQueryUmum.getAkunSubKategori(cmbAkunSubKategori, command, cmbAkunKategori.EditValue == null ? "" : cmbAkunKategori.EditValue.ToString());
                cmbAkunSubKategori.EditValue = OswCombo.getFirstEditValue(cmbAkunSubKategori);
                cmbAkunGroup = ComboQueryUmum.getAkunGroup(cmbAkunGroup, command, cmbAkunKategori.EditValue == null ? "" : cmbAkunKategori.EditValue.ToString(), cmbAkunSubKategori.EditValue == null ? "" : cmbAkunSubKategori.EditValue.ToString(), false, true);
                cmbAkunGroup.EditValue = OswCombo.getFirstEditValue(cmbAkunGroup);
                cmbAkunSubGroup = ComboQueryUmum.getAkunSubGroup(cmbAkunSubGroup, command, cmbAkunKategori.EditValue == null ? "" : cmbAkunKategori.EditValue.ToString(), cmbAkunSubKategori.EditValue == null ? "" : cmbAkunSubKategori.EditValue.ToString(), cmbAkunGroup.EditValue == null ? "" : cmbAkunGroup.EditValue.ToString(), false, true);
                cmbAkunSubGroup.EditValue = OswCombo.getFirstEditValue(cmbAkunSubGroup);
                cmbSaldoNormal = ComboConstantUmum.getJenisSaldoAkun(cmbSaldoNormal);
                cmbSaldoNormal.ItemIndex = 0;

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
                //cmbAkunKategori.Enabled = false;
                //cmbAkunSubKategori.Enabled = false;
                //cmbAkunGroup.Enabled = false;
                //cmbAkunSubGroup.Enabled = false;
                txtKode.Enabled = false;

                // data
                DataAkun dAkun = new DataAkun(command, strngKode);
                cmbAkunKategori.EditValue = dAkun.akunkategori;
                cmbAkunSubKategori.EditValue = dAkun.akunsubkategori;
                cmbAkunGroup.EditValue = dAkun.akungroup;
                cmbAkunSubGroup.EditValue = dAkun.akunsubgroup;
                
                cmbSaldoNormal.EditValue = dAkun.saldonormal;
                
                txtNama.EditValue = dAkun.nama;
                chkJurnalManual.Checked = dAkun.jurnalmanual == Constants.STATUS_YA;
                chkStatus.Checked = dAkun.status == Constants.STATUS_AKTIF;
            } else {
                OswControlDefaultProperties.resetAllInput(this);
                cmbAkunKategori.EditValue = OswCombo.getFirstEditValue(cmbAkunKategori);
                cmbAkunSubKategori = ComboQueryUmum.getAkunSubKategori(cmbAkunSubKategori, command, cmbAkunKategori.EditValue.ToString());
                cmbAkunSubKategori.EditValue = OswCombo.getFirstEditValue(cmbAkunSubKategori);
                cmbAkunGroup = ComboQueryUmum.getAkunGroup(cmbAkunGroup, command, cmbAkunKategori.EditValue == null ? "" : cmbAkunKategori.EditValue.ToString(), cmbAkunSubKategori.EditValue == null ? "" : cmbAkunSubKategori.EditValue.ToString(), false, true);
                cmbAkunGroup.EditValue = OswCombo.getFirstEditValue(cmbAkunGroup);
                cmbAkunSubGroup = ComboQueryUmum.getAkunSubGroup(cmbAkunSubGroup, command, cmbAkunKategori.EditValue == null ? "" : cmbAkunKategori.EditValue.ToString(), cmbAkunSubKategori.EditValue == null ? "" : cmbAkunSubKategori.EditValue.ToString(), cmbAkunGroup.EditValue == null ? "" : cmbAkunGroup.EditValue.ToString(), false, true);
                cmbAkunSubGroup.EditValue = OswCombo.getFirstEditValue(cmbAkunSubGroup);
                chkJurnalManual.Checked = false;
                chkStatus.Checked = true;
                cmbSaldoNormal.ItemIndex = 0;
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
                dxValidationProvider1.SetValidationRule(cmbAkunKategori, OswValidation.IsNotBlank());
                dxValidationProvider1.SetValidationRule(cmbAkunSubKategori, OswValidation.IsNotBlank());
                dxValidationProvider1.SetValidationRule(cmbSaldoNormal, OswValidation.IsNotBlank());
                dxValidationProvider1.SetValidationRule(txtNama, OswValidation.IsNotBlank());
                dxValidationProvider1.SetValidationRule(cmbSaldoNormal, OswValidation.IsNotBlank());
                dxValidationProvider1.SetValidationRule(txtKode, OswValidation.IsNotBlank());

                if(!dxValidationProvider1.Validate()) {
                    foreach(Control x in dxValidationProvider1.GetInvalidControls()) {
                        dxValidationProvider1.SetIconAlignment(x, ErrorIconAlignment.MiddleRight);
                    }
                    return;
                }

                // simpan header
                if(cmbAkunKategori.EditValue == null) {
                    throw new Exception("Silahkan pilih Kategori");
                }

                if(cmbAkunSubKategori.EditValue == null) {
                    throw new Exception("Silahkan pilih Sub Kategori");
                }

                String strngKode = txtKode.Text;
                String strngNama = txtNama.Text;
                String strngAkunKategori = cmbAkunKategori.EditValue.ToString();
                String strngAkunSubKategori = cmbAkunSubKategori.EditValue.ToString();
                String strngAkunGroup = cmbAkunGroup.EditValue.ToString();
                String strngAkunSubGroup = cmbAkunSubGroup.EditValue.ToString();
                String strngSaldoNormal = cmbSaldoNormal.EditValue.ToString();
                String strngJurnalManual = chkJurnalManual.Checked ? Constants.STATUS_YA : Constants.STATUS_TIDAK;
                String strngStatus = chkStatus.Checked ? Constants.STATUS_AKTIF : Constants.STATUS_AKTIF_TIDAK;

                DataAkun dAkun = new DataAkun(command, strngKode);
                dAkun.nama = strngNama;
                dAkun.akunkategori = strngAkunKategori;
                dAkun.akunsubkategori = strngAkunSubKategori;
                dAkun.akungroup = strngAkunGroup;
                dAkun.akunsubgroup = strngAkunSubGroup;
                dAkun.saldonormal = strngSaldoNormal;
                dAkun.jurnalmanual = strngJurnalManual;
                dAkun.status = strngStatus;

                if(this.isAdd) {
                    dAkun.tambah();
                } else {
                    dAkun.ubah();
                }

                // tulis log
                OswLog.setTransaksi(command, dokumen, dAkun.ToString());

                // reload grid di form header
                FrmAkun frmAkun = (FrmAkun)this.Owner;
                frmAkun.setGrid(command);

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

        private void cmbAkunKategori_EditValueChanged(object sender, EventArgs e) {
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
                cmbAkunSubKategori = ComboQueryUmum.getAkunSubKategori(cmbAkunSubKategori, command, cmbAkunKategori.EditValue == null ? "" : cmbAkunKategori.EditValue.ToString());
                cmbAkunSubKategori.EditValue = OswCombo.getFirstEditValue(cmbAkunSubKategori);
                cmbAkunGroup = ComboQueryUmum.getAkunGroup(cmbAkunGroup, command, cmbAkunKategori.EditValue == null ? "" : cmbAkunKategori.EditValue.ToString(), cmbAkunSubKategori.EditValue == null ? "" : cmbAkunSubKategori.EditValue.ToString(), false, true);
                cmbAkunGroup.EditValue = OswCombo.getFirstEditValue(cmbAkunGroup);
                cmbAkunSubGroup = ComboQueryUmum.getAkunSubGroup(cmbAkunSubGroup, command, cmbAkunKategori.EditValue == null ? "" : cmbAkunKategori.EditValue.ToString(), cmbAkunSubKategori.EditValue == null ? "" : cmbAkunSubKategori.EditValue.ToString(), cmbAkunGroup.EditValue == null ? "" : cmbAkunGroup.EditValue.ToString(), false, true);
                cmbAkunSubGroup.EditValue = OswCombo.getFirstEditValue(cmbAkunSubGroup);

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

        private void cmbAkunSubKategori_EditValueChanged(object sender, EventArgs e) {
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
                cmbAkunGroup = ComboQueryUmum.getAkunGroup(cmbAkunGroup, command, cmbAkunKategori.EditValue == null ? "" : cmbAkunKategori.EditValue.ToString(), cmbAkunSubKategori.EditValue == null ? "" : cmbAkunSubKategori.EditValue.ToString(), false, true);
                cmbAkunGroup.EditValue = OswCombo.getFirstEditValue(cmbAkunGroup);
                cmbAkunSubGroup = ComboQueryUmum.getAkunSubGroup(cmbAkunSubGroup, command, cmbAkunKategori.EditValue == null ? "" : cmbAkunKategori.EditValue.ToString(), cmbAkunSubKategori.EditValue == null ? "" : cmbAkunSubKategori.EditValue.ToString(), cmbAkunGroup.EditValue == null ? "" : cmbAkunGroup.EditValue.ToString(), false, true);
                cmbAkunSubGroup.EditValue = OswCombo.getFirstEditValue(cmbAkunSubGroup);

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

        private void cmbAkunGroup_EditValueChanged(object sender, EventArgs e) {
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
                cmbAkunSubGroup = ComboQueryUmum.getAkunSubGroup(cmbAkunSubGroup, command, cmbAkunKategori.EditValue == null ? "" : cmbAkunKategori.EditValue.ToString(), cmbAkunSubKategori.EditValue == null ? "" : cmbAkunSubKategori.EditValue.ToString(), cmbAkunGroup.EditValue == null ? "" : cmbAkunGroup.EditValue.ToString(), false, true);
                cmbAkunSubGroup.EditValue = OswCombo.getFirstEditValue(cmbAkunSubGroup);
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