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
    public partial class FrmReturPenjualanAdd : DevExpress.XtraEditors.XtraForm {
        private String id = "RETURPENJUALAN";
        private String dokumen = "RETURPENJUALAN";
        private String dokumenDetail = "RETURPENJUALAN";
        private Boolean isAdd;
        private double dblTotalFaktur;
        private double dblSisaBayar;
        private double dblKembaliPiutang;
        private double dblKembaliUangTitipan;

        public FrmReturPenjualanAdd(bool pIsAdd) {
            isAdd = pIsAdd;
            InitializeComponent();
        }

        private void FrmReturPenjualanAdd_Load(object sender, EventArgs e) {
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
                btnCariFaktur.Enabled = false;
                txtCatatan.Enabled = false;
                txtEfakturRetur.Enabled = false;
                btnCetakBuktiPelaporan.Enabled = true;
                btnCetakCSV.Enabled = true;

                // data
                DataReturPenjualan dReturPenjualan = new DataReturPenjualan(command, strngKode);
                deTanggal.DateTime = OswDate.getDateTimeFromStringTanggal(dReturPenjualan.tanggal);
                txtFakturPenjualan.Text = dReturPenjualan.fakturpenjualan;
                txtCustomer.Text = dReturPenjualan.customer;
                txtDiskon.EditValue = dReturPenjualan.diskon;
                txtCatatan.Text = dReturPenjualan.catatan;
                txtEfakturRetur.Text = dReturPenjualan.nofakturpajak;

                DataCustomer dCustomer = new DataCustomer(command, dReturPenjualan.customer);
                txtNama.EditValue = dCustomer.nama;
                txtAlamat.Text = dCustomer.alamat;
                txtTelepon.Text = dCustomer.telp;
                txtEmail.Text = dCustomer.email;

                rdoExcludePPN.Checked = dReturPenjualan.jenisppn == Constants.JENIS_PPN_EXCLUDE_PPN;
                rdoIncludePPN.Checked = dReturPenjualan.jenisppn == Constants.JENIS_PPN_INCLUDE_PPN;
                rdoNonPPN.Checked = dReturPenjualan.jenisppn == Constants.JENIS_PPN_NON_PPN;

                dblTotalFaktur = double.Parse(dReturPenjualan.totalfaktur);
                dblSisaBayar = double.Parse(dReturPenjualan.sisabayar);

                this.setGridBarang(command);
            } else {
                OswControlDefaultProperties.resetAllInput(this);
                btnCetakBuktiPelaporan.Enabled = false;
                btnCetakCSV.Enabled = false;
                rdoExcludePPN.Checked = true;

                dblTotalFaktur = 0;
                dblSisaBayar = 0;
                dblKembaliPiutang = 0;
                dblKembaliUangTitipan = 0;

                this.setGridBarang(command);
            }

            txtKode.Focus();
        }

        public void setGridBarang(MySqlCommand command) {
            String strngKode = txtKode.Text;

            String query = @"SELECT B.no AS No, B.barang AS 'Kode Barang', C.nama AS 'Nama Barang', C.nopart AS 'No Part', F.nama AS Rak,
                                    B.jumlahretur AS 'Jumlah Retur', D.kode AS 'Kode Satuan',D.nama AS Satuan, B.hargajual AS 'Harga Jual', 
                                    B.diskonitempersen AS 'Disk(%)',B.diskonitem AS Diskon, B.subtotal AS Subtotal, B.hpp AS HPP, 
                                    B.fakturpenjualandetail AS 'Faktur Penjualan Detail',B.fakturpenjualandetailno AS 'Faktur Penjualan Detail No'
                             FROM returpenjualan A
                             INNER JOIN returpenjualandetail B ON A.kode = B.returpenjualan
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
            widths.Add("Kode Barang", 105);
            widths.Add("Nama Barang", 187);
            widths.Add("No Part", 90);
            widths.Add("Rak", 70);
            widths.Add("Jumlah Retur", 75);
            widths.Add("Satuan", 70);
            widths.Add("Harga Jual", 80);
            widths.Add("Disk(%)", 50);
            widths.Add("Diskon", 80);
            widths.Add("Subtotal", 80);

            Dictionary<String, String> inputType = new Dictionary<string, string>();
            inputType.Add("Jumlah Retur", OswInputType.NUMBER);
            inputType.Add("Disk(%)", OswInputType.NUMBER);
            inputType.Add("Diskon", OswInputType.NUMBER);
            inputType.Add("Subtotal", OswInputType.NUMBER);
            inputType.Add("Harga Jual", OswInputType.NUMBER);

            OswGrid.getGridInput(gridControl1, command, query, parameters, widths, inputType,
                                 new String[] { "HPP", "Faktur Penjualan Detail", "Faktur Penjualan Detail No", "Kode Satuan" },
                                 new String[] { "No", "Nama Barang", "Satuan", "Subtotal", "No Part", "Rak", "Harga Jual", "Diskon","Disk(%)" });

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
                String strngFakturPenjualan = txtFakturPenjualan.Text;

                String query = @"SELECT B.barang AS 'Kode Barang', C.nama AS 'Nama Barang', C.nopart AS 'No Part', F.nama AS Rak,
                                    B.jumlahfaktur AS 'Jumlah Faktur', B.jumlahretur AS 'Jumlah Retur', E.kode AS 'Kode Satuan', E.nama AS 'Satuan', 
	                                B.hargajual AS 'Harga Jual', B.diskonitempersen AS 'Disk(%)',B.diskonitem AS Diskon, B.hpp AS HPP,
                                    B.fakturpenjualan AS 'Faktur Penjualan Detail',B.no AS 'Faktur Penjualan Detail No'
                                FROM fakturpenjualan A
                                INNER JOIN fakturpenjualandetail B ON A.kode = B.fakturpenjualan
                                INNER JOIN barang C ON B.barang = C.kode
                                INNER JOIN barangsatuan E ON B.satuan = E.kode
                                INNER JOIN barangrak F ON C.rak = F.kode
                                WHERE A.kode = @fakturpenjualan AND B.jumlahfaktur > B.jumlahretur
                                ORDER BY A.kode, C.nama;";

                Dictionary<String, String> parameters = new Dictionary<String, String>();
                parameters.Add("fakturpenjualan", strngFakturPenjualan);

                InfUtamaDataTable form = new InfUtamaDataTable("Info Barang", query, parameters,
                                                                 new String[] { "Faktur Penjualan Detail", "Faktur Penjualan Detail No", "HPP", "Kode Satuan" },
                                                                 new String[] { "Harga Jual", "Jumlah Faktur", "Jumlah Retur", "Disk(%)", "Diskon" },
                                                                 new DataTable());
                this.AddOwnedForm(form);
                form.ShowDialog();

                if(form.hasil.Rows.Count == 0) {
                    return;
                }

                GridView gridView = gridView1;

                foreach(DataRow row in form.hasil.Rows) {
                    String strngKode = row["Kode Barang"].ToString();
                    String strngNama = row["Nama Barang"].ToString();
                    String strngKodeSatuan = row["Kode Satuan"].ToString();
                    String strngSatuan = row["Satuan"].ToString();
                    String strngHargaJual = row["Harga Jual"].ToString();
                    String strngDiskonItemPersen = row["Disk(%)"].ToString();
                    String strngDiskonItem = row["Diskon"].ToString();
                    String strngFakturPenjualanDetail = row["Faktur Penjualan Detail"].ToString();
                    String strngFakturPenjualanDetailNo = row["Faktur Penjualan Detail No"].ToString();
                    String strngHPP = row["HPP"].ToString();

                    gridView.AddNewRow();
                    gridView.MoveLast();
                    gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Kode Barang"], strngKode);
                    gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Nama Barang"], strngNama);
                    gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Kode Satuan"], strngKodeSatuan);
                    gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Satuan"], strngSatuan);
                    gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Harga Jual"], strngHargaJual);
                    gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Diskon Persen"], strngDiskonItemPersen);
                    gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Diskon"], strngDiskonItem);
                    gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Faktur Penjualan Detail"], strngFakturPenjualanDetail);
                    gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Faktur Penjualan Detail No"], strngFakturPenjualanDetailNo);
                    gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["HPP"], strngHPP);

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
                dxValidationProvider1.SetValidationRule(txtCustomer, OswValidation.IsNotBlank());
                dxValidationProvider1.SetValidationRule(txtFakturPenjualan, OswValidation.IsNotBlank());

                if(!dxValidationProvider1.Validate()) {
                    foreach(Control x in dxValidationProvider1.GetInvalidControls()) {
                        dxValidationProvider1.SetIconAlignment(x, ErrorIconAlignment.MiddleRight);
                    }
                    return;
                }

                String strngKode = txtKode.Text;
                String strngTanggal = deTanggal.Text;
                String strngCustomer = txtCustomer.Text;
                String strngFakturPenjualan = txtFakturPenjualan.Text;
                String strngCatatan = txtCatatan.Text;
                String strngDiskonFooter = txtDiskon.EditValue.ToString();
                String strngEfaktur = txtEfakturRetur.Text;
                String strngJenisPPN = "";
                if(rdoExcludePPN.Checked) {
                    strngJenisPPN = Constants.JENIS_PPN_EXCLUDE_PPN;
                } else if(rdoIncludePPN.Checked) {
                    strngJenisPPN = Constants.JENIS_PPN_INCLUDE_PPN;
                } else if(rdoNonPPN.Checked) {
                    strngJenisPPN = Constants.JENIS_PPN_NON_PPN;
                }

                DataReturPenjualan dReturPenjualan = new DataReturPenjualan(command, strngKode);
                dReturPenjualan.tanggal = strngTanggal;
                dReturPenjualan.customer = strngCustomer;
                dReturPenjualan.fakturpenjualan = strngFakturPenjualan;
                dReturPenjualan.diskon = strngDiskonFooter;
                dReturPenjualan.catatan = strngCatatan;
                dReturPenjualan.nofakturpajak = strngEfaktur;
                dReturPenjualan.jenisppn = strngJenisPPN;

                if(this.isAdd) {
                    dReturPenjualan.total = "0";
                    dReturPenjualan.diskonnilai = "0";
                    dReturPenjualan.totaldpp = "0";
                    dReturPenjualan.totalppn = "0";
                    dReturPenjualan.grandtotal = "0";
                    dReturPenjualan.totalfaktur = "0";
                    dReturPenjualan.sisabayar = "0";
                    dReturPenjualan.kembalipiutang = "0";
                    dReturPenjualan.kembaliuangtitipan = "0";
                    dReturPenjualan.tambah();
                    // update kode header --> setelah generate
                    strngKode = dReturPenjualan.kode;
                } else {
                    dReturPenjualan.hapusDetail();
                    dReturPenjualan.ubah();
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
                    String strngFakturPenjualanDetail = gridView.GetRowCellValue(i, "Faktur Penjualan Detail").ToString();
                    String strngFakturPenjualanDetailNo = gridView.GetRowCellValue(i, "Faktur Penjualan Detail No").ToString();

                    double dblJumlah = Tools.getRoundMoney(double.Parse(gridView.GetRowCellValue(i, "Jumlah Retur").ToString()));
                    double dblHargaJual = Tools.getRoundMoney(double.Parse(gridView.GetRowCellValue(i, "Harga Jual").ToString()));
                    double dblDiskonItemPersen = Tools.getRoundMoney(double.Parse(gridView.GetRowCellValue(i, "Disk(%)").ToString()));
                    double dblDiskonItem = Tools.getRoundMoney((dblHargaJual * dblDiskonItemPersen) / 100);
                    double dblHPP = Tools.getRoundMoney(double.Parse(gridView.GetRowCellValue(i, "HPP").ToString()));

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
                    DataReturPenjualanDetail dReturPenjualanDetail = new DataReturPenjualanDetail(command, strngKode, strngNo);
                    dReturPenjualanDetail.barang = strngKodeBarang;
                    dReturPenjualanDetail.satuan = strngKodeSatuan;
                    dReturPenjualanDetail.jumlahretur = dblJumlah.ToString();
                    dReturPenjualanDetail.hargajual = dblHargaJual.ToString();
                    dReturPenjualanDetail.diskonitempersen = dblDiskonItemPersen.ToString();
                    dReturPenjualanDetail.diskonitem = dblDiskonItem.ToString();
                    dReturPenjualanDetail.diskonfaktur = dblDiskonFaktur.ToString();
                    dReturPenjualanDetail.hpp = dblHPP.ToString();
                    dReturPenjualanDetail.dpp = dblDPP.ToString();
                    dReturPenjualanDetail.ppn = dblPPN.ToString();
                    dReturPenjualanDetail.subtotal = dblSubtotal.ToString();
                    dReturPenjualanDetail.fakturpenjualandetail = strngFakturPenjualanDetail;
                    dReturPenjualanDetail.fakturpenjualandetailno = strngFakturPenjualanDetailNo;
                    dReturPenjualanDetail.tambah();

                    // tulis log detail
                    OswLog.setTransaksi(command, dokumenDetail, dReturPenjualanDetail.ToString());
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
                dblKembaliPiutang = 0;
                dblKembaliUangTitipan = 0;

                if(dblSisaBayar > 0) {
                    if(dblGrandTotal > dblSisaBayar) {
                        dblKembaliPiutang = dblSisaBayar;
                    } else {
                        dblKembaliPiutang = dblGrandTotal;
                    }
                }

                if(dblGrandTotal > dblSisaBayar) {
                    dblKembaliUangTitipan = Tools.getRoundMoney(dblGrandTotal - dblSisaBayar);
                }

                dReturPenjualan = new DataReturPenjualan(command, strngKode);
                dReturPenjualan.total = dblTotal.ToString();
                dReturPenjualan.diskonnilai = dblTotalDiskonFaktur.ToString();
                dReturPenjualan.totaldiskon = dblTotalDiskon.ToString();
                dReturPenjualan.totaldpp = dblTotalDPP.ToString();
                dReturPenjualan.totalppn = dblTotalPPN.ToString();
                dReturPenjualan.grandtotal = dblGrandTotal.ToString();
                dReturPenjualan.totalfaktur = dblTotalFaktur.ToString();
                dReturPenjualan.sisabayar = dblSisaBayar.ToString();
                dReturPenjualan.kembalipiutang = dblKembaliPiutang.ToString();
                dReturPenjualan.kembaliuangtitipan = dblKembaliUangTitipan.ToString();
                dReturPenjualan.ubah();

                // validasi setelah simpan
                dReturPenjualan.valJumlahDetail();
                dReturPenjualan.prosesJurnal();
                dReturPenjualan.updateMutasi();

                // tulis log
                OswLog.setTransaksi(command, dokumen, dReturPenjualan.ToString());

                // reload grid di form header
                FrmReturPenjualan frmReturPenjualan = (FrmReturPenjualan)this.Owner;
                frmReturPenjualan.setGrid(command);

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
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Rak"], "");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Kode Satuan"], "");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Satuan"], "");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Jumlah Retur"], "0");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Harga Jual"], "0");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Disk(%)"], "0");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Diskon"], "0");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Subtotal"], "0");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["HPP"], "0");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Faktur Penjualan Detail"], "");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Faktur Penjualan Detail No"], "0");
        }

        private void gridView1_RowDeleted(object sender, DevExpress.Data.RowDeletedEventArgs e) {
            GridView gridView = sender as GridView;
            for(int no = 1; no <= gridView.DataRowCount; no++) {
                gridView.SetRowCellValue(no - 1, "No", no);
            }

            setFooter();
        }

        private void gridView1_ValidateRow(object sender, DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs e) {
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

                String strngBarang = gridView.GetRowCellValue(gridView.FocusedRowHandle, "Kode Barang").ToString();

                if(strngBarang == "") {
                    return;
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
                dblKembaliPiutang = 0;
                dblKembaliUangTitipan = 0;

                if(dblSisaBayar > 0) {
                    if(dblGrandTotal > dblSisaBayar) {
                        dblKembaliPiutang = dblSisaBayar;
                    } else {
                        dblKembaliPiutang = dblGrandTotal;
                    }
                }

                if(dblGrandTotal > dblSisaBayar) {
                    dblKembaliUangTitipan = Tools.getRoundMoney(dblGrandTotal - dblSisaBayar);
                }

                lblTotalFaktur.Text = OswConvert.convertToRupiah(dblTotalFaktur);
                lblSisaBayar.Text = OswConvert.convertToRupiah(dblSisaBayar);
                lblPiutang.Text = OswConvert.convertToRupiah(dblKembaliPiutang);
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
                String query = @"SELECT A.kode AS 'Kode Customer',A.nama AS Customer, A.alamat AS Alamat,A.telp AS Telp
                                FROM customer A
                                WHERE A.status = @status
                                ORDER BY A.kode";

                Dictionary<String, String> parameters = new Dictionary<String, String>();
                parameters.Add("status", Constants.STATUS_AKTIF);

                InfUtamaDictionary form = new InfUtamaDictionary("Info Customer", query, parameters,
                                                                new String[] { "Kode Customer" },
                                                                new String[] { "Kode Customer" },
                                                                new String[] { });
                this.AddOwnedForm(form);
                form.ShowDialog();
                if(!form.hasil.ContainsKey("Kode Customer")) {
                    return;
                }

                String strngKodeCustomer = form.hasil["Kode Customer"];
                txtCustomer.Text = strngKodeCustomer;

                DataCustomer dCustomer = new DataCustomer(command, strngKodeCustomer);
                txtNama.EditValue = dCustomer.nama;
                txtAlamat.Text = dCustomer.alamat;
                txtTelepon.Text = dCustomer.telp;
                txtEmail.Text = dCustomer.email;

                txtFakturPenjualan.Text = "";
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

        private void btnCariFaktur_Click(object sender, EventArgs e) {
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
                String strngCustomer = txtCustomer.Text;

                String query = @"SELECT A.tanggal AS 'Tanggal Faktur', A.kode AS 'No. Faktur Penjualan', A.jenisppn AS 'Jenis PPN', A.grandtotal AS 'Total Faktur', A.diskon AS 'Diskon',
                                        CASE WHEN (A.grandtotal - A.totalbayar - A.totalretur) < 0 THEN 0 ELSE (A.grandtotal - A.totalbayar - A.totalretur) END AS 'Sisa Bayar'
                                FROM fakturpenjualan A
                                WHERE A.customer = @customer AND A.grandtotal > A.totalretur
                                ORDER BY A.kode";

                Dictionary<String, String> parameters = new Dictionary<String, String>();
                parameters.Add("customer", strngCustomer);

                InfUtamaDictionary form = new InfUtamaDictionary("Info Faktur Penjualan", query, parameters,
                                                                new String[] { "No. Faktur Penjualan", "Diskon", "Jenis PPN", "Total Faktur", "Sisa Bayar" },
                                                                new String[] { "Diskon", "Jenis PPN" },
                                                                new String[] { "Total Faktur", "Sisa Bayar" });
                this.AddOwnedForm(form);
                form.ShowDialog();
                if(!form.hasil.ContainsKey("No. Faktur Penjualan")) {
                    return;
                }

                String strngKodeFakturPenjualan = form.hasil["No. Faktur Penjualan"];
                String strngDiskon = form.hasil["Diskon"];
                String strngJenisPPN = form.hasil["Jenis PPN"];
                String strngTotalFaktur = form.hasil["Total Faktur"];
                String strngSisaBayar = form.hasil["Sisa Bayar"];

                txtFakturPenjualan.Text = strngKodeFakturPenjualan;
                txtDiskon.Text = strngDiskon;

                rdoExcludePPN.Checked = strngJenisPPN == Constants.JENIS_PPN_EXCLUDE_PPN;
                rdoIncludePPN.Checked = strngJenisPPN == Constants.JENIS_PPN_INCLUDE_PPN;
                rdoNonPPN.Checked = strngJenisPPN == Constants.JENIS_PPN_NON_PPN;

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

        private void btnCetakBuktiPelaporan_Click(object sender, EventArgs e) {
            String strngKodeRetur = txtKode.Text;
            cetakBuktiPelaporan(strngKodeRetur);
        }

        private void btnCetakCSV_Click(object sender, EventArgs e) {
            String strngKodeRetur = txtKode.Text;
            cetakCSV(strngKodeRetur);
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

                RptCetakReturPenjualanBuktiPenjualan report = new RptCetakReturPenjualanBuktiPenjualan();
                DataOswPerusahaan dPerusahaan = new DataOswPerusahaan(command);
                DataReturPenjualan dreturpenjualan = new DataReturPenjualan(command, kode);
                DataFakturPenjualan dFakturPenjualan = new DataFakturPenjualan(command, dreturpenjualan.fakturpenjualan);
                DataCustomer dcustomer = new DataCustomer(command, dreturpenjualan.customer);

                report.Parameters["Kode"].Value = kode;
                report.Parameters["Tanggal"].Value = dreturpenjualan.tanggal;

                report.Parameters["NoFakturPajak"].Value = dreturpenjualan.nofakturpajak;
                report.Parameters["TanggalFakturPajak"].Value = dreturpenjualan.tanggalfakturpajak;

                report.Parameters["PenjualNama"].Value = dPerusahaan.nama;
                report.Parameters["PenjualAlamat"].Value = dPerusahaan.alamat;
                report.Parameters["PenjualNPWP"].Value = dPerusahaan.npwp;

                report.Parameters["PembeliNama"].Value = dcustomer.nama;
                report.Parameters["PembeliAlamat"].Value = dcustomer.alamat;
                report.Parameters["PembeliNPWP"].Value = dcustomer.npwpkode;

                report.Parameters["JumlahHargaJual"].Value = Convert.ToDouble(dreturpenjualan.totaldpp);
                report.Parameters["JumlahPPN"].Value = Convert.ToDouble(dreturpenjualan.totalppn);
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

    }
}