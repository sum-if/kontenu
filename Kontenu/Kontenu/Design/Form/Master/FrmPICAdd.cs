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
using DevExpress.XtraEditors.Controls;
using Kontenu.Umum;

namespace Kontenu.Master {
    public partial class FrmPICAdd : DevExpress.XtraEditors.XtraForm {
        private String id = "PIC";
        private String dokumen = "PIC";
        private Boolean isAdd;

        public FrmPICAdd(bool pIsAdd) {
            isAdd = pIsAdd;
            InitializeComponent();
        }

        private void FrmPICAdd_Load(object sender, EventArgs e) {
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

                cmbJabatan = ComboQueryUmum.getJabatan(cmbJabatan, command);

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
                txtKode.Enabled = false;

                // data
                String strngKode = txtKode.Text;
                DataPIC dPIC = new DataPIC(command, strngKode);
                txtKTP.Text = dPIC.ktp;
                txtNama.Text = dPIC.nama;
                txtAlamat.Text = dPIC.alamat;
                txtProvinsi.Text = dPIC.provinsi;
                txtKota.Text = dPIC.kota;
                txtKodePos.Text = dPIC.kodepos;
                cmbJabatan.EditValue = dPIC.jabatan;
                txtHandphone.Text = dPIC.handphone;
                txtEmail.Text = dPIC.email;
            } else {
                OswControlDefaultProperties.resetAllInput(this);
                cmbJabatan.ItemIndex = 0;
            }

            txtKode.Focus();
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
                dxValidationProvider1.SetValidationRule(txtAlamat, OswValidation.IsNotBlank());
                dxValidationProvider1.SetValidationRule(cmbJabatan, OswValidation.IsNotBlank());

                if(!dxValidationProvider1.Validate()) {
                    foreach(Control x in dxValidationProvider1.GetInvalidControls()) {
                        dxValidationProvider1.SetIconAlignment(x, ErrorIconAlignment.MiddleRight);
                    }
                    return;
                }

                // simpan header
                String strngKode = txtKode.Text;
                String strngNama = txtNama.Text;
                String strngAlamat = txtAlamat.Text;
                String strngProvinsi = txtProvinsi.Text;
                String strngKota = txtKota.Text;
                String strngKodePos = txtKodePos.Text;
                String strngJabatan = cmbJabatan.EditValue.ToString();
                String strngHandphone = txtHandphone.Text;
                String strngEmail = txtEmail.Text;
                String strngKTP = txtKTP.Text;

                DataPIC dPIC = new DataPIC(command, strngKode);
                dPIC.nama = strngNama;
                dPIC.alamat = strngAlamat;
                dPIC.provinsi = strngProvinsi;
                dPIC.kota = strngKota;
                dPIC.kodepos = strngKodePos;
                dPIC.jabatan = strngJabatan;
                dPIC.handphone = strngHandphone;
                dPIC.email = strngEmail;
                dPIC.ktp = strngKTP;

                if(this.isAdd) {
                    dPIC.tambah();
                    // update kode header --> setelah generate
                    strngKode = dPIC.kode;
                } else {
                    dPIC.ubah();
                }

                // tulis log
                // OswLog.setTransaksi(command, dokumen, dPIC.ToString());

                // reload grid di form header
                FrmPIC frmPIC = (FrmPIC)this.Owner;
                frmPIC.setGrid(command);

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