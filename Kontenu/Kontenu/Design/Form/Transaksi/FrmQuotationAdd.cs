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

namespace Kontenu.Design
{
    public partial class FrmQuotationAdd : DevExpress.XtraEditors.XtraForm
    {
        private String id = "QUOTATION";
        private String dokumen = "QUOTATION";
        private String dokumenDetail = "QUOTATION";
        private Boolean isAdd;
        public Dictionary<string, DataTable> dicDetailJasa;

        public FrmQuotationAdd(bool pIsAdd)
        {
            isAdd = pIsAdd;
            InitializeComponent();
        }

        private void FrmQuotationAdd_Load(object sender, EventArgs e)
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
                OswControlDefaultProperties.setTanggal(deTanggalBerlaku);

                cmbProyekTujuan = ComboQueryUmum.getTujuanProyek(cmbProyekTujuan, command);
                cmbProyekPIC = ComboQueryUmum.getPIC(cmbProyekPIC, command);

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

                // QUOTATION
                DataQuotation dQuotation = new DataQuotation(command, strngKode);
                deTanggal.DateTime = OswDate.getDateTimeFromStringTanggal(dQuotation.tanggal);
                deTanggalBerlaku.DateTime = OswDate.getDateTimeFromStringTanggal(dQuotation.tanggalberlaku);
                chkTutup.Checked = dQuotation.status == Constants.STATUS_QUOTATION_PROSES;

                // KLIEN
                DataKlien dKlien = new DataKlien(command, dQuotation.klien);
                txtNama.EditValue = dKlien.nama;
                txtAlamat.Text = dKlien.alamat;
                txtProvinsi.Text = dKlien.provinsi;
                txtKota.Text = dKlien.kota;
                txtKodePos.Text = dKlien.kodepos;
                txtTelepon.Text = dKlien.telp;
                txtHandphone.Text = dKlien.handphone;
                txtEmail.Text = dKlien.email;

                // PROYEK
                txtProyekNama.Text = dQuotation.proyeknama;
                txtProyekAlamat.Text = dQuotation.proyekalamat;
                txtProyekKota.Text = dQuotation.proyekkota;
                txtProyekProvinsi.Text = dQuotation.proyekprovinsi;
                txtProyekKodePos.Text = dQuotation.proyekkodepos;

                cmbProyekTujuan.EditValue = dQuotation.tujuanproyek;
                cmbProyekJenis.EditValue = dQuotation.jenisproyek;
                cmbProyekPIC.EditValue = dQuotation.pic;


                // BUTTON
                btnPerincian.Enabled = true;
                chkTutup.Enabled = true;

                if (chkTutup.Checked)
                {
                    chkTutup.Enabled = false;
                    btnSimpan.Enabled = false;

                    btnPerincian.Enabled = false;
                }
            }
            else
            {
                OswControlDefaultProperties.resetAllInput(this);
                deTanggal.EditValue = "";
                deTanggalBerlaku.EditValue = "";
                btnPerincian.Enabled = false;

                chkTutup.Enabled = false;
                chkTutup.Checked = false;

                btnCetak.Enabled = false;

                cmbProyekTujuan.ItemIndex = 0;
                cmbProyekPIC.ItemIndex = 0;
            }

            this.setGrid(command);
            txtKode.Focus();
        }

        public void setGrid(MySqlCommand command)
        {
            String strngKode = txtKode.Text;

            String query = @"SELECT A.no AS No, B.kode AS 'Kode Jasa', B.nama AS Jasa, A.deskripsi AS Deskripsi, A.jumlah AS Qty, 
                                    C.kode 'Kode Unit', C.nama AS Unit, A.rate AS Rate, A.subtotal AS Subtotal, LEFT(UUID(),8) AS TEMP
                            FROM quotationdetail A
                            INNER JOIN jasa B ON A.jasa = B.kode
                            INNER JOIN unit C ON A.unit = C.kode
                            WHERE A.quotation = @kode
                            ORDER BY A.no";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("kode", strngKode);

            Dictionary<String, int> widths = new Dictionary<String, int>();
            // 960 - 21 (kiri) - 17 (vertikal lines) - 35 (No) = 927
            widths.Add("Jasa", 180);
            widths.Add("Deskripsi", 302);
            widths.Add("Qty", 90);
            widths.Add("Unit", 110);
            widths.Add("Rate", 120);
            widths.Add("Subtotal", 120);

            Dictionary<String, String> inputType = new Dictionary<string, string>();
            inputType.Add("Qty", OswInputType.NUMBER);
            inputType.Add("Rate", OswInputType.NUMBER);
            inputType.Add("Subtotal", OswInputType.NUMBER);

            OswGrid.getGridInput(gridControl1, command, query, parameters, widths, inputType,
                                 new String[] { "No", "Kode Jasa", "Kode Unit", "TEMP" },
                                 new String[] { "Unit", "Subtotal" });

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
            dxValidationProvider1.SetValidationRule(deTanggalBerlaku, OswValidation.IsNotBlank());
            dxValidationProvider1.SetValidationRule(txtNama, OswValidation.IsNotBlank());

            dxValidationProvider1.SetValidationRule(cmbProyekTujuan, OswValidation.IsNotBlank());
            dxValidationProvider1.SetValidationRule(cmbProyekJenis, OswValidation.IsNotBlank());
            dxValidationProvider1.SetValidationRule(cmbProyekPIC, OswValidation.IsNotBlank());

            if (!dxValidationProvider1.Validate())
            {
                foreach (Control x in dxValidationProvider1.GetInvalidControls())
                {
                    dxValidationProvider1.SetIconAlignment(x, ErrorIconAlignment.MiddleRight);
                }
                return;
            }

            //MySqlConnection con = new MySqlConnection(OswConfig.KONEKSI);
            //MySqlCommand command = con.CreateCommand();
            //MySqlTransaction trans;

            //try
            //{
            //    // buka koneksi
            //    con.Open();

            //    // set transaction
            //    trans = con.BeginTransaction();
            //    command.Transaction = trans;

            //    // Function Code

            //    String strngKode = txtKode.Text;
            //    String strngTanggal = deTanggal.Text;
            //    String strngDeadline = deDeadline.Text;
            //    String strngDelivery = deDelivery.Text;
            //    String strngPenawaran = dePenawaran.Text;
            //    String strngKlien = txtNama.Text;
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
            //    dQuotation.customer = strngKlien;
            //    dQuotation.alamat = strngAlamat;
            //    dQuotation.provinsi = strngProvinsi;
            //    dQuotation.kota = strngKota;
            //    dQuotation.kodepos = strngKodePos;
            //    dQuotation.telp = strngTelp;
            //    dQuotation.telp2 = strngTelp2;
            //    dQuotation.faks = strngFaks;
            //    dQuotation.email = strngEmail;
            //    dQuotation.catatan = strngCatatan;

            //    if (this.isAdd)
            //    {
            //        dQuotation.status = Constants.STATUS_Quotation_DALAM_PROSES;
            //        dQuotation.tambah();
            //        // update kode header --> setelah generate
            //        strngKode = dQuotation.kode;
            //        txtKode.Text = strngKode;

            //        btnTambahBarang.Enabled = true;
            //        btnUbahBarang.Enabled = true;
            //        btnHapusBarang.Enabled = true;

            //        this.isAdd = false;
            //    }
            //    else
            //    {
            //        if (chkTutup.Checked)
            //        {
            //            dQuotation.status = Constants.STATUS_Quotation_SELESAI;
            //            dQuotation.ubahStatus();
            //        }
            //        else
            //        {
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
            //}
            //catch (MySqlException ex)
            //{
            //    OswPesan.pesanErrorCatch(ex, command, dokumen);
            //}
            //catch (Exception ex)
            //{
            //    OswPesan.pesanErrorCatch(ex, command, dokumen);
            //}
            //finally
            //{
            //    con.Close();
            //    try
            //    {
            //        SplashScreenManager.CloseForm();
            //    }
            //    catch (Exception ex)
            //    {
            //    }
            //}
        }

        private void btnPerincian_Click(object sender, EventArgs e)
        {
            if (gridView1.GetSelectedRows().Length == 0)
            {
                OswPesan.pesanError("Silahkan pilih jasa.");
                return;
            }

            String strngTEMP = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "TEMP").ToString();
            String strngJasa = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Jasa").ToString();

            if (strngJasa == "")
            {
                OswPesan.pesanError("Silahkan pilih jasa.");
                return;
            }

            //FrmQuotationAddJasaDetail form = new FrmQuotationAddJasaDetail(false, strngKode, strngID);

            //MySqlConnection con = new MySqlConnection(OswConfig.KONEKSI);
            //MySqlCommand command = con.CreateCommand();
            //MySqlTransaction trans;

            //try
            //{
            //    // buka koneksi
            //    con.Open();

            //    // set transaction
            //    trans = con.BeginTransaction();
            //    command.Transaction = trans;

            //    // Function Code
            //    DataQuotationDetail dQuotationDetail = new DataQuotationDetail(command, Constants.CABANG, strngKode, strngID);
            //    if (!dQuotationDetail.isExist)
            //    {
            //        throw new Exception("Barang tidak ditemukan.");
            //    }

            //    this.AddOwnedForm(form);

            //    // Commit Transaction
            //    command.Transaction.Commit();
            //}
            //catch (MySqlException ex)
            //{
            //    OswPesan.pesanErrorCatch(ex, command, dokumen);
            //}
            //catch (Exception ex)
            //{
            //    OswPesan.pesanErrorCatch(ex, command, dokumen);
            //}
            //finally
            //{
            //    con.Close();
            //    try
            //    {
            //        SplashScreenManager.CloseForm();
            //    }
            //    catch (Exception ex)
            //    {
            //    }
            //}

            //form.ShowDialog();
        }

        private void btnCetak_Click(object sender, EventArgs e)
        {
        }

        private void btnCariKlien_Click(object sender, EventArgs e)
        {
            infoKlien();
        }

        private void infoKlien()
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
                String query = @"SELECT A.kode AS 'Kode Klien',A.nama AS Klien, A.alamat AS Alamat, A.provinsi AS Provinsi, A.kota AS Kota, A.kodepos AS 'Kode Pos', 
                                        A.telp AS Telp, A.handphone AS Handphone, A.email AS Email, A.ktp AS NIK
                                FROM klien A
                                ORDER BY A.kode";

                Dictionary<String, String> parameters = new Dictionary<String, String>();

                InfUtamaDictionary form = new InfUtamaDictionary("Info Klien", query, parameters,
                                                                new String[] { "Kode Klien" },
                                                                new String[] { "Kode Klien" },
                                                                new String[] { });
                this.AddOwnedForm(form);
                form.ShowDialog();
                if (!form.hasil.ContainsKey("Kode Klien"))
                {
                    return;
                }

                String strngKodeKlien = form.hasil["Kode Klien"];

                DataKlien dKlien = new DataKlien(command, strngKodeKlien);
                txtNama.EditValue = dKlien.nama;
                txtAlamat.Text = dKlien.alamat;
                txtProvinsi.Text = dKlien.provinsi;
                txtKota.Text = dKlien.kota;
                txtKodePos.Text = dKlien.kodepos;
                txtTelepon.Text = dKlien.telp;
                txtHandphone.Text = dKlien.handphone;
                txtEmail.Text = dKlien.email;

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

        private void cmbProyekTujuan_EditValueChanged(object sender, EventArgs e)
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
                cmbProyekJenis = ComboQueryUmum.getJenisProyek(cmbProyekJenis, command, cmbProyekTujuan.ItemIndex < 0 ? "" : cmbProyekTujuan.EditValue.ToString());
                cmbProyekJenis.ItemIndex = 0;

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

                String query = @"SELECT A.kode AS Kode, A.nama AS Nama, B.kode AS 'Kode Unit', B.nama AS Unit
                                    FROM jasa A
                                    INNER JOIN unit B ON A.unit = B.kode";

                Dictionary<String, String> parameters = new Dictionary<String, String>();

                InfUtamaDataTable form = new InfUtamaDataTable("Info Jasa", query, parameters,
                                                                new String[] { "Kode Unit" },
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

                    gridView.AddNewRow();
                    gridView.MoveLast();
                    gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Kode Jasa"], strngKode);
                    gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Jasa"], strngNama);
                    gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Kode Unit"], strngKodeUnit);
                    gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Unit"], strngUnit);
                    gridView.UpdateCurrentRow();
                }

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
        }

        private void gridView1_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            GridView gridView = sender as GridView;

            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["No"], gridView.DataRowCount + 1);
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Kode Jasa"], "");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Jasa"], "");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Kode Unit"], "");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Unit"], "");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Qty"], "0");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Rate"], "0");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Subtotal"], "0");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["TEMP"], OswRandom.generateFromString(8));            
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

    }
}