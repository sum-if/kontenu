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
    public partial class FrmFakturPenjualanAdd : DevExpress.XtraEditors.XtraForm {
        private String id = "FAKTURPENJUALAN";
        private String dokumen = "FAKTURPENJUALAN";
        private String dokumenDetail = "FAKTURPENJUALAN";
        private Boolean isAdd;
        private String jenisPesananPenjualan = "";

        public FrmFakturPenjualanAdd(bool pIsAdd) {
            isAdd = pIsAdd;
            InitializeComponent();
        }

        private void FrmFakturPenjualanAdd_Load(object sender, EventArgs e) {
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

                cmbEkspedisi = ComboQueryUmum.getEkspedisi(cmbEkspedisi, command);

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
                deTanggalJatuhTempo.Enabled = false;
                btnCariPesanan.Enabled = false;

                // data
                DataFakturPenjualan dFakturPenjualan = new DataFakturPenjualan(command, strngKode);
                deTanggal.DateTime = OswDate.getDateTimeFromStringTanggal(dFakturPenjualan.tanggal);
                deTanggalJatuhTempo.DateTime = OswDate.getDateTimeFromStringTanggal(dFakturPenjualan.tanggaljatuhtempo);
                txtCatatan.Text = dFakturPenjualan.catatan;
                txtDiskon.EditValue = dFakturPenjualan.diskon;
                txtNoPesanan.Text = dFakturPenjualan.pesananpenjualan;
                txtNoFaktur.Text = dFakturPenjualan.nofakturpajak;
                cmbEkspedisi.EditValue = dFakturPenjualan.ekspedisi;
                chkGunggung.Checked = dFakturPenjualan.gunggung == Constants.STATUS_YA;

                jenisPesananPenjualan = dFakturPenjualan.jenispesananpenjualan;

                if(dFakturPenjualan.status == Constants.STATUS_FAKTUR_PEMBELIAN_LUNAS) {
                    btnSimpan.Enabled = false;
                }

                if(dFakturPenjualan.jenispesananpenjualan == Constants.JENIS_PESANAN_PENJUALAN_PESANAN_PENJUALAN) {
                    DataPesananPenjualan dPesananPenjualan = new DataPesananPenjualan(command, dFakturPenjualan.pesananpenjualan);
                    DataSales dSales = new DataSales(command, dPesananPenjualan.sales);
                    txtSales.Text = dSales.nama;
                } else if(dFakturPenjualan.jenispesananpenjualan == Constants.JENIS_PESANAN_PENJUALAN_BACK_ORDER) {
                    DataBackOrder dBackOrder = new DataBackOrder(command, dFakturPenjualan.pesananpenjualan);
                    DataSales dSales = new DataSales(command, dBackOrder.sales);
                    txtSales.Text = dSales.nama;
                }

                DataCustomer dCustomer = new DataCustomer(command, dFakturPenjualan.customer);
                DataKota dKota = new DataKota(command, dCustomer.kota);

                txtCustomer.Text = dFakturPenjualan.customer;
                txtNama.EditValue = dCustomer.nama;
                txtAlamat.Text = dCustomer.alamat;
                txtTelepon.Text = dCustomer.telp;
                txtEmail.Text = dCustomer.email;
                txtKota.Text = dKota.nama;

                rdoExcludePPN.Checked = dFakturPenjualan.jenisppn == Constants.JENIS_PPN_EXCLUDE_PPN;
                rdoIncludePPN.Checked = dFakturPenjualan.jenisppn == Constants.JENIS_PPN_INCLUDE_PPN;
                rdoNonPPN.Checked = dFakturPenjualan.jenisppn == Constants.JENIS_PPN_NON_PPN;
                btnSimpanGunggung.Enabled = true;
            } else {
                OswControlDefaultProperties.resetAllInput(this);
                deTanggalJatuhTempo.Enabled = true;
                rdoExcludePPN.Checked = true;
                chkGunggung.Checked = false;
                cmbEkspedisi.EditValue = OswCombo.getFirstEditValue(cmbEkspedisi);
                btnCetak.Enabled = false;
                btnCetakSuratJalan.Enabled = false;
                btnSimpanGunggung.Enabled = false;
            }

            this.setGridBarang(command);
            txtKode.Focus();
        }

        public void setGridBarang(MySqlCommand command) {
            String strngKode = txtKode.Text;
            String query = @"SELECT B.no AS No, B.barang AS 'Kode Barang', C.nama AS 'Nama Barang', C.nopart AS 'No Part',
                                    E.kode AS 'Kode Satuan',E.nama AS Satuan, B.jumlahfaktur AS 'Jumlah', B.hargajual AS 'Harga Jual', B.diskonitempersen AS 'Disk(%)', B.diskonitem AS Diskon, B.subtotal AS Subtotal,B.hpp AS HPP,
                                    B.pesananpenjualandetail AS 'Pesanan Penjualan Detail', B.pesananpenjualandetailno  AS 'Pesanan Penjualan Detail No', B.colli AS Colli
                             FROM fakturpenjualan A
                             INNER JOIN fakturpenjualandetail B ON A.kode = B.fakturpenjualan
                             INNER JOIN barang C ON B.barang = C.kode
                             INNER JOIN barangsatuan E ON B.satuan = E.kode
                             WHERE A.kode = @kode
                             ORDER BY B.no";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("Kode", strngKode);

            Dictionary<String, int> widths = new Dictionary<String, int>();
            // 960 - 21 (kiri) - 17 (vertikal lines) - 35 (No) = 887
            widths.Add("No", 35);
            widths.Add("Kode Barang", 100);
            widths.Add("Nama Barang", 190);
            widths.Add("No Part", 90);
            widths.Add("Satuan", 80);
            widths.Add("Jumlah", 70);
            widths.Add("Harga Jual", 80);
            widths.Add("Disk(%)", 50);
            widths.Add("Diskon", 70);
            widths.Add("Subtotal", 80);
            widths.Add("Colli", 77);

            Dictionary<String, String> inputType = new Dictionary<string, string>();
            inputType.Add("Jumlah", OswInputType.NUMBER);
            inputType.Add("Disk(%)", OswInputType.NUMBER);
            inputType.Add("Diskon", OswInputType.NUMBER);
            inputType.Add("Subtotal", OswInputType.NUMBER);
            inputType.Add("Harga Jual", OswInputType.NUMBER);

            OswGrid.getGridInput(gridControl1, command, query, parameters, widths, inputType,
                                 new String[] { "HPP", "Pesanan Penjualan Detail", "Pesanan Penjualan Detail No", "Kode Satuan" },
                                 new String[] { "Kode Satuan", "No", "Nama Barang", "Satuan", "Subtotal", "No Part", "Diskon" });

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
                String strngKodePesananPenjualan = txtNoPesanan.Text;

                String query = "";
                if(jenisPesananPenjualan == Constants.JENIS_PESANAN_PENJUALAN_PESANAN_PENJUALAN) {
                    query = @"SELECT B.no AS 'No', C.kode AS 'Kode', C.nama AS 'Nama', C.nopart AS 'No Part', DD.nama AS Rak,D.kode AS 'Kode Satuan',D.nama AS Satuan, 
                                        COALESCE(F.jumlah,0) AS Stok, COALESCE(E.nilai,0) AS 'Modal', C.hargajual1 AS 'Jual 1', C.hargajual2 AS 'Jual 2',
                                        getTanggalTerakhirBarangCustomer(A.customer, B.barang) AS 'Tanggal Terakhir Customer', getHargaTerakhirBarangCustomer(A.customer, B.barang) AS 'Terakhir Customer',
                                        B.jumlahpesan AS 'Jumlah Pesan', B.jumlahfaktur AS 'Jumlah Faktur', B.jumlahpesan - B.jumlahfaktur - B.jumlahbackorder AS 'Sisa Faktur', 
                                        B.hargajual AS 'Pesanan', B.diskonitempersen AS 'Disk(%)',B.diskonitem AS 'Diskon', 
                                        B.pesananpenjualan AS 'Pesanan Penjualan',B.no AS 'Pesanan Penjualan No'
                                    FROM pesananpenjualan A
                                    INNER JOIN pesananpenjualandetail B ON A.kode = B.pesananpenjualan
                                    INNER JOIN barang C ON B.barang = C.kode
                                    INNER JOIN barangsatuan D ON B.satuan = D.kode
                                    INNER JOIN barangrak DD ON C.rak = DD.kode
                                    LEFT JOIN saldopersediaanhpp E ON B.barang = E.barang
                                    LEFT JOIN saldopersediaanaktual F ON B.barang = F.barang
                                    WHERE A.kode = @pesananpenjualan AND B.jumlahpesan > B.jumlahfaktur + B.jumlahbackorder
                                    ORDER BY A.kode";
                } else if(jenisPesananPenjualan == Constants.JENIS_PESANAN_PENJUALAN_BACK_ORDER) {
                    query = @"SELECT B.no AS 'No', C.kode AS 'Kode', C.nama AS 'Nama', C.nopart AS 'No Part', DD.nama AS Rak,D.kode AS 'Kode Satuan',D.nama AS Satuan, 
                                        COALESCE(F.jumlah,0) AS Stok, COALESCE(E.nilai,0) AS 'Modal', C.hargajual1 AS 'Jual 1', C.hargajual2 AS 'Jual 2',
                                        getTanggalTerakhirBarangCustomer(A.customer, B.barang) AS 'Tanggal Terakhir Customer', getHargaTerakhirBarangCustomer(A.customer, B.barang) AS 'Terakhir Customer',
                                        B.jumlahpesan AS 'Jumlah Pesan', B.jumlahfaktur AS 'Jumlah Faktur', B.jumlahpesan - B.jumlahfaktur AS 'Sisa Faktur', 
                                        B.hargajual AS 'Pesanan', B.diskonitempersen AS 'Disk(%)', B.diskonitem AS 'Diskon', 
                                        B.backorder AS 'Pesanan Penjualan',B.no AS 'Pesanan Penjualan No'
                                    FROM backorder A
                                    INNER JOIN backorderdetail B ON A.kode = B.backorder
                                    INNER JOIN barang C ON B.barang = C.kode
                                    INNER JOIN barangsatuan D ON B.satuan = D.kode
                                    INNER JOIN barangrak DD ON C.rak = DD.kode
                                    LEFT JOIN saldopersediaanhpp E ON B.barang = E.barang
                                    LEFT JOIN saldopersediaanaktual F ON B.barang = F.barang
                                    WHERE A.kode = @pesananpenjualan AND B.jumlahpesan > B.jumlahfaktur
                                    ORDER BY A.kode";
                }


                Dictionary<String, String> parameters = new Dictionary<String, String>();
                parameters.Add("pesananpenjualan", strngKodePesananPenjualan);

                InfUtamaDataTable form = new InfUtamaDataTable("Info Barang", query, parameters,
                                                                 new String[] { "No", "Kode Satuan", "Pesanan Penjualan", "Pesanan Penjualan No", "Sisa Faktur" },
                                                                 new String[] { "Pesanan", "Jumlah Pesan", "Jumlah Faktur", "Disk(%)", "Diskon", "Stok", "Modal", "Jual 1", "Jual 2", "Terakhir Customer" },
                                                                 new DataTable());
                this.AddOwnedForm(form);
                form.ShowDialog();

                if(form.hasil.Rows.Count == 0) {
                    return;
                }

                GridView gridView = gridView1;

                foreach(DataRow row in form.hasil.Rows) {
                    String strngKode = row["Kode"].ToString();
                    String strngNama = row["Nama"].ToString();
                    String strngNoPart = row["No Part"].ToString();
                    String strngKodeSatuan = row["Kode Satuan"].ToString();
                    String strngSatuan = row["Satuan"].ToString();
                    String strngHargaJual = row["Pesanan"].ToString();
                    String strngDiskonPersen = row["Disk(%)"].ToString();
                    String strngDiskon = row["Diskon"].ToString();
                    String strngHPP = row["Modal"].ToString();
                    String strngSisaFaktur = row["Sisa Faktur"].ToString();
                    String strngPesananPenjualan = row["Pesanan Penjualan"].ToString();
                    String strngPesananPenjualanNo = row["Pesanan Penjualan No"].ToString();

                    gridView.AddNewRow();
                    gridView.MoveLast();
                    gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Kode Barang"], strngKode);
                    gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Nama Barang"], strngNama);
                    gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["No Part"], strngNoPart);
                    gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Kode Satuan"], strngKodeSatuan);
                    gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Satuan"], strngSatuan);
                    gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Harga Jual"], strngHargaJual);
                    gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Disk(%)"], strngDiskonPersen);
                    gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Diskon"], strngDiskon);
                    gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["HPP"], strngHPP);
                    gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Jumlah"], strngSisaFaktur);
                    gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Pesanan Penjualan Detail"], strngPesananPenjualan);
                    gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Pesanan Penjualan Detail No"], strngPesananPenjualanNo);

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
                dxValidationProvider1.SetValidationRule(txtNoPesanan, OswValidation.IsNotBlank());
                dxValidationProvider1.SetValidationRule(txtCustomer, OswValidation.IsNotBlank());
                dxValidationProvider1.SetValidationRule(cmbEkspedisi, OswValidation.IsNotBlank());

                if(!dxValidationProvider1.Validate()) {
                    foreach(Control x in dxValidationProvider1.GetInvalidControls()) {
                        dxValidationProvider1.SetIconAlignment(x, ErrorIconAlignment.MiddleRight);
                    }
                    return;
                }

                String strngKode = txtKode.Text;
                String strngTanggal = deTanggal.Text;
                String strngCustomer = txtCustomer.Text;
                String strngPesananPenjualan = txtNoPesanan.Text;
                String strngTanggalJatuhTempo = deTanggalJatuhTempo.Text;
                String strngJenisPPN = "";
                if(rdoExcludePPN.Checked) {
                    strngJenisPPN = Constants.JENIS_PPN_EXCLUDE_PPN;
                } else if(rdoIncludePPN.Checked) {
                    strngJenisPPN = Constants.JENIS_PPN_INCLUDE_PPN;
                } else if(rdoNonPPN.Checked) {
                    strngJenisPPN = Constants.JENIS_PPN_NON_PPN;
                }
                String strngCatatan = txtCatatan.Text;
                String strngDiskonFooter = txtDiskon.EditValue.ToString();
                String strngEkspedisi = cmbEkspedisi.EditValue.ToString();
                String strngGunggung = chkGunggung.Checked ? Constants.STATUS_YA : Constants.STATUS_TIDAK;

                DataFakturPenjualan dFakturPenjualan = new DataFakturPenjualan(command, strngKode);
                dFakturPenjualan.tanggal = strngTanggal;
                dFakturPenjualan.customer = strngCustomer;
                dFakturPenjualan.jenispesananpenjualan = jenisPesananPenjualan;
                dFakturPenjualan.pesananpenjualan = strngPesananPenjualan;
                dFakturPenjualan.tanggaljatuhtempo = strngTanggalJatuhTempo;
                dFakturPenjualan.jenisppn = strngJenisPPN;
                dFakturPenjualan.catatan = strngCatatan;
                dFakturPenjualan.diskon = strngDiskonFooter;
                dFakturPenjualan.nofakturpajak = txtNoFaktur.Text;
                dFakturPenjualan.ekspedisi = strngEkspedisi;
                dFakturPenjualan.gunggung = strngGunggung;

                if(this.isAdd) {
                    dFakturPenjualan.total = "0";
                    dFakturPenjualan.diskonnilai = "0";
                    dFakturPenjualan.totaldpp = "0";
                    dFakturPenjualan.totalppn = "0";
                    dFakturPenjualan.grandtotal = "0";
                    dFakturPenjualan.totalbayar = "0";
                    dFakturPenjualan.totalretur = "0";
                    dFakturPenjualan.status = Constants.STATUS_FAKTUR_PENJUALAN_BELUM_LUNAS;
                    dFakturPenjualan.tambah();
                    // update kode header --> setelah generate
                    strngKode = dFakturPenjualan.kode;
                } else {
                    dFakturPenjualan.hapusDetail();
                    dFakturPenjualan.ubah();
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
                    String strngColli = gridView.GetRowCellValue(i, "Colli").ToString();

                    String strngPesananPenjualanDetail = gridView.GetRowCellValue(i, "Pesanan Penjualan Detail").ToString();
                    String strngPesananPenjualanDetailNo = gridView.GetRowCellValue(i, "Pesanan Penjualan Detail No").ToString();

                    double dblHPP = Tools.getRoundMoney(double.Parse(gridView.GetRowCellValue(i, "HPP").ToString()));
                    double dblJumlah = Tools.getRoundMoney(double.Parse(gridView.GetRowCellValue(i, "Jumlah").ToString()));
                    double dblHargaJual = Tools.getRoundMoney(double.Parse(gridView.GetRowCellValue(i, "Harga Jual").ToString()));
                    double dblDiskonItemPersen = Tools.getRoundMoney(double.Parse(gridView.GetRowCellValue(i, "Disk(%)").ToString()));
                    double dblDiskonItem = Tools.getRoundMoney((dblHargaJual * dblDiskonItemPersen) / 100);

                    double dblHargaBersihItem = Tools.getRoundMoney(dblHargaJual - dblDiskonItem);
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
                    DataFakturPenjualanDetail dFakturPenjualanDetail = new DataFakturPenjualanDetail(command, strngKode, strngNo);
                    dFakturPenjualanDetail.barang = strngKodeBarang;
                    dFakturPenjualanDetail.satuan = strngKodeSatuan;
                    dFakturPenjualanDetail.jumlahfaktur = dblJumlah.ToString();
                    dFakturPenjualanDetail.jumlahretur = "0";
                    dFakturPenjualanDetail.hargajual = dblHargaJual.ToString();
                    dFakturPenjualanDetail.diskonitempersen = dblDiskonItemPersen.ToString();
                    dFakturPenjualanDetail.diskonitem = dblDiskonItem.ToString();
                    dFakturPenjualanDetail.diskonfaktur = dblDiskonFaktur.ToString();
                    dFakturPenjualanDetail.hpp = dblHPP.ToString();
                    dFakturPenjualanDetail.dpp = dblDPP.ToString();
                    dFakturPenjualanDetail.ppn = dblPPN.ToString();
                    dFakturPenjualanDetail.subtotal = dblSubtotal.ToString();
                    dFakturPenjualanDetail.pesananpenjualandetail = strngPesananPenjualanDetail;
                    dFakturPenjualanDetail.pesananpenjualandetailno = strngPesananPenjualanDetailNo;
                    dFakturPenjualanDetail.colli = strngColli;
                    dFakturPenjualanDetail.tambah();

                    // tulis log detail
                    OswLog.setTransaksi(command, dokumenDetail, dFakturPenjualanDetail.ToString());
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

                dFakturPenjualan = new DataFakturPenjualan(command, strngKode);
                dFakturPenjualan.total = dblTotal.ToString();
                dFakturPenjualan.diskonnilai = dblTotalDiskonFaktur.ToString();
                dFakturPenjualan.totaldiskon = dblTotalDiskon.ToString();
                dFakturPenjualan.totaldpp = dblTotalDPP.ToString();
                dFakturPenjualan.totalppn = dblTotalPPN.ToString();
                dFakturPenjualan.grandtotal = dblGrandTotal.ToString();
                dFakturPenjualan.ubah();

                // jurnal
                dFakturPenjualan.prosesJurnal();
                dFakturPenjualan.updateMutasi();

                // tulis log
                OswLog.setTransaksi(command, dokumen, dFakturPenjualan.ToString());

                // reload grid di form header
                FrmFakturPenjualan frmFakturPenjualan = (FrmFakturPenjualan)this.Owner;
                frmFakturPenjualan.setGrid(command);

                // Commit Transaction
                command.Transaction.Commit();

                OswPesan.pesanInfo("Proses " + dokumen + " berhasil.");

                if(this.isAdd) {
                    if(OswPesan.pesanKonfirmasi("Konfirmasi", "Cetak Faktur Penjualan?") == DialogResult.Yes) {
                        cetak(strngKode);
                    }

                    if(OswPesan.pesanKonfirmasi("Konfirmasi", "Cetak Surat Jalan?") == DialogResult.Yes) {
                        cetakSuratJalan(strngKode);
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

            if(gridView.FocusedColumn.FieldName != "Kode Barang" && gridView.GetRowCellValue(gridView.FocusedRowHandle, "Kode Barang").ToString() == "") {
                gridView.FocusedColumn = gridView.Columns["Kode Barang"];
                return;
            }

            setFooter();
        }

        private void gridView1_InitNewRow(object sender, InitNewRowEventArgs e) {
            GridView gridView = sender as GridView;

            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["No"], gridView.DataRowCount + 1);
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Kode Barang"], "");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Nama Barang"], "");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["No Part"], "");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Kode Satuan"], "");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Satuan"], "");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Jumlah"], "0");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Harga Jual"], "0");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Disk(%)"], "0");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Diskon"], "0");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Subtotal"], "0");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["HPP"], "0");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Pesanan Penjualan Detail"], "");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Pesanan Penjualan Detail No"], "0");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Colli"], "");
        }

        private void gridView1_RowDeleted(object sender, DevExpress.Data.RowDeletedEventArgs e) {
            GridView gridView = sender as GridView;
            for(int no = 1; no <= gridView.DataRowCount; no++) {
                gridView.SetRowCellValue(no - 1, "No", no);
            }

            setFooter();
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
                    double dblHargaJual = Tools.getRoundMoney(double.Parse(gridView.GetRowCellValue(i, "Harga Jual").ToString()));
                    double dblDiskonItemPersen = Tools.getRoundMoney(double.Parse(gridView.GetRowCellValue(i, "Disk(%)").ToString()));
                    double dblDiskonItem = Tools.getRoundMoney((dblHargaJual * dblDiskonItemPersen) / 100);

                    double dblHargaBersihItem = Tools.getRoundMoney(dblHargaJual - dblDiskonItem);
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

        private void btnCariPesanan_Click(object sender, EventArgs e) {
            infoPesananPenjualan();
            for(int i = 0; i < gridView1.DataRowCount; i++) {
                gridView1.FocusedRowHandle = i;
            }
        }

        private void infoPesananPenjualan() {
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
                String query = @"SELECT *
                                FROM (
	                                SELECT 10 AS Urutan, @jenisPesananPenjualanPesananPenjualan AS 'Jenis Pesanan', A.kode AS 'No Pesanan', A.tanggal AS Tanggal, D.nama AS 'Customer', E.nama AS Kota, D.alamat AS 'Alamat', A.catatan AS Catatan, A.grandtotal AS 'Grand Total'
	                                FROM pesananpenjualan A
	                                INNER JOIN customer D ON A.customer = D.kode
	                                INNER JOIN kota E ON D.kota = E.kode
	                                WHERE A.status = @status
	                                UNION
	                                SELECT 20 AS Urutan, @jenisPesananPenjualanBackOrder AS 'Jenis Pesanan', A.kode AS 'No Pesanan', A.tanggal AS Tanggal, D.nama AS 'Customer', E.nama AS Kota, D.alamat AS 'Alamat', A.catatan AS Catatan, A.grandtotal AS 'Grand Total'
	                                FROM backorder A
	                                INNER JOIN customer D ON A.customer = D.kode
	                                INNER JOIN kota E ON D.kota = E.kode
	                                WHERE A.status = @status
                                ) Z
                                ORDER BY Z.Urutan, Z.`No Pesanan`";

                Dictionary<String, String> parameters = new Dictionary<String, String>();
                parameters.Add("jenisPesananPenjualanPesananPenjualan", Constants.JENIS_PESANAN_PENJUALAN_PESANAN_PENJUALAN);
                parameters.Add("jenisPesananPenjualanBackOrder", Constants.JENIS_PESANAN_PENJUALAN_BACK_ORDER);
                parameters.Add("status", Constants.STATUS_PESANAN_PENJUALAN_DALAM_PROSES);

                InfUtamaDictionary form = new InfUtamaDictionary("Info Pesanan Penjualan", query, parameters,
                                                                new String[] { "No Pesanan", "Jenis Pesanan" },
                                                                new String[] { "Urutan" },
                                                                new String[] { "Grand Total" });
                this.AddOwnedForm(form);
                form.ShowDialog();
                if(!form.hasil.ContainsKey("No Pesanan")) {
                    return;
                }

                String strngKodePesananPenjualan = form.hasil["No Pesanan"];
                jenisPesananPenjualan = form.hasil["Jenis Pesanan"];

                string strngDiskonFooter = "";
                string strngKodeSales = "";
                string strngKodeCustomer = "";
                string strngJenisPPN = "";

                if(jenisPesananPenjualan == Constants.JENIS_PESANAN_PENJUALAN_PESANAN_PENJUALAN) {
                    DataPesananPenjualan dPesananPenjualan = new DataPesananPenjualan(command, strngKodePesananPenjualan);
                    strngDiskonFooter = dPesananPenjualan.diskon;
                    strngKodeSales = dPesananPenjualan.sales;
                    strngKodeCustomer = dPesananPenjualan.customer;
                    strngJenisPPN = dPesananPenjualan.jenisppn;
                } else if(jenisPesananPenjualan == Constants.JENIS_PESANAN_PENJUALAN_BACK_ORDER) {
                    DataBackOrder dBackOrder = new DataBackOrder(command, strngKodePesananPenjualan);
                    strngDiskonFooter = dBackOrder.diskon;
                    strngKodeSales = dBackOrder.sales;
                    strngKodeCustomer = dBackOrder.customer;
                    strngJenisPPN = dBackOrder.jenisppn;
                }

                txtNoPesanan.Text = strngKodePesananPenjualan;
                txtDiskon.EditValue = strngDiskonFooter;

                DataSales dSales = new DataSales(command, strngKodeSales);
                txtSales.Text = dSales.nama;

                DataCustomer dCustomer = new DataCustomer(command, strngKodeCustomer);
                chkGunggung.Checked = dCustomer.gunggung == Constants.STATUS_YA;

                DataKota dKota = new DataKota(command, dCustomer.kota);

                txtCustomer.EditValue = dCustomer.kode;
                txtNama.EditValue = dCustomer.nama;
                txtAlamat.Text = dCustomer.alamat;
                txtTelepon.Text = dCustomer.telp;
                txtEmail.Text = dCustomer.email;
                txtKota.Text = dKota.nama;

                if(strngJenisPPN == Constants.JENIS_PPN_NON_PPN) {
                    rdoNonPPN.Checked = true;
                } else if(strngJenisPPN == Constants.JENIS_PPN_EXCLUDE_PPN) {
                    rdoExcludePPN.Checked = true;
                } else if(strngJenisPPN == Constants.JENIS_PPN_INCLUDE_PPN) {
                    rdoIncludePPN.Checked = true;
                }

                updateJatuhTempo();

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

        private void btnCetak_Click(object sender, EventArgs e) {
            String strngKode = txtKode.Text;
            cetak(strngKode);
        }

        private void cetak(string kode) {
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
                RptCetakFakturPenjualan report = new RptCetakFakturPenjualan();

                DataOswPerusahaan dPerusahaan = new DataOswPerusahaan(command);
                report.Parameters["perusahaanNama"].Value = dPerusahaan.nama;
                report.Parameters["perusahaanAlamat"].Value = dPerusahaan.alamat;
                report.Parameters["perusahaanTelepon"].Value = dPerusahaan.telp;
                report.Parameters["perusahaanEmail"].Value = dPerusahaan.email;
                report.Parameters["perusahaanNPWP"].Value = dPerusahaan.npwp;

                DataOswSetting dSetting = new DataOswSetting(command, "footeratasnama");
                report.Parameters["footerAtasNama"].Value = dSetting.isi;

                dSetting = new DataOswSetting(command, "footernorek");
                report.Parameters["footerNorek"].Value = dSetting.isi;

                DataFakturPenjualan dFakturPenjualan = new DataFakturPenjualan(command, kode);
                String sales = "";
                String ekspedisi = "";

                if(dFakturPenjualan.jenispesananpenjualan == Constants.JENIS_PESANAN_PENJUALAN_PESANAN_PENJUALAN) {
                    DataPesananPenjualan dPesananPenjualan = new DataPesananPenjualan(command, dFakturPenjualan.pesananpenjualan);
                    DataSales dSales = new DataSales(command, dPesananPenjualan.sales);
                    sales = dSales.nama;
                } else if(dFakturPenjualan.jenispesananpenjualan == Constants.JENIS_PESANAN_PENJUALAN_BACK_ORDER) {
                    DataBackOrder dBackOrder = new DataBackOrder(command, dFakturPenjualan.pesananpenjualan);
                    DataSales dSales = new DataSales(command, dBackOrder.sales);
                    sales = dSales.nama;
                }

                DataEkspedisi dEkspedisi = new DataEkspedisi(command, dFakturPenjualan.ekspedisi);
                ekspedisi = dEkspedisi.nama;

                report.Parameters["fakturNomor"].Value = kode;
                report.Parameters["fakturTanggal"].Value = dFakturPenjualan.tanggal;
                report.Parameters["fakturSales"].Value = sales;
                report.Parameters["fakturEkspedisi"].Value = ekspedisi;
                report.Parameters["fakturJatuhTempo"].Value = dFakturPenjualan.tanggaljatuhtempo;
                report.Parameters["fakturPPN"].Value = Convert.ToDouble(dFakturPenjualan.totalppn);
                report.Parameters["fakturDiskon"].Value = Convert.ToDouble(dFakturPenjualan.diskonnilai);
                report.Parameters["fakturDiskonPersen"].Value = "DISKON (" + dFakturPenjualan.diskon + "%)";
                report.Parameters["fakturTotal"].Value = Convert.ToDouble(dFakturPenjualan.total);
                report.Parameters["fakturGrandTotal"].Value = Tools.getRoundMoney(Convert.ToDouble(dFakturPenjualan.grandtotal));
                report.Parameters["fakturTerbilang"].Value = OswMath.Terbilang(Tools.getRoundMoney(Convert.ToDouble(dFakturPenjualan.grandtotal)));

                DataCustomer dCustomer = new DataCustomer(command, dFakturPenjualan.customer);
                DataKota dKota = new DataKota(command, dCustomer.kota);

                report.Parameters["customerNama"].Value = dCustomer.nama;
                report.Parameters["customerAlamat"].Value = dCustomer.alamat;
                report.Parameters["customerTelepon"].Value = dCustomer.telp;
                report.Parameters["customerKota"].Value = dKota.nama;
                report.Parameters["customerNPWP"].Value = dCustomer.npwpkode;



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

        private void deTanggal_EditValueChanged(object sender, EventArgs e) {
            updateJatuhTempo();
        }

        private void updateJatuhTempo() {
            String strngCustomer = txtCustomer.Text;

            if(strngCustomer == "") {
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
                String strngTanggal = deTanggal.Text;
                DataCustomer dCustomer = new DataCustomer(command, strngCustomer);
                deTanggalJatuhTempo.DateTime = OswDate.getDateTimeTanggalTambahHari(strngTanggal, int.Parse(dCustomer.maksjatuhtempo));

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

        private void btnCetakSuratJalan_Click(object sender, EventArgs e) {
            String kode = txtKode.Text;
            cetakSuratJalan(kode);
        }

        private void cetakSuratJalan(string kode) {
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
                RptCetakPengirimanPenjualan report = new RptCetakPengirimanPenjualan();

                DataOswPerusahaan dPerusahaan = new DataOswPerusahaan(command);
                report.Parameters["perusahaanNama"].Value = dPerusahaan.nama;
                report.Parameters["perusahaanAlamat"].Value = dPerusahaan.alamat;
                report.Parameters["perusahaanTelepon"].Value = dPerusahaan.telp;
                report.Parameters["perusahaanEmail"].Value = dPerusahaan.email;
                report.Parameters["perusahaanNPWP"].Value = dPerusahaan.npwp;

                DataOswSetting dSetting = new DataOswSetting(command, "footeratasnama");
                report.Parameters["footerAtasNama"].Value = dSetting.isi;

                dSetting = new DataOswSetting(command, "footernorek");
                report.Parameters["footerNorek"].Value = dSetting.isi;

                DataFakturPenjualan dFakturPenjualan = new DataFakturPenjualan(command, kode);
                report.Parameters["pengirimanNomor"].Value = kode;
                report.Parameters["pengirimanTanggal"].Value = dFakturPenjualan.tanggal;
                report.Parameters["pengirimanCatatan"].Value = dFakturPenjualan.catatan;

                DataCustomer dCustomer = new DataCustomer(command, dFakturPenjualan.customer);
                DataKota dKota = new DataKota(command, dCustomer.kota);

                report.Parameters["customerNama"].Value = dCustomer.alias == "" ? dCustomer.nama : dCustomer.alias;
                report.Parameters["customerAlamat"].Value = dCustomer.alamatalias == "" ? dCustomer.alamat : dCustomer.alamatalias;
                report.Parameters["customerTelepon"].Value = dCustomer.telp;
                report.Parameters["customerKota"].Value = dKota.nama;
                report.Parameters["customerNPWP"].Value = dCustomer.npwpkode;

                DataEkspedisi dEkspedisi = new DataEkspedisi(command, dFakturPenjualan.ekspedisi);
                report.Parameters["ekspedisiNama"].Value = dEkspedisi.nama;

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

        private void rdoExcludePPN_CheckedChanged(object sender, EventArgs e) {
            setFooter();
        }

        private void rdoIncludePPN_CheckedChanged(object sender, EventArgs e) {
            setFooter();
        }

        private void rdoNonPPN_CheckedChanged(object sender, EventArgs e) {
            setFooter();
        }

        private void btnSimpanGunggung_Click(object sender, EventArgs e) {
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

                String strngKode = txtKode.Text;
                String strngGunggung = chkGunggung.Checked ? Constants.STATUS_YA : Constants.STATUS_TIDAK;

                DataFakturPenjualan dFakturPenjualan = new DataFakturPenjualan(command, strngKode);
                dFakturPenjualan.gunggung = strngGunggung;
                dFakturPenjualan.ubahStatusGunggung();

                // tulis log
                OswLog.setTransaksi(command, dokumen, dFakturPenjualan.ToString());

                // reload grid di form header
                FrmFakturPenjualan frmFakturPenjualan = (FrmFakturPenjualan)this.Owner;
                frmFakturPenjualan.setGrid(command);

                // Commit Transaction
                command.Transaction.Commit();

                OswPesan.pesanInfo("Proses ubah status gunggung berhasil.");
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