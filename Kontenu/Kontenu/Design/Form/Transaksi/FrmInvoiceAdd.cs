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
using Kontenu.Master;

namespace Kontenu.Design
{
    public partial class FrmInvoiceAdd : DevExpress.XtraEditors.XtraForm
    {
        private String id = "INVOICE";
        private String dokumen = "INVOICE";
        private String dokumenDetail = "INVOICE";
        private Boolean isAdd;

        public FrmInvoiceAdd(bool pIsAdd)
        {
            isAdd = pIsAdd;
            InitializeComponent();
        }

        private void FrmInvoiceAdd_Load(object sender, EventArgs e)
        {
            SplashScreenManager.ShowForm(typeof(SplashUtama));
            MySqlConnection con = new MySqlConnection(OswConfig.KONEKSI);
            MySqlCommand command = con.CreateCommand();
            MySqlTransaction trans;

            try
            {
                // buka koneksi
                con.Open();

                // set transaction
                trans = con.BeginTransaction();
                command.Transaction = trans;

                // Function Code
                DataOswJenisDokumen dOswJenisDokumen = new DataOswJenisDokumen(command, id);
                if (!this.isAdd)
                {
                    this.dokumen = "Ubah " + dOswJenisDokumen.nama;
                    cmbProyekID = ComboQueryUmum.getProyek(cmbProyekID, command);
                }
                else
                {
                    this.dokumen = "Tambah " + dOswJenisDokumen.nama;
                    cmbProyekID = ComboQueryUmum.getProyekAktif(cmbProyekID, command);
                }

                this.dokumenDetail = "Tambah " + dOswJenisDokumen.nama;

                this.Text = this.dokumen;

                OswControlDefaultProperties.setInput(this, id, command);
                OswControlDefaultProperties.setTanggal(deTanggal);

                //cmbProyekID = ComboQueryUmum.getProyek(cmbProyekID, command);


                this.setDefaultInput(command);

                // Commit Transaction
                command.Transaction.Commit();
            }
            catch (MySqlException ex)
            {
                OswPesan.pesanErrorCatch(ex, command, dokumen);
            }
            catch (Exception ex)
            {
                OswPesan.pesanErrorCatch(ex, command, dokumen);
            }
            finally
            {
                con.Close();
                try
                {
                    SplashScreenManager.CloseForm();
                }
                catch (Exception ex)
                {
                }
            }
        }

        private void setDefaultInput(MySqlCommand command)
        {
            if (!this.isAdd)
            {
                String strngKode = txtKode.Text;
                deTanggal.Enabled = false;
                btnCetak.Enabled = true;

                // INVOICE
                DataInvoice dInvoice = new DataInvoice(command, strngKode);
                deTanggal.DateTime = OswDate.getDateTimeFromStringTanggal(dInvoice.tanggal);

                rdoJenisInvoiceInterior.Checked = (dInvoice.jenis == Constants.JENIS_INVOICE_INTERIOR);
                rdoJenisInvoiceProduct.Checked = (dInvoice.jenis == Constants.JENIS_INVOICE_PRODUCT);

                // PROYEK
                cmbProyekID.EditValue = dInvoice.proyek;
                updateDataProyek(command, true);
                cmbQuotation.EditValue = dInvoice.quotation;
            }
            else
            {
                OswControlDefaultProperties.resetAllInput(this);
                deTanggal.EditValue = "";
                rdoJenisInvoiceInterior.Checked = true;

                btnCetak.Enabled = false;

                cmbProyekID.ItemIndex = 0;
                updateDataProyek(command);
                cmbQuotation.ItemIndex = 0;
            }

            this.setGrid(command);
            txtKode.Focus();
        }

        private void updateDataProyek(MySqlCommand command, bool isFormEditLoad = false)
        {
            
            txtProyekNama.Text = "";
            txtProyekAlamat.Text = "";
            txtProyekKota.Text = "";
            txtProyekProvinsi.Text = "";
            txtProyekKodePos.Text = "";

            txtProyekTujuan.Text = "";
            txtProyekJenis.Text = "";
            txtProyekPIC.Text = "";

            if(cmbProyekID.EditValue == null)
            {
                return;
            }

            DataProyek dProyek = new DataProyek(command, cmbProyekID.EditValue.ToString());


            if (dProyek.isExist)
            {
                rdoJenisInvoiceInterior.Checked = (dProyek.kategori == Constants.JENIS_INVOICE_INTERIOR);
                rdoJenisInvoiceProduct.Checked = (dProyek.kategori == Constants.JENIS_INVOICE_PRODUCT);
                txtProyekNama.Text = dProyek.nama;
                txtProyekAlamat.Text = dProyek.alamat;
                txtProyekKota.Text = dProyek.kota;
                txtProyekProvinsi.Text = dProyek.provinsi;
                txtProyekKodePos.Text = dProyek.kodepos;

                DataTujuanProyek dTujuanProyek = new DataTujuanProyek(command, dProyek.tujuanproyek);
                DataJenisProyek dJenisProyek = new DataJenisProyek(command, dProyek.jenisproyek);
                DataPIC dPIC = new DataPIC(command, dProyek.pic);

                txtProyekTujuan.Text = dTujuanProyek.nama;
                txtProyekJenis.Text = dJenisProyek.nama;
                txtProyekPIC.Text = dPIC.nama;

                // KLIEN
                DataKlien dKlien = new DataKlien(command, dProyek.klien);
                txtKodeKlien.Text = dProyek.klien;
                txtNama.EditValue = dKlien.nama;
                txtAlamat.Text = dKlien.alamat;
                txtProvinsi.Text = dKlien.provinsi;
                txtKota.Text = dKlien.kota;
                txtKodePos.Text = dKlien.kodepos;
                txtTelepon.Text = dKlien.telp;
                txtHandphone.Text = dKlien.handphone;
                txtEmail.Text = dKlien.email;

                // QUOTATION
                cmbQuotation = ComboQueryUmum.getQuotation(cmbQuotation, command, txtKodeKlien.Text, false, true);
                if (!isFormEditLoad)
                {
                    setGrid(command, true);
                }
            }
        }

        public void setGrid(MySqlCommand command, bool isKosong = false)
        {
            String strngKode = txtKode.Text;

            String query = @"SELECT 1 AS No, '' AS 'Kode Jasa', '' AS Jasa, '' AS Deskripsi, '' AS Quotation, 0 AS 'Quotation Detail No', 0 AS Qty, 
                                    '' AS 'Kode Unit', '' AS Unit, 0 AS Rate, 0 AS Subtotal";

            Dictionary<String, String> parameters = new Dictionary<String, String>();

            if (!isKosong)
            {
                query = @"SELECT A.no AS No, B.kode AS 'Kode Jasa', B.nama AS Jasa, A.deskripsi AS Deskripsi, 
                                    A.quotation AS Quotation, A.quotationdetailno AS 'Quotation Detail No', A.jumlah AS Qty, 
                                    C.kode 'Kode Unit', C.nama AS Unit, A.rate AS Rate, A.subtotal AS Subtotal
                            FROM invoicedetail A
                            INNER JOIN jasa B ON A.jasa = B.kode
                            INNER JOIN unit C ON A.unit = C.kode
                            WHERE A.invoice = @kode
                            ORDER BY A.no";

                parameters.Add("kode", strngKode);
            }

            Dictionary<String, int> widths = new Dictionary<String, int>();
            // 960 - 21 (kiri) - 17 (vertikal lines) - 35 (No) = 927
            widths.Add("Jasa", 200);
            widths.Add("Deskripsi", 207);
            widths.Add("Quotation", 135);
            widths.Add("Qty", 80);
            widths.Add("Unit", 90);
            widths.Add("Rate", 100);
            widths.Add("Subtotal", 110);

            Dictionary<String, String> inputType = new Dictionary<string, string>();
            inputType.Add("Qty", OswInputType.NUMBER);
            inputType.Add("Rate", OswInputType.NUMBER);
            inputType.Add("Subtotal", OswInputType.NUMBER);

            OswGrid.getGridInput(gridControl1, command, query, parameters, widths, inputType,
                                 new String[] { "No", "Kode Jasa", "Kode Unit", "Quotation Detail No" },
                                 new String[] { "Unit", "Subtotal", "Quotation" });

            // search produk di kolom kode
            RepositoryItemButtonEdit searchJasa = new RepositoryItemButtonEdit();
            searchJasa.Buttons[0].Kind = ButtonPredefines.Glyph;
            searchJasa.Buttons[0].Image = OswResources.getImage("cari-16.png", typeof(Program).Assembly);
            searchJasa.Buttons[0].Visible = true;
            searchJasa.ButtonClick += searchJasa_ButtonClick;

            GridView gridView = gridView1;
            gridView.Columns["Jasa"].ColumnEdit = searchJasa;
            gridView.Columns["Jasa"].ColumnEdit.ReadOnly = true;

            setFooter();
        }

        void searchJasa_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            infoJasa();
        }

        private void btnSimpan_Click(object sender, EventArgs e)
        {
            // validation
            dxValidationProvider1.SetValidationRule(deTanggal, OswValidation.IsNotBlank());
            dxValidationProvider1.SetValidationRule(txtKodeKlien, OswValidation.IsNotBlank());
            dxValidationProvider1.SetValidationRule(cmbProyekID, OswValidation.IsNotBlank());
            dxValidationProvider1.SetValidationRule(txtProyekNama, OswValidation.IsNotBlank());
            dxValidationProvider1.SetValidationRule(txtKodeKlien, OswValidation.IsNotBlank());

            if (!dxValidationProvider1.Validate())
            {
                foreach (Control x in dxValidationProvider1.GetInvalidControls())
                {
                    dxValidationProvider1.SetIconAlignment(x, ErrorIconAlignment.MiddleRight);
                }
                return;
            }

            MySqlConnection con = new MySqlConnection(OswConfig.KONEKSI);
            MySqlCommand command = con.CreateCommand();
            MySqlTransaction trans;

            try
            {
                // buka koneksi
                con.Open();

                // set transaction
                trans = con.BeginTransaction();
                command.Transaction = trans;

                // Function Code
                String strngKode = txtKode.Text;
                String strngTanggal = deTanggal.Text;
                String strngJenis = rdoJenisInvoiceInterior.Checked ? Constants.JENIS_INVOICE_INTERIOR : Constants.JENIS_INVOICE_PRODUCT;

                String strngProyek = cmbProyekID.EditValue.ToString();
                String strngKodeKlien = txtKodeKlien.Text;
                String strngQuotation = cmbQuotation.EditValue.ToString();

                DataInvoice dInvoice = new DataInvoice(command, strngKode);
                dInvoice.tanggal = strngTanggal;
                dInvoice.jenis = strngJenis;
                dInvoice.proyek = strngProyek;
                dInvoice.klien = strngKodeKlien;
                dInvoice.quotation = strngQuotation;

                if (this.isAdd)
                {
                    dInvoice.status = Constants.STATUS_INVOICE_PROSES;
                    dInvoice.tambah();

                    // update kode header --> setelah generate
                    strngKode = dInvoice.kode;
                    txtKode.Text = strngKode;

                    this.isAdd = false;
                }
                else
                {
                    dInvoice.hapusDetail();
                    dInvoice.ubah();
                }

                // simpan detail
                setFooter();

                decimal dblGrandTotal = 0;
                for (int i = 0; i < gridView1.DataRowCount; i++)
                {
                    if (gridView1.GetRowCellValue(i, "Jasa") == null)
                    {
                        continue;
                    }

                    if (gridView1.GetRowCellValue(i, "Jasa").ToString() == "")
                    {
                        continue;
                    }

                    String strngNo = gridView1.GetRowCellValue(i, "No").ToString();
                    String strngKodeJasa = gridView1.GetRowCellValue(i, "Kode Jasa").ToString();
                    String strngDeskripsi = gridView1.GetRowCellValue(i, "Deskripsi").ToString();
                    String strngKodeUnit = gridView1.GetRowCellValue(i, "Kode Unit").ToString();
                    decimal dblJumlah = Tools.getRoundMoney(decimal.Parse(gridView1.GetRowCellValue(i, "Qty").ToString()));
                    decimal dblRate = Tools.getRoundMoney(decimal.Parse(gridView1.GetRowCellValue(i, "Rate").ToString()));

                    String strngQuotationDetail = gridView1.GetRowCellValue(i, "Quotation").ToString();
                    String strngQuotationDetailNo = gridView1.GetRowCellValue(i, "Quotation Detail No").ToString();

                    decimal dblSubtotal = Tools.getRoundMoney(dblJumlah * dblRate);
                    dblGrandTotal = Tools.getRoundMoney(dblGrandTotal + dblSubtotal);

                    // simpan detail
                    DataInvoiceDetail dInvoiceDetail = new DataInvoiceDetail(command, strngKode, strngNo);
                    dInvoiceDetail.jasa = strngKodeJasa;
                    dInvoiceDetail.deskripsi = strngDeskripsi;
                    dInvoiceDetail.jumlah = dblJumlah.ToString();
                    dInvoiceDetail.unit = strngKodeUnit;
                    dInvoiceDetail.rate = dblRate.ToString();
                    dInvoiceDetail.subtotal = dblSubtotal.ToString();
                    dInvoiceDetail.quotation = strngQuotationDetail;
                    dInvoiceDetail.quotationdetailno = strngQuotationDetailNo;
                    dInvoiceDetail.tambah();

                    // tulis log detail
                    // OswLog.setTransaksi(command, dokumenDetail, dInvoiceDetail.ToString());
                }

                // Update header
                dInvoice = new DataInvoice(command, strngKode);
                dInvoice.grandtotal = dblGrandTotal.ToString();
                dInvoice.ubah();

                // validasi setelah simpan
                dInvoice.valJumlahDetail();
                dInvoice.prosesJurnal();

                // tulis log
                // OswLog.setTransaksi(command, dokumen, dInvoice.ToString());

                // reload grid di form header
                FrmInvoice frmInvoice = (FrmInvoice)this.Owner;
                frmInvoice.setGrid(command);

                // Commit Transaction
                command.Transaction.Commit();

                OswPesan.pesanInfo("Proses simpan berhasil.");

                if (this.isAdd)
                {
                    setDefaultInput(command);
                }
                else
                {
                    this.Close();
                }
            }
            catch (MySqlException ex)
            {
                OswPesan.pesanErrorCatch(ex, command, dokumen);
            }
            catch (Exception ex)
            {
                OswPesan.pesanErrorCatch(ex, command, dokumen);
            }
            finally
            {
                con.Close();
                try
                {
                    SplashScreenManager.CloseForm();
                }
                catch (Exception ex)
                {
                }
            }
        }

        private void btnCetak_Click(object sender, EventArgs e)
        {
            String strngKode = txtKode.Text;
            cetak(strngKode);

        }

        private void cetak(String kode)
        {
            SplashScreenManager.ShowForm(typeof(SplashUtama));
            MySqlConnection con = new MySqlConnection(OswConfig.KONEKSI);
            MySqlCommand command = con.CreateCommand();
            MySqlTransaction trans;

            try
            {
                // buka koneksi
                con.Open();

                // set transaction
                trans = con.BeginTransaction();
                command.Transaction = trans;

                // function code
                RptInvoice report = new RptInvoice();

                // PERUSAHAAN
                DataPerusahaan dPerusahaan = new DataPerusahaan(command, Constants.PERUSAHAAN_KONTENU);
                report.Parameters["PerusahaanKode"].Value = dPerusahaan.kode;
                report.Parameters["PerusahaanNama"].Value = dPerusahaan.nama;
                report.Parameters["PerusahaanAlamat"].Value = dPerusahaan.alamat;
                report.Parameters["PerusahaanKota"].Value = dPerusahaan.kota;
                report.Parameters["PerusahaanEmail"].Value = dPerusahaan.email;
                report.Parameters["PerusahaanLogo"].Value = dPerusahaan.logo;
                report.Parameters["PerusahaanTelepon"].Value = dPerusahaan.telf;
                report.Parameters["PerusahaanWebsite"].Value = dPerusahaan.website;

                // TRANSAKSI
                DataInvoice dInvoice = new DataInvoice(command, kode);
                report.Parameters["Kode"].Value = kode;
                report.Parameters["kodePagar"].Value = "# " + dInvoice.kode;
                report.Parameters["Tanggal"].Value = dInvoice.tanggal;

                // PROYEK
                DataProyek dProyek = new DataProyek(command, dInvoice.proyek);
                report.Parameters["ProyekNama"].Value = dProyek.nama;
                report.Parameters["ProyekAlamat"].Value = dProyek.alamat;
                report.Parameters["ProyekKota"].Value = dProyek.kota;
                report.Parameters["ProyekJenis"].Value = dProyek.jenisproyek;
                report.Parameters["ProyekTanggalBerlaku"].Value = OswDate.ConvertDate(dProyek.tanggaldeal, "dd/MM/yyyy", "dd MMMM yyyy");

                // KLIEN
                DataKlien dKlien = new DataKlien(command, dInvoice.klien);
                report.Parameters["KlienNama"].Value = dKlien.nama;
                report.Parameters["KlienAlamat"].Value = dKlien.alamat;
                report.Parameters["KlienKota"].Value = dKlien.kota;
                report.Parameters["KlienEmail"].Value = dKlien.email;
                report.Parameters["KlienTelp"].Value = dKlien.telp;
                report.Parameters["KlienJabatan"].Value = "JABATAN";
                report.Parameters["KlienKTP"].Value = dKlien.ktp;

                if (dInvoice.quotation == "")
                {
                    //jika invoice tidak ada quot, maka PIC kosong. Sehingga didefault pake nama anet untuk design
                    String strKodePIC = "PIC-001";                    
                    DataPIC dPIC = new DataPIC(command, strKodePIC);
                    report.Parameters["PICNama"].Value = dPIC.nama;
                    report.Parameters["PICEmail"].Value = dPIC.email;
                    report.Parameters["PICTelp"].Value = dPIC.handphone;
                    report.Parameters["PICTtd"].Value = dPIC.ttd;
                }
                else {
                    DataQuotation dQuotation = new DataQuotation(command, dInvoice.quotation);
                    DataPIC dPIC = new DataPIC(command, dQuotation.pic);
                    report.Parameters["PICNama"].Value = dPIC.nama;
                    report.Parameters["PICEmail"].Value = dPIC.email;
                    report.Parameters["PICTelp"].Value = dPIC.handphone;
                    report.Parameters["PICTtd"].Value = dPIC.ttd;
                }
                
                

                // assign the printing system to the document viewer.
                LaporanPrintPreview laporan = new LaporanPrintPreview();
                laporan.documentViewer1.DocumentSource = report;

                //reportprinttool printtool = new reportprinttool(report);
                //printtool.print();

                OswLog.setLaporan(command, dokumen);

                laporan.Show();

                // commit transaction
                command.Transaction.Commit();
            }
            catch (MySqlException ex)
            {
                OswPesan.pesanErrorCatch(ex, command, dokumen);
            }
            catch (Exception ex)
            {
                OswPesan.pesanErrorCatch(ex, command, dokumen);
            }
            finally
            {
                con.Close();
                try
                {
                    SplashScreenManager.CloseForm();
                }
                catch (Exception ex)
                {
                }
            }
        }

        private void cetakFL(String kode)
        {
            SplashScreenManager.ShowForm(typeof(SplashUtama));
            MySqlConnection con = new MySqlConnection(OswConfig.KONEKSI);
            MySqlCommand command = con.CreateCommand();
            MySqlTransaction trans;

            try
            {
                // buka koneksi
                con.Open();

                // set transaction
                trans = con.BeginTransaction();
                command.Transaction = trans;

                // function code
                RptFormalLetter report = new RptFormalLetter();

                // PERUSAHAAN
                DataPerusahaan dPerusahaan = new DataPerusahaan(command, Constants.PERUSAHAAN_KONTENU);
                report.Parameters["PerusahaanKode"].Value = dPerusahaan.kode;
                report.Parameters["PerusahaanNama"].Value = dPerusahaan.nama;
                report.Parameters["PerusahaanAlamat"].Value = dPerusahaan.alamat;
                report.Parameters["PerusahaanKota"].Value = dPerusahaan.kota;
                report.Parameters["PerusahaanEmail"].Value = dPerusahaan.email;
                report.Parameters["PerusahaanLogo"].Value = dPerusahaan.logo;
                report.Parameters["PerusahaanTelepon"].Value = dPerusahaan.telf;
                report.Parameters["PerusahaanWebsite"].Value = dPerusahaan.website;

                // TRANSAKSI
                DataInvoice dInvoice = new DataInvoice(command, kode);
                report.Parameters["Kode"].Value = kode;
                report.Parameters["kodeFL"].Value = kode.Substring(7);
                report.Parameters["Tanggal"].Value = dInvoice.tanggal;

                // PROYEK
                DataProyek dProyek = new DataProyek(command, dInvoice.proyek);
                report.Parameters["ProyekNama"].Value = dProyek.nama;
                report.Parameters["ProyekTanggalBerlaku"].Value = OswDate.ConvertDate(dProyek.tanggaldeal, "dd/MM/yyyy", "dd MMMM yyyy");                

                // KLIEN
                DataKlien dKlien = new DataKlien(command, dInvoice.klien);
                report.Parameters["KlienNama"].Value = dKlien.nama;
                report.Parameters["KlienAlamat"].Value = dKlien.alamat;
                report.Parameters["KlienKota"].Value = dKlien.kota;
                report.Parameters["KlienEmail"].Value = dKlien.email;
                report.Parameters["KlienTelp"].Value = dKlien.telp;
                report.Parameters["KlienJabatan"].Value = dProyek.jabatanklien;
                report.Parameters["KlienKTP"].Value = dKlien.ktp;


                // PIC
                DataPIC dPIC = new DataPIC(command, dProyek.pic);
                report.Parameters["PICNama"].Value = dPIC.nama;
                report.Parameters["PICAlamat"].Value = dPIC.alamat;
                report.Parameters["PICEmail"].Value = dPIC.email;

                DataJabatan dJabatan = new DataJabatan(command, dPIC.jabatan);
                report.Parameters["PICJabatan"].Value = dJabatan.nama;
                report.Parameters["PICKTP"].Value = dPIC.ktp;
                report.Parameters["PICHandphone"].Value = dPIC.handphone;                


                // assign the printing system to the document viewer.
                LaporanPrintPreview laporan = new LaporanPrintPreview();
                laporan.documentViewer1.DocumentSource = report;

                //reportprinttool printtool = new reportprinttool(report);
                //printtool.print();

                OswLog.setLaporan(command, dokumen);

                laporan.Show();

                // commit transaction
                command.Transaction.Commit();
            }
            catch (MySqlException ex)
            {
                OswPesan.pesanErrorCatch(ex, command, dokumen);
            }
            catch (Exception ex)
            {
                OswPesan.pesanErrorCatch(ex, command, dokumen);
            }
            finally
            {
                con.Close();
                try
                {
                    SplashScreenManager.CloseForm();
                }
                catch (Exception ex)
                {
                }
            }
        }

        private void infoJasa()
        {
            MySqlConnection con = new MySqlConnection(OswConfig.KONEKSI);
            MySqlCommand command = con.CreateCommand();
            MySqlTransaction trans;

            try
            {
                // buka koneksi
                con.Open();

                // set transaction
                trans = con.BeginTransaction();
                command.Transaction = trans;

                String query = @"SELECT * 
                                    FROM (
	                                    SELECT A.kode AS Kode, A.nama AS Nama, B.kode AS 'Kode Unit', B.nama AS Unit, '' AS Quotation, '0' AS 'Quotation Detail No', '' AS Deskripsi,
                                               0.0 + '0' AS Qty, 0.0 + '0' AS Rate
	                                    FROM jasa A
	                                    INNER JOIN unit B ON A.unit = B.kode
	                                    UNION
	                                    SELECT B.kode AS Kode, B.nama AS Nama, C.kode AS 'Kode Unit', C.nama AS Unit, A.quotation AS Quotation, A.no AS 'Quotation Detail No', A.deskripsi AS Deskripsi,
                                               A.jumlah AS Qty, A.rate AS Rate
	                                    FROM quotationdetail A
	                                    INNER JOIN jasa B ON A.jasa = B.kode
	                                    INNER JOIN unit C ON A.unit = C.kode
	                                    WHERE A.quotation = @quotation
                                    ) A
                                    ORDER BY A.nama";

                Dictionary<String, String> parameters = new Dictionary<String, String>();
                parameters.Add("quotation", cmbQuotation.EditValue.ToString());

                InfUtamaDataTable form = new InfUtamaDataTable("Info Jasa", query, parameters,
                                                                new String[] { "Kode", "Kode Unit", "Quotation Detail No", "Qty", "Rate" },
                                                                new String[] { "Qty", "Rate" },
                                                                new DataTable());
                this.AddOwnedForm(form);
                form.ShowDialog();

                if (form.hasil.Rows.Count == 0)
                {
                    return;
                }

                GridView gridView = gridView1;

                foreach (DataRow row in form.hasil.Rows)
                {
                    String strngKode = row["Kode"].ToString();
                    String strngNama = row["Nama"].ToString();
                    String strngKodeUnit = row["Kode Unit"].ToString();
                    String strngUnit = row["Unit"].ToString();
                    String strngKodeQuotation = row["Quotation"].ToString();
                    String strngQuotationDetailNo = row["Quotation Detail No"].ToString();
                    String strngDeskripsi = row["Deskripsi"].ToString();
                    String strngQty = row["Qty"].ToString();
                    String strngRate = row["Rate"].ToString();

                    gridView.AddNewRow();
                    gridView.MoveLast();
                    gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Kode Jasa"], strngKode);
                    gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Jasa"], strngNama);
                    gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Kode Unit"], strngKodeUnit);
                    gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Unit"], strngUnit);
                    gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Quotation"], strngKodeQuotation);
                    gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Quotation Detail No"], strngQuotationDetailNo);
                    gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Deskripsi"], strngDeskripsi);
                    gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Qty"], decimal.Parse(strngQty).ToString());
                    gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Rate"], strngRate);
                    gridView.UpdateCurrentRow();
                }

                setFooter();

                // commit transaction
                command.Transaction.Commit();
            }
            catch (MySqlException ex)
            {
                OswPesan.pesanErrorCatch(ex, command, dokumen);
            }
            catch (Exception ex)
            {
                OswPesan.pesanErrorCatch(ex, command, dokumen);
            }
            finally
            {
                con.Close();
                try
                {
                    SplashScreenManager.CloseForm();
                }
                catch (Exception ex)
                {
                }
            }
        }

        private void gridControl1_ProcessGridKey(object sender, KeyEventArgs e)
        {
            GridView gridView = gridView1;
            if (e.KeyCode == Keys.F1 && gridView.FocusedColumn.FieldName == "Jasa")
            {
                infoJasa();
            }
        }

        private void gridView1_FocusedColumnChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventArgs e)
        {
            GridView gridView = sender as GridView;

            if (gridView.GetRowCellValue(gridView.FocusedRowHandle, "Jasa") == null)
            {
                gridView.FocusedColumn = gridView.Columns["Jasa"];
                return;
            }

            if (gridView.FocusedColumn.FieldName != "Jasa" && gridView.GetRowCellValue(gridView.FocusedRowHandle, "Jasa").ToString() == "")
            {
                gridView.FocusedColumn = gridView.Columns["Jasa"];
                return;
            }

            setFooter();
        }

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            GridView gridView = sender as GridView;

            if (gridView.GetRowCellValue(gridView.FocusedRowHandle, "Jasa") == null)
            {
                gridView.FocusedColumn = gridView.Columns["Jasa"];
                return;
            }

            if (gridView.FocusedColumn.FieldName != "Jasa" && gridView.GetRowCellValue(gridView.FocusedRowHandle, "Jasa").ToString() == "")
            {
                gridView.FocusedColumn = gridView.Columns["Jasa"];
                return;
            }

            setFooter();
        }

        private void gridView1_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            GridView gridView = sender as GridView;

            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["No"], gridView.DataRowCount + 1);
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Kode Jasa"], "");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Jasa"], "");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Deskripsi"], "");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Kode Unit"], "");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Unit"], "");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Qty"], "0");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Rate"], "0");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Subtotal"], "0");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Quotation"], "");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Quotation Detail No"], "0");
        }

        private void gridView1_RowDeleted(object sender, DevExpress.Data.RowDeletedEventArgs e)
        {
            GridView gridView = sender as GridView;
            for (int no = 1; no <= gridView.DataRowCount; no++)
            {
                gridView.SetRowCellValue(no - 1, "No", no);
            }

            setFooter();
        }

        private void gridView1_ValidateRow(object sender, DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs e)
        {
            GridView gridView = gridView1;

            if (gridView.GetRowCellValue(gridView.FocusedRowHandle, "Jasa") == null)
            {
                gridView.FocusedColumn = gridView.Columns["Jasa"];
                return;
            }

            if (gridView.GetRowCellValue(gridView.FocusedRowHandle, "Jasa").ToString() == "")
            {
                gridView.FocusedColumn = gridView.Columns["Jasa"];
                return;
            }

            setFooter();
        }

        private void setFooter()
        {
            MySqlConnection con = new MySqlConnection(OswConfig.KONEKSI);
            MySqlCommand command = con.CreateCommand();
            MySqlTransaction trans;

            try
            {
                // buka koneksi
                con.Open();

                // set transaction
                trans = con.BeginTransaction();
                command.Transaction = trans;

                // Function Code
                GridView gridView = gridView1;

                decimal dblGrandTotal = 0;

                for (int i = 0; i < gridView.DataRowCount; i++)
                {
                    if (gridView.GetRowCellValue(i, "Jasa") == null)
                    {
                        continue;
                    }

                    if (gridView.GetRowCellValue(i, "Jasa").ToString() == "")
                    {
                        continue;
                    }

                    decimal dblJumlah = Tools.getRoundMoney(decimal.Parse(gridView.GetRowCellValue(i, "Qty").ToString()));
                    decimal dblRate = Tools.getRoundMoney(decimal.Parse(gridView.GetRowCellValue(i, "Rate").ToString()));

                    decimal dblSubtotal = Tools.getRoundMoney(dblJumlah * dblRate);

                    dblGrandTotal = Tools.getRoundMoney(dblGrandTotal + dblSubtotal);

                    gridView.SetRowCellValue(i, gridView.Columns["Subtotal"], dblSubtotal);
                }

                lblGrandTotal.Text = OswConvert.convertToRupiah(dblGrandTotal);

                // Commit Transaction
                command.Transaction.Commit();
            }
            catch (MySqlException ex)
            {
                OswPesan.pesanErrorCatch(ex, command, dokumen);
            }
            catch (Exception ex)
            {
                OswPesan.pesanErrorCatch(ex, command, dokumen);
            }
            finally
            {
                con.Close();
            }
        }

        private void cmbProyekID_EditValueChanged(object sender, EventArgs e)
        {
            MySqlConnection con = new MySqlConnection(OswConfig.KONEKSI);
            MySqlCommand command = con.CreateCommand();
            MySqlTransaction trans;

            try
            {
                // buka koneksi
                con.Open();

                // set transaction
                trans = con.BeginTransaction();
                command.Transaction = trans;

                // Function Code
                updateDataProyek(command, true);
                cmbQuotation.ItemIndex = 0;

                // Commit Transaction
                command.Transaction.Commit();
            }
            catch (MySqlException ex)
            {
                OswPesan.pesanErrorCatch(ex, command, dokumen);
            }
            catch (Exception ex)
            {
                OswPesan.pesanErrorCatch(ex, command, dokumen);
            }
            finally
            {
                con.Close();
            }
        }

        private void cmbQuotation_EditValueChanged(object sender, EventArgs e)
        {
            MySqlConnection con = new MySqlConnection(OswConfig.KONEKSI);
            MySqlCommand command = con.CreateCommand();
            MySqlTransaction trans;

            try
            {
                // buka koneksi
                con.Open();

                // set transaction
                trans = con.BeginTransaction();
                command.Transaction = trans;

                // Function Code
                setGrid(command, true);

                // Commit Transaction
                command.Transaction.Commit();
            }
            catch (MySqlException ex)
            {
                OswPesan.pesanErrorCatch(ex, command, dokumen);
            }
            catch (Exception ex)
            {
                OswPesan.pesanErrorCatch(ex, command, dokumen);
            }
            finally
            {
                con.Close();
            }
        }

        private void btnCetakFL_Click(object sender, EventArgs e)
        {
            String strngKode = txtKode.Text;
            cetakFL(strngKode);
        }

    }
}