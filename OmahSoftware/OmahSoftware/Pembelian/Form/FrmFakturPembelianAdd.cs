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
using DevExpress.XtraEditors.Controls;
using OmahSoftware.Penjualan;

namespace OmahSoftware.Pembelian {
    public partial class FrmFakturPembelianAdd : DevExpress.XtraEditors.XtraForm {
        private String id = "FAKTURPEMBELIAN";
        private String dokumen = "FAKTURPEMBELIAN";
        private String dokumenDetail = "FAKTURPEMBELIAN";
        private Boolean isAdd;

        public FrmFakturPembelianAdd(bool pIsAdd) {
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
                OswControlDefaultProperties.setTanggal(deTanggal);
                OswControlDefaultProperties.setTanggal(deTanggalJatuhTempo);

                OswControlDefaultProperties.setAngka(txtDiskon);
                OswControlDefaultProperties.setAngka(txtBiayaEkspedisi);

                cmbEkspedisi = ComboQueryUmum.getEkspedisi(cmbEkspedisi, command, false, true);

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
                btnCariPesananPembelian.Enabled = false;

                // data
                DataFakturPembelian dFakturPembelian = new DataFakturPembelian(command, strngKode);
                txtSupplier.Text = dFakturPembelian.supplier;
                txtFakturSupplier.Text = dFakturPembelian.faktursupplier;
                deTanggal.DateTime = OswDate.getDateTimeFromStringTanggal(dFakturPembelian.tanggal);
                deTanggalJatuhTempo.DateTime = OswDate.getDateTimeFromStringTanggal(dFakturPembelian.jatuhtempo);
                txtFakturPajak.Text = dFakturPembelian.nofakturpajak;
                txtCatatan.EditValue = dFakturPembelian.catatan;
                txtDiskon.EditValue = dFakturPembelian.diskon;
                txtPesananPembelian.Text = dFakturPembelian.pesananpembelian;

                rdoExcludePPN.Checked = dFakturPembelian.jenisppn == Constants.JENIS_PPN_EXCLUDE_PPN;
                rdoIncludePPN.Checked = dFakturPembelian.jenisppn == Constants.JENIS_PPN_INCLUDE_PPN;
                rdoNonPPN.Checked = dFakturPembelian.jenisppn == Constants.JENIS_PPN_NON_PPN;

                DataSupplier dSupplier = new DataSupplier(command, dFakturPembelian.supplier);
                DataKota dKota = new DataKota(command, dSupplier.kota);

                txtNama.EditValue = dSupplier.nama;
                txtKota.Text = dKota.nama;
                txtAlamat.Text = dSupplier.alamat;
                txtTelepon.Text = dSupplier.telp;
                txtEmail.Text = dSupplier.email;

                cmbEkspedisi.EditValue = dFakturPembelian.ekspedisi;
                txtBiayaEkspedisi.EditValue = dFakturPembelian.biayaekspedisi;

                this.setGridBarang(command);

            } else {
                OswControlDefaultProperties.resetAllInput(this);
                rdoExcludePPN.Checked = true;
                cmbEkspedisi.EditValue = "";

                this.setGridBarang(command);
            }

            txtKode.Focus();
        }

        private void updateJatuhTempo() {
            String strngSupplier = txtSupplier.Text;

            if(strngSupplier == "") {
                return;
            }

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
                String strngTanggal = deTanggal.Text;
                DataSupplier dSupplier = new DataSupplier(command, strngSupplier);
                deTanggalJatuhTempo.DateTime = OswDate.getDateTimeTanggalTambahHari(strngTanggal, int.Parse(dSupplier.maksjatuhtempo));

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

        public void setGridBarang(MySqlCommand command) {
            String strngKode = txtKode.Text;

            String query = @"SELECT B.no AS 'No', C.kode AS 'Kode Barang', C.nama AS 'Nama Barang', C.nopart AS 'No Part', F.nama AS Kategori,E.kode AS 'Kode Satuan', E.nama AS 'Satuan', 
                                B.jumlah AS 'Jumlah', B.hargabeli AS 'Harga Beli', B.diskonitempersen AS 'Disk(%)', B.diskonitem AS 'Diskon', B.subtotal AS 'Subtotal',
                                B.pesananpembeliandetail AS 'Pesanan Pembelian Detail',B.pesananpembeliandetailno AS 'Pesanan Pembelian Detail No'
                                FROM fakturpembelian A
                                INNER JOIN fakturpembeliandetail B ON A.kode = B.fakturpembelian
                                INNER JOIN barang C ON B.barang = C.kode
                                INNER JOIN barangsatuan E ON B.satuan = E.kode
                                INNER JOIN barangkategori F ON C.barangkategori = F.kode
                                WHERE A.kode = @kode
                                ORDER BY B.no";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("kode", strngKode);

            Dictionary<String, int> widths = new Dictionary<String, int>();
            // 960 - 21 (kiri) - 17 (vertikal lines) - 35 (No) = 927
            widths.Add("No", 35);
            widths.Add("Kode Barang", 100);
            widths.Add("Nama Barang", 200);
            widths.Add("No Part", 80);
            widths.Add("Kategori", 77);
            widths.Add("Satuan", 80);
            widths.Add("Jumlah", 60);
            widths.Add("Harga Beli", 80);
            widths.Add("Disk(%)", 50);
            widths.Add("Diskon", 80);
            widths.Add("Subtotal", 80);

            Dictionary<String, String> inputType = new Dictionary<string, string>();
            inputType.Add("Jumlah", OswInputType.NUMBER);
            inputType.Add("Disk(%)", OswInputType.NUMBER);
            inputType.Add("Diskon", OswInputType.NUMBER);
            inputType.Add("Subtotal", OswInputType.NUMBER);
            inputType.Add("Harga Beli", OswInputType.NUMBER);

            OswGrid.getGridInput(gridControl1, command, query, parameters, widths, inputType,
                                 new String[] { "Kode Satuan", "Pesanan Pembelian Detail", "Pesanan Pembelian Detail No" },
                                 new String[] { "No", "Nama Barang", "No Part", "Kategori", "Satuan", "Subtotal", "Diskon" });

            // search produk di kolom kode
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

                String strngPesananPembelian = txtPesananPembelian.Text;
                String strngSupplier = txtSupplier.Text;

                String query;
                Dictionary<String, String> parameters;
                InfUtamaDataTable form;

                // tanpa pesanan pembelian
                if(strngPesananPembelian == "") {
                    query = @"SELECT A.kode AS 'Kode Barang', A.nama AS 'Nama Barang', A.nopart AS 'No Part', C.nama AS Kategori, 
                                    B.kode AS 'Kode Satuan', B.nama AS Satuan, D.nama AS Rak, 
                                    0 AS 'Sisa Faktur', A.hargabeliterakhir AS 'Harga Beli',0 AS 'Disk(%)', 0 AS 'Diskon', 
                                    A.stokminimum AS 'Stok Minimum', COALESCE(E.jumlah,0) AS Stok, A.hargabeliterakhir AS 'Harga Terakhir', 
                                    getHargaTerakhirBarangSupplier(@supplier, A.kode) AS 'Harga Terakhir Supplier',
                                    '' AS 'Pesanan Pembelian Detail', 0 AS 'Pesanan Pembelian Detail No'
                                FROM barang A
                                INNER JOIN barangsatuan B ON A.standarsatuan = B.kode
                                INNER JOIN barangkategori C ON A.barangkategori = C.kode
                                INNER JOIN barangrak D ON A.rak = D.kode
                                LEFT JOIN saldopersediaanaktual E ON E.barang = A.kode
                                WHERE A.status = @status
                                ORDER BY C.nama,A.nama,B.nama";

                    parameters = new Dictionary<String, String>();
                    parameters.Add("supplier", strngSupplier);
                    parameters.Add("status", Constants.STATUS_AKTIF);

                    form = new InfUtamaDataTable("Info Barang", query, parameters,
                                                                new String[] { "Sisa Faktur", "Harga Beli", "Diskon", "Disk(%)", "Kode Satuan", "Pesanan Pembelian Detail", "Pesanan Pembelian Detail No" },
                                                                new String[] { "Stok", "Stok Minimum", "Harga Terakhir", "Harga Terakhir Supplier" },
                                                                new DataTable());
                } else {
                    query = @"SELECT C.kode AS 'Kode Barang', C.nama AS 'Nama Barang', C.nopart AS 'No Part', F.nama AS Kategori,E.kode AS 'Kode Satuan', E.nama AS 'Satuan', 
                                    (B.jumlah - B.jumlahfaktur) AS 'Sisa Faktur', B.hargabeli AS 'Harga Beli', B.diskonitempersen AS 'Disk(%)', B.diskonitem AS 'Diskon', 
                                    B.pesananpembelian 'Pesanan Pembelian Detail', B.no AS 'Pesanan Pembelian Detail No'
                                FROM pesananpembelian A
                                INNER JOIN pesananpembeliandetail B ON A.kode = B.pesananpembelian
                                INNER JOIN barang C ON B.barang = C.kode
                                INNER JOIN barangsatuan E ON B.satuan = E.kode
                                INNER JOIN barangkategori F ON C.barangkategori = F.kode
                                WHERE A.kode = @kode
                                ORDER BY B.no";

                    parameters = new Dictionary<String, String>();
                    parameters.Add("kode", strngPesananPembelian);

                    form = new InfUtamaDataTable("Info Barang", query, parameters,
                                                                    new String[] { "Kode Satuan", "Pesanan Pembelian Detail", "Pesanan Pembelian Detail No" },
                                                                    new String[] { "Sisa Faktur", "Harga Beli", "Disk(%)", "Diskon" },
                                                                    new DataTable());
                }

                this.AddOwnedForm(form);
                form.ShowDialog();

                if(form.hasil.Rows.Count == 0) {
                    return;
                }

                foreach(DataRow row in form.hasil.Rows) {
                    String strngKode = row["Kode Barang"].ToString();
                    String strngNama = row["Nama Barang"].ToString();
                    String strngNoPart = row["No Part"].ToString();
                    String strngKodeSatuan = row["Kode Satuan"].ToString();
                    String strngSatuan = row["Satuan"].ToString();
                    String strngKategori = row["Kategori"].ToString();
                    String strngSisaFaktur = row["Sisa Faktur"].ToString();
                    String strngHargaBeli = row["Harga Beli"].ToString();
                    String strngDiskonPersen = row["Disk(%)"].ToString();
                    String strngDiskon = row["Diskon"].ToString();
                    String strngPesananPembelianDetail = row["Pesanan Pembelian Detail"].ToString();
                    String strngPesananPembelianDetailNo = row["Pesanan Pembelian Detail No"].ToString();

                    gridView.AddNewRow();
                    gridView.MoveLast();
                    gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Kode Barang"], strngKode);
                    gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Nama Barang"], strngNama);
                    gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["No Part"], strngNoPart);
                    gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Kode Satuan"], strngKodeSatuan);
                    gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Satuan"], strngSatuan);
                    gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Kategori"], strngKategori);
                    gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Harga Beli"], strngHargaBeli);
                    gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Jumlah"], strngSisaFaktur);
                    gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Disk(%)"], strngDiskonPersen);
                    gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Diskon"], strngDiskon);
                    gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Pesanan Pembelian Detail"], strngPesananPembelianDetail);
                    gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Pesanan Pembelian Detail No"], strngPesananPembelianDetailNo);
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
                String query = @"SELECT A.kode AS 'Kode Supplier',A.nama AS Supplier, B.nama AS Kota, A.alamat AS Alamat, A.cp AS CP, A.telp AS Telp, A.email AS 'Email', A.pajak AS 'PKP'
                                FROM supplier A
                                INNER JOIN kota B ON A.kota = B.kode
                                WHERE A.status = @status 
                                ORDER BY A.kode";

                Dictionary<String, String> parameters = new Dictionary<String, String>();
                parameters.Add("status", Constants.STATUS_AKTIF);

                InfUtamaDictionary form = new InfUtamaDictionary("Info Supplier", query, parameters,
                                                                new String[] { "Kode Supplier", "Kota" },
                                                                new String[] { "Kode Supplier" },
                                                                new String[] { });
                this.AddOwnedForm(form);
                form.ShowDialog();
                if(!form.hasil.ContainsKey("Kode Supplier")) {
                    return;
                }

                String strngKodeSupplier = form.hasil["Kode Supplier"];
                String strngKotaSupplier = form.hasil["Kota"];

                txtSupplier.Text = strngKodeSupplier;

                DataSupplier dSupplier = new DataSupplier(command, strngKodeSupplier);
                txtNama.EditValue = dSupplier.nama;
                txtKota.Text = strngKotaSupplier;
                txtAlamat.Text = dSupplier.alamat;
                txtTelepon.Text = dSupplier.telp;
                txtEmail.Text = dSupplier.email;

                if(dSupplier.pajak == Constants.STATUS_YA) {
                    rdoNonPPN.Checked = true;
                } else {
                    rdoExcludePPN.Checked = true;
                }

                // set tanggal jatuh tempo
                updateJatuhTempo();

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
                dxValidationProvider1.SetValidationRule(deTanggal, OswValidation.IsNotBlank());
                dxValidationProvider1.SetValidationRule(deTanggalJatuhTempo, OswValidation.IsNotBlank());
                dxValidationProvider1.SetValidationRule(txtSupplier, OswValidation.IsNotBlank());
                dxValidationProvider1.SetValidationRule(txtFakturSupplier, OswValidation.IsNotBlank());
                dxValidationProvider1.SetValidationRule(txtDiskon, OswValidation.IsNotBlank());

                if(!dxValidationProvider1.Validate()) {
                    foreach(Control x in dxValidationProvider1.GetInvalidControls()) {
                        dxValidationProvider1.SetIconAlignment(x, ErrorIconAlignment.MiddleRight);
                    }
                    return;
                }

                String strngKode = txtKode.Text;
                String strngTanggal = deTanggal.Text;
                String strngSupplier = txtSupplier.Text;
                String strngFakturSupplier = txtFakturSupplier.Text;
                String strngTanggalJatuhTempo = deTanggalJatuhTempo.Text;
                String strngFakturPajak = txtFakturPajak.Text;
                String strngCatatan = txtCatatan.Text;
                String strngPesananPembelian = txtPesananPembelian.Text;
                String strngDiskonFooter = txtDiskon.EditValue.ToString();
                String strngJenisPPN = "";
                if(rdoExcludePPN.Checked) {
                    strngJenisPPN = Constants.JENIS_PPN_EXCLUDE_PPN;
                } else if(rdoIncludePPN.Checked) {
                    strngJenisPPN = Constants.JENIS_PPN_INCLUDE_PPN;
                } else if(rdoNonPPN.Checked) {
                    strngJenisPPN = Constants.JENIS_PPN_NON_PPN;
                }

                String strngEkspedisi = cmbEkspedisi.EditValue == null ? "" : cmbEkspedisi.EditValue.ToString();
                String strngBiayaEkspedisi = txtBiayaEkspedisi.EditValue.ToString();

                DataFakturPembelian dFakturPembelian = new DataFakturPembelian(command, strngKode);
                dFakturPembelian.tanggal = strngTanggal;
                dFakturPembelian.supplier = strngSupplier;
                dFakturPembelian.faktursupplier = strngFakturSupplier;
                dFakturPembelian.pesananpembelian = strngPesananPembelian;
                dFakturPembelian.jatuhtempo = strngTanggalJatuhTempo;
                dFakturPembelian.jenisppn = strngJenisPPN;
                dFakturPembelian.nofakturpajak = strngFakturPajak;
                dFakturPembelian.catatan = strngCatatan;
                dFakturPembelian.diskon = strngDiskonFooter;
                dFakturPembelian.ekspedisi = strngEkspedisi;
                dFakturPembelian.biayaekspedisi = strngBiayaEkspedisi;

                if(this.isAdd) {
                    dFakturPembelian.total = "0";
                    dFakturPembelian.diskonnilai = "0";
                    dFakturPembelian.totaldpp = "0";
                    dFakturPembelian.totalppn = "0";
                    dFakturPembelian.grandtotal = "0";
                    dFakturPembelian.totalretur = "0";
                    dFakturPembelian.totalbayar = "0";
                    dFakturPembelian.status = Constants.STATUS_FAKTUR_PEMBELIAN_BELUM_LUNAS;
                    dFakturPembelian.tambah();
                    // update kode header --> setelah generate
                    strngKode = dFakturPembelian.kode;
                } else {
                    dFakturPembelian.hapusDetail();
                    dFakturPembelian.ubah();
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

                    String strngPesananPembelianDetail = gridView.GetRowCellValue(i, "Pesanan Pembelian Detail").ToString();
                    String strngPesananPembelianDetailNo = gridView.GetRowCellValue(i, "Pesanan Pembelian Detail No").ToString();

                    double dblJumlah = Tools.getRoundMoney(double.Parse(gridView.GetRowCellValue(i, "Jumlah").ToString()));
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
                    DataFakturPembelianDetail dFakturPembelianDetail = new DataFakturPembelianDetail(command, strngKode, strngNo);
                    dFakturPembelianDetail.barang = strngKodeBarang;
                    dFakturPembelianDetail.satuan = strngKodeSatuan;
                    dFakturPembelianDetail.jumlah = dblJumlah.ToString();
                    dFakturPembelianDetail.jumlahretur = "0";
                    dFakturPembelianDetail.hargabeli = dblHargaBeli.ToString();
                    dFakturPembelianDetail.diskonitempersen = dblDiskonItemPersen.ToString();
                    dFakturPembelianDetail.diskonitem = dblDiskonItem.ToString();
                    dFakturPembelianDetail.diskonfaktur = dblDiskonFaktur.ToString();
                    dFakturPembelianDetail.dpp = dblDPP.ToString();
                    dFakturPembelianDetail.ppn = dblPPN.ToString();
                    dFakturPembelianDetail.subtotal = dblSubtotal.ToString();
                    dFakturPembelianDetail.pesananpembeliandetail = strngPesananPembelianDetail;
                    dFakturPembelianDetail.pesananpembeliandetailno = strngPesananPembelianDetailNo;
                    dFakturPembelianDetail.tambah();

                    // tulis log detail
                    OswLog.setTransaksi(command, dokumenDetail, dFakturPembelianDetail.ToString());
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

                double dblPembulatan = dblGrandTotal - (dblTotalDPP + dblTotalPPN);

                dFakturPembelian = new DataFakturPembelian(command, strngKode);
                dFakturPembelian.total = dblTotal.ToString();
                dFakturPembelian.diskonnilai = dblTotalDiskonFaktur.ToString();
                dFakturPembelian.totaldiskon = dblTotalDiskon.ToString();
                dFakturPembelian.totaldpp = dblTotalDPP.ToString();
                dFakturPembelian.totalppn = dblTotalPPN.ToString();
                dFakturPembelian.pembulatan = dblPembulatan.ToString();
                dFakturPembelian.grandtotal = dblGrandTotal.ToString();
                dFakturPembelian.ubah();

                // validasi setelah simpan
                dFakturPembelian.valJumlahDetail();
                dFakturPembelian.prosesJurnal();
                dFakturPembelian.updateMutasi();

                // tulis log
                OswLog.setTransaksi(command, dokumen, dFakturPembelian.ToString());

                // reload grid di form header
                FrmFakturPembelian frmFakturPembelian = (FrmFakturPembelian)this.Owner;
                frmFakturPembelian.setGrid(command);

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
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["No Part"], "");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Kategori"], "");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Satuan"], "");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Jumlah"], "0");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Harga Beli"], "0");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Disk(%)"], "0");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Diskon"], "0");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Subtotal"], "0");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Pesanan Pembelian Detail"], "");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Pesanan Pembelian Detail No"], "0");
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

                    double dblJumlah = Tools.getRoundMoney(double.Parse(gridView.GetRowCellValue(i, "Jumlah").ToString()));
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

                    gridView.SetRowCellValue(i, gridView.Columns["Diskon Faktur"], dblDiskonFaktur.ToString());
                    gridView.SetRowCellValue(i, gridView.Columns["DPP"], dblDPP.ToString());
                    gridView.SetRowCellValue(i, gridView.Columns["PPN"], dblPPN.ToString());
                    gridView.SetRowCellValue(i, gridView.Columns["Diskon"], dblDiskonItem.ToString());
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

        private void btnCariPesananPembelian_Click(object sender, EventArgs e) {
            infoPesananPembelian();
        }

        private void infoPesananPembelian() {
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
                String strngSupplier = txtSupplier.Text;
                String query = @"SELECT A.kode AS Nomor, A.tanggal AS Tanggal, B.nama AS Supplier, C.nama AS Kota, A.jenisppn AS 'Jenis PPN', A.catatan AS Catatan, A.grandtotal AS Total, A.status AS Status, A.diskon AS 'Diskon Faktur Persentase'
                            FROM pesananpembelian A
                            INNER JOIN supplier B ON A.supplier = B.kode
                            INNER JOIN kota C ON B.kota = C.kode
                            WHERE A.supplier = @supplier AND A.status = @status
                            ORDER BY A.kode";

                Dictionary<String, String> parameters = new Dictionary<String, String>();
                parameters.Add("status", Constants.STATUS_PESANAN_PEMBELIAN_DALAM_PROSES);
                parameters.Add("supplier", strngSupplier);

                InfUtamaDictionary form = new InfUtamaDictionary("Info Pesanan Pembelian", query, parameters,
                                                                new String[] { "Nomor", "Jenis PPN", "Diskon Faktur Persentase" },
                                                                new String[] { "Diskon Faktur Persentase" },
                                                                new String[] { });
                this.AddOwnedForm(form);
                form.ShowDialog();
                if(!form.hasil.ContainsKey("Nomor")) {
                    return;
                }

                String strngPesananPembelian = form.hasil["Nomor"];
                String strngJenisPPN = form.hasil["Jenis PPN"];
                String strngDiskonFakturPersentase = form.hasil["Diskon Faktur Persentase"];

                txtPesananPembelian.Text = strngPesananPembelian;
                txtDiskon.EditValue = strngDiskonFakturPersentase;

                if(strngJenisPPN == Constants.JENIS_PPN_NON_PPN) {
                    rdoNonPPN.Checked = true;
                } else if(strngJenisPPN == Constants.JENIS_PPN_EXCLUDE_PPN) {
                    rdoExcludePPN.Checked = true;
                } else if(strngJenisPPN == Constants.JENIS_PPN_INCLUDE_PPN) {
                    rdoIncludePPN.Checked = true;
                }

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

        private void rdoExcludePPN_CheckedChanged(object sender, EventArgs e) {
            setFooter();
        }

        private void rdoIncludePPN_CheckedChanged(object sender, EventArgs e) {
            setFooter();
        }

        private void rdoNonPPN_CheckedChanged(object sender, EventArgs e) {
            setFooter();
        }

        private void btnHapusPesananPembelian_Click(object sender, EventArgs e) {
            if(txtPesananPembelian.Text == "") {
                return;
            }

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
                String strngPesananPembelian = "";
                String strngJenisPPN = Constants.JENIS_PPN_EXCLUDE_PPN;
                String strngDiskonFakturPersentase = "0";

                txtPesananPembelian.Text = strngPesananPembelian;
                txtDiskon.EditValue = strngDiskonFakturPersentase;

                if(strngJenisPPN == Constants.JENIS_PPN_NON_PPN) {
                    rdoNonPPN.Checked = true;
                } else if(strngJenisPPN == Constants.JENIS_PPN_EXCLUDE_PPN) {
                    rdoExcludePPN.Checked = true;
                } else if(strngJenisPPN == Constants.JENIS_PPN_INCLUDE_PPN) {
                    rdoIncludePPN.Checked = true;
                }

                rdoExcludePPN.Checked = true;

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

        private void btnSimpanEkspedisi_Click(object sender, EventArgs e) {
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
                String strngKode = txtKode.Text;

                String strngEkspedisi = cmbEkspedisi.EditValue == null ? "" : cmbEkspedisi.EditValue.ToString();
                String strngBiayaEkspedisi = txtBiayaEkspedisi.EditValue.ToString();

                DataFakturPembelian dFakturPembelian = new DataFakturPembelian(command, strngKode);
                dFakturPembelian.ekspedisi = strngEkspedisi;
                dFakturPembelian.biayaekspedisi = strngBiayaEkspedisi;
                dFakturPembelian.ubahEkspedisi();
                dFakturPembelian.prosesJurnal();

                // tulis log
                OswLog.setTransaksi(command, dokumen, dFakturPembelian.ToString());

                // reload grid di form header
                FrmFakturPembelian frmFakturPembelian = (FrmFakturPembelian)this.Owner;
                frmFakturPembelian.setGrid(command);

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

        private void deTanggal_EditValueChanged(object sender, EventArgs e) {
            updateJatuhTempo();
        }
    }
}