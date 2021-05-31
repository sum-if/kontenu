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
using OmahSoftware.Umum;
using DevExpress.XtraEditors.DXErrorProvider;
using System.Net;
using OmahSoftware.OswLib;
using DevExpress.XtraEditors.Controls;

namespace OmahSoftware.Pembelian {
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
                OswControlDefaultProperties.setAngka(txtMaksJthTempo);
                cmbKota = ComboQueryUmum.getKota(cmbKota, command);

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

                // data
                DataSupplier dSupplier = new DataSupplier(command, strngKode);
                txtKode.EditValue = strngKode;
                txtNama.EditValue = dSupplier.nama;
                cmbKota.EditValue = dSupplier.kota;
                txtAlamat.Text = dSupplier.alamat;
                txtTelepon.Text = dSupplier.telp;
                txtCP.Text = dSupplier.cp;
                txtEmail.Text = dSupplier.email;
                txtCatatan.Text = dSupplier.catatan;

                txtBank.Text = dSupplier.bank;
                txtNoRek.Text = dSupplier.norek;
                txtAtasNama.Text = dSupplier.atasnama;

                txtMaksJthTempo.EditValue = dSupplier.maksjatuhtempo;

                txtNPWPKode.Text = dSupplier.npwpkode;
                txtNPWPNama.Text = dSupplier.npwpnama;
                txtNPWPJalan.Text = dSupplier.npwpjalan;
                txtNPWPBlok.Text = dSupplier.npwpblok;
                txtNPWPNomor.Text = dSupplier.npwpnomor;
                txtNPWPRT.Text = dSupplier.npwprt;
                txtNPWPRW.Text = dSupplier.npwprw;
                txtNPWPKelurahan.Text = dSupplier.npwpkelurahan;
                txtNPWPKecamatan.Text = dSupplier.npwpkecamatan;
                txtNPWPKabupaten.Text = dSupplier.npwpkabupaten;
                txtNPWPProvinsi.Text = dSupplier.npwpprovinsi;
                txtNPWPKodePos.Text = dSupplier.npwpkodepos;
                txtNPWPNoTelpn.Text = dSupplier.npwptelp;

                chkPajak.Checked = dSupplier.pajak == Constants.STATUS_YA;
                chkStatus.Checked = dSupplier.status == Constants.STATUS_AKTIF;
            } else {
                OswControlDefaultProperties.resetAllInput(this);

                chkPajak.Checked = true;
                chkStatus.Checked = true;
                cmbKota.EditValue = OswCombo.getFirstEditValue(cmbKota);
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
                dxValidationProvider1.SetValidationRule(cmbKota, OswValidation.IsNotBlank());

                if(!dxValidationProvider1.Validate()) {
                    foreach(Control x in dxValidationProvider1.GetInvalidControls()) {
                        dxValidationProvider1.SetIconAlignment(x, ErrorIconAlignment.MiddleRight);
                    }
                    return;
                }


                if(chkPajak.Checked) {
                    //validasi semua inputan pajak harus terisi
                    dxValidationProvider1.SetValidationRule(txtNPWPKode, OswValidation.IsNotBlank());
                    dxValidationProvider1.SetValidationRule(txtNPWPNama, OswValidation.IsNotBlank());
                    dxValidationProvider1.SetValidationRule(txtNPWPJalan, OswValidation.IsNotBlank());
                    dxValidationProvider1.SetValidationRule(txtNPWPBlok, OswValidation.IsNotBlank());
                    dxValidationProvider1.SetValidationRule(txtNPWPNomor, OswValidation.IsNotBlank());
                    dxValidationProvider1.SetValidationRule(txtNPWPRT, OswValidation.IsNotBlank());
                    dxValidationProvider1.SetValidationRule(txtNPWPRW, OswValidation.IsNotBlank());
                    dxValidationProvider1.SetValidationRule(txtNPWPKelurahan, OswValidation.IsNotBlank());
                    dxValidationProvider1.SetValidationRule(txtNPWPKecamatan, OswValidation.IsNotBlank());
                    dxValidationProvider1.SetValidationRule(txtNPWPKabupaten, OswValidation.IsNotBlank());
                    dxValidationProvider1.SetValidationRule(txtNPWPProvinsi, OswValidation.IsNotBlank());
                    dxValidationProvider1.SetValidationRule(txtNPWPKodePos, OswValidation.IsNotBlank());
                    dxValidationProvider1.SetValidationRule(txtNPWPNoTelpn, OswValidation.IsNotBlank());

                    if(!dxValidationProvider1.Validate()) {
                        foreach(Control x in dxValidationProvider1.GetInvalidControls()) {
                            dxValidationProvider1.SetIconAlignment(x, ErrorIconAlignment.MiddleRight);
                        }
                        return;
                    }
                }

                // simpan header
                String strngKode = txtKode.Text;
                String strngNama = txtNama.Text;
                String strngKota = cmbKota.EditValue.ToString();
                String strngAlamat = txtAlamat.Text;
                String strngTelp = txtTelepon.Text;
                String strngCP = txtCP.Text;
                String strngEmail = txtEmail.Text;
                String strngCatatan = txtCatatan.Text;
                String strngBank = txtBank.Text;
                String strngNoRek = txtNoRek.Text;
                String strngAtasNama = txtAtasNama.Text;
                String strngMaksJthTempo = txtMaksJthTempo.EditValue.ToString();

                String strngNPWPKode = txtNPWPKode.Text;
                String strngNPWPNama = txtNPWPNama.Text;
                String strngNPWPJalan = txtNPWPJalan.Text;
                String strngNPWPBlok = txtNPWPBlok.Text;
                String strngNPWPNomor = txtNPWPNomor.Text;
                String strngNPWPRT = txtNPWPRT.Text;
                String strngNPWPRW = txtNPWPRW.Text;
                String strngNPWPKelurahan = txtNPWPKelurahan.Text;
                String strngNPWPKecamatan = txtNPWPKecamatan.Text;
                String strngNPWPKabupaten = txtNPWPKabupaten.Text;
                String strngNPWPProvinsi = txtNPWPProvinsi.Text;
                String strngNPWPKodePos = txtNPWPKodePos.Text;
                String strngNPWPNoTelpn = txtNPWPNoTelpn.Text;

                String strngPajak = chkPajak.Checked ? Constants.STATUS_YA : Constants.STATUS_TIDAK;
                String strngStatus = chkStatus.Checked ? Constants.STATUS_AKTIF : Constants.STATUS_AKTIF_TIDAK;

                DataSupplier dSupplier = new DataSupplier(command, strngKode);
                dSupplier.nama = strngNama;
                dSupplier.kota = strngKota;
                dSupplier.alamat = strngAlamat;
                dSupplier.telp = strngTelp;
                dSupplier.cp = strngCP;
                dSupplier.email = strngEmail;
                dSupplier.catatan = strngCatatan;
                dSupplier.bank = strngBank;
                dSupplier.norek = strngNoRek;
                dSupplier.atasnama = strngAtasNama;
                dSupplier.maksjatuhtempo = strngMaksJthTempo;

                dSupplier.npwpkode = strngNPWPKode;
                dSupplier.npwpnama = strngNPWPNama;
                dSupplier.npwpjalan = strngNPWPJalan;
                dSupplier.npwpblok = strngNPWPBlok;
                dSupplier.npwpnomor = strngNPWPNomor;
                dSupplier.npwprt = strngNPWPRT;
                dSupplier.npwprw = strngNPWPRW;
                dSupplier.npwpkelurahan = strngNPWPKelurahan;
                dSupplier.npwpkecamatan = strngNPWPKecamatan;
                dSupplier.npwpkabupaten = strngNPWPKabupaten;
                dSupplier.npwpprovinsi = strngNPWPProvinsi;
                dSupplier.npwpkodepos = strngNPWPKodePos;
                dSupplier.npwptelp = strngNPWPNoTelpn;

                dSupplier.pajak = strngPajak;
                dSupplier.status = strngStatus;

                if(this.isAdd) {
                    dSupplier.tambah();
                    // update kode header --> setelah generate
                    strngKode = dSupplier.kode;
                } else {
                    dSupplier.ubah();
                }

                // tulis log
                OswLog.setTransaksi(command, dokumen, dSupplier.ToString());

                // reload grid di form header
                FrmSupplier frmSupplier = (FrmSupplier)this.Owner;
                frmSupplier.setGrid(command);

                // Commit Transaction
                command.Transaction.Commit();

                OswPesan.pesanInfo("Proses " + dokumen + " berhasil.");

                tabControl1.SelectedTabPageIndex = 0;

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