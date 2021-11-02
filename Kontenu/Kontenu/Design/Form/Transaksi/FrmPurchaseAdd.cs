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

                // PURCHASE
                DataPurchase dPurchase = new DataPurchase(command, strngKode);
                deTanggal.DateTime = OswDate.getDateTimeFromStringTanggal(dPurchase.tanggal);

                // PROYEK
                DataProyek dProyek = new DataProyek(command, dPurchase.proyek);
                txtProyekKode.Text = dProyek.kode;
                txtProyekNama.Text = dProyek.nama;
                txtProyekAlamat.Text = dProyek.alamat;
                txtProyekKota.Text = dProyek.kota;
                txtProyekProvinsi.Text = dProyek.provinsi;
                txtProyekKodePos.Text = dProyek.provinsi;

                DataTujuanProyek dTujuanProyek = new DataTujuanProyek(command, dProyek.tujuanproyek);
                DataJenisProyek dJenisProyek = new DataJenisProyek(command, dProyek.jenisproyek);
                DataPIC dPIC = new DataPIC(command, dProyek.pic);

                txtProyekTujuan.Text = dTujuanProyek.nama;
                txtProyekJenis.Text = dJenisProyek.nama;
                txtProyekPIC.Text = dPIC.nama;

                // OUTSOURCE
                DataOutsource dOutsource = new DataOutsource(command, dPurchase.outsource);
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

            String query = @"SELECT A.no AS No, B.kode AS 'Kode Jasa Outsource', B.nama AS 'Jasa Outsource', A.deskripsi AS Deskripsi, 
                                    A.jumlah AS Qty, C.kode 'Kode Unit', C.nama AS Unit, A.rate AS Rate, A.subtotal AS Subtotal
                            FROM purchasedetail A
                            INNER JOIN jasaoutsource B ON A.jasaoutsource = B.kode
                            INNER JOIN unit C ON A.unit = C.kode
                            WHERE A.purchase = @kode
                            ORDER BY A.no";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("kode", strngKode);

            Dictionary<String, int> widths = new Dictionary<String, int>();
            // 960 - 21 (kiri) - 17 (vertikal lines) - 35 (No) = 927
            widths.Add("Jasa Outsource", 220);
            widths.Add("Deskripsi", 292);
            widths.Add("Qty", 80);
            widths.Add("Unit", 100);
            widths.Add("Rate", 110);
            widths.Add("Subtotal", 120);

            Dictionary<String, String> inputType = new Dictionary<string, string>();
            inputType.Add("Qty", OswInputType.NUMBER);
            inputType.Add("Rate", OswInputType.NUMBER);
            inputType.Add("Subtotal", OswInputType.NUMBER);

            OswGrid.getGridInput(gridControl1, command, query, parameters, widths, inputType,
                                 new String[] { "No", "Kode Jasa Outsource", "Kode Unit" },
                                 new String[] { "Unit", "Subtotal" });

            // search produk di kolom kode
            RepositoryItemButtonEdit searchJasa = new RepositoryItemButtonEdit();
            searchJasa.Buttons[0].Kind = ButtonPredefines.Glyph;
            searchJasa.Buttons[0].Image = OswResources.getImage("cari-16.png", typeof(Program).Assembly);
            searchJasa.Buttons[0].Visible = true;
            searchJasa.ButtonClick += searchJasa_ButtonClick;

            GridView gridView = gridView1;
            gridView.Columns["Jasa Outsource"].ColumnEdit = searchJasa;
            gridView.Columns["Jasa Outsource"].ColumnEdit.ReadOnly = true;

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
            dxValidationProvider1.SetValidationRule(txtKodeOutsource, OswValidation.IsNotBlank());
            dxValidationProvider1.SetValidationRule(txtProyekKode, OswValidation.IsNotBlank());
            dxValidationProvider1.SetValidationRule(txtProyekNama, OswValidation.IsNotBlank());
            dxValidationProvider1.SetValidationRule(txtKodeOutsource, OswValidation.IsNotBlank());

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
                String strngProyek = txtProyekKode.Text;
                String strngOutsource = txtKodeOutsource.Text;

                DataPurchase dPurchase = new DataPurchase(command, strngKode);
                dPurchase.tanggal = strngTanggal;
                dPurchase.outsource = strngOutsource;
                dPurchase.proyek = strngProyek;

                if (this.isAdd)
                {
                    dPurchase.status = Constants.STATUS_PURCHASE_BELUM_LUNAS;
                    dPurchase.tambah();

                    // update kode header --> setelah generate
                    strngKode = dPurchase.kode;
                    txtKode.Text = strngKode;
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
                    if (gridView1.GetRowCellValue(i, "Jasa Outsource") == null)
                    {
                        continue;
                    }

                    if (gridView1.GetRowCellValue(i, "Jasa Outsource").ToString() == "")
                    {
                        continue;
                    }

                    String strngNo = gridView1.GetRowCellValue(i, "No").ToString();
                    String strngKodeJasaOutsource = gridView1.GetRowCellValue(i, "Kode Jasa Outsource").ToString();
                    String strngDeskripsi = gridView1.GetRowCellValue(i, "Deskripsi").ToString();
                    String strngKodeUnit = gridView1.GetRowCellValue(i, "Kode Unit").ToString();
                    decimal dblJumlah = Tools.getRoundMoney(decimal.Parse(gridView1.GetRowCellValue(i, "Qty").ToString()));
                    decimal dblRate = Tools.getRoundMoney(decimal.Parse(gridView1.GetRowCellValue(i, "Rate").ToString()));

                    decimal dblSubtotal = Tools.getRoundMoney(dblJumlah * dblRate);
                    dblGrandTotal = Tools.getRoundMoney(dblGrandTotal + dblSubtotal);

                    // simpan detail
                    DataPurchaseDetail dPurchaseDetail = new DataPurchaseDetail(command, strngKode, strngNo);
                    dPurchaseDetail.jasaoutsource = strngKodeJasaOutsource;
                    dPurchaseDetail.deskripsi = strngDeskripsi;
                    dPurchaseDetail.jumlah = dblJumlah.ToString();
                    dPurchaseDetail.unit = strngKodeUnit;
                    dPurchaseDetail.rate = dblRate.ToString();
                    dPurchaseDetail.subtotal = dblSubtotal.ToString();
                    dPurchaseDetail.tambah();

                    // tulis log detail
                    // OswLog.setTransaksi(command, dokumenDetail, dPurchaseDetail.ToString());
                }

                // Update header
                dPurchase = new DataPurchase(command, strngKode);
                dPurchase.grandtotal = dblGrandTotal.ToString();
                dPurchase.ubah();

                // validasi setelah simpan
                dPurchase.valJumlahDetail();
                dPurchase.prosesJurnal();

                // tulis log
                // OswLog.setTransaksi(command, dokumen, dPurchase.ToString());

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
	                                FROM jasaoutsource A
	                                INNER JOIN unit B ON A.unit = B.kode
                                    ORDER BY A.nama";

                Dictionary<String, String> parameters = new Dictionary<String, String>();

                InfUtamaDataTable form = new InfUtamaDataTable("Info Jasa Outsource", query, parameters,
                                                                new String[] { "Kode", "Kode Unit" },
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
                    gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Kode Jasa Outsource"], strngKode);
                    gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Jasa Outsource"], strngNama);
                    gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Kode Unit"], strngKodeUnit);
                    gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Unit"], strngUnit);
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
            if (e.KeyCode == Keys.F1 && gridView.FocusedColumn.FieldName == "Jasa Outsource")
            {
                infoJasa();
            }
        }

        private void gridView1_FocusedColumnChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventArgs e)
        {
            GridView gridView = sender as GridView;

            if (gridView.GetRowCellValue(gridView.FocusedRowHandle, "Jasa Outsource") == null)
            {
                gridView.FocusedColumn = gridView.Columns["Jasa Outsource"];
                return;
            }

            if (gridView.FocusedColumn.FieldName != "Jasa Outsource" && gridView.GetRowCellValue(gridView.FocusedRowHandle, "Jasa Outsource").ToString() == "")
            {
                gridView.FocusedColumn = gridView.Columns["Jasa Outsource"];
                return;
            }

            setFooter();
        }

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            GridView gridView = sender as GridView;

            if (gridView.GetRowCellValue(gridView.FocusedRowHandle, "Jasa Outsource") == null)
            {
                gridView.FocusedColumn = gridView.Columns["Jasa Outsource"];
                return;
            }

            if (gridView.FocusedColumn.FieldName != "Jasa Outsource" && gridView.GetRowCellValue(gridView.FocusedRowHandle, "Jasa Outsource").ToString() == "")
            {
                gridView.FocusedColumn = gridView.Columns["Jasa Outsource"];
                return;
            }

            setFooter();
        }

        private void gridView1_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            GridView gridView = sender as GridView;

            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["No"], gridView.DataRowCount + 1);
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Kode Jasa Outsource"], "");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Jasa Outsource"], "");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Deskripsi"], "");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Kode Unit"], "");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Unit"], "");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Qty"], "0");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Rate"], "0");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Subtotal"], "0");
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

            if (gridView.GetRowCellValue(gridView.FocusedRowHandle, "Jasa Outsource") == null)
            {
                gridView.FocusedColumn = gridView.Columns["Jasa Outsource"];
                return;
            }

            if (gridView.GetRowCellValue(gridView.FocusedRowHandle, "Jasa Outsource").ToString() == "")
            {
                gridView.FocusedColumn = gridView.Columns["Jasa Outsource"];
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
                    if (gridView.GetRowCellValue(i, "Jasa Outsource") == null)
                    {
                        continue;
                    }

                    if (gridView.GetRowCellValue(i, "Jasa Outsource").ToString() == "")
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

        private void btnCariProyek_Click(object sender, EventArgs e)
        {
            infoProyek();
        }

        private void infoProyek()
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
                String query = @"SELECT A.kode AS Kode, A.nama AS Nama, A.tanggaldeal AS 'Tanggal Deal', A.alamat AS Alamat, A.kota AS Kota, A.provinsi AS Provinsi, A.kodepos AS 'Kode Pos',
                                        B.nama AS Klien, C.nama AS 'Tujuan Proyek', D.nama AS 'Jenis Proyek', E.nama AS PIC
                                FROM proyek A
                                INNER JOIN klien B ON A.klien = B.kode
                                INNER JOIN tujuanproyek C ON A.tujuanproyek = C.kode
                                INNER JOIN jenisproyek D ON A.jenisproyek = D.kode
                                INNER JOIN pic E ON A.pic = E.kode
                                WHERE A.status = @status
                                ORDER BY A.kode";

                Dictionary<String, String> parameters = new Dictionary<String, String>();
                parameters.Add("status", Constants.STATUS_PROYEK_AKTIF);

                InfUtamaDictionary form = new InfUtamaDictionary("Info Proyek", query, parameters,
                                                                new String[] { "Kode", "Nama", "Alamat", "Kota", "Provinsi", "Kode Pos", "Klien", "Tujuan Proyek", "Jenis Proyek", "PIC" },
                                                                new String[] { "Kode" },
                                                                new String[] { });
                this.AddOwnedForm(form);
                form.ShowDialog();
                if (!form.hasil.ContainsKey("Kode"))
                {
                    return;
                }

                String strngKode = form.hasil["Kode"];
                String strngNama = form.hasil["Nama"];
                String strngAlamat = form.hasil["Alamat"];
                String strngKota = form.hasil["Kota"];
                String strngProvinsi = form.hasil["Provinsi"];
                String strngKodePos = form.hasil["Kode Pos"];
                String strngKlien = form.hasil["Klien"];
                String strngTujuanProyek = form.hasil["Tujuan Proyek"];
                String strngJenisProyek = form.hasil["Jenis Proyek"];
                String strngPIC = form.hasil["PIC"];

                txtProyekKode.Text = strngKode;
                txtProyekNama.Text = strngNama;
                txtProyekAlamat.Text = strngAlamat;
                txtProyekKota.Text = strngKota;
                txtProyekProvinsi.Text = strngProvinsi;
                txtProyekKodePos.Text = strngKodePos;
                txtProyekTujuan.Text = strngTujuanProyek;
                txtProyekJenis.Text = strngJenisProyek;
                txtProyekPIC.Text = strngPIC;

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