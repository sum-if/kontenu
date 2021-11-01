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
    public partial class FrmPurchasePaymentAdd : DevExpress.XtraEditors.XtraForm
    {
        private String id = "PURCHASEPAYMENT";
        private String dokumen = "PURCHASEPAYMENT";
        private String dokumenDetail = "PURCHASEPAYMENT";
        private Boolean isAdd;

        public FrmPurchasePaymentAdd(bool pIsAdd)
        {
            isAdd = pIsAdd;
            InitializeComponent();
        }

        private void FrmPurchasePaymentAdd_Load(object sender, EventArgs e)
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

                cmbAkunKasBayar = ComboQueryUmum.getAkun(cmbAkunKasBayar, command, Constants.AKUN_LIST_PURCHASE_PAYMENT);
                cmbAkunKasBayar.EditValue = OswCombo.getFirstEditValue(cmbAkunKasBayar);

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

                // PURCHASE PAYMENT
                DataPurchasePayment dPurchasePayment = new DataPurchasePayment(command, strngKode);
                deTanggal.DateTime = OswDate.getDateTimeFromStringTanggal(dPurchasePayment.tanggal);
                cmbAkunKasBayar.EditValue = dPurchasePayment.akunkas;

                // OUTSOURCE
                DataOutsource dOutsource = new DataOutsource(command, dPurchasePayment.outsource);
                txtKodeOutsource.EditValue = dOutsource.kode;
                txtNama.EditValue = dOutsource.nama;
                txtAlamat.Text = dOutsource.alamat;
                txtProvinsi.Text = dOutsource.provinsi;
                txtKota.Text = dOutsource.kota;
                txtKodePos.Text = dOutsource.kodepos;
                txtTelepon.Text = dOutsource.telp;
                txtHandphone.Text = dOutsource.handphone;
                txtEmail.Text = dOutsource.email;
            }
            else
            {
                OswControlDefaultProperties.resetAllInput(this);
                deTanggal.EditValue = "";
            }

            this.setGrid(command);
            txtKode.Focus();
        }

        public void setGrid(MySqlCommand command, bool isKosong = false)
        {
            String strngKode = txtKode.Text;

            String query = @"SELECT A.no AS No, B.kode AS 'No Purchase', C.kode AS 'Proyek ID', C.nama AS 'Nama Proyek', 
                                    A.grandtotal AS 'Grand Total', A.telahdibayar AS 'Telah Dibayar', A.nominalbayar AS 'Nominal Bayar'
                            FROM purchasepaymentdetail A
                            INNER JOIN purchase B ON A.purchase = B.kode
                            INNER JOIN proyek C ON B.proyek = C.kode
                            WHERE A.purchasepayment = @kode
                            ORDER BY A.no";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("kode", strngKode);

            Dictionary<String, int> widths = new Dictionary<String, int>();
            // 960 - 21 (kiri) - 17 (vertikal lines) - 35 (No) = 927
            widths.Add("No Purchase", 220);
            widths.Add("Proyek ID", 292);
            widths.Add("Nama Proyek", 80);
            widths.Add("Grand Total", 100);
            widths.Add("Telah Dibayar", 110);
            widths.Add("Nominal Bayar", 120);

            Dictionary<String, String> inputType = new Dictionary<string, string>();
            inputType.Add("Grand Total", OswInputType.NUMBER);
            inputType.Add("Telah Dibayar", OswInputType.NUMBER);
            inputType.Add("Nominal Bayar", OswInputType.NUMBER);

            OswGrid.getGridInput(gridControl1, command, query, parameters, widths, inputType,
                                 new String[] { "No" },
                                 new String[] { "Proyek ID", "Nama Proyek", "Grand Total", "Telah Dibayar" });

            // search produk di kolom kode
            RepositoryItemButtonEdit searchPurchase = new RepositoryItemButtonEdit();
            searchPurchase.Buttons[0].Kind = ButtonPredefines.Glyph;
            searchPurchase.Buttons[0].Image = OswResources.getImage("cari-16.png", typeof(Program).Assembly);
            searchPurchase.Buttons[0].Visible = true;
            searchPurchase.ButtonClick += searchPurchase_ButtonClick;

            GridView gridView = gridView1;
            gridView.Columns["No Purchase"].ColumnEdit = searchPurchase;
            gridView.Columns["No Purchase"].ColumnEdit.ReadOnly = true;

            setFooter();
        }

        void searchPurchase_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            infoPurchase();
        }

        private void infoPurchase()
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

                String strngKodeOutsource = txtKodeOutsource.Text;

                String query = @"SELECT A.kode AS Purchase, A.tanggal AS Tanggal, B.kode AS 'Proyek ID', B.nama AS 'Proyek Nama',
                                        A.grandtotal AS 'Grand Total', C.totalbayar AS 'Telah Dibayar', (A.grandtotal-C.totalbayar) AS 'Sisa Bayar'
	                                FROM purchase A
	                                INNER JOIN proyek B ON A.proyek = B.kode
                                    INNER JOIN v_purchase_totalbayar C ON A.kode = C.purchase
                                    WHERE A.status = @status AND A.outsource = @outsource
                                    ORDER BY A.kode";

                Dictionary<String, String> parameters = new Dictionary<String, String>();
                parameters.Add("status", Constants.STATUS_PURCHASE_BELUM_LUNAS);
                parameters.Add("outsource", strngKodeOutsource);

                InfUtamaDataTable form = new InfUtamaDataTable("Info Purchase", query, parameters,
                                                                new String[] { },
                                                                new String[] { "Grand Total", "Telah Dibayar", "Sisa Bayar" },
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
                    String strngPurchase = row["Purchase"].ToString();
                    String strngProyekID = row["Proyek ID"].ToString();
                    String strngProyekNama = row["Proyek Nama"].ToString();
                    String strngGrandTotal = row["Grand Total"].ToString();
                    String strngTelahDibayar = row["Telah Dibayar"].ToString();
                    String strngSisaBayar = row["Sisa Bayar"].ToString();

                    gridView.AddNewRow();
                    gridView.MoveLast();
                    gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["No Purchase"], strngPurchase);
                    gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Proyek ID"], strngProyekID);
                    gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Nama Proyek"], strngProyekNama);
                    gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Grand Total"], strngGrandTotal);
                    gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Telah Dibayar"], strngTelahDibayar);
                    gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Nominal Bayar"], strngSisaBayar);
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

        private void btnSimpan_Click(object sender, EventArgs e)
        {
            // validation
            dxValidationProvider1.SetValidationRule(deTanggal, OswValidation.IsNotBlank());
            dxValidationProvider1.SetValidationRule(txtKodeOutsource, OswValidation.IsNotBlank());
            dxValidationProvider1.SetValidationRule(cmbAkunKasBayar, OswValidation.IsNotBlank());

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
                String strngAkunKas = cmbAkunKasBayar.EditValue.ToString();
                String strngOutsource = txtKodeOutsource.Text;

                DataPurchasePayment dPurchasePayment = new DataPurchasePayment(command, strngKode);
                dPurchasePayment.tanggal = strngTanggal;
                dPurchasePayment.outsource = strngOutsource;
                dPurchasePayment.akunkas = strngAkunKas;

                if (this.isAdd)
                {
                    dPurchasePayment.tambah();

                    // update kode header --> setelah generate
                    strngKode = dPurchasePayment.kode;
                    txtKode.Text = strngKode;
                }
                else
                {
                    dPurchasePayment.hapusDetail();
                    dPurchasePayment.ubah();
                }

                // simpan detail
                setFooter();

                decimal dblTotalNominalBayar = 0;
                for (int i = 0; i < gridView1.DataRowCount; i++)
                {
                    if (gridView1.GetRowCellValue(i, "No Purchase") == null)
                    {
                        continue;
                    }

                    if (gridView1.GetRowCellValue(i, "No Purchase").ToString() == "")
                    {
                        continue;
                    }

                    String strngNo = gridView1.GetRowCellValue(i, "No").ToString();
                    String strngPurchase = gridView1.GetRowCellValue(i, "No Purchase").ToString();
                    decimal dblGrandTotal = Tools.getRoundMoney(decimal.Parse(gridView1.GetRowCellValue(i, "Grand Total").ToString()));
                    decimal dblTelahDibayar = Tools.getRoundMoney(decimal.Parse(gridView1.GetRowCellValue(i, "Telah Dibayar").ToString()));
                    decimal dblNominalBayar = Tools.getRoundMoney(decimal.Parse(gridView1.GetRowCellValue(i, "Nominal Bayar").ToString()));

                    dblTotalNominalBayar = Tools.getRoundMoney(dblTotalNominalBayar + dblNominalBayar);

                    // simpan detail
                    DataPurchasePaymentDetail dPurchasePaymentDetail = new DataPurchasePaymentDetail(command, strngKode, strngNo);
                    dPurchasePaymentDetail.purchase = strngPurchase;
                    dPurchasePaymentDetail.grandtotal = dblGrandTotal.ToString();
                    dPurchasePaymentDetail.telahdibayar = dblTelahDibayar.ToString();
                    dPurchasePaymentDetail.nominalbayar = dblNominalBayar.ToString();
                    dPurchasePaymentDetail.tambah();

                    // tulis log detail
                    OswLog.setTransaksi(command, dokumenDetail, dPurchasePaymentDetail.ToString());
                }

                // Update header
                dPurchasePayment = new DataPurchasePayment(command, strngKode);
                dPurchasePayment.grandtotal = dblTotalNominalBayar.ToString();
                dPurchasePayment.ubah();

                // validasi setelah simpan
                dPurchasePayment.valJumlahDetail();
                dPurchasePayment.prosesJurnal();

                // tulis log
                OswLog.setTransaksi(command, dokumen, dPurchasePayment.ToString());

                // reload grid di form header
                FrmPurchasePayment frmPurchasePayment = (FrmPurchasePayment)this.Owner;
                frmPurchasePayment.setGrid(command);

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



        private void gridControl1_ProcessGridKey(object sender, KeyEventArgs e)
        {
            GridView gridView = gridView1;
            if (e.KeyCode == Keys.F1 && gridView.FocusedColumn.FieldName == "No Purchase")
            {
                infoPurchase();
            }
        }

        private void gridView1_FocusedColumnChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventArgs e)
        {
            GridView gridView = sender as GridView;

            if (gridView.GetRowCellValue(gridView.FocusedRowHandle, "No Purchase") == null)
            {
                gridView.FocusedColumn = gridView.Columns["No Purchase"];
                return;
            }

            if (gridView.FocusedColumn.FieldName != "No Purchase" && gridView.GetRowCellValue(gridView.FocusedRowHandle, "No Purchase").ToString() == "")
            {
                gridView.FocusedColumn = gridView.Columns["No Purchase"];
                return;
            }

            setFooter();
        }

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            GridView gridView = sender as GridView;

            if (gridView.GetRowCellValue(gridView.FocusedRowHandle, "No Purchase") == null)
            {
                gridView.FocusedColumn = gridView.Columns["No Purchase"];
                return;
            }

            if (gridView.FocusedColumn.FieldName != "No Purchase" && gridView.GetRowCellValue(gridView.FocusedRowHandle, "No Purchase").ToString() == "")
            {
                gridView.FocusedColumn = gridView.Columns["No Purchase"];
                return;
            }

            setFooter();
        }

        private void gridView1_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            GridView gridView = sender as GridView;

            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["No"], gridView.DataRowCount + 1);
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["No Purchase"], "");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Proyek ID"], "");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Nama Proyek"], "");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Grand Total"], "0");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Telah Dibayar"], "0");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Nominal Bayar"], "0");
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

            if (gridView.GetRowCellValue(gridView.FocusedRowHandle, "No Purchase") == null)
            {
                gridView.FocusedColumn = gridView.Columns["No Purchase"];
                return;
            }

            if (gridView.GetRowCellValue(gridView.FocusedRowHandle, "No Purchase").ToString() == "")
            {
                gridView.FocusedColumn = gridView.Columns["No Purchase"];
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
                    if (gridView.GetRowCellValue(i, "No Purchase") == null)
                    {
                        continue;
                    }

                    if (gridView.GetRowCellValue(i, "No Purchase").ToString() == "")
                    {
                        continue;
                    }

                    decimal dblNominalBayar = Tools.getRoundMoney(decimal.Parse(gridView.GetRowCellValue(i, "Nominal Bayar").ToString()));

                    dblGrandTotal = Tools.getRoundMoney(dblGrandTotal + dblNominalBayar);
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

        private void btnCariOutsource_Click(object sender, EventArgs e)
        {
            infoOutsource();
        }

        private void infoOutsource()
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
                String query = @"SELECT A.kode AS 'Kode Outsource',A.nama AS Outsource, A.alamat AS Alamat, A.provinsi AS Provinsi, A.kota AS Kota, A.kodepos AS 'Kode Pos', 
                                        A.telp AS Telp, A.handphone AS Handphone, A.email AS Email
                                FROM outsource A
                                ORDER BY A.kode";

                Dictionary<String, String> parameters = new Dictionary<String, String>();

                InfUtamaDictionary form = new InfUtamaDictionary("Info Outsource", query, parameters,
                                                                new String[] { "Kode Outsource" },
                                                                new String[] { "Kode Outsource" },
                                                                new String[] { });
                this.AddOwnedForm(form);
                form.ShowDialog();
                if (!form.hasil.ContainsKey("Kode Outsource"))
                {
                    return;
                }

                String strngKodeKlien = form.hasil["Kode Outsource"];

                DataOutsource dOutsource = new DataOutsource(command, strngKodeKlien);
                txtKodeOutsource.EditValue = strngKodeKlien;
                txtNama.EditValue = dOutsource.nama;
                txtAlamat.Text = dOutsource.alamat;
                txtProvinsi.Text = dOutsource.provinsi;
                txtKota.Text = dOutsource.kota;
                txtKodePos.Text = dOutsource.kodepos;
                txtTelepon.Text = dOutsource.telp;
                txtHandphone.Text = dOutsource.handphone;
                txtEmail.Text = dOutsource.email;

                // reset grid
                setGrid(command);

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
    }
}