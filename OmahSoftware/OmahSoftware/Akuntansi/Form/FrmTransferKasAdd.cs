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
using DevExpress.XtraEditors.Controls;

namespace OmahSoftware.Akuntansi {
    public partial class FrmTransferKasAdd : DevExpress.XtraEditors.XtraForm {
        private String id = "TRANSFERKAS";
        private String dokumen = "TRANSFERKAS";
        private String dokumenDetail = "TRANSFERKAS";
        private Boolean isAdd;

        public FrmTransferKasAdd(bool pIsAdd) {
            isAdd = pIsAdd;
            InitializeComponent();
        }

        private void FrmTransferKasAdd_Load(object sender, EventArgs e) {
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

                this.dokumenDetail = "Tambah " + dOswJenisDokumen.nama;

                this.Text = this.dokumen;

                OswControlDefaultProperties.setInput(this, id, command);
                OswControlDefaultProperties.setTanggal(deTanggal);
                OswControlDefaultProperties.setAngka(txtNominal);

                cmbAkunAsal = ComboQueryUmum.getAkun(cmbAkunAsal, command, "akun_kas_bank");
                cmbAkunAsal.EditValue = OswCombo.getFirstEditValue(cmbAkunAsal);

                cmbAkunTujuan = ComboQueryUmum.getAkun(cmbAkunTujuan, command, "akun_kas_bank");
                cmbAkunTujuan.EditValue = OswCombo.getFirstEditValue(cmbAkunTujuan);

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
                DataTransferKas dTransferKas = new DataTransferKas(command, strngKode);
                deTanggal.DateTime = OswDate.getDateTimeFromStringTanggal(dTransferKas.tanggal);
                txtCatatan.EditValue = dTransferKas.catatan;
                cmbAkunAsal.EditValue = dTransferKas.akunawal;
                cmbAkunTujuan.EditValue = dTransferKas.akunakhir;
                txtNominal.EditValue = dTransferKas.nominal;
            } else {
                OswControlDefaultProperties.resetAllInput(this);
                cmbAkunAsal.EditValue = OswCombo.getFirstEditValue(cmbAkunAsal);
                cmbAkunTujuan.EditValue = OswCombo.getFirstEditValue(cmbAkunTujuan);
                txtNominal.EditValue = "0";
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
                dxValidationProvider1.SetValidationRule(deTanggal, OswValidation.IsNotBlank());
                dxValidationProvider1.SetValidationRule(cmbAkunAsal, OswValidation.IsNotBlank());
                dxValidationProvider1.SetValidationRule(cmbAkunTujuan, OswValidation.IsNotBlank());

                if(!dxValidationProvider1.Validate()) {
                    foreach(Control x in dxValidationProvider1.GetInvalidControls()) {
                        dxValidationProvider1.SetIconAlignment(x, ErrorIconAlignment.MiddleRight);
                    }
                    return;
                }

                String strngKode = txtKode.Text;
                String strngTanggal = deTanggal.Text;
                String strngCatatan = txtCatatan.Text;
                String strngAkunAsal = cmbAkunAsal.EditValue.ToString();
                String strngAkunTujuan = cmbAkunTujuan.EditValue.ToString();
                String strngNominal = txtNominal.EditValue.ToString();

                DataTransferKas dTransferKas = new DataTransferKas(command, strngKode);
                dTransferKas.tanggal = strngTanggal;
                dTransferKas.catatan = strngCatatan;
                dTransferKas.akunawal = strngAkunAsal;
                dTransferKas.akunakhir = strngAkunTujuan;
                dTransferKas.nominal = strngNominal;

                if(this.isAdd) {
                    dTransferKas.tambah();
                } else {
                    dTransferKas.ubah();
                }

                // tulis log
                OswLog.setTransaksi(command, dokumen, dTransferKas.ToString());

                // reload grid di form header
                FrmTransferKas frmTransferKas = (FrmTransferKas)this.Owner;
                frmTransferKas.setGrid(command);

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