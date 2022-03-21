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
    public partial class FrmSupplierAdd : DevExpress.XtraEditors.XtraForm {
        private String id = "SUPPLIER";
        private String dokumen = "SUPPLIER";
        private Boolean isAdd;

        public FrmSupplierAdd(bool pIsAdd) {
            isAdd = pIsAdd;
            InitializeComponent();
        }

        private void FrmSupplierAdd_Load(object sender, EventArgs e) {
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
                DataSupplier dSupplier = new DataSupplier(command, strngKode);
                txtNama.Text = dSupplier.nama;
                txtAlamat.Text = dSupplier.alamat;
                txtProvinsi.Text = dSupplier.provinsi;
                txtKota.Text = dSupplier.kota;
                txtKodePos.Text = dSupplier.kodepos;
                txtTelp.Text = dSupplier.telp;
                txtHandphone.Text = dSupplier.handphone;
                txtEmail.Text = dSupplier.email;
                txtJatuhTempo.EditValue = dSupplier.jatuhtempo;
            } else {
                OswControlDefaultProperties.resetAllInput(this);
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

                DataSupplier dSupplier = new DataSupplier(command, strngKode);
                dSupplier.nama = strngNama;
                dSupplier.alamat = strngAlamat;
                dSupplier.provinsi = strngProvinsi;
                dSupplier.kota = strngKota;
                dSupplier.kodepos = strngKodePos;
                dSupplier.telp = strngTelp;
                dSupplier.handphone = strngHandphone;
                dSupplier.email = strngEmail;
                dSupplier.jatuhtempo = strngJatuhTempo;

                if(this.isAdd) {
                    dSupplier.tambah();
                    // update kode header --> setelah generate
                    strngKode = dSupplier.kode;
                } else {
                    dSupplier.ubah();
                }

                // tulis log
                // OswLog.setTransaksi(command, dokumen, dSupplier.ToString());

                // reload grid di form header
                FrmSupplier frmSupplier = (FrmSupplier)this.Owner;
                frmSupplier.setGrid(command);

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