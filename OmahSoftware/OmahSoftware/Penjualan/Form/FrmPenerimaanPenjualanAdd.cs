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
    public partial class FrmPenerimaanPenjualanAdd : DevExpress.XtraEditors.XtraForm {
        private String id = "PENERIMAANPENJUALAN";
        private String dokumen = "PENERIMAANPENJUALAN";
        private String dokumenDetail = "PENERIMAANPENJUALAN";
        private Boolean isAdd;
        private String strngAkunPostDatedCheque = "";

        public FrmPenerimaanPenjualanAdd(bool pIsAdd) {
            isAdd = pIsAdd;
            InitializeComponent();
        }

        private void FrmPenerimaanPenjualanAdd_Load(object sender, EventArgs e) {
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
                OswControlDefaultProperties.setTanggal(deTanggalCek);
                OswControlDefaultProperties.setAngka(txtUangTitipan);

                DataOswSetting dOswSetting = new DataOswSetting(command, Constants.AKUN_POST_DATED_CHEQUE);
                this.strngAkunPostDatedCheque = dOswSetting.isi;

                cmbAkunPenerimaan = ComboQueryUmum.getAkun(cmbAkunPenerimaan, command, "akun_penerimaan_penjualan");

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
                btnCari.Enabled = false;
                rdoCek.Enabled = false;
                rdoTunai.Enabled = false;

                // data
                DataPenerimaanPenjualan dPenerimaanPenjualan = new DataPenerimaanPenjualan(command, strngKode);
                deTanggal.DateTime = OswDate.getDateTimeFromStringTanggal(dPenerimaanPenjualan.tanggal);
                
                txtCustomer.Text = dPenerimaanPenjualan.customer;
                txtCatatan.Text = dPenerimaanPenjualan.catatan;
                txtNoUrut.EditValue = dPenerimaanPenjualan.nourut;

                if(dPenerimaanPenjualan.jenis == Constants.JENIS_PENERIMAAN_PENJUALAN_TUNAI) {
                    rdoTunai.Checked = true;
                } else {
                    txtNoCek.Text = dPenerimaanPenjualan.nocek;
                    deTanggalCek.DateTime = OswDate.getDateTimeFromStringTanggal(dPenerimaanPenjualan.tanggalcek);
                    rdoCek.Checked = true;
                }

                DataCustomer dCustomer = new DataCustomer(command, dPenerimaanPenjualan.customer);
                DataKota dKota = new DataKota(command, dCustomer.kota);
                DataSaldoUangTitipanCustomerAktual dSaldoUangTitipanCustomerAktual = new DataSaldoUangTitipanCustomerAktual(command, dPenerimaanPenjualan.customer);

                txtNama.EditValue = dCustomer.nama;
                txtKota.Text = dKota.nama;
                txtAlamat.Text = dCustomer.alamat;
                txtTelepon.Text = dCustomer.telp;
                txtEmail.Text = dCustomer.email;
                txtUangTitipan.EditValue = dSaldoUangTitipanCustomerAktual.jumlah;

                chkLunas.Checked = dPenerimaanPenjualan.lunas == Constants.STATUS_YA;
                lblSelisih.Text = OswConvert.convertToRupiah(double.Parse(dPenerimaanPenjualan.totalselisih));

                cmbAkunPenerimaan.EditValue = dPenerimaanPenjualan.akunpenerimaan;

                setGrid(command);
            } else {
                DateTime dt1 = deTanggal.DateTime;
                OswControlDefaultProperties.resetAllInput(this);
                rdoTunai.Checked = true;
                cmbAkunPenerimaan.EditValue = OswCombo.getFirstEditValue(cmbAkunPenerimaan);
                chkLunas.Checked = false;
                lblSelisih.Text = OswConvert.convertToRupiah(0);
                rdoTunai.Checked = true;

                setGrid(command);
            }
            txtKode.Focus();
        }

        private void infoCustomer() {
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
                String query = @"SELECT A.kode AS 'Kode Customer',A.nama AS Customer, A.alamat AS Alamat, A.telp AS Telp, B.nama AS Kota, COALESCE(C.jumlah,0) AS 'Uang Titipan'
                                FROM customer A
                                INNER JOIN kota B ON A.kota = B.kode
                                LEFT JOIN saldouangtitipancustomeraktual C ON A.kode = C.customer
                                WHERE A.status = @status
                                ORDER BY A.kode";

                Dictionary<String, String> parameters = new Dictionary<String, String>();
                parameters.Add("status", Constants.STATUS_AKTIF);

                InfUtamaDictionary form = new InfUtamaDictionary("Info Customer", query, parameters,
                                                                new String[] { "Kode Customer", "Kota", "Uang Titipan" },
                                                                new String[] { "Kode Customer" },
                                                                new String[] { "Uang Titipan" });
                this.AddOwnedForm(form);
                form.ShowDialog();
                if(!form.hasil.ContainsKey("Kode Customer")) {
                    return;
                }

                String strngKodeCustomer = form.hasil["Kode Customer"];
                String strngKota = form.hasil["Kota"];
                String strngUangTitipan = form.hasil["Uang Titipan"];

                txtCustomer.Text = strngKodeCustomer;
                txtUangTitipan.EditValue = strngUangTitipan;

                DataCustomer dCustomer = new DataCustomer(command, strngKodeCustomer);

                txtNama.EditValue = dCustomer.nama;
                txtTelepon.Text = dCustomer.telp;
                txtEmail.Text = dCustomer.email;
                txtAlamat.Text = dCustomer.alamat;
                txtKota.Text = strngKota;

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
                dxValidationProvider1.SetValidationRule(txtCustomer, OswValidation.IsNotBlank());
                dxValidationProvider1.SetValidationRule(cmbAkunPenerimaan, OswValidation.IsNotBlank());
                dxValidationProvider1.SetValidationRule(txtNoUrut, OswValidation.IsNotBlank());

                if(!dxValidationProvider1.Validate()) {
                    foreach(Control x in dxValidationProvider1.GetInvalidControls()) {
                        dxValidationProvider1.SetIconAlignment(x, ErrorIconAlignment.MiddleRight);
                    }
                    return;
                }

                String strngKode = txtKode.Text;
                String strngJenis = rdoTunai.Checked ? Constants.JENIS_PENERIMAAN_PENJUALAN_TUNAI : Constants.JENIS_PENERIMAAN_PENJUALAN_CEK;
                String strngTanggal = deTanggal.Text;
                String strngCustomer = txtCustomer.Text;
                String strngAkunPenerimaan = cmbAkunPenerimaan.EditValue.ToString();
                String strngNoCek = txtNoCek.Text;
                String strngTglCek = deTanggalCek.Text;
                String strngStatusCek = rdoTunai.Checked ? Constants.STATUS_CEK_TIDAK_ADA : Constants.STATUS_CEK_MENUNGGU;
                String strngCatatan = txtCatatan.Text;
                String strngNoUrut = txtNoUrut.EditValue.ToString();
                String strngLunas = chkLunas.Checked ? Constants.STATUS_YA : Constants.STATUS_TIDAK;

                // jika cek, harus isi untuk tanggal dan no cek nya
                if(rdoCek.Checked) {
                    if(strngNoCek == "" || strngTglCek == "") {
                        throw new Exception("No dan Tanggal Cek harus diisi");
                    }
                }

                DataPenerimaanPenjualan dPenerimaanPenjualan = new DataPenerimaanPenjualan(command, strngKode);
                dPenerimaanPenjualan.tanggal = strngTanggal;
                dPenerimaanPenjualan.jenis = strngJenis;
                dPenerimaanPenjualan.customer = strngCustomer;
                dPenerimaanPenjualan.akunpenerimaan = strngAkunPenerimaan;
                dPenerimaanPenjualan.nocek = strngNoCek;
                dPenerimaanPenjualan.tanggalcek = strngTglCek;
                dPenerimaanPenjualan.statuscek = strngStatusCek;
                dPenerimaanPenjualan.catatan = strngCatatan;
                dPenerimaanPenjualan.nourut = strngNoUrut;
                dPenerimaanPenjualan.lunas = strngLunas;

                if(this.isAdd) {
                    dPenerimaanPenjualan.tambah();
                    // update kode header --> setelah generate
                    strngKode = dPenerimaanPenjualan.kode;
                } else {
                    dPenerimaanPenjualan.valStatusCek();
                    dPenerimaanPenjualan.hapusDetail();
                    dPenerimaanPenjualan.ubah();
                }

                // simpan detail
                GridView gridView = gridView1;

                double dblTotal = 0;
                double dblTotalSelisih = 0;

                for(int i = 0; i < gridView.DataRowCount; i++) {
                    if(gridView.GetRowCellValue(i, "Faktur Penjualan").ToString() == "") {
                        continue;
                    }

                    String strngNo = gridView.GetRowCellValue(i, "No").ToString();
                    String strngFakturPenjualan = gridView.GetRowCellValue(i, "Faktur Penjualan").ToString();
                    double dblTotalFaktur = Tools.getRoundMoney(double.Parse(gridView.GetRowCellValue(i, "Total Faktur").ToString()));
                    double dblTotalRetur = Tools.getRoundMoney(double.Parse(gridView.GetRowCellValue(i, "Total Retur").ToString()));
                    double dblTotalBayar = Tools.getRoundMoney(double.Parse(gridView.GetRowCellValue(i, "Total Bayar").ToString()));
                    double dblJumlah = Tools.getRoundMoney(double.Parse(gridView.GetRowCellValue(i, "Jumlah").ToString()));
                    double dblSelisih = 0;

                    if(strngLunas == Constants.STATUS_YA) {
                        dblSelisih = Tools.getRoundMoney(dblTotalFaktur - (dblJumlah + dblTotalRetur + dblTotalBayar));
                    }

                    dblTotal = Tools.getRoundMoney(dblTotal + dblJumlah);
                    dblTotalSelisih = Tools.getRoundMoney(dblTotalSelisih + dblSelisih);

                    // simpan detail
                    DataPenerimaanPenjualanDetail dPenerimaanPenjualanDetail = new DataPenerimaanPenjualanDetail(command, strngKode, strngNo);
                    dPenerimaanPenjualanDetail.fakturpenjualan = strngFakturPenjualan;
                    dPenerimaanPenjualanDetail.totalfaktur = dblTotalFaktur.ToString();
                    dPenerimaanPenjualanDetail.totalbayar = dblTotalBayar.ToString();
                    dPenerimaanPenjualanDetail.totalretur = dblTotalRetur.ToString();
                    dPenerimaanPenjualanDetail.jumlah = dblJumlah.ToString();
                    dPenerimaanPenjualanDetail.selisih = dblSelisih.ToString();
                    dPenerimaanPenjualanDetail.tambah();

                    // tulis log detail
                    OswLog.setTransaksi(command, dokumenDetail, dPenerimaanPenjualanDetail.ToString());
                }

                dPenerimaanPenjualan = new DataPenerimaanPenjualan(command, strngKode);
                dPenerimaanPenjualan.total = dblTotal.ToString();
                dPenerimaanPenjualan.totalselisih = dblTotalSelisih.ToString();
                dPenerimaanPenjualan.ubah();

                // validasi setelah simpan
                dPenerimaanPenjualan.valJumlahDetail();
                dPenerimaanPenjualan.prosesJurnal();
                dPenerimaanPenjualan.updateMutasi();

                // tulis log
                OswLog.setTransaksi(command, dokumen, dPenerimaanPenjualan.ToString());

                // reload grid di form header
                FrmPenerimaanPenjualan frmPenerimaanPenjualan = (FrmPenerimaanPenjualan)this.Owner;
                frmPenerimaanPenjualan.setGrid(command);

                // Commit Transaction
                command.Transaction.Commit();

                OswPesan.pesanInfo("Proses " + dokumen + " berhasil.");

                if(this.isAdd) {
                    if(OswPesan.pesanKonfirmasi("Konfirmasi", "Cetak Bukti Penerimaan Penjualan?") == DialogResult.Yes) {
                        cetak(strngKode);
                    }

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

        private void btnCari_Click(object sender, EventArgs e) {
            infoCustomer();
        }

        private void radioButtons_CheckedChanged(object sender, EventArgs e) {
            if(rdoCek.Checked) {
                txtNoCek.Enabled = true;
                deTanggalCek.Enabled = true;
                cmbAkunPenerimaan.EditValue = strngAkunPostDatedCheque;
                cmbAkunPenerimaan.Enabled = false;
            } else {
                txtNoCek.Enabled = false;
                deTanggalCek.Enabled = false;
                txtNoCek.Text = "";
                deTanggalCek.EditValue = "";
                cmbAkunPenerimaan.EditValue = OswCombo.getFirstEditValue(cmbAkunPenerimaan);
                cmbAkunPenerimaan.Enabled = true;
            }
        }

        public void setGrid(MySqlCommand command) {
            String strngKode = txtKode.Text;

            String query = @"SELECT B.no AS 'No', B.fakturpenjualan AS 'Faktur Penjualan', B.totalfaktur AS 'Total Faktur', B.totalbayar AS 'Total Bayar', B.totalretur AS 'Total Retur', B.jumlah AS Jumlah
                                FROM penerimaanpenjualan A
                                INNER JOIN penerimaanpenjualandetail B ON A.kode = B.penerimaanpenjualan
                                WHERE A.kode = @kode
                                ORDER BY B.no";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("kode", strngKode);

            Dictionary<String, int> widths = new Dictionary<String, int>();
            // 917 - 21 (kiri) - 17 (vertikal lines) - 35 (No) = 927
            widths.Add("No", 35);
            widths.Add("Faktur Penjualan", 364);
            widths.Add("Total Faktur", 130);
            widths.Add("Total Bayar", 130);
            widths.Add("Total Retur", 130);
            widths.Add("Jumlah", 130);

            Dictionary<String, String> inputType = new Dictionary<string, string>();
            inputType.Add("Jumlah", OswInputType.NUMBER);
            inputType.Add("Total Faktur", OswInputType.NUMBER);
            inputType.Add("Total Bayar", OswInputType.NUMBER);
            inputType.Add("Total Retur", OswInputType.NUMBER);

            OswGrid.getGridInput(gridControl1, command, query, parameters, widths, inputType,
                                 new String[] { },
                                 new String[] { "No", "Total Faktur", "Total Bayar", "Total Retur" });

            // search
            RepositoryItemButtonEdit search = new RepositoryItemButtonEdit();
            search.Buttons[0].Kind = ButtonPredefines.Glyph;
            search.Buttons[0].Image = OswResources.getImage("cari-16.png", typeof(Program).Assembly);
            search.Buttons[0].Visible = true;
            search.ButtonClick += search_ButtonClick;

            GridView gridView = gridView1;
            gridView.OptionsView.ShowFooter = true;

            gridView.Columns["Faktur Penjualan"].ColumnEdit = search;
            gridView.Columns["Faktur Penjualan"].ColumnEdit.ReadOnly = true;

            gridView.Columns["Total Faktur"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            gridView.Columns["Total Faktur"].SummaryItem.DisplayFormat = "{0:N2}";
            gridView.Columns["Total Bayar"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            gridView.Columns["Total Bayar"].SummaryItem.DisplayFormat = "{0:N2}";
            gridView.Columns["Total Retur"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            gridView.Columns["Total Retur"].SummaryItem.DisplayFormat = "{0:N2}";
            gridView.Columns["Jumlah"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            gridView.Columns["Jumlah"].SummaryItem.DisplayFormat = "{0:N2}";
        }

        void search_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e) {
            infoFakturPenjualan();
        }

        private void infoFakturPenjualan() {
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
                GridView gridView = gridView1;

                String strngKodeCustomer = txtCustomer.Text;

                String query = @"SELECT A.tanggaljatuhtempo AS 'Jatuh Tempo', A.tanggal AS Tanggal, A.kode AS 'Faktur Penjualan', 
		                                A.grandtotal AS 'Total Faktur', A.totalretur AS 'Total Retur', A.totalbayar AS 'Total Bayar', CASE WHEN (A.grandtotal - A.totalbayar - A.totalretur) < 0 THEN 0 ELSE (A.grandtotal - A.totalbayar - A.totalretur) END AS 'Sisa Bayar'
                                FROM fakturpenjualan A
                                WHERE A.customer = @customer AND A.status = @status
                                ORDER BY toDate(A.tanggaljatuhtempo), toDate(A.tanggal), A.kode";

                Dictionary<String, String> parameters = new Dictionary<String, String>();
                parameters.Add("customer", strngKodeCustomer);
                parameters.Add("status", Constants.STATUS_FAKTUR_PENJUALAN_BELUM_LUNAS);

                InfUtamaDataTable form = new InfUtamaDataTable("Info Faktur Penjualan", query, parameters,
                                                                new String[] { },
                                                                new String[] { "Total Faktur", "Total Retur", "Total Bayar", "Sisa Bayar" },
                                                                new DataTable());
                this.AddOwnedForm(form);
                form.ShowDialog();

                if(form.hasil.Rows.Count == 0) {
                    return;
                }

                foreach(DataRow row in form.hasil.Rows) {
                    String strngFakturPenjualan = row["Faktur Penjualan"].ToString();
                    String strngTotalFaktur = row["Total Faktur"].ToString();
                    String strngTotalRetur = row["Total Retur"].ToString();
                    String strngTotalBayar = row["Total Bayar"].ToString();
                    String strngSisaBayar = row["Sisa Bayar"].ToString();

                    gridView.AddNewRow();
                    gridView.MoveLast();
                    gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Faktur Penjualan"], strngFakturPenjualan);
                    gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Total Faktur"], strngTotalFaktur);
                    gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Total Retur"], strngTotalRetur);
                    gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Total Bayar"], strngTotalBayar);
                    gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Jumlah"], strngSisaBayar);
                    gridView.UpdateCurrentRow();
                }

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

        private void gridControl1_ProcessGridKey(object sender, KeyEventArgs e) {
            GridView gridView = gridView1;
            if(e.KeyCode == Keys.F1 && gridView.FocusedColumn.FieldName == "Faktur Penjualan") {
                infoFakturPenjualan();
            }
        }

        private void gridView1_FocusedColumnChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventArgs e) {
            GridView gridView = sender as GridView;

            if(gridView.GetRowCellValue(gridView.FocusedRowHandle, "Faktur Penjualan") == null) {
                gridView.FocusedColumn = gridView.Columns["Faktur Penjualan"];
                return;
            }

            if(gridView.FocusedColumn == null) {
                gridView.FocusedColumn = gridView.Columns["Faktur Penjualan"];
                return;
            }

            if(gridView.FocusedColumn.FieldName != "Faktur Penjualan" && gridView.GetRowCellValue(gridView.FocusedRowHandle, "Faktur Penjualan").ToString() == "") {
                gridView.FocusedColumn = gridView.Columns["Faktur Penjualan"];
                return;
            }

            setFooter();
        }

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e) {
            GridView gridView = sender as GridView;

            if(gridView.GetRowCellValue(gridView.FocusedRowHandle, "Faktur Penjualan") == null) {
                gridView.FocusedColumn = gridView.Columns["Faktur Penjualan"];
                return;
            }

            if(gridView.FocusedColumn == null) {
                gridView.FocusedColumn = gridView.Columns["Faktur Penjualan"];
                return;
            }

            if(gridView.FocusedColumn.FieldName != "Faktur Penjualan" && gridView.GetRowCellValue(gridView.FocusedRowHandle, "Faktur Penjualan").ToString() == "") {
                gridView.FocusedColumn = gridView.Columns["Faktur Penjualan"];
                return;
            }

            setFooter();
        }

        private void gridView1_InitNewRow(object sender, InitNewRowEventArgs e) {
            GridView gridView = sender as GridView;

            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["No"], gridView.DataRowCount + 1);
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Faktur Penjualan"], "");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Total Faktur"], "0");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Total Bayar"], "0");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Total Retur"], "0");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Jumlah"], "0");
        }

        private void gridView1_RowDeleted(object sender, DevExpress.Data.RowDeletedEventArgs e) {
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
                GridView gridView = sender as GridView;
                for(int no = 1; no <= gridView.DataRowCount; no++) {
                    gridView.SetRowCellValue(no - 1, "No", no);
                }

                setFooter();

                // Commit Transaction
                command.Transaction.Commit();
            } catch(MySqlException ex) {
                OswPesan.pesanErrorCatch(ex, command, dokumen);
            } catch(Exception ex) {
                OswPesan.pesanErrorCatch(ex, command, dokumen);
            } finally {
                con.Close();
            }
        }

        private void gridView1_ValidateRow(object sender, DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs e) {
            GridView gridView = gridView1;

            if(gridView.GetRowCellValue(gridView.FocusedRowHandle, "Faktur Penjualan") == null) {
                gridView.FocusedColumn = gridView.Columns["Faktur Penjualan"];
                return;
            }

            if(gridView.GetRowCellValue(gridView.FocusedRowHandle, "Faktur Penjualan").ToString() == "") {
                gridView.FocusedColumn = gridView.Columns["Faktur Penjualan"];
                return;
            }

            setFooter();
        }

        private void setFooter() {
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
                GridView gridView = gridView1;

                String strngLunas = chkLunas.Checked ? Constants.STATUS_YA : Constants.STATUS_TIDAK;

                double dblTotalSelisih = 0;

                for(int i = 0; i < gridView.DataRowCount; i++) {
                    if(gridView.GetRowCellValue(i, "Faktur Penjualan").ToString() == "") {
                        continue;
                    }

                    String strngNo = gridView.GetRowCellValue(i, "No").ToString();
                    double dblTotalFaktur = Tools.getRoundMoney(double.Parse(gridView.GetRowCellValue(i, "Total Faktur").ToString()));
                    double dblTotalRetur = Tools.getRoundMoney(double.Parse(gridView.GetRowCellValue(i, "Total Retur").ToString()));
                    double dblTotalBayar = Tools.getRoundMoney(double.Parse(gridView.GetRowCellValue(i, "Total Bayar").ToString()));
                    double dblJumlah = Tools.getRoundMoney(double.Parse(gridView.GetRowCellValue(i, "Jumlah").ToString()));
                    double dblSelisih = 0;

                    if(strngLunas == Constants.STATUS_YA) {
                        dblSelisih = Tools.getRoundMoney(dblTotalFaktur - (dblJumlah + dblTotalRetur + dblTotalBayar));
                    }

                    dblTotalSelisih = Tools.getRoundMoney(dblTotalSelisih + dblSelisih);
                }

                lblSelisih.Text = OswConvert.convertToRupiah(dblTotalSelisih);

                // Commit Transaction
                command.Transaction.Commit();
            } catch(MySqlException ex) {
                OswPesan.pesanErrorCatch(ex, command, dokumen);
            } catch(Exception ex) {
                OswPesan.pesanErrorCatch(ex, command, dokumen);
            } finally {
                con.Close();
            }
        }

        private void chkLunas_CheckedChanged(object sender, EventArgs e) {
            setFooter();
        }

        private void btnCetak_Click(object sender, EventArgs e) {
            String strngKode = txtKode.Text;
            cetak(strngKode);
        }

        private void cetak(string kode) {
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

                RptCetakPenerimaanPenjualan report = new RptCetakPenerimaanPenjualan();

                DataOswPerusahaan dPerusahaan = new DataOswPerusahaan(command);
                report.Parameters["perusahaanNama"].Value = dPerusahaan.nama;
                report.Parameters["perusahaanAlamat"].Value = dPerusahaan.alamat;
                report.Parameters["perusahaanTelepon"].Value = dPerusahaan.telp;
                report.Parameters["perusahaanEmail"].Value = dPerusahaan.email;
                report.Parameters["perusahaanNPWP"].Value = dPerusahaan.npwp;

                DataPenerimaanPenjualan dPenerimaanPenjualan = new DataPenerimaanPenjualan(command, kode);
                DataAkun dAkun = new DataAkun(command, dPenerimaanPenjualan.akunpenerimaan);
                DataCustomer dCustomer = new DataCustomer(command, dPenerimaanPenjualan.customer);
                DataKota dKota = new DataKota(command, dCustomer.kota);

                report.Parameters["nomor"].Value = kode;
                report.Parameters["tanggal"].Value = dPenerimaanPenjualan.tanggal;
                report.Parameters["akun"].Value = dPenerimaanPenjualan.akunpenerimaan + " " + dAkun.nama;
                report.Parameters["total"].Value = dPenerimaanPenjualan.total;
                report.Parameters["terbilang"].Value = OswMath.Terbilang(Convert.ToDouble(dPenerimaanPenjualan.total));
                report.Parameters["catatan"].Value = dPenerimaanPenjualan.catatan;
                report.Parameters["nourut"].Value = dPenerimaanPenjualan.nourut;
                report.Parameters["customer"].Value = dCustomer.nama + " [" + dKota.nama + "]";

                // Assign the printing system to the document viewer.
                LaporanPrintPreview laporan = new LaporanPrintPreview();
                laporan.documentViewer1.DocumentSource = report;

                //ReportPrintTool printTool = new ReportPrintTool(report);
                //printTool.Print();

                OswLog.setLaporan(command, dokumen);

                laporan.Show();

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
    }
}