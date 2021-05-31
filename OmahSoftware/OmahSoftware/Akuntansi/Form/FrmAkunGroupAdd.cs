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
    public partial class FrmAkunGroupAdd : DevExpress.XtraEditors.XtraForm {
        private String id = "AKUNGROUP";
        private String dokumen = "AKUNGROUP";
        private Boolean isAdd;

        public FrmAkunGroupAdd(bool pIsAdd) {
            isAdd = pIsAdd;
            InitializeComponent();
        }

        private void FrmAkunGroupAdd_Load(object sender, EventArgs e) {
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
                cmbAkunKategori.Enabled = false;
                cmbAkunSubKategori.Enabled = false;

                // data
                DataAkunGroup dAkunGroup = new DataAkunGroup(command, strngKode);
                cmbAkunSubKategori.EditValue = dAkunGroup.akunsubkategori;
                DataAkunSubKategori dAkunSubKategori = new DataAkunSubKategori(command, dAkunGroup.akunsubkategori);
                cmbAkunKategori.EditValue = dAkunSubKategori.akunkategori;
                txtNama.EditValue = dAkunGroup.nama;
            } else {
                OswControlDefaultProperties.resetAllInput(this);

                cmbAkunKategori.EditValue = OswCombo.getFirstEditValue(cmbAkunKategori);
                cmbAkunSubKategori = ComboQueryUmum.getAkunSubKategori(cmbAkunSubKategori, command, cmbAkunKategori.EditValue == null ? "" : cmbAkunKategori.EditValue.ToString());
                cmbAkunSubKategori.EditValue = OswCombo.getFirstEditValue(cmbAkunSubKategori);
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
                dxValidationProvider1.SetValidationRule(cmbAkunSubKategori, OswValidation.IsNotBlank());
                dxValidationProvider1.SetValidationRule(txtNama, OswValidation.IsNotBlank());

                if(!dxValidationProvider1.Validate()) {
                    foreach(Control x in dxValidationProvider1.GetInvalidControls()) {
                        dxValidationProvider1.SetIconAlignment(x, ErrorIconAlignment.MiddleRight);
                    }
                    return;
                }

                // simpan header
                if(cmbAkunSubKategori.EditValue == null) {
                    throw new Exception("Silahkan pilih Sub Kategori");
                }

                String strngKode = txtKode.Text;
                String strngAkunSubKategori = cmbAkunSubKategori.EditValue.ToString();
                String strngNama = txtNama.EditValue.ToString();

                DataAkunGroup dAkunGroup = new DataAkunGroup(command, strngKode);
                dAkunGroup.akunsubkategori = strngAkunSubKategori;
                dAkunGroup.nama = strngNama;

                if(this.isAdd) {
                    dAkunGroup.tambah();
                } else {
                    dAkunGroup.ubah();
                }

                // tulis log
                OswLog.setTransaksi(command, dokumen, dAkunGroup.ToString());

                // reload grid di form header
                FrmAkunGroup frmAkunGroup = (FrmAkunGroup)this.Owner;
                frmAkunGroup.setGrid(command);

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