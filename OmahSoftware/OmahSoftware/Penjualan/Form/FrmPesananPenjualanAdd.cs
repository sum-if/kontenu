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
using DevExpress.XtraEditors.Controls;
using OmahSoftware.Penjualan.Laporan;

namespace OmahSoftware.Penjualan {
    public partial class FrmPesananPenjualanAdd : DevExpress.XtraEditors.XtraForm {
        private String id = "PESANANPENJUALAN";
        private String dokumen = "PESANANPENJUALAN";
        private String dokumenDetail = "PESANANPENJUALAN";
        private Boolean isAdd;

        public FrmPesananPenjualanAdd(bool pIsAdd) {
            isAdd = pIsAdd;
            InitializeComponent();
        }

        private void FrmPesananPenjualanAdd_Load(object sender, EventArgs e) {
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
                OswControlDefaultProperties.setAngka(txtDiskon);
                OswControlDefaultProperties.setAngka(txtLimitPiutang);
                cmbSales = ComboQueryUmum.getSales(cmbSales, command);

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
                DataPesananPenjualan dPesananPenjualan = new DataPesananPenjualan(command, strngKode);
                deTanggal.DateTime = OswDate.getDateTimeFromStringTanggal(dPesananPenjualan.tanggal);
                cmbSales.EditValue = dPesananPenjualan.sales;
                txtCustomer.EditValue = dPesananPenjualan.customer;
                txtCatatan.EditValue = dPesananPenjualan.catatan;
                chkTutupPesanan.Checked = dPesananPenjualan.status == Constants.STATUS_PESANAN_PENJUALAN_SELESAI;
                txtDiskon.EditValue = dPesananPenjualan.diskon;

                if(chkTutupPesanan.Checked) {
                    chkTutupPesanan.Enabled = false;
                    btnSimpan.Enabled = false;
                }

                DataCustomer dCustomer = new DataCustomer(command, dPesananPenjualan.customer);
                txtNama.EditValue = dCustomer.nama;
                txtAlamat.Text = dCustomer.alamat;
                txtTelepon.Text = dCustomer.telp;
                txtEmail.Text = dCustomer.email;
                txtLimitPiutang.EditValue = dCustomer.limitpiutang;

                rdoExcludePPN.Checked = dPesananPenjualan.jenisppn == Constants.JENIS_PPN_EXCLUDE_PPN;
                rdoIncludePPN.Checked = dPesananPenjualan.jenisppn == Constants.JENIS_PPN_INCLUDE_PPN;
                rdoNonPPN.Checked = dPesananPenjualan.jenisppn == Constants.JENIS_PPN_NON_PPN;
            } else {
                OswControlDefaultProperties.resetAllInput(this);

                chkTutupPesanan.Enabled = false;
                chkTutupPesanan.Checked = false;

                txtDiskon.EditValue = "0";
                txtLimitPiutang.EditValue = "0";

                cmbSales.EditValue = OswCombo.getFirstEditValue(cmbSales);
                btnCetak.Enabled = false;
            }

            this.setGrid(command);
            txtKode.Focus();
        }

        public void setGrid(MySqlCommand command) {
            String strngKode = txtKode.Text;

            String query = @"SELECT B.no AS No, B.barang AS 'Kode Barang', C.nama AS 'Nama Barang', C.nopart AS 'No Part', F.nama AS Rak,B.jumlahpesan AS Jumlah, B.satuan AS 'Kode Satuan',D.nama AS Satuan, B.hargajual AS 'Harga Jual', 
                                    B.diskonitempersen AS 'Disk(%)',B.diskonitem AS 'Diskon', B.subtotal AS Subtotal
                             FROM pesananpenjualan A
                             INNER JOIN pesananpenjualandetail B ON A.kode = B.pesananpenjualan
                             INNER JOIN barang C ON B.barang = C.kode
                             INNER JOIN barangsatuan D ON B.satuan = D.kode
                             INNER JOIN barangrak F ON C.rak = F.kode
                             WHERE A.kode = @kode
                             ORDER BY B.no";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("Kode", strngKode);

            Dictionary<String, int> widths = new Dictionary<String, int>();
            // 960 - 21 (kiri) - 17 (vertikal lines) - 35 (No) = 887
            widths.Add("No", 35);
            widths.Add("Kode Barang", 90);
            widths.Add("Nama Barang", 230);
            widths.Add("No Part", 100);
            widths.Add("Rak", 67);
            widths.Add("Satuan", 70);
            widths.Add("Jumlah", 60);
            widths.Add("Harga Jual", 70);
            widths.Add("Disk(%)", 50);
            widths.Add("Diskon", 70);
            widths.Add("Subtotal", 80);

            Dictionary<String, String> inputType = new Dictionary<string, string>();
            inputType.Add("Jumlah", OswInputType.NUMBER);
            inputType.Add("Disk(%)", OswInputType.NUMBER);
            inputType.Add("Diskon", OswInputType.NUMBER);
            inputType.Add("Subtotal", OswInputType.NUMBER);
            inputType.Add("Harga Jual", OswInputType.NUMBER);

            OswGrid.getGridInput(gridControl1, command, query, parameters, widths, inputType,
                                 new String[] { "Kode Satuan" },
                                 new String[] { "No", "Nama Barang", "Satuan", "Subtotal", "No Part", "Rak", "Diskon" });

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

                String strngCustomer = txtCustomer.Text;

                String query = @"SELECT A.kode AS Kode, A.nama AS Nama,A.nopart AS 'No Part', F.nama AS Rak, B.kode AS 'Kode Satuan', B.nama AS Satuan, 
                                        COALESCE(E.jumlah,0) AS Stok,COALESCE(G.nilai,0) AS 'Harga Modal', A.hargajual1 AS 'Harga Jual 1', A.hargajual2 AS 'Harga Jual 2', getTanggalTerakhirBarangCustomer(@customer, A.kode) AS 'Tanggal Terakhir Customer', getHargaTerakhirBarangCustomer(@customer, A.kode) AS 'Harga Terakhir Customer'
                                FROM barang A
                                INNER JOIN barangsatuan B ON A.standarsatuan = B.kode
                                INNER JOIN barangrak F ON A.rak = F.kode
                                LEFT JOIN saldopersediaanaktual E ON E.barang = A.kode
                                LEFT JOIN saldopersediaanhpp G ON A.kode = G.barang
                                ORDER BY A.nama,B.nama";

                Dictionary<String, String> parameters = new Dictionary<String, String>();
                parameters.Add("customer", strngCustomer);

                InfUtamaDataTable form = new InfUtamaDataTable("Info Barang", query, parameters,
                                                                new String[] { "Kode Satuan" },
                                                                new String[] { "Stok", "Harga Modal", "Harga Terakhir Customer", "Harga Jual 1", "Harga Jual 2" },
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
                    String strngRak = row["Rak"].ToString();
                    String strngKodeSatuan = row["Kode Satuan"].ToString();
                    String strngSatuan = row["Satuan"].ToString();
                    String strngHargaJual = row["Harga Terakhir Customer"].ToString();

                    gridView.AddNewRow();
                    gridView.MoveLast();
                    gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Kode Barang"], strngKode);
                    gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Nama Barang"], strngNama);
                    gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["No Part"], strngNoPart);
                    gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Rak"], strngRak);
                    gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Kode Satuan"], strngKodeSatuan);
                    gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Satuan"], strngSatuan);
                    gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Harga Jual"], strngHargaJual);
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
                dxValidationProvider1.SetValidationRule(cmbSales, OswValidation.IsNotBlank());
                dxValidationProvider1.SetValidationRule(txtCustomer, OswValidation.IsNotBlank());

                if(!dxValidationProvider1.Validate()) {
                    foreach(Control x in dxValidationProvider1.GetInvalidControls()) {
                        dxValidationProvider1.SetIconAlignment(x, ErrorIconAlignment.MiddleRight);
                    }
                    return;
                }

                String strngKode = txtKode.Text;
                String strngTanggal = deTanggal.Text;
                String strngCustomer = txtCustomer.Text;
                String strngCatatan = txtCatatan.Text;
                String strngTutupPesanan = chkTutupPesanan.Checked ? Constants.STATUS_YA : Constants.STATUS_TIDAK;
                String strngJenisPPN = "";
                if(rdoExcludePPN.Checked) {
                    strngJenisPPN = Constants.JENIS_PPN_EXCLUDE_PPN;
                } else if(rdoIncludePPN.Checked) {
                    strngJenisPPN = Constants.JENIS_PPN_INCLUDE_PPN;
                } else if(rdoNonPPN.Checked) {
                    strngJenisPPN = Constants.JENIS_PPN_NON_PPN;
                }
                String strngDiskonFooter = txtDiskon.EditValue.ToString();
                String strngSales = cmbSales.EditValue.ToString();

                DataPesananPenjualan dPesananPenjualan = new DataPesananPenjualan(command, strngKode);
                dPesananPenjualan.tanggal = strngTanggal;
                dPesananPenjualan.sales = strngSales;
                dPesananPenjualan.customer = strngCustomer;
                dPesananPenjualan.catatan = strngCatatan;
                dPesananPenjualan.jenisppn = strngJenisPPN;
                dPesananPenjualan.diskon = strngDiskonFooter;

                bool simpanDetail = true;

                if(this.isAdd) {
                    dPesananPenjualan.total = "0";
                    dPesananPenjualan.diskonnilai = "0";
                    dPesananPenjualan.totaldpp = "0";
                    dPesananPenjualan.totalppn = "0";
                    dPesananPenjualan.grandtotal = "0";
                    dPesananPenjualan.status = Constants.STATUS_PESANAN_PENJUALAN_DALAM_PROSES;
                    dPesananPenjualan.tambah();
                    // update kode header --> setelah generate
                    strngKode = dPesananPenjualan.kode;
                } else {
                    if(chkTutupPesanan.Checked) {
                        dPesananPenjualan.status = Constants.STATUS_PESANAN_PENJUALAN_SELESAI;
                        dPesananPenjualan.ubahStatus();
                        simpanDetail = false;
                    } else {
                        dPesananPenjualan.hapusDetail();
                        dPesananPenjualan.ubah();
                    }
                }

                // simpan detail
                if(simpanDetail) {
                    // simpan barang
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
                        DataPesananPenjualanDetail dPesananPenjualanDetail = new DataPesananPenjualanDetail(command, strngKode, strngNo);
                        dPesananPenjualanDetail.barang = strngKodeBarang;
                        dPesananPenjualanDetail.satuan = strngKodeSatuan;
                        dPesananPenjualanDetail.jumlahpesan = dblJumlah.ToString();
                        dPesananPenjualanDetail.jumlahfaktur = "0";
                        dPesananPenjualanDetail.hargajual = dblHargaJual.ToString();
                        dPesananPenjualanDetail.diskonitempersen = dblDiskonItemPersen.ToString();
                        dPesananPenjualanDetail.diskonitem = dblDiskonItem.ToString();
                        dPesananPenjualanDetail.diskonfaktur = dblDiskonFaktur.ToString();
                        dPesananPenjualanDetail.dpp = dblDPP.ToString();
                        dPesananPenjualanDetail.ppn = dblPPN.ToString();
                        dPesananPenjualanDetail.subtotal = dblSubtotal.ToString();
                        dPesananPenjualanDetail.tambah();

                        // tulis log detail
                        OswLog.setTransaksi(command, dokumenDetail, dPesananPenjualanDetail.ToString());
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

                    dPesananPenjualan = new DataPesananPenjualan(command, strngKode);
                    dPesananPenjualan.total = dblTotal.ToString();
                    dPesananPenjualan.diskonnilai = dblTotalDiskonFaktur.ToString();
                    dPesananPenjualan.totaldiskon = dblTotalDiskon.ToString();
                    dPesananPenjualan.totaldpp = dblTotalDPP.ToString();
                    dPesananPenjualan.totalppn = dblTotalPPN.ToString();
                    dPesananPenjualan.grandtotal = dblGrandTotal.ToString();
                    dPesananPenjualan.ubah();

                    // validasi setelah simpan                    
                    dPesananPenjualan.valJumlahDetail();
                    dPesananPenjualan.valLimitPiutang();
                }

                // tulis log
                OswLog.setTransaksi(command, dokumen, dPesananPenjualan.ToString());

                // reload grid di form header
                FrmPesananPenjualan frmPesananPenjualan = (FrmPesananPenjualan)this.Owner;
                frmPesananPenjualan.setGrid(command);

                // Commit Transaction
                command.Transaction.Commit();

                OswPesan.pesanInfo("Proses " + dokumen + " berhasil.");

                if(this.isAdd) {
                    if(OswPesan.pesanKonfirmasi("Konfirmasi", "Cetak Pesanan Penjualan?") == DialogResult.Yes) {
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

        private void gridView1_InitNewRow(object sender, InitNewRowEventArgs e) {
            GridView gridView = sender as GridView;

            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["No"], gridView.DataRowCount + 1);
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Kode Barang"], "");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Nama Barang"], "");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Merk"], "");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Satuan"], "");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Jumlah"], "0");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Harga Jual"], "0");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Disk(%)"], "0");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Diskon"], "0");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Subtotal"], "0");
        }

        private void gridView1_RowDeleted(object sender, DevExpress.Data.RowDeletedEventArgs e) {
            GridView gridView = sender as GridView;
            for(int no = 1; no <= gridView.DataRowCount; no++) {
                gridView.SetRowCellValue(no - 1, "No", no);
            }

            setFooter();
        }

        private void gridView1_ValidateRow(object sender, DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs e) {
            GridView gridView = sender as GridView;

            String strngBarang = gridView.GetRowCellValue(gridView.FocusedRowHandle, "Kode Barang").ToString();

            if(strngBarang == "") {
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

                    String strngNo = gridView.GetRowCellValue(i, "No").ToString();
                    String strngKodeBarang = gridView.GetRowCellValue(i, "Kode Barang").ToString();

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
                    gridView.SetRowCellValue(i, gridView.Columns["Diskon"], dblDiskonItem);
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
            infoCustomer();
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
                String query = @"SELECT A.kode AS 'Kode Customer',A.nama AS Customer, A.alamat AS Alamat, A.telp AS Telp, B.nama AS Kota
                                FROM customer A
                                INNER JOIN kota B ON A.kota = B.kode
                                WHERE A.status = @status
                                ORDER BY A.kode";

                Dictionary<String, String> parameters = new Dictionary<String, String>();
                parameters.Add("status", Constants.STATUS_AKTIF);

                InfUtamaDictionary form = new InfUtamaDictionary("Info Customer", query, parameters,
                                                                new String[] { "Kode Customer", "Kota" },
                                                                new String[] { "Kode Customer" },
                                                                new String[] { });
                this.AddOwnedForm(form);
                form.ShowDialog();
                if(!form.hasil.ContainsKey("Kode Customer")) {
                    return;
                }

                String strngKodeCustomer = form.hasil["Kode Customer"];
                String strngKota = form.hasil["Kota"];

                txtCustomer.Text = strngKodeCustomer;

                DataCustomer dCustomer = new DataCustomer(command, strngKodeCustomer);

                txtNama.EditValue = dCustomer.nama;
                txtTelepon.Text = dCustomer.telp;
                txtEmail.Text = dCustomer.email;
                txtAlamat.Text = dCustomer.alamat;
                txtKota.Text = strngKota;

                if(dCustomer.pajak == Constants.STATUS_YA) {
                    rdoNonPPN.Checked = true;
                } else {
                    rdoExcludePPN.Checked = true;
                }
                txtLimitPiutang.EditValue = dCustomer.limitpiutang;

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

        private void cetak(String kode) {
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
                RptCetakPesananPenjualan report = new RptCetakPesananPenjualan();

                DataOswPerusahaan dPerusahaan = new DataOswPerusahaan(command);
                report.Parameters["perusahaanNama"].Value = dPerusahaan.nama;
                report.Parameters["perusahaanAlamat"].Value = dPerusahaan.alamat;
                report.Parameters["perusahaanTelepon"].Value = dPerusahaan.telp;
                report.Parameters["perusahaanEmail"].Value = dPerusahaan.email;
                report.Parameters["perusahaanNPWP"].Value = dPerusahaan.npwp;

                DataPesananPenjualan dPesananPenjualan = new DataPesananPenjualan(command, kode);
                report.Parameters["pesananNomor"].Value = kode;
                report.Parameters["pesananTanggal"].Value = dPesananPenjualan.tanggal;
                report.Parameters["pesananDiskon"].Value = Convert.ToDouble(dPesananPenjualan.diskonnilai);
                report.Parameters["pesananGrandTotal"].Value = Convert.ToDouble(dPesananPenjualan.grandtotal);
                report.Parameters["pesananTerbilang"].Value = OswMath.Terbilang(Convert.ToDouble(dPesananPenjualan.grandtotal));
                report.Parameters["pesananTotal"].Value = Convert.ToDouble(dPesananPenjualan.total);
                report.Parameters["pesananCatatan"].Value = dPesananPenjualan.catatan;

                DataCustomer dCustomer = new DataCustomer(command, dPesananPenjualan.customer);
                DataKota dKota = new DataKota(command, dCustomer.kota);

                report.Parameters["customerNama"].Value = dCustomer.alias == "" ? dCustomer.nama : dCustomer.alias;
                report.Parameters["customerAlamat"].Value = dCustomer.alamatalias == "" ? dCustomer.alamat : dCustomer.alamatalias;
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

        private void rdoExcludePPN_CheckedChanged(object sender, EventArgs e) {
            setFooter();
        }

        private void rdoIncludePPN_CheckedChanged(object sender, EventArgs e) {
            setFooter();
        }

        private void rdoNonPPN_CheckedChanged(object sender, EventArgs e) {
            setFooter();
        }


    }
}