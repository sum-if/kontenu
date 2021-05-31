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

namespace OmahSoftware.Penjualan {
    public partial class FrmCustomerAdd : DevExpress.XtraEditors.XtraForm {
        private String id = "CUSTOMER";
        private String dokumen = "CUSTOMER";
        private Boolean isAdd;

        public FrmCustomerAdd(bool pIsAdd) {
            isAdd = pIsAdd;
            InitializeComponent();
        }

        private void FrmCustomerAdd_Load(object sender, EventArgs e) {
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

                cmbKota = ComboQueryUmum.getKota(cmbKota, command);

                OswControlDefaultProperties.setAngka(txtLimitPiutang);
                OswControlDefaultProperties.setAngka(txtMaksJatuhTempo);

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
                DataCustomer dCustomer = new DataCustomer(command, strngKode);
                txtKode.EditValue = strngKode;
                txtNama.EditValue = dCustomer.nama;
                txtAlias.EditValue = dCustomer.alias;
                txtCP.Text = dCustomer.cp;
                cmbKota.EditValue = dCustomer.kota;
                txtAlamat.Text = dCustomer.alamat;
                txtAlamatAlias.Text = dCustomer.alamatalias;
                txtTelepon.Text = dCustomer.telp;
                txtEmail.Text = dCustomer.email;
                txtCatatan.Text = dCustomer.catatan;
                txtBank.Text = dCustomer.bank;
                txtNoRek.Text = dCustomer.norek;
                txtAtasNama.Text = dCustomer.atasnama;
                txtLimitPiutang.EditValue = dCustomer.limitpiutang;
                txtMaksJatuhTempo.EditValue = dCustomer.maksjatuhtempo;
                txtNPWPKode.Text = dCustomer.npwpkode;
                txtNPWPNama.Text = dCustomer.npwpnama;
                txtNPWPJalan.Text = dCustomer.npwpjalan;
                txtNPWPBlok.Text = dCustomer.npwpblok;
                txtNPWPNomor.Text = dCustomer.npwpnomor;
                txtNPWPRT.Text = dCustomer.npwprt;
                txtNPWPRW.Text = dCustomer.npwprw;
                txtNPWPKelurahan.Text = dCustomer.npwpkelurahan;
                txtNPWPKecamatan.Text = dCustomer.npwpkecamatan;
                txtNPWPKabupaten.Text = dCustomer.npwpkabupaten;
                txtNPWPProvinsi.Text = dCustomer.npwpprovinsi;
                txtNPWPKodePos.Text = dCustomer.npwpkodepos;
                txtNPWPNoTelpn.Text = dCustomer.npwptelp;

                chkPajak.Checked = dCustomer.pajak == Constants.STATUS_YA;
                chkStatus.Checked = dCustomer.status == Constants.STATUS_AKTIF;
                chkGunggung.Checked = dCustomer.gunggung == Constants.STATUS_YA;
            } else {
                OswControlDefaultProperties.resetAllInput(this);
                chkPajak.Checked = true;
                chkStatus.Checked = true;
                chkGunggung.Checked = false;
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
                String strngAlias = txtAlias.Text;
                String strngCP = txtCP.Text;
                String strngKota = cmbKota.EditValue.ToString();
                String strngAlamat = txtAlamat.Text;
                String strngAlamatAlias = txtAlamatAlias.Text;
                String strngTelp = txtTelepon.Text;
                String strngEmail = txtEmail.Text;
                String strngCatatan = txtCatatan.Text;
                String strngBank = txtBank.Text;
                String strngNoRek = txtNoRek.Text;
                String strngAtasNama = txtAtasNama.Text;
                String strngLimitPiutang = txtLimitPiutang.EditValue.ToString();
                String strngMaksJatuhTempo = txtMaksJatuhTempo.EditValue.ToString();
                String strngNPWPKode = txtNPWPKode.Text;
                String strngNPWPNama = txtNPWPNama.Text;
                String strngNPWPJalan = txtNPWPJalan.Text;
                String strngNPWPBlok = txtNPWPBlok.Text;
                String strngNPWPNomor = txtNPWPNomor.Text;
                String strngNPWPRT = txtNPWPRT.Text;
                String strngNPWPRW = txtNPWPRW.Text;
                String strngNPWPkelurahan = txtNPWPKelurahan.Text;
                String strngNPWPKecamatan = txtNPWPKecamatan.Text;
                String strngNPWPKabupaten = txtNPWPKabupaten.Text;
                String strngNPWPProvinsi = txtNPWPProvinsi.Text;
                String strngNPWPKodePos = txtNPWPKodePos.Text;
                String strngNPWPNoTelpn = txtNPWPNoTelpn.Text;

                String strngPajak = chkPajak.Checked ? Constants.STATUS_YA : Constants.STATUS_TIDAK;
                String strngStatus = chkStatus.Checked ? Constants.STATUS_AKTIF : Constants.STATUS_AKTIF_TIDAK;
                String strngGunggung = chkGunggung.Checked ? Constants.STATUS_YA : Constants.STATUS_TIDAK;

                DataCustomer dCustomer = new DataCustomer(command, strngKode);
                dCustomer.nama = strngNama;
                dCustomer.alias = strngAlias;
                dCustomer.cp = strngCP;
                dCustomer.kota = strngKota;
                dCustomer.alamat = strngAlamat;
                dCustomer.alamatalias = strngAlamatAlias;
                dCustomer.telp = strngTelp;
                dCustomer.email = strngEmail;
                dCustomer.catatan = strngCatatan;
                dCustomer.bank = strngBank;
                dCustomer.norek = strngNoRek;
                dCustomer.atasnama = strngAtasNama;
                dCustomer.limitpiutang = strngLimitPiutang;
                dCustomer.maksjatuhtempo = strngMaksJatuhTempo;
                dCustomer.npwpkode = strngNPWPKode;
                dCustomer.npwpnama = strngNPWPNama;
                dCustomer.npwpjalan = strngNPWPJalan;
                dCustomer.npwpblok = strngNPWPBlok;
                dCustomer.npwpnomor = strngNPWPNomor;
                dCustomer.npwprt = strngNPWPRT;
                dCustomer.npwprw = strngNPWPRW;
                dCustomer.npwpkelurahan = strngNPWPkelurahan;
                dCustomer.npwpkecamatan = strngNPWPKecamatan;
                dCustomer.npwpkabupaten = strngNPWPKabupaten;
                dCustomer.npwpprovinsi = strngNPWPProvinsi;
                dCustomer.npwpkodepos = strngNPWPKodePos;
                dCustomer.npwptelp = strngNPWPNoTelpn;
                dCustomer.pajak = strngPajak;
                dCustomer.gunggung = strngGunggung;
                dCustomer.status = strngStatus;

                if(this.isAdd) {
                    dCustomer.tambah();
                    // update kode header --> setelah generate
                    strngKode = dCustomer.kode;
                } else {
                    dCustomer.ubah();
                }

                // tulis log
                OswLog.setTransaksi(command, dokumen, dCustomer.ToString());

                // reload grid di form header
                FrmCustomer frmCustomer = (FrmCustomer)this.Owner;
                frmCustomer.setGrid(command);

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