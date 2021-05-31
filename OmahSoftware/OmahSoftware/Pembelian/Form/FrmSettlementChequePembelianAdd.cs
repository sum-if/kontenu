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
using OmahSoftware.Umum.Laporan;
using OmahSoftware.Umum;
using OmahSoftware.Akuntansi;
using OmahSoftware.Pembelian.Laporan;
using DevExpress.XtraEditors.Controls;
using OmahSoftware.Penjualan;

namespace OmahSoftware.Pembelian {
    public partial class FrmSettlementChequePembelianAdd : DevExpress.XtraEditors.XtraForm {
        private String id = "SETTLEMENTCHEQUEPEMBELIAN";
        private String dokumen = "SETTLEMENTCHEQUEPEMBELIAN";
        private Boolean isAdd;

        public FrmSettlementChequePembelianAdd(bool pIsAdd) {
            isAdd = pIsAdd;
            InitializeComponent();
        }

        private void FrmSettlementChequePembelianAdd_Load(object sender, EventArgs e) {
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
                OswControlDefaultProperties.setTanggal(deTanggal);
                OswControlDefaultProperties.setTanggal(deTanggalCheque);

                cmbDepositKe = ComboQueryUmum.getAkun(cmbDepositKe, command, "akun_kas_bank");
                cmbAkunCheque = ComboQueryUmum.getAkun(cmbAkunCheque, command, "akun_cheque");
                cmbAkunCheque.Enabled = false;
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
                btnCariPesanan.Enabled = false;
                rdoTerima.Enabled = false;
                rdoTolak.Enabled = false;

                // data
                DataSettlementChequePembelian dSettlementChequePembelian = new DataSettlementChequePembelian(command, strngKode);
                DataPembayaranPembelian dPembayaranPembelian = new DataPembayaranPembelian(command, dSettlementChequePembelian.pembayaranpembelian);
                deTanggal.DateTime = OswDate.getDateTimeFromStringTanggal(dSettlementChequePembelian.tanggal);
                txtNoPembayaran.Text = dPembayaranPembelian.kode;
                txtNoCheque.Text = dPembayaranPembelian.nocek;
                cmbDepositKe.EditValue = dSettlementChequePembelian.akundeposit;
                cmbAkunCheque.EditValue = dSettlementChequePembelian.akuncheque;
                txtTotal.Text = dPembayaranPembelian.total;
                txtCatatan.Text = dSettlementChequePembelian.catatan;
                deTanggalCheque.DateTime = OswDate.getDateTimeFromStringTanggal(dPembayaranPembelian.tanggalcek);
                txtNoUrut.EditValue = dSettlementChequePembelian.nourut;

                if(dSettlementChequePembelian.jenis == Constants.JENIS_SETTLEMENT_CHEQUE_TERIMA) {
                    rdoTerima.Checked = true;
                } else {
                    rdoTolak.Checked = true;
                }

                DataSupplier dSupplier = new DataSupplier(command, dPembayaranPembelian.supplier);
                DataKota dKota = new DataKota(command, dSupplier.kota);
                txtSupplier.Text = dPembayaranPembelian.supplier;
                txtNama.EditValue = dSupplier.nama;
                txtAlamat.Text = dSupplier.alamat;
                txtKota.Text = dKota.nama;
                txtTelepon.Text = dSupplier.telp;
                txtEmail.Text = dSupplier.email;

                gantiJenis(dSettlementChequePembelian.jenis);
            } else {
                OswControlDefaultProperties.resetAllInput(this);
            }

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
                dxValidationProvider1.SetValidationRule(txtNoPembayaran, OswValidation.IsNotBlank());
                dxValidationProvider1.SetValidationRule(txtNoUrut, OswValidation.IsNotBlank());

                if(!dxValidationProvider1.Validate()) {
                    foreach(Control x in dxValidationProvider1.GetInvalidControls()) {
                        dxValidationProvider1.SetIconAlignment(x, ErrorIconAlignment.MiddleRight);
                    }
                    return;
                }

                DataPembayaranPembelian dPembayaranPembelian = new DataPembayaranPembelian(command, txtNoPembayaran.Text);
                String strngKode = txtKode.Text;
                String strngTanggal = deTanggal.Text;
                String strngSupplier = txtSupplier.Text;
                String strngPembayaranPembelian = txtNoPembayaran.Text;
                String strngCatatan = txtCatatan.Text;
                String strngAkunDeposit = cmbDepositKe.EditValue.ToString();
                String strngAkunCheque = cmbAkunCheque.EditValue.ToString();
                String strngJenis = Constants.JENIS_SETTLEMENT_CHEQUE_TERIMA;
                String strngNoUrut = txtNoUrut.EditValue.ToString();

                if(rdoTolak.Checked == true) {
                    strngJenis = Constants.JENIS_SETTLEMENT_CHEQUE_TOLAK;
                    //update status
                    dPembayaranPembelian.statuscek = Constants.STATUS_CEK_DIBATALKAN;
                    dPembayaranPembelian.ubahDataCek();
                } else {
                    // update no dan tanggal cek
                    String strngNoCek = txtNoCheque.Text;
                    String strngTanggalCek = deTanggalCheque.Text;

                    dPembayaranPembelian.nocek = strngNoCek;
                    dPembayaranPembelian.tanggalcek = strngTanggalCek;
                    dPembayaranPembelian.statuscek = Constants.STATUS_CEK_SELESAI;
                    dPembayaranPembelian.ubahDataCek();
                }


                DataSettlementChequePembelian dSettlementChequePembelian = new DataSettlementChequePembelian(command, strngKode);
                dSettlementChequePembelian.tanggal = strngTanggal;
                dSettlementChequePembelian.supplier = strngSupplier;
                dSettlementChequePembelian.pembayaranpembelian = strngPembayaranPembelian;
                dSettlementChequePembelian.jenis = strngJenis;
                dSettlementChequePembelian.akundeposit = strngAkunDeposit;
                dSettlementChequePembelian.akuncheque = strngAkunCheque;
                dSettlementChequePembelian.catatan = strngCatatan;
                dSettlementChequePembelian.nourut = strngNoUrut;

                if(this.isAdd) {
                    dSettlementChequePembelian.tambah();
                } else {
                    dSettlementChequePembelian.ubah();
                }

                // tulis log
                OswLog.setTransaksi(command, dokumen, dSettlementChequePembelian.ToString());

                // reload grid di form header
                FrmSettlementChequePembelian frmSettlementChequePembelian = (FrmSettlementChequePembelian)this.Owner;
                frmSettlementChequePembelian.setGrid(command);

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

        private void btnCariPesanan_Click(object sender, EventArgs e) {
            InfoPembayaran();
        }

        private void InfoPembayaran() {
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
                String query = @"SELECT A.tanggalcek AS 'Tgl. Cheque', A.nocek AS 'No. Cheque', A.total AS 'Total', A.kode AS 'No. Pembayaran', A.supplier AS 'Kode Supplier', A.akunpembayaran AS 'Akun Pembayaran'
                                FROM pembayaranpembelian A
                                WHERE A.statuscek = @status
                                GROUP BY A.kode ORDER BY A.kode";

                Dictionary<String, String> parameters = new Dictionary<String, String>();
                parameters.Add("status", Constants.STATUS_CEK_MENUNGGU);

                InfUtamaDictionary form = new InfUtamaDictionary("Info Pembayaran Pembelian", query, parameters,
                                                                new String[] { "No. Pembayaran", "Kode Supplier" },
                                                                new String[] { "Kode Supplier", "Akun Pembayaran" },
                                                                new String[] { "Total" });
                this.AddOwnedForm(form);
                form.ShowDialog();
                if(!form.hasil.ContainsKey("No. Pembayaran")) {
                    return;
                }

                String strngKodePembayaranPembelian = form.hasil["No. Pembayaran"];
                txtNoPembayaran.Text = strngKodePembayaranPembelian;

                DataPembayaranPembelian dPembayaranPembelian = new DataPembayaranPembelian(command, strngKodePembayaranPembelian);
                txtNoPembayaran.Text = dPembayaranPembelian.kode;
                deTanggalCheque.DateTime = OswDate.getDateTimeFromStringTanggal(dPembayaranPembelian.tanggalcek);

                txtNoCheque.Text = dPembayaranPembelian.nocek;
                cmbAkunCheque.EditValue = dPembayaranPembelian.akunpembayaran;
                txtTotal.Text = dPembayaranPembelian.total;
                txtCatatan.Text = dPembayaranPembelian.catatan;

                DataSupplier dSupplier = new DataSupplier(command, form.hasil["Kode Supplier"]);
                DataKota dKota = new DataKota(command, dSupplier.kota);
                txtSupplier.EditValue = form.hasil["Kode Supplier"];
                txtNama.EditValue = dSupplier.nama;
                txtAlamat.Text = dSupplier.alamat;
                txtKota.Text = dKota.nama;
                txtTelepon.Text = dSupplier.telp;
                txtEmail.Text = dSupplier.email;
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

        private void rdoTerima_CheckedChanged(object sender, EventArgs e) {
            gantiJenis(Constants.JENIS_SETTLEMENT_CHEQUE_TERIMA);
        }

        private void rdoTolak_CheckedChanged(object sender, EventArgs e) {
            gantiJenis(Constants.JENIS_SETTLEMENT_CHEQUE_TOLAK);
        }

        private void gantiJenis(String jenis) {
            txtNoCheque.Enabled = false;
            txtCatatan.Enabled = false;
            deTanggalCheque.Enabled = false;

            if(jenis == Constants.JENIS_SETTLEMENT_CHEQUE_TERIMA) {
                cmbDepositKe.Enabled = true;

                if(this.isAdd) {
                    txtNoCheque.Enabled = true;
                    txtCatatan.Enabled = true;
                    deTanggalCheque.Enabled = true;
                }
            } else if(jenis == Constants.JENIS_SETTLEMENT_CHEQUE_TOLAK) {
                cmbDepositKe.Enabled = false;
            }
        }

    }
}