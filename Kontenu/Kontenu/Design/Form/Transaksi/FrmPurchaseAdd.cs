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
    public partial class FrmPurchaseAdd : DevExpress.XtraEditors.XtraForm
    {
        private String id = "PURCHASE";
        private String dokumen = "PURCHASE";
        private String dokumenDetail = "PURCHASE";
        private Boolean isAdd;

        public FrmPurchaseAdd(bool pIsAdd)
        {
            isAdd = pIsAdd;
            InitializeComponent();
        }

        private void FrmPurchaseAdd_Load(object sender, EventArgs e)
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
                }
                else
                {
                    this.dokumen = "Tambah " + dOswJenisDokumen.nama;
                }

                this.dokumenDetail = "Tambah " + dOswJenisDokumen.nama;

                this.Text = this.dokumen;

                OswControlDefaultProperties.setInput(this, id, command);
                OswControlDefaultProperties.setTanggal(deTanggal);

                cmbProyekID = ComboQueryUmum.getProyek(cmbProyekID, command);


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

                // PURCHASE
                DataPurchase dPurchase = new DataPurchase(command, strngKode);
                deTanggal.DateTime = OswDate.getDateTimeFromStringTanggal(dPurchase.tanggal);

                // PROYEK
                cmbProyekID.EditValue = dPurchase.proyek;
                updateDataProyek(command, true);
                //cmbQuotation.EditValue = dPurchase.quotation;
            }
            else
            {
                OswControlDefaultProperties.resetAllInput(this);
                deTanggal.EditValue = "";
                rdoJenisPurchaseInterior.Checked = true;

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

            DataProyek dProyek = new DataProyek(command, cmbProyekID.EditValue.ToString());

            if (dProyek.isExist)
            {
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

                //// KLIEN
                //DataOutsource dOutsource = new DataOutsource(command, dProyek.outsource);
                //txtKodeOutsource.Text = dProyek.outsource;
                //txtNama.EditValue = dOutsource.nama;
                //txtAlamat.Text = dOutsource.alamat;
                //txtProvinsi.Text = dOutsource.provinsi;
                //txtKota.Text = dOutsource.kota;
                //txtKodePos.Text = dOutsource.kodepos;
                //txtTelepon.Text = dOutsource.telp;
                //txtHandphone.Text = dOutsource.handphone;
                //txtEmail.Text = dOutsource.email;

                //// QUOTATION
                //cmbQuotation = ComboQueryUmum.getQuotation(cmbQuotation, command, txtKodeOutsource.Text, false, true);
                //if (!isFormEditLoad)
                //{
                //    setGrid(command, true);
                //}
            }
        }

        public void setGrid(MySqlCommand command, bool isKosong = false)
        {
            String strngKode = txtKode.Text;

            String query = @"SELECT 0 AS No, '' AS 'Kode Jasa', '' AS Jasa, '' AS Deskripsi, '' AS Quotation, 0 AS 'Quotation Detail No', 0 AS Qty, 
                                    '' AS 'Kode Unit', '' AS Unit, 0 AS Rate, 0 AS Subtotal";

            Dictionary<String, String> parameters = new Dictionary<String, String>();

            if (!isKosong)
            {
                query = @"SELECT A.no AS No, B.kode AS 'Kode Jasa', B.nama AS Jasa, A.deskripsi AS Deskripsi, 
                                    A.quotation AS Quotation, A.quotationdetailno AS 'Quotation Detail No', A.jumlah AS Qty, 
                                    C.kode 'Kode Unit', C.nama AS Unit, A.rate AS Rate, A.subtotal AS Subtotal
                            FROM purchasedetail A
                            INNER JOIN jasa B ON A.jasa = B.kode
                            INNER JOIN unit C ON A.unit = C.kode
                            WHERE A.purchase = @kode
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
            //// validation
            //dxValidationProvider1.SetValidationRule(deTanggal, OswValidation.IsNotBlank());
            //dxValidationProvider1.SetValidationRule(txtKodeOutsource, OswValidation.IsNotBlank());
            //dxValidationProvider1.SetValidationRule(cmbProyekID, OswValidation.IsNotBlank());
            //dxValidationProvider1.SetValidationRule(txtProyekNama, OswValidation.IsNotBlank());
            //dxValidationProvider1.SetValidationRule(txtKodeOutsource, OswValidation.IsNotBlank());

            //if (!dxValidationProvider1.Validate())
            //{
            //    foreach (Control x in dxValidationProvider1.GetInvalidControls())
            //    {
            //        dxValidationProvider1.SetIconAlignment(x, ErrorIconAlignment.MiddleRight);
            //    }
            //    return;
            //}

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

                String strngProyek = cmbProyekID.EditValue.ToString();
                String strngQuotation = cmbQuotation.EditValue.ToString();

                DataPurchase dPurchase = new DataPurchase(command, strngKode);
                dPurchase.tanggal = strngTanggal;
                dPurchase.proyek = strngProyek;

                if (this.isAdd)
                {
                    dPurchase.status = Constants.STATUS_PURCHASE_BELUM_LUNAS;
                    dPurchase.tambah();

                    // update kode header --> setelah generate
                    strngKode = dPurchase.kode;
                    txtKode.Text = strngKode;

                    this.isAdd = false;
                }
                else
                {
                    dPurchase.hapusDetail();
                    dPurchase.ubah();
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
                    DataPurchaseDetail dPurchaseDetail = new DataPurchaseDetail(command, strngKode, strngNo);
                    dPurchaseDetail.jasaoutsource = strngKodeJasa;
                    dPurchaseDetail.deskripsi = strngDeskripsi;
                    dPurchaseDetail.jumlah = dblJumlah.ToString();
                    dPurchaseDetail.unit = strngKodeUnit;
                    dPurchaseDetail.rate = dblRate.ToString();
                    dPurchaseDetail.subtotal = dblSubtotal.ToString();
                    dPurchaseDetail.tambah();

                    // tulis log detail
                    OswLog.setTransaksi(command, dokumenDetail, dPurchaseDetail.ToString());
                }

                // Update header
                dPurchase = new DataPurchase(command, strngKode);
                dPurchase.grandtotal = dblGrandTotal.ToString();
                dPurchase.ubah();

                // validasi setelah simpan
                dPurchase.valJumlahDetail();
                dPurchase.prosesJurnal();

                // tulis log
                OswLog.setTransaksi(command, dokumen, dPurchase.ToString());

                // reload grid di form header
                FrmPurchase frmPurchase = (FrmPurchase)this.Owner;
                frmPurchase.setGrid(command);

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
                //RptPurchase report = new RptPurchase();

                //// PERUSAHAAN
                //DataPerusahaan dPerusahaan = new DataPerusahaan(command, Constants.PERUSAHAAN_KONTENU);
                //report.Parameters["PerusahaanKode"].Value = dPerusahaan.kode;
                //report.Parameters["PerusahaanNama"].Value = dPerusahaan.nama;
                //report.Parameters["PerusahaanAlamat"].Value = dPerusahaan.alamat;
                //report.Parameters["PerusahaanKota"].Value = dPerusahaan.kota;
                //report.Parameters["PerusahaanEmail"].Value = dPerusahaan.email;
                //report.Parameters["PerusahaanTelepon"].Value = "+62 811 318 6880";
                //report.Parameters["PerusahaanWebsite"].Value = dPerusahaan.website;

                //// TRANSAKSI
                //DataPurchase dPurchase = new DataPurchase(command, kode);
                //report.Parameters["Kode"].Value = kode;
                //report.Parameters["Tanggal"].Value = dPurchase.tanggal;

                //// PROYEK
                //DataProyek dProyek = new DataProyek(command, dPurchase.proyek);
                //report.Parameters["ProyekNama"].Value = dProyek.nama;
                //report.Parameters["ProyekTanggalBerlaku"].Value = OswDate.ConvertDate(dProyek.tanggaldeal, "dd/MM/yyyy", "dd MMMM yyyy");

                //// KLIEN
                //DataOutsource dOutsource = new DataOutsource(command, dPurchase.outsource);
                //report.Parameters["OutsourceNama"].Value = dOutsource.nama;
                //report.Parameters["OutsourceAlamat"].Value = dOutsource.alamat;
                //report.Parameters["OutsourceKota"].Value = dOutsource.kota;
                //report.Parameters["OutsourceEmail"].Value = dOutsource.email;
                //report.Parameters["OutsourceTelp"].Value = dOutsource.telp;
                //report.Parameters["OutsourceJabatan"].Value = "JABATAN";
                //report.Parameters["OutsourceKTP"].Value = dOutsource.ktp;



                //// assign the printing system to the document viewer.
                //LaporanPrintPreview laporan = new LaporanPrintPreview();
                //laporan.documentViewer1.DocumentSource = report;

                ////reportprinttool printtool = new reportprinttool(report);
                ////printtool.print();

                //OswLog.setLaporan(command, dokumen);

                //laporan.Show();

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
                                               0 AS Qty, 0 AS Rate
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
                                                                new String[] { },
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
                    gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Qty"], strngQty);
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

    }
}