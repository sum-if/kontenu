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
    public partial class FrmReturPembelianAdd : DevExpress.XtraEditors.XtraForm {
        private String id = "RETURPEMBELIAN";
        private String dokumen = "RETURPEMBELIAN";
        private String dokumenDetail = "RETURPEMBELIAN";
        private Boolean isAdd;
        private double dblTotalFaktur;
        private double dblSisaBayar;
        private double dblKembaliHutang;
        private double dblKembaliUangTitipan;

        public FrmReturPembelianAdd(bool pIsAdd) {
            isAdd = pIsAdd;
            InitializeComponent();
        }

        private void FrmFakturPembelianAdd_Load(object sender, EventArgs e) {
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
                OswControlDefaultProperties.setTanggal(deTanggalInput);
                OswControlDefaultProperties.setAngka(txtDiskon);

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
                deTanggalInput.Enabled = false;
                btnCari.Enabled = false;
                btnCariFaktur.Enabled = false;

                // data
                DataReturPembelian dReturPembelian = new DataReturPembelian(command, strngKode);
                txtKodeSupplier.Text = dReturPembelian.supplier;
                txtFakturPembelian.Text = dReturPembelian.fakturpembelian;
                deTanggalInput.DateTime = OswDate.getDateTimeFromStringTanggal(dReturPembelian.tanggal);
                txtCatatan.EditValue = dReturPembelian.catatan;
                txtDiskon.EditValue = dReturPembelian.diskon;

                DataSupplier dSupplier = new DataSupplier(command, dReturPembelian.supplier);
                DataKota dKota = new DataKota(command, dSupplier.kota);

                txtNama.EditValue = dSupplier.nama;
                txtKota.Text = dKota.nama;
                txtAlamat.Text = dSupplier.alamat;
                txtTelepon.Text = dSupplier.telp;
                txtEmail.Text = dSupplier.email;

                rdoExcludePPN.Checked = dReturPembelian.jenisppn == Constants.JENIS_PPN_EXCLUDE_PPN;
                rdoIncludePPN.Checked = dReturPembelian.jenisppn == Constants.JENIS_PPN_INCLUDE_PPN;
                rdoNonPPN.Checked = dReturPembelian.jenisppn == Constants.JENIS_PPN_NON_PPN;

                dblTotalFaktur = double.Parse(dReturPembelian.totalfaktur);
                dblSisaBayar = double.Parse(dReturPembelian.sisabayar);

                this.setGridBarang(command);
            } else {
                OswControlDefaultProperties.resetAllInput(this);
                rdoExcludePPN.Checked = true;
                btnCetak.Enabled = false;
                btnCetakBuktiPelaporan.Enabled = false;
                btnCetakCSV.Enabled = false;

                dblTotalFaktur = 0;
                dblSisaBayar = 0;
                dblKembaliHutang = 0;
                dblKembaliUangTitipan = 0;

                this.setGridBarang(command);
            }

            txtKode.Focus();
        }

        public void setGridBarang(MySqlCommand command) {
            String strngKode = txtKode.Text;

            String query = @"SELECT B.no AS No, B.barang AS 'Kode Barang', C.nama AS 'Nama Barang', C.nopart AS 'No Part', E.nama AS Rak,
                                    B.jumlahfaktur AS 'Jumlah Faktur', B.jumlahretur AS 'Jumlah Retur', D.kode AS 'Kode Satuan', D.nama AS Satuan, B.hargabeli AS 'Harga Beli', 
                                    B.diskonitempersen AS 'Disk(%)', B.diskonitem AS Diskon, B.subtotal AS Subtotal, 
	                                B.fakturpembeliandetail AS 'Faktur Pembelian Detail',B.fakturpembeliandetailno AS 'Faktur Pembelian Detail No'
                             FROM returpembelian A
                             INNER JOIN returpembeliandetail B ON A.kode = B.returpembelian
                             INNER JOIN barang C ON B.barang = C.kode
                             INNER JOIN barangsatuan D ON B.satuan = D.kode
                             INNER JOIN barangrak E ON C.rak = E.kode
                             WHERE A.kode = @kode
                             ORDER BY B.no";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("Kode", strngKode);

            Dictionary<String, int> widths = new Dictionary<String, int>();
            // 944 - 21 (kiri) - 17 (vertikal lines) - 35 (No) = 871
            widths.Add("No", 35);
            widths.Add("Kode Barang", 100);
            widths.Add("Nama Barang", 121);
            widths.Add("No Part", 90);
            widths.Add("Rak", 60);
            widths.Add("Jumlah Faktur", 75);
            widths.Add("Jumlah Retur", 75);
            widths.Add("Satuan", 70);
            widths.Add("Harga Beli", 80);
            widths.Add("Disk(%)", 50);
            widths.Add("Diskon", 70);
            widths.Add("Subtotal", 80);

            Dictionary<String, String> inputType = new Dictionary<string, string>();
            inputType.Add("Jumlah Faktur", OswInputType.NUMBER);
            inputType.Add("Jumlah Retur", OswInputType.NUMBER);
            inputType.Add("Disk(%)", OswInputType.NUMBER);
            inputType.Add("Diskon", OswInputType.NUMBER);
            inputType.Add("Subtotal", OswInputType.NUMBER);
            inputType.Add("Harga Beli", OswInputType.NUMBER);

            OswGrid.getGridInput(gridControl1, command, query, parameters, widths, inputType,
                                 new String[] { "Faktur Pembelian Detail", "Faktur Pembelian Detail No", "Kode Satuan" },
                                 new String[] { "No", "Nama Barang", "Jumlah Faktur", "Satuan", "Disk(%)", "Diskon", "Subtotal", "No Part", "Rak", "Harga Beli" });

            RepositoryItemButtonEdit searchBarang = new RepositoryItemButtonEdit();
            searchBarang.Buttons[0].Kind = ButtonPredefines.Glyph;
            searchBarang.Buttons[0].Image = OswResources.getImage("cari-16.png", typeof(Program).Assembly);
            searchBarang.Buttons[0].Visible = true;
            searchBarang.ButtonClick += searchBarang_ButtonClick;

            GridView gridView = gridView1;
            gridView.Columns["Kode Barang"].ColumnEdit = searchBarang;
            gridView.Columns["Kode Barang"].ColumnEdit.ReadOnly = true;

            setFooter();
        }

        void searchBarang_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e) {
            infoBarang();
        }

        private void infoBarang() {
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

                String strngKodeSupplier = txtKodeSupplier.Text;
                String strngFakturPembelian = txtFakturPembelian.Text;

                String query = @"SELECT B.barang AS 'Kode Barang', C.nama AS 'Nama Barang',C.nopart AS 'No Part', F.nama AS Rak, 
                                    B.jumlah AS 'Jumlah Faktur', B.jumlahretur AS 'Jumlah Retur', 
	                                E.kode AS 'Kode Satuan',E.nama AS 'Satuan', B.hargabeli AS 'Harga Beli', B.diskonitempersen AS 'Disk(%)', B.diskonitem AS 'Diskon', B.subtotal AS Subtotal,
                                    B.fakturpembelian AS 'Faktur Pembelian Detail',B.no AS 'Faktur Pembelian Detail No'
                                FROM fakturpembelian A
                                INNER JOIN fakturpembeliandetail B ON A.kode = B.fakturpembelian
                                INNER JOIN barang C ON B.barang = C.kode
                                INNER JOIN barangsatuan E ON B.satuan = E.kode
                                INNER JOIN barangrak F ON C.rak = F.kode
                                WHERE A.kode = @fakturpembelian AND A.supplier = @supplier AND B.jumlah > B.jumlahretur
                                ORDER BY A.kode, C.nama;";

                Dictionary<String, String> parameters = new Dictionary<String, String>();
                parameters.Add("fakturpembelian", strngFakturPembelian);
                parameters.Add("supplier", strngKodeSupplier);

                InfUtamaDataTable form = new InfUtamaDataTable("Info Barang", query, parameters,
                                                                 new String[] { "Faktur Pembelian Detail", "Faktur Pembelian Detail No" },
                                                                 new String[] { "Jumlah Faktur", "Jumlah Retur", "Harga Beli","Disk(%)", "Diskon", "Subtotal" },
                                                                 new DataTable());
                this.AddOwnedForm(form);
                form.ShowDialog();

                if(form.hasil.Rows.Count == 0) {
                    return;
                }

                foreach(DataRow row in form.hasil.Rows) {
                    String strngKodeBarang = row["Kode Barang"].ToString();
                    String strngNamaBarang = row["Nama Barang"].ToString();
                    String strngNoPart = row["No Part"].ToString();
                    String strngRak = row["Rak"].ToString();
                    String strngJumlahFaktur = row["Jumlah Faktur"].ToString();
                    String strngJumlahRetur = row["Jumlah Retur"].ToString();
                    String strngKodeSatuan = row["Kode Satuan"].ToString();
                    String strngNamaSatuan = row["Satuan"].ToString();
                    String strngHargaBeli = row["Harga Beli"].ToString();
                    String strngDiskonPersen = row["Disk(%)"].ToString();
                    String strngDiskon = row["Diskon"].ToString();
                    String strngFakturPembelianDetail = row["Faktur Pembelian Detail"].ToString();
                    String strngFakturPembelianDetailNo = row["Faktur Pembelian Detail No"].ToString();

                    gridView.AddNewRow();
                    gridView.MoveLast();
                    gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Kode Barang"], strngKodeBarang);
                    gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Nama Barang"], strngNamaBarang);
                    gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["No Part"], strngNoPart);
                    gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Rak"], strngRak);
                    gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Jumlah Faktur"], strngJumlahFaktur);
                    gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Jumlah Retur"], "0");
                    gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Kode Satuan"], strngKodeSatuan);
                    gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Satuan"], strngNamaSatuan);
                    gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Harga Beli"], strngHargaBeli);
                    gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Disk(%)"], strngDiskonPersen);
                    gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Diskon"], strngDiskon);
                    gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Faktur Pembelian Detail"], strngFakturPembelianDetail);
                    gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Faktur Pembelian Detail No"], strngFakturPembelianDetailNo);
                    gridView.UpdateCurrentRow();
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
                try {
                    SplashScreenManager.CloseForm();
                } catch(Exception ex) {
                }
            }
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
                String query = @"SELECT A.kode AS Nomor, A.nama AS Nama, B.nama AS Kota, A.alamat AS Alamat, A.telp AS Telp, A.cp AS CP
                                FROM supplier A
                                INNER JOIN kota B ON A.kota = B.kode
                                WHERE A.status = @status
                                ORDER BY A.kode";

                Dictionary<String, String> parameters = new Dictionary<String, String>();
                parameters.Add("status", Constants.STATUS_AKTIF);

                InfUtamaDictionary form = new InfUtamaDictionary("Info Supplier", query, parameters,
                                                                new String[] { "Nomor", "Kota" },
                                                                new String[] { },
                                                                new String[] { });
                this.AddOwnedForm(form);
                form.ShowDialog();
                if(!form.hasil.ContainsKey("Nomor")) {
                    return;
                }

                String strngKodeSupplier = form.hasil["Nomor"];
                String strngKotaSupplier = form.hasil["Kota"];

                txtKodeSupplier.Text = strngKodeSupplier;

                DataSupplier dSupplier = new DataSupplier(command, strngKodeSupplier);
                txtNama.EditValue = dSupplier.nama;
                txtKota.Text = strngKotaSupplier;
                txtAlamat.Text = dSupplier.alamat;
                txtTelepon.Text = dSupplier.telp;
                txtEmail.Text = dSupplier.email;

                txtFakturPembelian.Text = "";
                txtDiskon.EditValue = "0";

                setGridBarang(command);

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

        private void infoFaktur() {
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
                String strngKodeSuplier = txtKodeSupplier.Text;

                String query = @"SELECT A.tanggal AS 'Tanggal Faktur', A.kode AS 'No. Faktur Pembelian', A.faktursupplier AS 'Faktur Supplier', B.nama AS Supplier, C.nama AS Kota, 
                                        A.jenisppn AS 'Jenis PPN', A.diskon AS Diskon, A.grandtotal AS 'Total Faktur', 
                                        CASE WHEN (A.grandtotal - A.totalbayar - A.totalretur) < 0 THEN 0 ELSE (A.grandtotal - A.totalbayar - A.totalretur) END AS 'Sisa Bayar'
                                FROM fakturpembelian A
                                INNER JOIN supplier B ON A.supplier = B.kode 
                                INNER JOIN kota C ON B.kota = C.kode
                                WHERE A.supplier = @supplier AND A.grandtotal > A.totalretur
                                ORDER BY A.kode";

                Dictionary<String, String> parameters = new Dictionary<String, String>();
                parameters.Add("supplier", strngKodeSuplier);

                InfUtamaDictionary form = new InfUtamaDictionary("Info Faktur", query, parameters,
                                                                new String[] { "No. Faktur Pembelian", "Jenis PPN", "Diskon", "Total Faktur", "Sisa Bayar" },
                                                                new String[] { "Jenis PPN", "Diskon" },
                                                                new String[] { "Total Faktur", "Sisa Bayar" });
                this.AddOwnedForm(form);
                form.ShowDialog();
                if(!form.hasil.ContainsKey("No. Faktur Pembelian")) {
                    return;
                }

                String strngFakturPembelian = form.hasil["No. Faktur Pembelian"];
                String strngJenisPPN = form.hasil["Jenis PPN"];
                String strngDiskon = form.hasil["Diskon"];
                String strngTotalFaktur = form.hasil["Total Faktur"];
                String strngSisaBayar = form.hasil["Sisa Bayar"];

                txtFakturPembelian.Text = strngFakturPembelian;

                rdoExcludePPN.Checked = strngJenisPPN == Constants.JENIS_PPN_EXCLUDE_PPN;
                rdoIncludePPN.Checked = strngJenisPPN == Constants.JENIS_PPN_INCLUDE_PPN;
                rdoNonPPN.Checked = strngJenisPPN == Constants.JENIS_PPN_NON_PPN;

                txtDiskon.EditValue = strngDiskon;

                dblTotalFaktur = Tools.getRoundMoney(double.Parse(strngTotalFaktur));
                dblSisaBayar = Tools.getRoundMoney(double.Parse(strngSisaBayar));

                setGridBarang(command);

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
                dxValidationProvider1.SetValidationRule(deTanggalInput, OswValidation.IsNotBlank());
                dxValidationProvider1.SetValidationRule(txtKodeSupplier, OswValidation.IsNotBlank());
                dxValidationProvider1.SetValidationRule(txtFakturPembelian, OswValidation.IsNotBlank());
                dxValidationProvider1.SetValidationRule(txtDiskon, OswValidation.IsNotBlank());

                if(!dxValidationProvider1.Validate()) {
                    foreach(Control x in dxValidationProvider1.GetInvalidControls()) {
                        dxValidationProvider1.SetIconAlignment(x, ErrorIconAlignment.MiddleRight);
                    }
                    return;
                }

                String strngKode = txtKode.Text;
                String strngTanggalInput = deTanggalInput.Text;
                String strngSupplier = txtKodeSupplier.Text;
                String strngFakturPembelian = txtFakturPembelian.Text;
                String strngCatatan = txtCatatan.Text;
                String strngDiskonFooter = txtDiskon.EditValue.ToString();
                String strngJenisPPN = "";
                if(rdoExcludePPN.Checked) {
                    strngJenisPPN = Constants.JENIS_PPN_EXCLUDE_PPN;
                } else if(rdoIncludePPN.Checked) {
                    strngJenisPPN = Constants.JENIS_PPN_INCLUDE_PPN;
                } else if(rdoNonPPN.Checked) {
                    strngJenisPPN = Constants.JENIS_PPN_NON_PPN;
                }

                DataReturPembelian dReturPembelian = new DataReturPembelian(command, strngKode);
                dReturPembelian.tanggal = strngTanggalInput;
                dReturPembelian.supplier = strngSupplier;
                dReturPembelian.fakturpembelian = strngFakturPembelian;
                dReturPembelian.catatan = strngCatatan;
                dReturPembelian.diskon = strngDiskonFooter;
                dReturPembelian.jenisppn = strngJenisPPN;

                if(this.isAdd) {
                    dReturPembelian.total = "0";
                    dReturPembelian.diskonnilai = "0";
                    dReturPembelian.totaldpp = "0";
                    dReturPembelian.totalppn = "0";
                    dReturPembelian.grandtotal = "0";
                    dReturPembelian.totalfaktur = "0";
                    dReturPembelian.sisabayar = "0";
                    dReturPembelian.kembalihutang = "0";
                    dReturPembelian.kembaliuangtitipan = "0";
                    dReturPembelian.tambah();
                    // update kode header --> setelah generate
                    strngKode = dReturPembelian.kode;
                } else {
                    dReturPembelian.hapusDetail();
                    dReturPembelian.ubah();
                }

                // simpan detail
                GridView gridView = gridView1;
                setFooter();

                double dblTotal = 0;
                double dblTotalDiskonItem = 0;

                double dblDiskonFakturPersentase = Tools.getRoundCalc(double.Parse(txtDiskon.EditValue.ToString()));

                for(int i = 0; i < gridView.DataRowCount; i++) {
                    if(gridView.GetRowCellValue(i, "Kode Barang").ToString() == "") {
                        continue;
                    }

                    String strngNo = gridView.GetRowCellValue(i, "No").ToString();
                    String strngKodeBarang = gridView.GetRowCellValue(i, "Kode Barang").ToString();
                    String strngKodeSatuan = gridView.GetRowCellValue(i, "Kode Satuan").ToString();
                    String strngFakturPembelianDetail = gridView.GetRowCellValue(i, "Faktur Pembelian Detail").ToString();
                    String strngFakturPembelianDetailNo = gridView.GetRowCellValue(i, "Faktur Pembelian Detail No").ToString();

                    double dblJumlahFaktur = Tools.getRoundMoney(double.Parse(gridView.GetRowCellValue(i, "Jumlah Faktur").ToString()));
                    double dblJumlah = Tools.getRoundMoney(double.Parse(gridView.GetRowCellValue(i, "Jumlah Retur").ToString()));
                    double dblHargaBeli = Tools.getRoundMoney(double.Parse(gridView.GetRowCellValue(i, "Harga Beli").ToString()));
                    double dblDiskonItemPersen = Tools.getRoundMoney(double.Parse(gridView.GetRowCellValue(i, "Disk(%)").ToString()));
                    double dblDiskonItem = Tools.getRoundMoney((dblHargaBeli * dblDiskonItemPersen) / 100);

                    double dblHargaBersihItem = Tools.getRoundMoney(dblHargaBeli - dblDiskonItem);
                    double dblDiskonFaktur = Tools.getRoundMoney((dblHargaBersihItem * dblDiskonFakturPersentase) / 100);
                    double dblSubtotal = Tools.getRoundMoney(dblHargaBersihItem * dblJumlah);

                    double dblHargaBersihItem2 = Tools.getRoundMoney(dblHargaBersihItem - dblDiskonFaktur);
                    double dblDPP = dblHargaBersihItem2;
                    if(rdoIncludePPN.Checked) {
                        dblDPP = Tools.getRoundMoney((dblHargaBersihItem2 * 100) / 110);
                    }

                    double dblPPN = Tools.getRoundMoney(dblDPP / 10);

                    if(rdoNonPPN.Checked) {
                        dblPPN = 0;
                    }

                    dblTotal = Tools.getRoundMoney(dblTotal + dblSubtotal);
                    dblTotalDiskonItem = Tools.getRoundMoney(dblTotalDiskonItem + dblDiskonItem);

                    // simpan detail
                    DataReturPembelianDetail dReturPembelianDetail = new DataReturPembelianDetail(command, strngKode, strngNo);
                    dReturPembelianDetail.barang = strngKodeBarang;
                    dReturPembelianDetail.satuan = strngKodeSatuan;
                    dReturPembelianDetail.jumlahfaktur = dblJumlahFaktur.ToString();
                    dReturPembelianDetail.jumlahretur = dblJumlah.ToString();
                    dReturPembelianDetail.hargabeli = dblHargaBeli.ToString();
                    dReturPembelianDetail.diskonitempersen = dblDiskonItemPersen.ToString();
                    dReturPembelianDetail.diskonitem = dblDiskonItem.ToString();
                    dReturPembelianDetail.diskonfaktur = dblDiskonFaktur.ToString();
                    dReturPembelianDetail.dpp = dblDPP.ToString();
                    dReturPembelianDetail.ppn = dblPPN.ToString();
                    dReturPembelianDetail.subtotal = dblSubtotal.ToString();
                    dReturPembelianDetail.fakturpembeliandetail = strngFakturPembelianDetail;
                    dReturPembelianDetail.fakturpembeliandetailno = strngFakturPembelianDetailNo;
                    dReturPembelianDetail.tambah();

                    // tulis log detail
                    OswLog.setTransaksi(command, dokumenDetail, dReturPembelianDetail.ToString());
                }

                double dblTotalDiskonFaktur = Tools.getRoundMoney((dblTotal * dblDiskonFakturPersentase) / 100);
                double dblTotalDiskon = dblTotalDiskonItem + dblTotalDiskonFaktur;
                double dblTotalBersih = Tools.getRoundMoney(dblTotal - dblTotalDiskonFaktur);

                double dblTotalDPP = Tools.getRoundMoney(dblTotal - dblTotalDiskonFaktur);
                if(rdoIncludePPN.Checked) {
                    dblTotalDPP = Tools.getRoundMoney((dblTotalBersih * 100) / 110);
                }

                double dblTotalPPN = Tools.getRoundMoney(dblTotalDPP / 10);
                if(rdoNonPPN.Checked) {
                    dblTotalPPN = 0;
                }

                double dblGrandTotal = 0;

                if(rdoIncludePPN.Checked) {
                    dblGrandTotal = Tools.getRoundMoney(dblTotal - dblTotalDiskonFaktur);
                } else {
                    dblGrandTotal = Tools.getRoundMoney(dblTotal - dblTotalDiskonFaktur + dblTotalPPN);
                }

                // hitung untuk footer kiri
                dblKembaliHutang = 0;
                dblKembaliUangTitipan = 0;

                if(dblSisaBayar > 0) {
                    if(dblGrandTotal > dblSisaBayar) {
                        dblKembaliHutang = dblSisaBayar;
                    } else {
                        dblKembaliHutang = dblGrandTotal;
                    }
                }

                if(dblGrandTotal > dblSisaBayar) {
                    dblKembaliUangTitipan = Tools.getRoundMoney(dblGrandTotal - dblSisaBayar);
                }

                dReturPembelian = new DataReturPembelian(command, strngKode);
                dReturPembelian.total = dblTotal.ToString();
                dReturPembelian.diskonnilai = dblTotalDiskonFaktur.ToString();
                dReturPembelian.totaldiskon = dblTotalDiskon.ToString();
                dReturPembelian.totaldpp = dblTotalDPP.ToString();
                dReturPembelian.totalppn = dblTotalPPN.ToString();
                dReturPembelian.grandtotal = dblGrandTotal.ToString();
                dReturPembelian.totalfaktur = dblTotalFaktur.ToString();
                dReturPembelian.sisabayar = dblSisaBayar.ToString();
                dReturPembelian.kembalihutang = dblKembaliHutang.ToString();
                dReturPembelian.kembaliuangtitipan = dblKembaliUangTitipan.ToString();
                dReturPembelian.ubah();

                // validasi setelah simpan
                dReturPembelian.valJumlahDetail();
                dReturPembelian.prosesJurnal();
                dReturPembelian.updateMutasi();

                // tulis log
                OswLog.setTransaksi(command, dokumen, dReturPembelian.ToString());

                // reload grid di form header
                FrmReturPembelian frmReturPembelian = (FrmReturPembelian)this.Owner;
                frmReturPembelian.setGrid(command);

                // Commit Transaction
                command.Transaction.Commit();

                OswPesan.pesanInfo("Proses " + dokumen + " berhasil.");

                if(this.isAdd) {
                    if(OswPesan.pesanKonfirmasi("Konfirmasi", "Cetak Retur Pembelian?") == DialogResult.Yes) {
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

        private void gridControl1_ProcessGridKey(object sender, KeyEventArgs e) {
            GridView gridView = gridView1;
            if(e.KeyCode == Keys.F1 && gridView.FocusedColumn.FieldName == "Kode Barang") {
                infoBarang();
            }
        }

        private void gridView1_FocusedColumnChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventArgs e) {
            GridView gridView = sender as GridView;

            if(gridView.GetRowCellValue(gridView.FocusedRowHandle, "Kode Barang") == null) {
                gridView.FocusedColumn = gridView.Columns["Kode Barang"];
                return;
            }

            if(gridView.FocusedColumn == null) {
                gridView.FocusedColumn = gridView.Columns["Kode Barang"];
                return;
            }

            if(gridView.FocusedColumn.FieldName != "Kode Barang" && gridView.GetRowCellValue(gridView.FocusedRowHandle, "Kode Barang").ToString() == "") {
                gridView.FocusedColumn = gridView.Columns["Kode Barang"];
                return;
            }

            setFooter();
        }

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e) {
            GridView gridView = sender as GridView;

            if(gridView.GetRowCellValue(gridView.FocusedRowHandle, "Kode Barang") == null) {
                gridView.FocusedColumn = gridView.Columns["Kode Barang"];
                return;
            }

            if(gridView.FocusedColumn == null) {
                gridView.FocusedColumn = gridView.Columns["Kode Barang"];
                return;
            }

            if(gridView.FocusedColumn.FieldName != "Kode Barang" && gridView.GetRowCellValue(gridView.FocusedRowHandle, "Kode Barang").ToString() == "") {
                gridView.FocusedColumn = gridView.Columns["Kode Barang"];
                return;
            }
        }

        private void gridView1_InitNewRow(object sender, InitNewRowEventArgs e) {
            GridView gridView = sender as GridView;

            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["No"], gridView.DataRowCount + 1);
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Kode Barang"], "");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Nama Barang"], "");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Jumlah Faktur"], "0");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Jumlah Retur"], "0");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Kode Satuan"], "");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Satuan"], "");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Harga Beli"], "0");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Disk(%)"], "0");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Diskon"], "0");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Subtotal"], "0");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Faktur Pembelian Detail"], "");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Faktur Pembelian Detail No"], "0");
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

            if(gridView.GetRowCellValue(gridView.FocusedRowHandle, "Kode Barang") == null) {
                gridView.FocusedColumn = gridView.Columns["Kode Barang"];
                return;
            }

            if(gridView.GetRowCellValue(gridView.FocusedRowHandle, "Kode Barang").ToString() == "") {
                gridView.FocusedColumn = gridView.Columns["Kode Barang"];
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

                double dblTotal = 0;
                double dblTotalDiskonItem = 0;

                double dblDiskonFakturPersentase = Tools.getRoundCalc(double.Parse(txtDiskon.EditValue.ToString()));

                for(int i = 0; i < gridView.DataRowCount; i++) {
                    if(gridView.GetRowCellValue(i, "Kode Barang") == null) {
                        continue;
                    }

                    if(gridView.GetRowCellValue(i, "Kode Barang").ToString() == "") {
                        continue;
                    }

                    double dblJumlah = Tools.getRoundMoney(double.Parse(gridView.GetRowCellValue(i, "Jumlah Retur").ToString()));
                    double dblHargaBeli = Tools.getRoundMoney(double.Parse(gridView.GetRowCellValue(i, "Harga Beli").ToString()));
                    double dblDiskonItemPersen = Tools.getRoundMoney(double.Parse(gridView.GetRowCellValue(i, "Disk(%)").ToString()));
                    double dblDiskonItem = Tools.getRoundMoney((dblHargaBeli * dblDiskonItemPersen) / 100);

                    double dblHargaBersihItem = Tools.getRoundMoney(dblHargaBeli - dblDiskonItem);
                    double dblDiskonFaktur = Tools.getRoundMoney((dblHargaBersihItem * dblDiskonFakturPersentase) / 100);
                    double dblSubtotal = Tools.getRoundMoney(dblHargaBersihItem * dblJumlah);

                    double dblHargaBersihItem2 = Tools.getRoundMoney(dblHargaBersihItem - dblDiskonFaktur);
                    double dblDPP = dblHargaBersihItem2;
                    if(rdoIncludePPN.Checked) {
                        dblDPP = Tools.getRoundMoney((dblHargaBersihItem2 * 100) / 110);
                    }

                    double dblPPN = Tools.getRoundMoney(dblDPP / 10);

                    if(rdoNonPPN.Checked) {
                        dblPPN = 0;
                    }

                    dblTotal = Tools.getRoundMoney(dblTotal + dblSubtotal);
                    dblTotalDiskonItem = Tools.getRoundMoney(dblTotalDiskonItem + dblDiskonItem);

                    gridView.SetRowCellValue(i, gridView.Columns["Subtotal"], dblSubtotal.ToString());
                }

                double dblTotalDiskonFaktur = Tools.getRoundMoney((dblTotal * dblDiskonFakturPersentase) / 100);
                double dblTotalDiskon = dblTotalDiskonItem + dblTotalDiskonFaktur;
                double dblTotalBersih = Tools.getRoundMoney(dblTotal - dblTotalDiskonFaktur);

                double dblTotalDPP = Tools.getRoundMoney(dblTotal - dblTotalDiskonFaktur);
                if(rdoIncludePPN.Checked) {
                    dblTotalDPP = Tools.getRoundMoney((dblTotalBersih * 100) / 110);
                }

                double dblTotalPPN = Tools.getRoundMoney(dblTotalDPP / 10);
                if(rdoNonPPN.Checked) {
                    dblTotalPPN = 0;
                }

                double dblGrandTotal = 0;

                if(rdoIncludePPN.Checked) {
                    dblGrandTotal = Tools.getRoundMoney(dblTotal - dblTotalDiskonFaktur);
                } else {
                    dblGrandTotal = Tools.getRoundMoney(dblTotal - dblTotalDiskonFaktur + dblTotalPPN);
                }

                lblTotal.Text = OswConvert.convertToRupiah(dblTotal);
                lblDiskon.Text = OswConvert.convertToRupiah(dblTotalDiskonFaktur);
                lblTotalPPN.Text = OswConvert.convertToRupiah(dblTotalPPN);
                lblGrandTotal.Text = OswConvert.convertToRupiah(dblGrandTotal);

                // hitung untuk footer kiri
                dblKembaliHutang = 0;
                dblKembaliUangTitipan = 0;

                if(dblSisaBayar > 0) {
                    if(dblGrandTotal > dblSisaBayar) {
                        dblKembaliHutang = dblSisaBayar;
                    } else {
                        dblKembaliHutang = dblGrandTotal;
                    }
                }

                if(dblGrandTotal > dblSisaBayar) {
                    dblKembaliUangTitipan = Tools.getRoundMoney(dblGrandTotal - dblSisaBayar);
                }

                lblTotalFaktur.Text = OswConvert.convertToRupiah(dblTotalFaktur);
                lblSisaBayar.Text = OswConvert.convertToRupiah(dblSisaBayar);
                lblHutang.Text = OswConvert.convertToRupiah(dblKembaliHutang);
                lblUangTitipan.Text = OswConvert.convertToRupiah(dblKembaliUangTitipan);

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

        private void txtDiskon_EditValueChanged(object sender, EventArgs e) {
            setFooter();
        }

        private void btnCari_Click(object sender, EventArgs e) {
            infoSupplier();
        }

        private void btnCariFaktur_Click(object sender, EventArgs e) {
            infoFaktur();
        }

        private void btnCetak_Click(object sender, EventArgs e) {
            String strngKodeRetur = txtKode.Text;
            cetak(strngKodeRetur);
        }

        private void cetak(String kodeRetur) {
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

                // function code
                string kode = kodeRetur;

                RptCetakReturPembelian report = new RptCetakReturPembelian();

                DataOswPerusahaan dPerusahaan = new DataOswPerusahaan(command);
                report.Parameters["perusahaanNama"].Value = dPerusahaan.nama;
                report.Parameters["perusahaanAlamat"].Value = dPerusahaan.alamat;
                report.Parameters["perusahaanTelepon"].Value = dPerusahaan.telp;
                report.Parameters["perusahaanEmail"].Value = dPerusahaan.email;
                report.Parameters["perusahaanNPWP"].Value = dPerusahaan.npwp;

                DataReturPembelian dreturpembelian = new DataReturPembelian(command, kode);
                DataFakturPembelian dFakturPembelian = new DataFakturPembelian(command, dreturpembelian.fakturpembelian);

                report.Parameters["returNomor"].Value = kode;
                report.Parameters["returTanggal"].Value = dreturpembelian.tanggal;
                report.Parameters["fakturPembelian"].Value = dreturpembelian.fakturpembelian;
                report.Parameters["fakturSupplier"].Value = dFakturPembelian.faktursupplier;
                report.Parameters["returPPN"].Value = Convert.ToDouble(dreturpembelian.totalppn);
                report.Parameters["returDiskon"].Value = Convert.ToDouble(dreturpembelian.diskonnilai);
                report.Parameters["returGrandTotal"].Value = Convert.ToDouble(dreturpembelian.grandtotal);
                report.Parameters["returKembaliHutang"].Value = Convert.ToDouble(dreturpembelian.kembalihutang);
                report.Parameters["returKembaliUangTitipan"].Value = Convert.ToDouble(dreturpembelian.kembaliuangtitipan);
                report.Parameters["returTerbilang"].Value = OswMath.Terbilang(Convert.ToDouble(dreturpembelian.grandtotal));
                report.Parameters["returCatatan"].Value = dreturpembelian.catatan;

                DataSupplier dsupplier = new DataSupplier(command, dreturpembelian.supplier);
                report.Parameters["supplierNama"].Value = dsupplier.nama;
                report.Parameters["supplierAlamat"].Value = dsupplier.alamat;
                report.Parameters["supplierTelepon"].Value = dsupplier.telp;
                report.Parameters["supplierKota"].Value = dsupplier.kota;
                report.Parameters["supplierNPWP"].Value = dsupplier.npwpkode;

                // assign the printing system to the document viewer.
                LaporanPrintPreview laporan = new LaporanPrintPreview();
                laporan.documentViewer1.DocumentSource = report;

                //reportprinttool printtool = new reportprinttool(report);
                //printtool.print();

                OswLog.setLaporan(command, dokumen);

                laporan.Show();

                // commit transaction
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

        private void btnCetakBuktiPelaporan_Click(object sender, EventArgs e) {
            String strngKodeRetur = txtKode.Text;
            cetakBuktiPelaporan(strngKodeRetur);
        }

        private void cetakBuktiPelaporan(String kodeRetur) {
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

                // function code
                string kode = kodeRetur;

                RptCetakReturPembelianBuktiPembelian report = new RptCetakReturPembelianBuktiPembelian();
                DataOswPerusahaan dPerusahaan = new DataOswPerusahaan(command);
                DataReturPembelian dreturpembelian = new DataReturPembelian(command, kode);
                DataFakturPembelian dFakturPembelian = new DataFakturPembelian(command, dreturpembelian.fakturpembelian);
                DataSupplier dsupplier = new DataSupplier(command, dreturpembelian.supplier);

                report.Parameters["Kode"].Value = kode;
                report.Parameters["Tanggal"].Value = dreturpembelian.tanggal;

                report.Parameters["NoFakturPajak"].Value = dreturpembelian.nofakturpajak;
                report.Parameters["TanggalFakturPajak"].Value = dreturpembelian.tanggalfakturpajak;

                report.Parameters["PembeliNama"].Value = dPerusahaan.nama;
                report.Parameters["PembeliAlamat"].Value = dPerusahaan.alamat;
                report.Parameters["PembeliNPWP"].Value = dPerusahaan.npwp;

                report.Parameters["PenjualNama"].Value = dsupplier.nama;
                report.Parameters["PenjualAlamat"].Value = dsupplier.alamat;
                report.Parameters["PenjualNPWP"].Value = dsupplier.npwpkode;

                report.Parameters["JumlahHargaJual"].Value = Convert.ToDouble(dreturpembelian.totaldpp);
                report.Parameters["JumlahPPN"].Value = Convert.ToDouble(dreturpembelian.totalppn);
                report.Parameters["JumlahPPnBM"].Value = 0;

                // assign the printing system to the document viewer.
                LaporanPrintPreview laporan = new LaporanPrintPreview();
                laporan.documentViewer1.DocumentSource = report;

                //reportprinttool printtool = new reportprinttool(report);
                //printtool.print();

                OswLog.setLaporan(command, dokumen);

                laporan.Show();

                // commit transaction
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

        private void cetakCSV(String kodeRetur) {
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

                // function code
                string kode = kodeRetur;

                RptCetakReturCSV report = new RptCetakReturCSV();
                report.Parameters["Kode"].Value = kode;

                // assign the printing system to the document viewer.
                LaporanPrintPreview laporan = new LaporanPrintPreview();
                laporan.documentViewer1.DocumentSource = report;

                //reportprinttool printtool = new reportprinttool(report);
                //printtool.print();

                OswLog.setLaporan(command, dokumen);

                laporan.Show();

                // commit transaction
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

        private void btnCetakCSV_Click(object sender, EventArgs e) {
            String strngKodeRetur = txtKode.Text;
            cetakCSV(strngKodeRetur);
        }
    }
}