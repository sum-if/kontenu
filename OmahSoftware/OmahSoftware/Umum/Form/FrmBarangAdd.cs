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
using DevExpress.XtraEditors.Controls;

namespace OmahSoftware.Umum {
    public partial class FrmBarangAdd : DevExpress.XtraEditors.XtraForm {
        private String id = "BARANG";
        private String dokumen = "BARANG";
        private Boolean isAdd;

        public FrmBarangAdd(bool pIsAdd) {
            isAdd = pIsAdd;
            InitializeComponent();
        }

        private void FrmBarangAdd_Load(object sender, EventArgs e) {
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
                OswControlDefaultProperties.setAngka(txtHargaJual1);
                OswControlDefaultProperties.setAngka(txtHargaBeli);
                OswControlDefaultProperties.setAngka(txtHargaJual2);
                OswControlDefaultProperties.setAngka(txtStokMin);

                cmbKategori = ComboQueryUmum.getBarangKategori(cmbKategori, command);
                cmbSatuan = ComboQueryUmum.getBarangSatuan(cmbSatuan, command);
                cmbRak = ComboQueryUmum.getBarangRak(cmbRak, command);

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
                DataBarang dBarang = new DataBarang(command, strngKode);
                txtNama.Text = dBarang.nama;
                txtNoPart.Text = dBarang.nopart;
                txtHargaBeli.EditValue = dBarang.hargabeliterakhir;
                txtHargaJual1.EditValue = dBarang.hargajual1;
                txtHargaJual2.EditValue = dBarang.hargajual2;
                txtCatatan.EditValue = dBarang.keterangan;
                chkStatus.Checked = (dBarang.status == Constants.STATUS_AKTIF);
                cmbSatuan.EditValue = dBarang.standarsatuan;
                cmbRak.EditValue = dBarang.rak;
                txtStokMin.EditValue = dBarang.stokminimum;

                // info tambahan
                cmbKategori.EditValue = dBarang.barangkategori;
                txtStokMin.EditValue = dBarang.stokminimum;
            } else {
                OswControlDefaultProperties.resetAllInput(this);
                cmbKategori.ItemIndex = 0;
                cmbSatuan.ItemIndex = 0;
                cmbRak.ItemIndex = 0;
                chkStatus.Checked = true;
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
                dxValidationProvider1.SetValidationRule(txtHargaJual1, OswValidation.IsNotBlank());
                dxValidationProvider1.SetValidationRule(txtHargaJual2, OswValidation.IsNotBlank());
                dxValidationProvider1.SetValidationRule(cmbSatuan, OswValidation.IsNotBlank());
                dxValidationProvider1.SetValidationRule(cmbRak, OswValidation.IsNotBlank());
                dxValidationProvider1.SetValidationRule(txtHargaBeli, OswValidation.IsNotBlank());
                dxValidationProvider1.SetValidationRule(cmbKategori, OswValidation.IsNotBlank());
                dxValidationProvider1.SetValidationRule(txtStokMin, OswValidation.IsNotBlank());

                if(!dxValidationProvider1.Validate()) {
                    foreach(Control x in dxValidationProvider1.GetInvalidControls()) {
                        dxValidationProvider1.SetIconAlignment(x, ErrorIconAlignment.MiddleRight);
                    }
                    return;
                }

                // simpan header
                String strngKode = txtKode.Text;
                String strngNama = txtNama.Text;
                String strngNoPart = txtNoPart.Text;
                String strngStandarSat = cmbSatuan.EditValue.ToString();
                String strngHargaJual1 = txtHargaJual1.EditValue.ToString();
                String strngHargaJual2 = txtHargaJual2.EditValue.ToString();
                String strngHargaBeli = txtHargaBeli.EditValue.ToString();
                String strngKeterangan = txtCatatan.Text;
                String strngRak = cmbRak.EditValue.ToString();
                String strngStatus = chkStatus.Checked ? Constants.STATUS_AKTIF : Constants.STATUS_AKTIF_TIDAK;
                String strngBarangKategori = cmbKategori.EditValue.ToString();
                String strngStokMin = txtStokMin.EditValue.ToString();

                DataBarang dBarang = new DataBarang(command, strngKode);
                dBarang.nama = strngNama;
                dBarang.nopart = strngNoPart;
                dBarang.standarsatuan = strngStandarSat;
                dBarang.hargajual1 = strngHargaJual1;
                dBarang.hargajual2 = strngHargaJual2;
                dBarang.hargabeliterakhir = strngHargaBeli;
                dBarang.keterangan = strngKeterangan;
                dBarang.rak = strngRak;
                dBarang.barangkategori = strngBarangKategori;
                dBarang.stokminimum = strngStokMin;
                dBarang.status = strngStatus;

                if(this.isAdd) {
                    dBarang.tambah();
                    // update kode header --> setelah generate
                    strngKode = dBarang.kode;
                } else {
                    dBarang.ubah();
                }

                // tulis log
                OswLog.setTransaksi(command, dokumen, dBarang.ToString());

                // reload grid di form header
                FrmBarang frmBarang = (FrmBarang)this.Owner;
                frmBarang.setGrid(command);

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