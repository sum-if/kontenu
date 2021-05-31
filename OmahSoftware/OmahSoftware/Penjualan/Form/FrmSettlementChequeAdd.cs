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
using OmahSoftware.Penjualan.Laporan;
using DevExpress.XtraEditors.Controls;

namespace OmahSoftware.Penjualan {
    public partial class FrmSettlementChequeAdd : DevExpress.XtraEditors.XtraForm {
        private String id = "SETTLEMENTCHEQUE";
        private String dokumen = "SETTLEMENTCHEQUE";
        private Boolean isAdd;

        public FrmSettlementChequeAdd(bool pIsAdd) {
            isAdd = pIsAdd;
            InitializeComponent();
        }

        private void FrmSettlementChequeAdd_Load(object sender, EventArgs e) {
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
                DataSettlementCheque dSettlementCheque = new DataSettlementCheque(command, strngKode);
                DataPenerimaanPenjualan dPenerimaanPenjualan = new DataPenerimaanPenjualan(command, dSettlementCheque.penerimaanpenjualan);
                deTanggal.DateTime = OswDate.getDateTimeFromStringTanggal(dSettlementCheque.tanggal);
                txtNoPenerimaan.Text = dPenerimaanPenjualan.kode;
                txtNoCheque.Text = dPenerimaanPenjualan.nocek;
                cmbDepositKe.EditValue = dSettlementCheque.akundeposit;
                cmbAkunCheque.EditValue = dSettlementCheque.akuncheque;
                txtTotal.Text = dPenerimaanPenjualan.total;
                txtCatatan.Text = dSettlementCheque.catatan;
                deTanggalCheque.DateTime = OswDate.getDateTimeFromStringTanggal(dPenerimaanPenjualan.tanggalcek);
                txtNoUrut.EditValue = dSettlementCheque.nourut;

                if(dSettlementCheque.jenis == Constants.JENIS_SETTLEMENT_CHEQUE_TERIMA) {
                    rdoTerima.Checked = true;
                } else {
                    rdoTolak.Checked = true;
                }

                DataCustomer dCustomer = new DataCustomer(command, dPenerimaanPenjualan.customer);
                DataKota dKota = new DataKota(command, dCustomer.kota);
                txtCustomer.Text = dPenerimaanPenjualan.customer;
                txtNama.EditValue = dCustomer.nama;
                txtAlamat.Text = dCustomer.alamat;
                txtKota.Text = dKota.nama;
                txtTelepon.Text = dCustomer.telp;
                txtEmail.Text = dCustomer.email;

                gantiJenis(dSettlementCheque.jenis);
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
                dxValidationProvider1.SetValidationRule(txtNoPenerimaan, OswValidation.IsNotBlank());
                dxValidationProvider1.SetValidationRule(txtNoUrut, OswValidation.IsNotBlank());

                if(!dxValidationProvider1.Validate()) {
                    foreach(Control x in dxValidationProvider1.GetInvalidControls()) {
                        dxValidationProvider1.SetIconAlignment(x, ErrorIconAlignment.MiddleRight);
                    }
                    return;
                }

                DataPenerimaanPenjualan dPenerimaanPenjualan = new DataPenerimaanPenjualan(command, txtNoPenerimaan.Text);
                String strngKode = txtKode.Text;
                String strngTanggal = deTanggal.Text;
                String strngCustomer = txtCustomer.Text;
                String strngPenerimaanPenjualan = txtNoPenerimaan.Text;
                String strngCatatan = txtCatatan.Text;
                String strngAkunDeposit = cmbDepositKe.EditValue.ToString();
                String strngAkunCheque = cmbAkunCheque.EditValue.ToString();
                String strngJenis = Constants.JENIS_SETTLEMENT_CHEQUE_TERIMA;
                String strngNoUrut = txtNoUrut.EditValue.ToString();

                if(rdoTolak.Checked == true) {
                    strngJenis = Constants.JENIS_SETTLEMENT_CHEQUE_TOLAK;
                    //update status
                    dPenerimaanPenjualan.statuscek = Constants.STATUS_CEK_DIBATALKAN;
                    dPenerimaanPenjualan.ubahDataCek();
                } else {
                    // update no dan tanggal cek
                    String strngNoCek = txtNoCheque.Text;
                    String strngTanggalCek = deTanggalCheque.Text;

                    dPenerimaanPenjualan.nocek = strngNoCek;
                    dPenerimaanPenjualan.tanggalcek = strngTanggalCek;
                    dPenerimaanPenjualan.statuscek = Constants.STATUS_CEK_SELESAI;
                    dPenerimaanPenjualan.ubahDataCek();
                }


                DataSettlementCheque dSettlementCheque = new DataSettlementCheque(command, strngKode);
                dSettlementCheque.tanggal = strngTanggal;
                dSettlementCheque.customer = strngCustomer;
                dSettlementCheque.penerimaanpenjualan = strngPenerimaanPenjualan;
                dSettlementCheque.jenis = strngJenis;
                dSettlementCheque.akundeposit = strngAkunDeposit;
                dSettlementCheque.akuncheque = strngAkunCheque;
                dSettlementCheque.catatan = strngCatatan;
                dSettlementCheque.nourut = strngNoUrut;

                if(this.isAdd) {
                    dSettlementCheque.tambah();
                } else {
                    dSettlementCheque.ubah();
                }

                // tulis log
                OswLog.setTransaksi(command, dokumen, dSettlementCheque.ToString());

                // reload grid di form header
                FrmSettlementCheque frmSettlementCheque = (FrmSettlementCheque)this.Owner;
                frmSettlementCheque.setGrid(command);

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
            InfoPenerimaan();
        }

        private void InfoPenerimaan() {
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
                String query = @"SELECT A.tanggalcek AS 'Tgl. Cheque', A.nocek AS 'No. Cheque', A.total AS 'Total', A.kode AS 'No. Penerimaan', A.customer AS 'Kode Customer', A.akunpenerimaan AS 'Akun Penerimaan'
                                FROM penerimaanpenjualan A
                                WHERE A.statuscek = @status
                                GROUP BY A.kode ORDER BY A.kode";

                Dictionary<String, String> parameters = new Dictionary<String, String>();
                parameters.Add("status", Constants.STATUS_CEK_MENUNGGU);

                InfUtamaDictionary form = new InfUtamaDictionary("Info Penerimaan Penjualan", query, parameters,
                                                                new String[] { "No. Penerimaan", "Kode Customer" },
                                                                new String[] { "Kode Customer", "Akun Penerimaan" },
                                                                new String[] { "Total" });
                this.AddOwnedForm(form);
                form.ShowDialog();
                if(!form.hasil.ContainsKey("No. Penerimaan")) {
                    return;
                }

                String strngKodePenerimaanPenjualan = form.hasil["No. Penerimaan"];
                txtNoPenerimaan.Text = strngKodePenerimaanPenjualan;

                DataPenerimaanPenjualan dPenerimaanPenjualan = new DataPenerimaanPenjualan(command, strngKodePenerimaanPenjualan);
                txtNoPenerimaan.Text = dPenerimaanPenjualan.kode;
                deTanggalCheque.DateTime = OswDate.getDateTimeFromStringTanggal(dPenerimaanPenjualan.tanggalcek);

                txtNoCheque.Text = dPenerimaanPenjualan.nocek;
                cmbAkunCheque.EditValue = dPenerimaanPenjualan.akunpenerimaan;
                txtTotal.Text = dPenerimaanPenjualan.total;
                txtCatatan.Text = dPenerimaanPenjualan.catatan;

                DataCustomer dCustomer = new DataCustomer(command, form.hasil["Kode Customer"]);
                DataKota dKota = new DataKota(command, dCustomer.kota);
                txtCustomer.EditValue = form.hasil["Kode Customer"];
                txtNama.EditValue = dCustomer.nama;
                txtAlamat.Text = dCustomer.alamat;
                txtKota.Text = dKota.nama;
                txtTelepon.Text = dCustomer.telp;
                txtEmail.Text = dCustomer.email;
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