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
using Kontenu.Sistem;
using DevExpress.XtraEditors.DXErrorProvider;
using System.Net;
using Kontenu.OswLib;
using Kontenu.Umum.Laporan;
using Kontenu.Umum;
using DevExpress.XtraEditors.Controls;


using DevExpress.XtraEditors.ViewInfo;
using DevExpress.Utils.Drawing;

namespace Kontenu.Design {
    public partial class FrmQuotationAdd : DevExpress.XtraEditors.XtraForm {
        private String id = "QUOTATION";
        private String dokumen = "QUOTATION";
        private String dokumenDetail = "QUOTATION";
        private Boolean isAdd;
        private Boolean isDashboard;
        public Dictionary<string, DataTable> dicDetailBarang;
        public Dictionary<string, DataTable> dicDetailBiaya;

        public FrmQuotationAdd(bool pIsAdd, bool pIsDashboard = false) {
            isAdd = pIsAdd;
            isDashboard = pIsDashboard;
            InitializeComponent();
        }

        private void FrmQuotationAdd_Load(object sender, EventArgs e) {
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
                OswControlDefaultProperties.setTanggal(deDeadline);

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
            //if(!this.isAdd) {
            //    String strngKode = txtKode.Text;
            //    deTanggal.Enabled = false;
            //    btnCetak.Enabled = true;

            //    // data
            //    DataQuotation dQuotation = new DataQuotation(command, Constants.CABANG, strngKode);
            //    deTanggal.DateTime = OswDate.getDateTimeFromStringTanggal(dQuotation.tanggal);
            //    deDeadline.DateTime = OswDate.getDateTimeFromStringTanggal(dQuotation.deadline);

            //    if(dQuotation.delivery != "") {
            //        deDelivery.DateTime = OswDate.getDateTimeFromStringTanggal(dQuotation.delivery);
            //    }

            //    if(dQuotation.penawaran != "") {
            //        dePenawaran.DateTime = OswDate.getDateTimeFromStringTanggal(dQuotation.penawaran);
            //    }
                
            //    cmbSales.EditValue = dQuotation.sales;
            //    txtKomisi.EditValue = dQuotation.komisi;

            //    txtNama.EditValue = dQuotation.customer;
            //    txtAlamat.Text = dQuotation.alamat;
            //    txtProvinsi.Text = dQuotation.provinsi;
            //    txtKota.Text = dQuotation.kota;
            //    txtKodePos.Text = dQuotation.kodepos;
            //    txtTelepon.Text = dQuotation.telp;
            //    txtTelepon2.Text = dQuotation.telp2;
            //    txtFaks.Text = dQuotation.faks;
            //    txtEmail.Text = dQuotation.email;

            //    txtCatatan.EditValue = dQuotation.catatan;
            //    chkTutup.Checked = dQuotation.status == Constants.STATUS_Quotation_SELESAI;

            //    btnTambahBarang.Enabled = true;
            //    btnUbahBarang.Enabled = true;
            //    btnHapusBarang.Enabled = true;
            //    btnPerincian.Enabled = true;
            //    chkTutup.Enabled = true;

            //    if(chkTutup.Checked) {
            //        chkTutup.Enabled = false;
            //        btnSimpan.Enabled = false;

            //        btnTambahBarang.Enabled = false;
            //        btnUbahBarang.Enabled = false;
            //        btnHapusBarang.Enabled = false;
            //    }
            //} else {
            //    OswControlDefaultProperties.resetAllInput(this);
            //    deDelivery.EditValue = "";
            //    dePenawaran.EditValue = "";
            //    btnTambahBarang.Enabled = false;
            //    btnUbahBarang.Enabled = false;
            //    btnHapusBarang.Enabled = false;
            //    btnPerincian.Enabled = false;

            //    chkTutup.Enabled = false;
            //    chkTutup.Checked = false;

            //    btnCetak.Enabled = false;
            //}

            //if(isDashboard) {
            //    chkTutup.Enabled = false;
            //    btnCariCustomer.Enabled = false;
            //    btnSimpan.Enabled = false;
            //    btnHapusBarang.Enabled = false;
            //    btnTambahBarang.Enabled = false;
            //    btnUbahBarang.Enabled = false;
            //}

            //this.setGrid(command);
            //txtKode.Focus();
        }

        public void setGrid(MySqlCommand command) {
            String strngKode = txtKode.Text;

            String query = @"SELECT B.id AS ID, B.namabarang AS 'Nama Barang', B.jumlah AS Qty, B.satuan AS Satuan, 
	                                B.hargabarang AS 'H. Beli', B.biaya AS 'Biaya', B.profit AS Profit, B.subhargadasar AS 'H. Dasar', B.komisi AS 'CN & Nego',
	                                B.hargapenawaran AS 'H. Penawaran', B.subtotal AS 'Subtotal'
                            FROM jcs A
                            INNER JOIN jcsdetail B ON A.cabang = B.cabang AND A.kode = B.jcs
                            WHERE A.cabang = @cabang AND A.kode = @kode
                            ORDER BY LPAD(B.id,4,'0')";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("kode", strngKode);

            Dictionary<String, int> widths = new Dictionary<String, int>();
            // 960 - 21 (kiri) - 17 (vertikal lines) - 35 (No) = 927
            widths.Add("Nama Barang", 152);
            widths.Add("Qty", 40);
            widths.Add("Satuan", 60);
            widths.Add("H. Beli", 100);
            widths.Add("Biaya", 90);
            widths.Add("Profit", 90);
            widths.Add("H. Dasar", 100);
            widths.Add("CN & Nego", 90);
            widths.Add("H. Penawaran", 100);
            widths.Add("Subtotal", 100);

            Dictionary<String, String> inputType = new Dictionary<string, string>();
            inputType.Add("Nama Barang", OswInputType.TEXTAREA);
            inputType.Add("Qty", OswInputType.NUMBER);
            inputType.Add("H. Beli", OswInputType.NUMBER);
            inputType.Add("Biaya", OswInputType.NUMBER);
            inputType.Add("Profit", OswInputType.NUMBER);
            inputType.Add("H. Dasar", OswInputType.NUMBER);
            inputType.Add("CN & Nego", OswInputType.NUMBER);
            inputType.Add("H. Penawaran", OswInputType.NUMBER);
            inputType.Add("Subtotal", OswInputType.NUMBER);

            OswGrid.getGridInput(gridControl1, command, query, parameters, widths, inputType,
                                 new String[] { "ID" },
                                 new String[] { "Qty", "Satuan", "H. Beli", "Biaya", "Profit", "H. Dasar", "CN & Nego", "H. Penawaran", "Subtotal" },
                                 false);

            // GridView gridView = gridView1;
            // gridView.Columns["Nama Barang"].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;

            //DataQuotation dQuotation = new DataQuotation(command, Constants.CABANG, strngKode);
            //lblTotalBiaya.Text = OswConvert.convertToRupiah(Math.Round(double.Parse(dQuotation.totalbiaya),2));
            //lblTotalProfit.Text = OswConvert.convertToRupiah(Math.Round(double.Parse(dQuotation.totalprofit),2));
            //lblTotalHDasar.Text = OswConvert.convertToRupiah(Math.Round(double.Parse(dQuotation.totalhargadasar),2));
            //lblTotalKomisi.Text = OswConvert.convertToRupiah(Math.Round(double.Parse(dQuotation.totalkomisi), 2));

            //lblTotalDPP.Text = OswConvert.convertToRupiah(double.Parse(dQuotation.dpp));
            //lblTotalPPN.Text = OswConvert.convertToRupiah(double.Parse(dQuotation.ppn));
            //lblGrandTotal.Text = OswConvert.convertToRupiah(double.Parse(dQuotation.grandtotal));
        }

        private void btnSimpan_Click(object sender, EventArgs e) {
            //MySqlConnection con = new MySqlConnection(OswConfig.KONEKSI);
            //MySqlCommand command = con.CreateCommand();
            //MySqlTransaction trans;

            //try {
            //    // buka koneksi
            //    con.Open();

            //    // set transaction
            //    trans = con.BeginTransaction();
            //    command.Transaction = trans;

            //    // Function Code
            //    // validation
            //    dxValidationProvider1.SetValidationRule(deTanggal, OswValidation.IsNotBlank());
            //    dxValidationProvider1.SetValidationRule(deDeadline, OswValidation.IsNotBlank());
            //    dxValidationProvider1.SetValidationRule(cmbSales, OswValidation.IsNotBlank());
            //    dxValidationProvider1.SetValidationRule(txtNama, OswValidation.IsNotBlank());
            //    dxValidationProvider1.SetValidationRule(txtAlamat, OswValidation.IsNotBlank());

            //    if(!dxValidationProvider1.Validate()) {
            //        foreach(Control x in dxValidationProvider1.GetInvalidControls()) {
            //            dxValidationProvider1.SetIconAlignment(x, ErrorIconAlignment.MiddleRight);
            //        }
            //        return;
            //    }

            //    String strngKode = txtKode.Text;
            //    String strngTanggal = deTanggal.Text;
            //    String strngDeadline = deDeadline.Text;
            //    String strngDelivery = deDelivery.Text;
            //    String strngPenawaran = dePenawaran.Text;
            //    String strngCustomer = txtNama.Text;
            //    String strngCatatan = txtCatatan.Text;
            //    String strngTutupPesanan = chkTutup.Checked ? Constants.STATUS_YA : Constants.STATUS_TIDAK;
            //    String strngSales = cmbSales.EditValue.ToString();
            //    String strngKomisi = txtKomisi.EditValue.ToString();
            //    String strngAlamat = txtAlamat.Text;
            //    String strngProvinsi = txtProvinsi.Text;
            //    String strngKota = txtKota.Text;
            //    String strngKodePos = txtKodePos.Text;
            //    String strngTelp = txtTelepon.Text;
            //    String strngTelp2 = txtTelepon2.Text;
            //    String strngFaks = txtFaks.Text;
            //    String strngEmail = txtEmail.Text;

            //    DataQuotation dQuotation = new DataQuotation(command, Constants.CABANG, strngKode);
            //    dQuotation.tanggal = strngTanggal;
            //    dQuotation.deadline = strngDeadline;
            //    dQuotation.delivery = strngDelivery;
            //    dQuotation.penawaran = strngPenawaran;
            //    dQuotation.sales = strngSales;
            //    dQuotation.komisi = strngKomisi;
            //    dQuotation.customer = strngCustomer;
            //    dQuotation.alamat = strngAlamat;
            //    dQuotation.provinsi = strngProvinsi;
            //    dQuotation.kota = strngKota;
            //    dQuotation.kodepos = strngKodePos;
            //    dQuotation.telp = strngTelp;
            //    dQuotation.telp2 = strngTelp2;
            //    dQuotation.faks = strngFaks;
            //    dQuotation.email = strngEmail;
            //    dQuotation.catatan = strngCatatan;

            //    if(this.isAdd) {
            //        dQuotation.status = Constants.STATUS_Quotation_DALAM_PROSES;
            //        dQuotation.tambah();
            //        // update kode header --> setelah generate
            //        strngKode = dQuotation.kode;
            //        txtKode.Text = strngKode;

            //        btnTambahBarang.Enabled = true;
            //        btnUbahBarang.Enabled = true;
            //        btnHapusBarang.Enabled = true;

            //        this.isAdd = false;
            //    } else {
            //        if(chkTutup.Checked) {
            //            dQuotation.status = Constants.STATUS_Quotation_SELESAI;
            //            dQuotation.ubahStatus();
            //        } else {
            //            dQuotation.ubah();
            //        }

            //        dQuotation.prosesHitung();
            //    }

            //    this.setDefaultInput(command);

            //    // tulis log
            //    OswLog.setTransaksi(command, dokumen, dQuotation.ToString());

            //    // reload grid di form header
            //    FrmQuotation frmQuotation = (FrmQuotation)this.Owner;
            //    frmQuotation.setGrid(command);

            //    // Commit Transaction
            //    command.Transaction.Commit();

            //    OswPesan.pesanInfo("Proses simpan header berhasil.");
            //} catch(MySqlException ex) {
            //    OswPesan.pesanErrorCatch(ex, command, dokumen);
            //} catch(Exception ex) {
            //    OswPesan.pesanErrorCatch(ex, command, dokumen);
            //} finally {
            //    con.Close();
            //    try {
            //        SplashScreenManager.CloseForm();
            //    } catch(Exception ex) {
            //    }
            //}
        }

        private void gridView1_DoubleClick(object sender, EventArgs e) {
            btnPerincian.PerformClick();
        }

        private void btnTambahBarang_Click(object sender, EventArgs e) {
            //MySqlConnection con = new MySqlConnection(OswConfig.KONEKSI);
            //MySqlCommand command = con.CreateCommand();
            //MySqlTransaction trans;

            //try {
            //    // buka koneksi
            //    con.Open();

            //    // set transaction
            //    trans = con.BeginTransaction();
            //    command.Transaction = trans;

            //    // Function Code
            //    String jcs = txtKode.Text;
            //    FrmQuotationAddBarang form = new FrmQuotationAddBarang(true, Constants.CABANG, jcs, "");
            //    this.AddOwnedForm(form);
            //    form.ShowDialog();
                
            //    // Commit Transaction
            //    command.Transaction.Commit();
            //} catch(MySqlException ex) {
            //    OswPesan.pesanErrorCatch(ex, command, dokumen);
            //} catch(Exception ex) {
            //    OswPesan.pesanErrorCatch(ex, command, dokumen);
            //} finally {
            //    con.Close();
            //    try {
            //        SplashScreenManager.CloseForm();
            //    } catch(Exception ex) {
            //    }
            //}
        }

        private void btnUbahBarang_Click(object sender, EventArgs e) {
            //if(gridView1.GetSelectedRows().Length == 0) {
            //    OswPesan.pesanError("Silahkan pilih barang yang akan diubah.");
            //    return;
            //}

            //String strngKode = txtKode.Text;
            //String strngID = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "ID").ToString();
            //String strngNama = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Nama Barang").ToString();

            //FrmQuotationAddBarang form = new FrmQuotationAddBarang(false, Constants.CABANG, strngKode, strngID);

            //MySqlConnection con = new MySqlConnection(OswConfig.KONEKSI);
            //MySqlCommand command = con.CreateCommand();
            //MySqlTransaction trans;

            //try {
            //    // buka koneksi
            //    con.Open();

            //    // set transaction
            //    trans = con.BeginTransaction();
            //    command.Transaction = trans;

            //    // Function Code
            //    DataQuotationDetail dQuotationDetail = new DataQuotationDetail(command, Constants.CABANG, strngKode, strngID);
            //    if(!dQuotationDetail.isExist) {
            //        throw new Exception("Barang tidak ditemukan.");
            //    }

            //    this.AddOwnedForm(form);

            //    // Commit Transaction
            //    command.Transaction.Commit();
            //} catch(MySqlException ex) {
            //    OswPesan.pesanErrorCatch(ex, command, dokumen);
            //} catch(Exception ex) {
            //    OswPesan.pesanErrorCatch(ex, command, dokumen);
            //} finally {
            //    con.Close();
            //    try {
            //        SplashScreenManager.CloseForm();
            //    } catch(Exception ex) {
            //    }
            //}

            //form.ShowDialog();
        }

        private void btnHapusBarang_Click(object sender, EventArgs e) {
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
                if(gridView1.GetSelectedRows().Length == 0) {
                    OswPesan.pesanError("Silahkan pilih barang yang akan dihapus.");
                    return;
                }

                if(OswPesan.pesanKonfirmasi("Konfirmasi Hapus", "Apakah anda yakin untuk menghapus barang ini?") == DialogResult.No) {
                    return;
                }

                String strngKode = txtKode.Text;
                String strngID = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "ID").ToString();

                DataQuotationDetail dQuotationDetail = new DataQuotationDetail(command, strngKode, strngID);
                dQuotationDetail.hapus();

                this.setGrid(command);

                OswPesan.pesanInfo("Proses hapus detail berhasil.");

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

        private void btnPerincian_Click(object sender, EventArgs e) {
            //GridView gridView = gridView1;

            //if(gridView.GetRowCellValue(gridView.FocusedRowHandle, "Nama Barang") == null) {
            //    gridView.FocusedColumn = gridView.Columns["Nama Barang"];
            //    return;
            //}

            //if(gridView.GetRowCellValue(gridView.FocusedRowHandle, "Nama Barang").ToString() == "") {
            //    gridView.FocusedColumn = gridView.Columns["Nama Barang"];
            //    return;
            //}

            //String strngKode = txtKode.Text;
            //String strngID = gridView.GetRowCellValue(gridView.FocusedRowHandle, "ID").ToString();
            //String strngNama = gridView.GetRowCellValue(gridView.FocusedRowHandle, "Nama Barang").ToString();
            //bool isEdit = chkTutup.Checked ? false : true;

            //FrmQuotationAddDetail form = new FrmQuotationAddDetail(Constants.CABANG, strngKode, strngID, strngNama, isEdit, isDashboard);
            //this.AddOwnedForm(form);
            //form.ShowDialog();
        }

        private void btnCetak_Click(object sender, EventArgs e) {
            //MySqlConnection con = new MySqlConnection(OswConfig.KONEKSI);
            //MySqlCommand command = con.CreateCommand();
            //MySqlTransaction trans;

            //try {
            //    // buka koneksi
            //    con.Open();

            //    // set transaction
            //    trans = con.BeginTransaction();
            //    command.Transaction = trans;

            //     //Function Code
            //    string kode = txtKode.Text;
            //    RptCetakQuotation report = new RptCetakQuotation();

            //    DataCabang dCabang = new DataCabang(command, Constants.CABANG);
            //    DataOswPerusahaan dPerusahaan = new DataOswPerusahaan(command);
            //    report.Parameters["perusahaanKode"].Value = Constants.CABANG;
            //    report.Parameters["perusahaanLogo"].Value = dPerusahaan.icon;
            //    report.Parameters["perusahaanNama"].Value = dPerusahaan.nama;
            //    report.Parameters["perusahaanAlamat"].Value = dPerusahaan.alamat;
            //    report.Parameters["perusahaanKota"].Value = dPerusahaan.kota;
            //    report.Parameters["perusahaanTelepon"].Value = "Telp " + dPerusahaan.telp + "/ Faks : " + dPerusahaan.faks;
            //    report.Parameters["perusahaanFaks"].Value = dPerusahaan.faks;
            //    report.Parameters["perusahaanEmail"].Value = dPerusahaan.email;
            //    report.Parameters["perusahaanHandphone"].Value = dPerusahaan.handphone;
            //    report.Parameters["perusahaanDetail"].Value = dPerusahaan.alamat + " " + dPerusahaan.kota + "\r\n"
            //                                                    + "Telp. : " + dPerusahaan.telp + "\r\n"
            //                                                    + "Fax. : " + dPerusahaan.faks + "\r\n"
            //                                                    + "Email : " + dPerusahaan.email;
            //    string perusahaanNPWP = "";
            //    perusahaanNPWP = Constants.NPWP;
            //    report.Parameters["perusahaanNPWP"].Value = perusahaanNPWP;

            //    DataOswSetting dSetting = new DataOswSetting(command, "footeratasnama");
            //    report.Parameters["footerAtasNama"].Value = dSetting.isi;

            //    dSetting = new DataOswSetting(command, "footernorek");
            //    report.Parameters["footerNorek"].Value = dSetting.isi;

            //    DataQuotation dQuotation = new DataQuotation(command, Constants.CABANG, kode);                
            //    DataOswUser dUser = new DataOswUser(command, dQuotation.sales);

            //    report.Parameters["pesananNomor"].Value = kode;               
            //    report.Parameters["pesananTanggal"].Value = dQuotation.tanggal;
            //    report.Parameters["pesananTotal"].Value = Convert.ToDouble(dQuotation.dpp);
            //    report.Parameters["pesananPajak"].Value = Convert.ToDouble(dQuotation.ppn);
            //    report.Parameters["pesananGrandTotal"].Value = Convert.ToDouble(dQuotation.grandtotal);                
            //    report.Parameters["pesananTerbilang"].Value = OswMath.Terbilang(Convert.ToDouble(dQuotation.grandtotal)).ToUpper();
            //    report.Parameters["pesananCatatan"].Value = "Delivery Time : " + dQuotation.delivery + "\r\n" + "\r\n"
            //                                                + dQuotation.catatan;
            //    report.Parameters["headerDataCetakan"].Value = "Tanggal \t : " + dQuotation.tanggal + "\n"
            //                                                + "Nomor \t : " + dQuotation.kode + "\n";
            //    report.Parameters["pesananSales"].Value = dUser.nama;                                
            //    report.Parameters["customerNama"].Value = "Kepada Yth, " + "\r\n"
            //                                                    + dQuotation.customer + "\r\n"
            //                                                    + dQuotation.alamat + "\r\n"
            //                                                    + dQuotation.kota + "\r\n"
            //                                                    + dQuotation.provinsi;
            //    report.Parameters["customerAlamat"].Value = dQuotation.alamat;
            //    report.Parameters["customerKota"].Value = dQuotation.kota;
            //    report.Parameters["customerTelepon"].Value = dQuotation.telp;                

            //    // Assign the printing system to the document viewer.
            //    LaporanPrintPreview laporan = new LaporanPrintPreview();
            //    laporan.documentViewer1.DocumentSource = report;

            //    //ReportPrintTool printTool = new ReportPrintTool(report);
            //    //printTool.Print();

            //    OswLog.setLaporan(command, dokumen);

            //    laporan.Show();

            //    // Commit Transaction
            //    command.Transaction.Commit();
            //} catch(MySqlException ex) {
            //    OswPesan.pesanErrorCatch(ex, command, dokumen);
            //} catch(Exception ex) {
            //    OswPesan.pesanErrorCatch(ex, command, dokumen);
            //} finally {
            //    con.Close();
            //    try {
            //        SplashScreenManager.CloseForm();
            //    } catch(Exception ex) {
            //    }
            //}
        }

        private void btnCariCustomer_Click(object sender, EventArgs e) {
            infoCustomer();
        }

        private void infoCustomer() {
//            MySqlConnection con = new MySqlConnection(OswConfig.KONEKSI);
//            MySqlCommand command = con.CreateCommand();
//            MySqlTransaction trans;

//            try {
//                // buka koneksi
//                con.Open();

//                // set transaction
//                trans = con.BeginTransaction();
//                command.Transaction = trans;

//                // Function Code
//                String query = @"SELECT A.kode AS 'Kode Customer',A.nama AS Customer, A.alamat AS Alamat, A.kota AS Kota, A.provinsi AS Provinsi, A.telp AS Telp, A.telp2 AS Telp2
//                                FROM customer A
//                                WHERE A.status = @status
//                                ORDER BY A.kode";

//                Dictionary<String, String> parameters = new Dictionary<String, String>();
//                parameters.Add("status", Constants.STATUS_AKTIF);

//                InfUtamaDictionary form = new InfUtamaDictionary("Info Customer", query, parameters,
//                                                                new String[] { "Kode Customer" },
//                                                                new String[] { "Kode Customer" },
//                                                                new String[] { });
//                this.AddOwnedForm(form);
//                form.ShowDialog();
//                if(!form.hasil.ContainsKey("Kode Customer")) {
//                    return;
//                }

//                String strngKodeCustomer = form.hasil["Kode Customer"];

//                DataCustomer dCustomer = new DataCustomer(command, strngKodeCustomer);
//                txtNama.EditValue = dCustomer.nama;

//                // cari no alamat kirim default
//                String strngNoAlamatKirim = dCustomer.getNoAlamatKirimDefault().ToString();
//                DataCustomerAlamatKirim dCustomerAlamatKirim = new DataCustomerAlamatKirim(command, strngKodeCustomer, strngNoAlamatKirim);
//                if(dCustomerAlamatKirim.isExist) {
//                    txtAlamat.Text = dCustomerAlamatKirim.alamat;
//                    txtProvinsi.Text = dCustomerAlamatKirim.provinsi;
//                    txtKota.Text = dCustomerAlamatKirim.kota;
//                    txtKodePos.Text = dCustomerAlamatKirim.kodepos;
//                } else {
//                    txtAlamat.Text = dCustomer.alamat;
//                    txtProvinsi.Text = dCustomer.provinsi;
//                    txtKota.Text = dCustomer.kota;
//                    txtKodePos.Text = dCustomer.kodepos;
//                }

//                txtTelepon.Text = dCustomer.telp;
//                txtTelepon2.Text = dCustomer.telp2;
//                txtFaks.Text = dCustomer.faks;
//                txtEmail.Text = dCustomer.email;

//                // Commit Transaction
//                command.Transaction.Commit();
//            } catch(MySqlException ex) {
//                OswPesan.pesanErrorCatch(ex, command, dokumen);
//            } catch(Exception ex) {
//                OswPesan.pesanErrorCatch(ex, command, dokumen);
//            } finally {
//                con.Close();
//                try {
//                    SplashScreenManager.CloseForm();
//                } catch(Exception ex) {
//                }
//            }
        }

    }
}