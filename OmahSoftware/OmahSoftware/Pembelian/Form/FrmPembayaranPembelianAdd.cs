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
using OmahSoftware.Umum.Laporan;
using OmahSoftware.Akuntansi;
using OmahSoftware.Pembelian.Laporan;
using DevExpress.XtraEditors.Controls;
using OmahSoftware.Penjualan;

namespace OmahSoftware.Pembelian {
    public partial class FrmPembayaranPembelianAdd : DevExpress.XtraEditors.XtraForm {
        private String id = "PEMBAYARANPEMBELIAN";
        private String dokumen = "PEMBAYARANPEMBELIAN";
        private String dokumenDetail = "PEMBAYARANPEMBELIAN";
        private Boolean isAdd;
        private String strngAkunPostDatedCheque = "";

        public FrmPembayaranPembelianAdd(bool pIsAdd) {
            isAdd = pIsAdd;
            InitializeComponent();
        }

        private void FrmPembayaranPembelianAdd_Load(object sender, EventArgs e) {
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
                OswControlDefaultProperties.setTanggal(deTanggalCek);
                OswControlDefaultProperties.setAngka(txtUangTitipan);


                DataOswSetting dOswSetting = new DataOswSetting(command, Constants.AKUN_POST_DATED_CHEQUE);
                this.strngAkunPostDatedCheque = dOswSetting.isi;

                cmbAkunPembayaran = ComboQueryUmum.getAkun(cmbAkunPembayaran, command, "akun_pembayaran_pembelian");

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
                cmbAkunPembayaran.Enabled = false;
                rdoCek.Enabled = false;
                rdoTunai.Enabled = false;

                // data
                DataPembayaranPembelian dPembayaranPembelian = new DataPembayaranPembelian(command, strngKode);
                deTanggal.DateTime = OswDate.getDateTimeFromStringTanggal(dPembayaranPembelian.tanggal);
                
                txtCatatan.Text = dPembayaranPembelian.catatan;
                txtSupplier.Text = dPembayaranPembelian.supplier;
                txtNoUrut.EditValue = dPembayaranPembelian.nourut;

                if(dPembayaranPembelian.jenis == Constants.JENIS_PEMBAYARAN_PEMBELIAN_TUNAI) {
                    rdoTunai.Checked = true;
                } else {
                    txtNoCek.Text = dPembayaranPembelian.nocek;
                    deTanggalCek.DateTime = OswDate.getDateTimeFromStringTanggal(dPembayaranPembelian.tanggalcek);
                    rdoCek.Checked = true;
                }

                DataSupplier dSupplier = new DataSupplier(command, dPembayaranPembelian.supplier);
                DataKota dKota = new DataKota(command, dSupplier.kota);
                DataSaldoUangTitipanSupplierAktual dSaldoUangTitipanSupplierAktual = new DataSaldoUangTitipanSupplierAktual(command, dPembayaranPembelian.supplier);

                txtNama.EditValue = dSupplier.nama;
                txtKota.Text = dKota.nama;
                txtAlamat.Text = dSupplier.alamat;
                txtTelepon.Text = dSupplier.telp;
                txtEmail.Text = dSupplier.email;
                txtUangTitipan.EditValue = dSaldoUangTitipanSupplierAktual.jumlah;

                chkLunas.Checked = dPembayaranPembelian.lunas == Constants.STATUS_YA;
                lblSelisih.Text = OswConvert.convertToRupiah(double.Parse(dPembayaranPembelian.totalselisih));
                cmbAkunPembayaran.EditValue = dPembayaranPembelian.akunpembayaran;
                setGrid(command);
            } else {
                OswControlDefaultProperties.resetAllInput(this);
                cmbAkunPembayaran.EditValue = OswCombo.getFirstEditValue(cmbAkunPembayaran);
                chkLunas.Checked = false;
                lblSelisih.Text = OswConvert.convertToRupiah(0);
                rdoTunai.Checked = true;

                setGrid(command);
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
                dxValidationProvider1.SetValidationRule(cmbAkunPembayaran, OswValidation.IsNotBlank());
                dxValidationProvider1.SetValidationRule(txtSupplier, OswValidation.IsNotBlank());
                dxValidationProvider1.SetValidationRule(txtNoUrut, OswValidation.IsNotBlank());

                if(!dxValidationProvider1.Validate()) {
                    foreach(Control x in dxValidationProvider1.GetInvalidControls()) {
                        dxValidationProvider1.SetIconAlignment(x, ErrorIconAlignment.MiddleRight);
                    }
                    return;
                }

                String strngKode = txtKode.Text;
                String strngTanggal = deTanggal.Text;
                String strngJenis = rdoTunai.Checked ? Constants.JENIS_PEMBAYARAN_PEMBELIAN_TUNAI : Constants.JENIS_PEMBAYARAN_PEMBELIAN_CEK;
                String strngSupplier = txtSupplier.Text;
                String strngAkunPembayaran = cmbAkunPembayaran.EditValue.ToString();
                String strngNoCek = txtNoCek.Text;
                String strngTglCek = deTanggalCek.Text;
                String strngStatusCek = rdoTunai.Checked ? Constants.STATUS_CEK_TIDAK_ADA : Constants.STATUS_CEK_MENUNGGU;
                String strngCatatan = txtCatatan.Text;
                String strngNoUrut = txtNoUrut.EditValue.ToString();
                String strngLunas = chkLunas.Checked ? Constants.STATUS_YA : Constants.STATUS_TIDAK;

                DataPembayaranPembelian dPembayaranPembelian = new DataPembayaranPembelian(command, strngKode);
                dPembayaranPembelian.tanggal = strngTanggal;
                dPembayaranPembelian.jenis = strngJenis;
                dPembayaranPembelian.supplier = strngSupplier;
                dPembayaranPembelian.akunpembayaran = strngAkunPembayaran;
                dPembayaranPembelian.nocek = strngNoCek;
                dPembayaranPembelian.tanggalcek = strngTglCek;
                dPembayaranPembelian.statuscek = strngStatusCek;
                dPembayaranPembelian.catatan = strngCatatan;
                dPembayaranPembelian.nourut = strngNoUrut;
                dPembayaranPembelian.lunas = strngLunas;

                if(this.isAdd) {
                    dPembayaranPembelian.tambah();
                    // update kode header --> setelah generate
                    strngKode = dPembayaranPembelian.kode;
                } else {
                    dPembayaranPembelian.valStatusCek();
                    dPembayaranPembelian.hapusDetail();
                    dPembayaranPembelian.ubah();
                }

                // simpan detail
                GridView gridView = gridView1;

                double dblTotal = 0;
                double dblTotalSelisih = 0;

                for(int i = 0; i < gridView.DataRowCount; i++) {
                    if(gridView.GetRowCellValue(i, "Faktur Pembelian").ToString() == "") {
                        continue;
                    }

                    String strngNo = gridView.GetRowCellValue(i, "No").ToString();
                    String strngFakturPembelian = gridView.GetRowCellValue(i, "Faktur Pembelian").ToString();
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
                    DataPembayaranPembelianDetail dPembayaranPembelianDetail = new DataPembayaranPembelianDetail(command, strngKode, strngNo);
                    dPembayaranPembelianDetail.fakturpembelian = strngFakturPembelian;
                    dPembayaranPembelianDetail.totalfaktur = dblTotalFaktur.ToString();
                    dPembayaranPembelianDetail.totalbayar = dblTotalBayar.ToString();
                    dPembayaranPembelianDetail.totalretur = dblTotalRetur.ToString();
                    dPembayaranPembelianDetail.jumlah = dblJumlah.ToString();
                    dPembayaranPembelianDetail.selisih = dblSelisih.ToString();
                    dPembayaranPembelianDetail.tambah();

                    // tulis log detail
                    OswLog.setTransaksi(command, dokumenDetail, dPembayaranPembelianDetail.ToString());
                }

                dPembayaranPembelian = new DataPembayaranPembelian(command, strngKode);
                dPembayaranPembelian.total = dblTotal.ToString();
                dPembayaranPembelian.totalselisih = dblTotalSelisih.ToString();
                dPembayaranPembelian.ubah();

                // validasi setelah simpan
                dPembayaranPembelian.valJumlahDetail();
                dPembayaranPembelian.prosesJurnal();
                dPembayaranPembelian.updateMutasi();

                // tulis log
                OswLog.setTransaksi(command, dokumen, dPembayaranPembelian.ToString());

                // reload grid di form header
                FrmPembayaranPembelian frmPembayaranPembelian = (FrmPembayaranPembelian)this.Owner;
                frmPembayaranPembelian.setGrid(command);

                // Commit Transaction
                command.Transaction.Commit();

                OswPesan.pesanInfo("Proses " + dokumen + " berhasil.");

                if(this.isAdd) {
                    if(OswPesan.pesanKonfirmasi("Konfirmasi", "Cetak Bukti Pembayaran Pembelian?") == DialogResult.Yes) {
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
            infoSupplier();
        }

        private void infoSupplier() {
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
                String query = @"SELECT A.kode AS 'Kode Supplier',A.nama AS Supplier, B.nama AS Kota, A.alamat AS Alamat, A.cp AS CP, A.telp AS Telp, A.email AS 'Email', A.pajak AS 'PKP', COALESCE(C.jumlah,0) AS 'Uang Titipan'
                                FROM supplier A
                                INNER JOIN kota B ON A.kota = B.kode
                                LEFT JOIN saldouangtitipansupplieraktual C ON A.kode = C.supplier
                                WHERE A.status = @status 
                                ORDER BY A.kode";

                Dictionary<String, String> parameters = new Dictionary<String, String>();
                parameters.Add("status", Constants.STATUS_AKTIF);

                InfUtamaDictionary form = new InfUtamaDictionary("Info Supplier", query, parameters,
                                                                new String[] { "Kode Supplier", "Kota", "Uang Titipan" },
                                                                new String[] { "Kode Supplier" },
                                                                new String[] { "Uang Titipan" });
                this.AddOwnedForm(form);
                form.ShowDialog();
                if(!form.hasil.ContainsKey("Kode Supplier")) {
                    return;
                }

                String strngKodeSupplier = form.hasil["Kode Supplier"];
                String strngKotaSupplier = form.hasil["Kota"];
                String strngUangTitipan = form.hasil["Uang Titipan"];

                txtSupplier.Text = strngKodeSupplier;
                txtUangTitipan.EditValue = strngUangTitipan;

                DataSupplier dSupplier = new DataSupplier(command, strngKodeSupplier);
                txtNama.EditValue = dSupplier.nama;
                txtKota.Text = strngKotaSupplier;
                txtAlamat.Text = dSupplier.alamat;
                txtTelepon.Text = dSupplier.telp;
                txtEmail.Text = dSupplier.email;

                setGrid(command);

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

        public void setGrid(MySqlCommand command) {
            String strngKode = txtKode.Text;

            String query = @"SELECT B.no AS 'No', B.fakturpembelian AS 'Faktur Pembelian', B.totalfaktur AS 'Total Faktur', B.totalbayar AS 'Total Bayar', B.totalretur AS 'Total Retur', B.jumlah AS Jumlah
                                FROM pembayaranpembelian A
                                INNER JOIN pembayaranpembeliandetail B ON A.kode = B.pembayaranpembelian
                                WHERE A.kode = @kode
                                ORDER BY B.no";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("kode", strngKode);

            Dictionary<String, int> widths = new Dictionary<String, int>();
            // 917 - 21 (kiri) - 17 (vertikal lines) - 35 (No) = 927
            widths.Add("No", 35);
            widths.Add("Faktur Pembelian", 324);
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

            gridView.Columns["Faktur Pembelian"].ColumnEdit = search;
            gridView.Columns["Faktur Pembelian"].ColumnEdit.ReadOnly = true;

            gridView.Columns["Total Faktur"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            gridView.Columns["Total Faktur"].SummaryItem.DisplayFormat = "{0:N2}";
            gridView.Columns["Total Bayar"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            gridView.Columns["Total Bayar"].SummaryItem.DisplayFormat = "{0:N2}";
            gridView.Columns["Total Retur"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            gridView.Columns["Total Retur"].SummaryItem.DisplayFormat = "{0:N2}";
            gridView.Columns["Jumlah"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            gridView.Columns["Jumlah"].SummaryItem.DisplayFormat = "{0:N2}";
        }

        private void gridControl1_ProcessGridKey(object sender, KeyEventArgs e) {
            GridView gridView = gridView1;
            if(e.KeyCode == Keys.F1 && gridView.FocusedColumn.FieldName == "Faktur Pembelian") {
                infoFakturPembelian();
            }
        }

        void search_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e) {
            infoFakturPembelian();
        }

        private void infoFakturPembelian() {
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

                String strngKodeSupplier = txtSupplier.Text;

                String query = @"SELECT A.jatuhtempo AS 'Jatuh Tempo', A.tanggal AS Tanggal, A.kode AS 'Faktur Pembelian', A.faktursupplier AS 'Faktur Supplier', 
		                                A.grandtotal AS 'Total Faktur', A.totalretur AS 'Total Retur', A.totalbayar AS 'Total Bayar', CASE WHEN (A.grandtotal - A.totalbayar - A.totalretur) < 0 THEN 0 ELSE (A.grandtotal - A.totalbayar - A.totalretur) END AS 'Sisa Bayar'
                                FROM fakturpembelian A
                                WHERE A.supplier = @supplier AND A.status = @status
                                ORDER BY toDate(A.jatuhtempo), toDate(A.tanggal), A.kode";

                Dictionary<String, String> parameters = new Dictionary<String, String>();
                parameters.Add("supplier", strngKodeSupplier);
                parameters.Add("status", Constants.STATUS_FAKTUR_PEMBELIAN_BELUM_LUNAS);

                InfUtamaDataTable form = new InfUtamaDataTable("Info Faktur Pembelian", query, parameters,
                                                                new String[] { },
                                                                new String[] { "Total Faktur", "Total Retur", "Total Bayar", "Sisa Bayar" },
                                                                new DataTable());
                this.AddOwnedForm(form);
                form.ShowDialog();

                if(form.hasil.Rows.Count == 0) {
                    return;
                }

                foreach(DataRow row in form.hasil.Rows) {
                    String strngFakturPembelian = row["Faktur Pembelian"].ToString();
                    String strngTotalFaktur = row["Total Faktur"].ToString();
                    String strngTotalRetur = row["Total Retur"].ToString();
                    String strngTotalBayar = row["Total Bayar"].ToString();
                    String strngSisaBayar = row["Sisa Bayar"].ToString();

                    gridView.AddNewRow();
                    gridView.MoveLast();
                    gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Faktur Pembelian"], strngFakturPembelian);
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

        private void gridView1_FocusedColumnChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventArgs e) {
            GridView gridView = sender as GridView;

            if(gridView.GetRowCellValue(gridView.FocusedRowHandle, "Faktur Pembelian") == null) {
                gridView.FocusedColumn = gridView.Columns["Faktur Pembelian"];
                return;
            }

            if(gridView.FocusedColumn == null) {
                gridView.FocusedColumn = gridView.Columns["Faktur Pembelian"];
                return;
            }

            if(gridView.FocusedColumn.FieldName != "Faktur Pembelian" && gridView.GetRowCellValue(gridView.FocusedRowHandle, "Faktur Pembelian").ToString() == "") {
                gridView.FocusedColumn = gridView.Columns["Faktur Pembelian"];
                return;
            }

            setFooter();
        }

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e) {
            GridView gridView = sender as GridView;

            if(gridView.GetRowCellValue(gridView.FocusedRowHandle, "Faktur Pembelian") == null) {
                gridView.FocusedColumn = gridView.Columns["Faktur Pembelian"];
                return;
            }

            if(gridView.FocusedColumn == null) {
                gridView.FocusedColumn = gridView.Columns["Faktur Pembelian"];
                return;
            }

            if(gridView.FocusedColumn.FieldName != "Faktur Pembelian" && gridView.GetRowCellValue(gridView.FocusedRowHandle, "Faktur Pembelian").ToString() == "") {
                gridView.FocusedColumn = gridView.Columns["Faktur Pembelian"];
                return;
            }

            setFooter();
        }

        private void gridView1_InitNewRow(object sender, InitNewRowEventArgs e) {
            GridView gridView = sender as GridView;

            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["No"], gridView.DataRowCount + 1);
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Faktur Pembelian"], "");
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

            if(gridView.GetRowCellValue(gridView.FocusedRowHandle, "Faktur Pembelian") == null) {
                gridView.FocusedColumn = gridView.Columns["Faktur Pembelian"];
                return;
            }

            if(gridView.GetRowCellValue(gridView.FocusedRowHandle, "Faktur Pembelian").ToString() == "") {
                gridView.FocusedColumn = gridView.Columns["Faktur Pembelian"];
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
                    if(gridView.GetRowCellValue(i, "Faktur Pembelian").ToString() == "") {
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

                RptCetakPembayaranPembelian report = new RptCetakPembayaranPembelian();

                DataOswPerusahaan dPerusahaan = new DataOswPerusahaan(command);
                report.Parameters["perusahaanNama"].Value = dPerusahaan.nama;
                report.Parameters["perusahaanAlamat"].Value = dPerusahaan.alamat;
                report.Parameters["perusahaanTelepon"].Value = dPerusahaan.telp;
                report.Parameters["perusahaanEmail"].Value = dPerusahaan.email;
                report.Parameters["perusahaanNPWP"].Value = dPerusahaan.npwp;

                DataPembayaranPembelian dPembayaranPembelian = new DataPembayaranPembelian(command, kode);
                DataAkun dAkun = new DataAkun(command, dPembayaranPembelian.akunpembayaran);
                DataSupplier dSupplier = new DataSupplier(command, dPembayaranPembelian.supplier);
                DataKota dKota = new DataKota(command, dSupplier.kota);

                report.Parameters["nomor"].Value = kode;
                report.Parameters["tanggal"].Value = dPembayaranPembelian.tanggal;
                report.Parameters["akun"].Value = dPembayaranPembelian.akunpembayaran + " " + dAkun.nama;
                report.Parameters["total"].Value = dPembayaranPembelian.total;
                report.Parameters["terbilang"].Value = OswMath.Terbilang(Convert.ToDouble(dPembayaranPembelian.total));
                report.Parameters["catatan"].Value = dPembayaranPembelian.catatan;
                report.Parameters["nourut"].Value = dPembayaranPembelian.nourut;
                report.Parameters["supplier"].Value = dSupplier.nama + " [" + dKota.nama + "]";

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

        private void radioButtons_CheckedChanged(object sender, EventArgs e) {
            if(rdoCek.Checked) {
                txtNoCek.Enabled = true;
                deTanggalCek.Enabled = true;
                cmbAkunPembayaran.EditValue = strngAkunPostDatedCheque;
                cmbAkunPembayaran.Enabled = false;
            } else {
                txtNoCek.Enabled = false;
                deTanggalCek.Enabled = false;
                txtNoCek.Text = "";
                deTanggalCek.EditValue = "";
                cmbAkunPembayaran.EditValue = OswCombo.getFirstEditValue(cmbAkunPembayaran);
                cmbAkunPembayaran.Enabled = true;
            }
        }

    }
}