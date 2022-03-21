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

namespace Kontenu.Master {
    public partial class FrmSubconAdd : DevExpress.XtraEditors.XtraForm {
        private String id = "SUBCON";
        private String dokumen = "SUBCON";
        private Boolean isAdd;

        public FrmSubconAdd(bool pIsAdd) {
            isAdd = pIsAdd;
            InitializeComponent();
        }

        private void FrmSubconAdd_Load(object sender, EventArgs e) {
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
                OswControlDefaultProperties.setAngka(txtJatuhTempo);

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
                DataSubcon dSubcon = new DataSubcon(command, strngKode);
                txtNama.Text = dSubcon.nama;
                txtAlamat.Text = dSubcon.alamat;
                txtProvinsi.Text = dSubcon.provinsi;
                txtKota.Text = dSubcon.kota;
                txtKodePos.Text = dSubcon.kodepos;
                txtTelp.Text = dSubcon.telp;
                txtHandphone.Text = dSubcon.handphone;
                txtEmail.Text = dSubcon.email;
                txtJatuhTempo.EditValue = dSubcon.jatuhtempo;
                rdoJenisInternal.Checked = (dSubcon.jenis == Constants.JENIS_SUBCON_INTERNAL);
                rdoJenisExternal.Checked = (dSubcon.jenis == Constants.JENIS_SUBCON_EXTERNAL);
                txtNIK.Text = dSubcon.nik;
            } else {
                OswControlDefaultProperties.resetAllInput(this);
                rdoJenisInternal.Checked = true;
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
                dxValidationProvider1.SetValidationRule(txtNIK, OswValidation.IsNotBlank());
                dxValidationProvider1.SetValidationRule(txtTelp, OswValidation.IsNotBlank());
                dxValidationProvider1.SetValidationRule(txtAlamat, OswValidation.IsNotBlank());

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
                String strngTelp = txtTelp.Text;
                String strngHandphone = txtHandphone.Text;
                String strngEmail = txtEmail.Text;
                String strngJatuhTempo = txtJatuhTempo.EditValue.ToString();
                String strngNIK = txtNIK.Text;

                DataSubcon dSubcon = new DataSubcon(command, strngKode);
                dSubcon.nama = strngNama;
                dSubcon.alamat = strngAlamat;
                dSubcon.provinsi = strngProvinsi;
                dSubcon.kota = strngKota;
                dSubcon.kodepos = strngKodePos;
                dSubcon.telp = strngTelp;
                dSubcon.handphone = strngHandphone;
                dSubcon.email = strngEmail;
                dSubcon.jatuhtempo = strngJatuhTempo;
                dSubcon.jenis = rdoJenisInternal.Checked ? Constants.JENIS_SUBCON_INTERNAL : Constants.JENIS_SUBCON_EXTERNAL;
                dSubcon.nik = strngNIK;

                if(this.isAdd) {
                    dSubcon.tambah();
                    // update kode header --> setelah generate
                    strngKode = dSubcon.kode;
                } else {
                    dSubcon.ubah();
                }

                // tulis log
                // OswLog.setTransaksi(command, dokumen, dSubcon.ToString());

                // reload grid di form header
                FrmSubcon frmSubcon = (FrmSubcon)this.Owner;
                frmSubcon.setGrid(command);

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