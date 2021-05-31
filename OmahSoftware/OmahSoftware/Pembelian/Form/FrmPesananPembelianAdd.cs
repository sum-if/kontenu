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
using OmahSoftware.Pembelian;
using OmahSoftware.Penjualan;
using DevExpress.XtraEditors.Controls;

namespace OmahSoftware.Pembelian {
    public partial class FrmPesananPembelianAdd : DevExpress.XtraEditors.XtraForm {
        private String id = "PESANANPEMBELIAN";
        private String dokumen = "PESANANPEMBELIAN";
        private String dokumenDetail = "PESANANPEMBELIAN";
        private Boolean isAdd;

        public FrmPesananPembelianAdd(bool pIsAdd) {
            isAdd = pIsAdd;
            InitializeComponent();
        }

        private void FrmPesananPembelianAdd_Load(object sender, EventArgs e) {
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
                OswControlDefaultProperties.setTanggal(deTanggalPesanan);
                OswControlDefaultProperties.setTanggal(deTanggalEstimasi);

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
                txtStatus.Enabled = false;

                // data
                DataPesananPembelian dPesananPembelian = new DataPesananPembelian(command, strngKode);
                deTanggalPesanan.DateTime = OswDate.getDateTimeFromStringTanggal(dPesananPembelian.tanggal);
                deTanggalEstimasi.DateTime = OswDate.getDateTimeFromStringTanggal(dPesananPembelian.tanggalestimasi);

                txtSupplier.Text = dPesananPembelian.supplier;
                txtCatatan.EditValue = dPesananPembelian.catatan;
                chkTutupPesanan.Checked = dPesananPembelian.status == Constants.STATUS_PESANAN_PEMBELIAN_SELESAI;
                txtDiskon.EditValue = dPesananPembelian.diskon;
                txtStatus.EditValue = dPesananPembelian.status;

                if(dPesananPembelian.jenisppn == Constants.JENIS_PPN_NON_PPN) {
                    rdoNonPPN.Checked = true;
                } else if(dPesananPembelian.jenisppn == Constants.JENIS_PPN_INCLUDE_PPN) {
                    rdoIncludePPN.Checked = true;
                } else if(dPesananPembelian.jenisppn == Constants.JENIS_PPN_EXCLUDE_PPN) {
                    rdoExcludePPN.Checked = true;
                }

                if(chkTutupPesanan.Checked) {
                    chkTutupPesanan.Enabled = false;
                    btnSimpan.Enabled = false;
                }

                DataSupplier dSupplier = new DataSupplier(command, dPesananPembelian.supplier);
                DataKota dKota = new DataKota(command, dSupplier.kota);

                txtNama.EditValue = dSupplier.nama;
                txtKota.Text = dKota.nama;
                txtAlamat.Text = dSupplier.alamat;
                txtTelepon.Text = dSupplier.telp;
                txtEmail.Text = dSupplier.email;

                this.setGrid(command);

                // set footer
                lblTotal.Text = OswConvert.convertToRupiah(double.Parse(dPesananPembelian.total));
                lblDiskon.Text = OswConvert.convertToRupiah(double.Parse(dPesananPembelian.diskonnilai));
                lblTotalPPN.Text = OswConvert.convertToRupiah(double.Parse(dPesananPembelian.totalppn));
                lblGrandTotal.Text = OswConvert.convertToRupiah(double.Parse(dPesananPembelian.grandtotal));
            } else {
                OswControlDefaultProperties.resetAllInput(this);
                chkTutupPesanan.Enabled = false;
                chkTutupPesanan.Checked = false;
                rdoExcludePPN.Checked = true;
                this.setGrid(command);

                btnCetak.Enabled = false;
            }

            txtKode.Focus();
        }

        public void setGrid(MySqlCommand command) {
            String strngKode = txtKode.Text;

            String query = @"SELECT B.no AS 'No', C.kode AS 'Kode Barang', C.nama AS 'Nama Barang', C.nopart AS 'No Part', F.nama AS Kategori,E.kode AS 'Kode Satuan', E.nama AS 'Satuan', 
                                B.jumlah AS 'Jumlah', B.hargabeli AS 'Harga Beli', B.diskonitempersen AS 'Disk(%)', B.diskonitem AS 'Diskon', B.subtotal AS 'Subtotal'
                                FROM pesananpembelian A
                                INNER JOIN pesananpembeliandetail B ON A.kode = B.pesananpembelian
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
                                 new String[] { "Kode Satuan" },
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

                String strngKodeSupplier = txtSupplier.Text;

                String query = @"SELECT A.kode AS Kode, A.nama AS Nama, C.nama AS Kategori, A.nopart AS 'No Part', D.nama AS Rak, B.kode AS 'Kode Satuan', B.nama AS Satuan, A.stokminimum AS 'Stok Minimum', COALESCE(E.jumlah,0) AS Stok, A.hargabeliterakhir AS 'Harga Terakhir', getHargaTerakhirBarangSupplier(@supplier, A.kode) AS 'Harga Terakhir Supplier'
                                FROM barang A
                                INNER JOIN barangsatuan B ON A.standarsatuan = B.kode
                                INNER JOIN barangkategori C ON A.barangkategori = C.kode
                                INNER JOIN barangrak D ON A.rak = D.kode
                                LEFT JOIN saldopersediaanaktual E ON E.barang = A.kode
                                WHERE A.status = @status
                                ORDER BY C.nama,A.nama,B.nama";

                Dictionary<String, String> parameters = new Dictionary<String, String>();
                parameters.Add("supplier", strngKodeSupplier);
                parameters.Add("status", Constants.STATUS_AKTIF);

                InfUtamaDataTable form = new InfUtamaDataTable("Info Barang", query, parameters,
                                                                new String[] { "Kode Satuan" },
                                                                new String[] { "Stok", "Stok Minimum", "Harga Terakhir", "Harga Terakhir Supplier" },
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
                    String strngKategori = row["Kategori"].ToString();
                    String strngHargaTerakhir = row["Harga Terakhir"].ToString();

                    gridView.AddNewRow();
                    gridView.MoveLast();
                    gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Kode Barang"], strngKode);
                    gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Nama Barang"], strngNama);
                    gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["No Part"], strngNoPart);
                    gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Kode Satuan"], strngKodeSatuan);
                    gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Satuan"], strngSatuan);
                    gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Kategori"], strngKategori);
                    gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Harga Beli"], strngHargaTerakhir);
                    gridView.UpdateCurrentRow();
                }

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
                dxValidationProvider1.SetValidationRule(deTanggalPesanan, OswValidation.IsNotBlank());
                dxValidationProvider1.SetValidationRule(deTanggalEstimasi, OswValidation.IsNotBlank());
                dxValidationProvider1.SetValidationRule(txtSupplier, OswValidation.IsNotBlank());
                dxValidationProvider1.SetValidationRule(txtDiskon, OswValidation.IsNotBlank());

                if(!dxValidationProvider1.Validate()) {
                    foreach(Control x in dxValidationProvider1.GetInvalidControls()) {
                        dxValidationProvider1.SetIconAlignment(x, ErrorIconAlignment.MiddleRight);
                    }
                    return;
                }

                String strngKode = txtKode.Text;
                String strngTanggalPesanan = deTanggalPesanan.Text;
                String strngTanggalEstimasi = deTanggalEstimasi.Text;
                String strngSupplier = txtSupplier.Text;
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

                DataPesananPembelian dPesananPembelian = new DataPesananPembelian(command, strngKode);
                dPesananPembelian.tanggal = strngTanggalPesanan;
                dPesananPembelian.tanggalestimasi = strngTanggalEstimasi;
                dPesananPembelian.supplier = strngSupplier;
                dPesananPembelian.jenisppn = strngJenisPPN;
                dPesananPembelian.catatan = strngCatatan;
                dPesananPembelian.diskon = strngDiskonFooter;

                bool simpanDetail = true;

                if(this.isAdd) {
                    dPesananPembelian.total = "0";
                    dPesananPembelian.diskonnilai = "0";
                    dPesananPembelian.totaldpp = "0";
                    dPesananPembelian.totalppn = "0";
                    dPesananPembelian.grandtotal = "0";
                    dPesananPembelian.status = Constants.STATUS_PESANAN_PEMBELIAN_DALAM_PROSES;
                    dPesananPembelian.tambah();
                    // update kode header --> setelah generate
                    strngKode = dPesananPembelian.kode;
                } else {
                    if(chkTutupPesanan.Checked) {
                        dPesananPembelian.status = Constants.STATUS_PESANAN_PEMBELIAN_SELESAI;
                        dPesananPembelian.ubahStatus();
                        simpanDetail = false;
                    } else {
                        dPesananPembelian.hapusDetail();
                        dPesananPembelian.ubah();
                    }
                }

                // simpan detail
                if(simpanDetail) {
                    GridView gridView = gridView1;
                    setFooter();

                    double dblTotal = 0;
                    double dblTotalDiskonFaktur = 0;

                    double dblTotalDiskon = 0;
                    double dblTotalPPN = 0;
                    double dblTotalDPP = 0;

                    double dblDiskonFakturPersentase = Tools.getRoundCalc(double.Parse(txtDiskon.EditValue.ToString()));

                    for(int i = 0; i < gridView.DataRowCount; i++) {
                        if(gridView.GetRowCellValue(i, "Kode Barang").ToString() == "") {
                            continue;
                        }

                        String strngNo = gridView.GetRowCellValue(i, "No").ToString();
                        String strngKodeBarang = gridView.GetRowCellValue(i, "Kode Barang").ToString();
                        String strngKodeSatuan = gridView.GetRowCellValue(i, "Kode Satuan").ToString();

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
                        dblTotalDiskonFaktur = Tools.getRoundMoney(dblTotalDiskonFaktur + (dblDiskonFaktur * dblJumlah));

                        dblTotalDiskon = Tools.getRoundMoney(dblTotalDiskon + dblDiskonItem + dblDiskonFaktur);
                        dblTotalDPP = Tools.getRoundMoney(dblTotalDPP + (dblDPP * dblJumlah));
                        dblTotalPPN = Tools.getRoundMoney(dblTotalPPN + (dblPPN * dblJumlah));

                        // simpan detail
                        DataPesananPembelianDetail dPesananPembelianDetail = new DataPesananPembelianDetail(command, strngKode, strngNo);
                        dPesananPembelianDetail.barang = strngKodeBarang;
                        dPesananPembelianDetail.satuan = strngKodeSatuan;
                        dPesananPembelianDetail.jumlah = dblJumlah.ToString();
                        dPesananPembelianDetail.jumlahfaktur = "0";
                        dPesananPembelianDetail.hargabeli = dblHargaBeli.ToString();
                        dPesananPembelianDetail.diskonitempersen = dblDiskonItemPersen.ToString();
                        dPesananPembelianDetail.diskonitem = dblDiskonItem.ToString();
                        dPesananPembelianDetail.diskonfaktur = dblDiskonFaktur.ToString();
                        dPesananPembelianDetail.dpp = dblDPP.ToString();
                        dPesananPembelianDetail.ppn = dblPPN.ToString();
                        dPesananPembelianDetail.subtotal = dblSubtotal.ToString();
                        dPesananPembelianDetail.tambah();

                        // tulis log detail
                        OswLog.setTransaksi(command, dokumenDetail, dPesananPembelianDetail.ToString());
                    }

                    double dblGrandTotal = 0;

                    if(rdoIncludePPN.Checked) {
                        dblGrandTotal = Tools.getRoundMoney(dblTotal - dblTotalDiskonFaktur);
                    } else {
                        dblGrandTotal = Tools.getRoundMoney(dblTotal - dblTotalDiskonFaktur + dblTotalPPN);
                    }

                    dPesananPembelian = new DataPesananPembelian(command, strngKode);
                    dPesananPembelian.total = dblTotal.ToString();
                    dPesananPembelian.diskonnilai = dblTotalDiskonFaktur.ToString();
                    dPesananPembelian.totaldiskon = dblTotalDiskon.ToString();
                    dPesananPembelian.totaldpp = dblTotalDPP.ToString();
                    dPesananPembelian.totalppn = dblTotalPPN.ToString();
                    dPesananPembelian.grandtotal = dblGrandTotal.ToString();
                    dPesananPembelian.ubah();

                    // validasi setelah simpan
                    dPesananPembelian.valJumlahDetail();
                }

                // tulis log
                OswLog.setTransaksi(command, dokumen, dPesananPembelian.ToString());

                // reload grid di form header
                FrmPesananPembelian frmPesananPembelian = (FrmPesananPembelian)this.Owner;
                frmPesananPembelian.setGrid(command);

                // Commit Transaction
                command.Transaction.Commit();

                OswPesan.pesanInfo("Proses " + dokumen + " berhasil.");

                if(this.isAdd) {
                    if(OswPesan.pesanKonfirmasi("Konfirmasi", "Cetak Pesanan Pembelian?") == DialogResult.Yes) {
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

                    gridView.SetRowCellValue(i, gridView.Columns["Diskon Faktur"], dblDiskonFaktur);
                    gridView.SetRowCellValue(i, gridView.Columns["DPP"], dblDPP);
                    gridView.SetRowCellValue(i, gridView.Columns["PPN"], dblPPN);
                    gridView.SetRowCellValue(i, gridView.Columns["Diskon"], dblDiskonItem);
                    gridView.SetRowCellValue(i, gridView.Columns["Subtotal"], dblSubtotal);
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
                                                                new String[] { "Kode Supplier","Kota" },
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
            String strngKodePesanan = txtKode.Text;
            cetak(strngKodePesanan);
        }

        private void gridControl1_ProcessGridKey(object sender, KeyEventArgs e) {
            GridView gridView = gridView1;
            if(e.KeyCode == Keys.F1 && gridView.FocusedColumn.FieldName == "Kode Barang") {
                infoBarang();
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

        private void cetak(String kodePesanan) {
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
                string kode = kodePesanan;

                RptCetakPesananPembelian report = new RptCetakPesananPembelian();

                DataOswPerusahaan dPerusahaan = new DataOswPerusahaan(command);
                report.Parameters["perusahaanNama"].Value = dPerusahaan.nama;
                report.Parameters["perusahaanAlamat"].Value = dPerusahaan.alamat;
                report.Parameters["perusahaanTelepon"].Value = dPerusahaan.telp;
                report.Parameters["perusahaanEmail"].Value = dPerusahaan.email;
                report.Parameters["perusahaanNPWP"].Value = dPerusahaan.npwp;

                DataPesananPembelian dpesananpembelian = new DataPesananPembelian(command, kode);
                report.Parameters["pesananNomor"].Value = kode;
                report.Parameters["pesananTanggal"].Value = dpesananpembelian.tanggal;
                report.Parameters["pesananTanggalEstimasi"].Value = dpesananpembelian.tanggalestimasi;
                report.Parameters["pesananPPN"].Value = Convert.ToDouble(dpesananpembelian.totalppn);
                report.Parameters["pesananDiskon"].Value = Convert.ToDouble(dpesananpembelian.diskonnilai);
                report.Parameters["pesananGrandTotal"].Value = Convert.ToDouble(dpesananpembelian.grandtotal);
                report.Parameters["pesananTerbilang"].Value = OswMath.Terbilang(Convert.ToDouble(dpesananpembelian.grandtotal));
                report.Parameters["pesananCatatan"].Value = dpesananpembelian.catatan;

                DataSupplier dsupplier = new DataSupplier(command, dpesananpembelian.supplier);
                report.Parameters["supplierNama"].Value = dsupplier.nama;
                report.Parameters["supplierAlamat"].Value = dsupplier.alamat;
                report.Parameters["supplierKota"].Value = dsupplier.kota;
                report.Parameters["supplierTelepon"].Value = dsupplier.telp;
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


    }
}