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
    public partial class FrmFinalisasiProyekAdd : DevExpress.XtraEditors.XtraForm
    {
        private String id = "FINALISASIPROYEK";
        private String dokumen = "FINALISASIPROYEK";
        private String dokumenDetail = "FINALISASIPROYEK";
        private Boolean isAdd;

        public FrmFinalisasiProyekAdd(bool pIsAdd)
        {
            isAdd = pIsAdd;
            InitializeComponent();
        }

        private void FrmFinalisasiProyekAdd_Load(object sender, EventArgs e)
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

                // FINALISASIPROYEK
                DataFinalisasiProyek dFinalisasiProyek = new DataFinalisasiProyek(command, strngKode);
                deTanggal.DateTime = OswDate.getDateTimeFromStringTanggal(dFinalisasiProyek.tanggal);

                // PROYEK
                DataProyek dProyek = new DataProyek(command, dFinalisasiProyek.proyek);
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

                // KLIEN
                DataKlien dKlien = new DataKlien(command, dProyek.klien);
                txtKodeKlien.EditValue = dKlien.kode;
                txtNama.EditValue = dKlien.nama;
                txtAlamat.Text = dKlien.alamat;
                txtProvinsi.Text = dKlien.provinsi;
                txtKota.Text = dKlien.kota;
                txtKodePos.Text = dKlien.kodepos;
                txtTelepon.Text = dKlien.telp;
                txtHandphone.Text = dKlien.handphone;
                txtEmail.Text = dKlien.email;
            }
            else
            {
                OswControlDefaultProperties.resetAllInput(this);
                deTanggal.EditValue = "";
            }

            this.setGridPenjualan(command);
            this.setGridPurchase(command);

            txtKode.Focus();
        }

        public void setGridPenjualan(MySqlCommand command, bool info = false)
        {
            String strngKode = txtKode.Text;

            String query = @"SELECT B.kode AS 'No Invoice', B.tanggal AS Tanggal, B.jenis AS 'Jenis Invoice', 
                                    A.totalinvoice AS 'Total Invoice', A.totalditerima AS 'Total Diterima', A.sisa AS Sisa
                            FROM finalisasiproyekpenjualan A
                            INNER JOIN invoice B ON A.invoice = B.kode
                            WHERE A.finalisasiproyek = @kode
                            ORDER BY A.no";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("kode", strngKode);

            if (info)
            {
                String strngKodeProyek = txtProyekKode.Text;

                query = @"SELECT A.kode AS 'No Invoice', A.tanggal AS Tanggal, A.jenis AS 'Jenis Invoice', 
                                    A.grandtotal AS 'Total Invoice', B.totalterima AS 'Total Diterima', (A.grandtotal - B.totalterima) AS Sisa
                            FROM invoice A
                            LEFT JOIN v_invoice_totalterima B ON A.kode = B.invoice
                            WHERE A.proyek = @proyek               
                            ORDER BY A.kode";

                parameters.Add("proyek", strngKodeProyek);
            }
            

            Dictionary<String, int> widths = new Dictionary<String, int>();
            // 929 - 21 (kiri) - 17 (vertikal lines) - 35 (No) = 927
            widths.Add("No Invoice", 211);
            widths.Add("Tanggal", 110);
            widths.Add("Jenis Invoice", 180);
            widths.Add("Total Invoice", 130);
            widths.Add("Total Diterima", 130);
            widths.Add("Sisa", 130);

            Dictionary<String, String> inputType = new Dictionary<string, string>();
            inputType.Add("Total Invoice", OswInputType.NUMBER);
            inputType.Add("Total Diterima", OswInputType.NUMBER);
            inputType.Add("Sisa", OswInputType.NUMBER);

            OswGrid.getGridInput(gridControl1, command, query, parameters, widths, inputType,
                                 new String[] { },
                                 new String[] { "No Invoice", "Tanggal", "Jenis Invoice", "Total Invoice", "Total Diterima", "Sisa" },
                                 false);

            setFooterPenjualan();
        }

        public void setGridPurchase(MySqlCommand command, bool info = false)
        {
            String strngKode = txtKode.Text;

            String query = @"SELECT B.kode AS 'No Purchase', B.tanggal AS Tanggal, C.nama AS 'Outsource', 
                                    A.totalpurchase AS 'Total Purchase', A.totalbayar AS 'Total Bayar', A.sisa AS Sisa
                            FROM finalisasiproyekpurchase A
                            INNER JOIN purchase B ON A.purchase = B.kode
                            INNER JOIN outsource C ON B.outsource = C.kode
                            WHERE A.finalisasiproyek = @kode
                            ORDER BY A.no";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("kode", strngKode);

            if (info)
            {
                String strngKodeProyek = txtProyekKode.Text;

                query = @"SELECT A.kode AS 'No Purchase', A.tanggal AS Tanggal, C.nama AS 'Outsource', 
                                    A.grandtotal AS 'Total Purchase', B.totalbayar AS 'Total Bayar', (A.grandtotal - B.totalbayar) AS Sisa
                            FROM purchase A
                            INNER JOIN outsource C ON A.outsource = C.kode
                            LEFT JOIN v_purchase_totalbayar B ON A.kode = B.purchase
                            WHERE A.proyek = @proyek               
                            ORDER BY A.kode";

                parameters.Add("proyek", strngKodeProyek);
            }


            Dictionary<String, int> widths = new Dictionary<String, int>();
            // 929 - 21 (kiri) - 17 (vertikal lines) - 35 (No) = 927
            widths.Add("No Purchase", 211);
            widths.Add("Tanggal", 110);
            widths.Add("Outsource", 180);
            widths.Add("Total Purchase", 130);
            widths.Add("Total Bayar", 130);
            widths.Add("Sisa", 130);

            Dictionary<String, String> inputType = new Dictionary<string, string>();
            inputType.Add("Total Purchase", OswInputType.NUMBER);
            inputType.Add("Total Bayar", OswInputType.NUMBER);
            inputType.Add("Sisa", OswInputType.NUMBER);

            OswGrid.getGridInput(gridControl2, command, query, parameters, widths, inputType,
                                 new String[] { },
                                 new String[] { "No Purchase", "Tanggal", "Outsource", "Total Purchase", "Total Bayar", "Sisa" },
                                 false);

            setFooterPurchase();
        }


        private void btnSimpan_Click(object sender, EventArgs e)
        {
            // validation
            dxValidationProvider1.SetValidationRule(deTanggal, OswValidation.IsNotBlank());
            dxValidationProvider1.SetValidationRule(txtProyekKode, OswValidation.IsNotBlank());
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
                String strngProyek = txtProyekKode.Text;

                DataFinalisasiProyek dFinalisasiProyek = new DataFinalisasiProyek(command, strngKode);
                dFinalisasiProyek.tanggal = strngTanggal;
                dFinalisasiProyek.proyek = strngProyek;

                if (this.isAdd)
                {
                    dFinalisasiProyek.tambah();

                    // update kode header --> setelah generate
                    strngKode = dFinalisasiProyek.kode;
                    txtKode.Text = strngKode;
                }
                else
                {
                    dFinalisasiProyek.hapusDetail();
                    dFinalisasiProyek.ubah();
                }

                // SIMPAN DETAIL PENJUALAN
                setFooterPenjualan();

                decimal dblPenjualanTotalInvoice = 0;
                decimal dblPenjualanTotalDiterima = 0;
                decimal dblPenjualanTotalSisa = 0;
                decimal noUrut = 0;
                for (int i = 0; i < gridView1.DataRowCount; i++)
                {
                    if (gridView1.GetRowCellValue(i, "No Invoice") == null)
                    {
                        continue;
                    }

                    if (gridView1.GetRowCellValue(i, "No Invoice").ToString() == "")
                    {
                        continue;
                    }

                    String strngNoInvoice = gridView1.GetRowCellValue(i, "No Invoice").ToString();
                    decimal dblInvoice = Tools.getRoundMoney(decimal.Parse(gridView1.GetRowCellValue(i, "Total Invoice").ToString()));
                    decimal dblDiterima = Tools.getRoundMoney(decimal.Parse(gridView1.GetRowCellValue(i, "Total Diterima").ToString()));
                    decimal dblSisa = Tools.getRoundMoney(decimal.Parse(gridView1.GetRowCellValue(i, "Sisa").ToString()));

                    dblPenjualanTotalInvoice = Tools.getRoundMoney(dblPenjualanTotalInvoice + dblInvoice);
                    dblPenjualanTotalDiterima = Tools.getRoundMoney(dblPenjualanTotalDiterima + dblDiterima);
                    dblPenjualanTotalSisa = Tools.getRoundMoney(dblPenjualanTotalSisa + dblSisa);

                    // simpan detail
                    DataFinalisasiProyekPenjualan dFinalisasiProyekPenjualan = new DataFinalisasiProyekPenjualan(command, strngKode, noUrut++.ToString());
                    dFinalisasiProyekPenjualan.invoice = strngNoInvoice;
                    dFinalisasiProyekPenjualan.totalinvoice = dblInvoice.ToString();
                    dFinalisasiProyekPenjualan.totalditerima = dblDiterima.ToString();
                    dFinalisasiProyekPenjualan.sisa = dblSisa.ToString();
                    dFinalisasiProyekPenjualan.tambah();

                    // tulis log detail
                    // OswLog.setTransaksi(command, dokumenDetail, dFinalisasiProyekDetail.ToString());
                }

                // SIMPAN DETAIL PURCHASE
                setFooterPurchase();

                decimal dblPurchaseTotalPurchase = 0;
                decimal dblPurchaseTotalBayar = 0;
                decimal dblPurchaseTotalSisa = 0;
                noUrut = 0;
                for (int i = 0; i < gridView2.DataRowCount; i++)
                {
                    if (gridView2.GetRowCellValue(i, "No Purchase") == null)
                    {
                        continue;
                    }

                    if (gridView2.GetRowCellValue(i, "No Purchase").ToString() == "")
                    {
                        continue;
                    }

                    String strngNoPurchase = gridView2.GetRowCellValue(i, "No Purchase").ToString();
                    decimal dblPurchase = Tools.getRoundMoney(decimal.Parse(gridView2.GetRowCellValue(i, "Total Purchase").ToString()));
                    decimal dblBayar = Tools.getRoundMoney(decimal.Parse(gridView2.GetRowCellValue(i, "Total Bayar").ToString()));
                    decimal dblSisa = Tools.getRoundMoney(decimal.Parse(gridView2.GetRowCellValue(i, "Sisa").ToString()));

                    dblPurchaseTotalPurchase = Tools.getRoundMoney(dblPurchaseTotalPurchase + dblPurchase);
                    dblPurchaseTotalBayar = Tools.getRoundMoney(dblPurchaseTotalBayar + dblBayar);
                    dblPurchaseTotalSisa = Tools.getRoundMoney(dblPurchaseTotalSisa + dblSisa);

                    // simpan detail
                    DataFinalisasiProyekPurchase dFinalisasiProyekPurchase = new DataFinalisasiProyekPurchase(command, strngKode, noUrut++.ToString());
                    dFinalisasiProyekPurchase.purchase = strngNoPurchase;
                    dFinalisasiProyekPurchase.totalpurchase = dblPurchase.ToString();
                    dFinalisasiProyekPurchase.totalbayar = dblBayar.ToString();
                    dFinalisasiProyekPurchase.sisa = dblSisa.ToString();
                    dFinalisasiProyekPurchase.tambah();

                    // tulis log detail
                    // OswLog.setTransaksi(command, dokumenDetail, dFinalisasiProyekDetail.ToString());
                }

                // Update header
                dFinalisasiProyek = new DataFinalisasiProyek(command, strngKode);
                dFinalisasiProyek.penjualantotalinvoice = dblPenjualanTotalInvoice.ToString();
                dFinalisasiProyek.penjualantotalditerima = dblPenjualanTotalDiterima.ToString();
                dFinalisasiProyek.penjualansisa = dblPenjualanTotalSisa.ToString();

                dFinalisasiProyek.purchasetotalpurchase = dblPurchaseTotalPurchase.ToString();
                dFinalisasiProyek.purchasetotalbayar = dblPurchaseTotalBayar.ToString();
                dFinalisasiProyek.purchasesisa = dblPurchaseTotalSisa.ToString();

                dFinalisasiProyek.ubah();

                // validasi setelah simpan
                dFinalisasiProyek.valJumlahDetail();
                dFinalisasiProyek.prosesJurnal();
                dFinalisasiProyek.prosesTutupProyek();

                // tulis log
                // OswLog.setTransaksi(command, dokumen, dFinalisasiProyek.ToString());

                // reload grid di form header
                FrmFinalisasiProyek frmFinalisasiProyek = (FrmFinalisasiProyek)this.Owner;
                frmFinalisasiProyek.setGrid(command);

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
            if (e.KeyCode == Keys.F1 && gridView.FocusedColumn.FieldName == "No Invoice")
            {
            }
        }

        private void gridView1_FocusedColumnChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventArgs e)
        {
            GridView gridView = sender as GridView;

            if (gridView.GetRowCellValue(gridView.FocusedRowHandle, "No Invoice") == null)
            {
                gridView.FocusedColumn = gridView.Columns["No Invoice"];
                return;
            }

            if (gridView.FocusedColumn.FieldName != "No Invoice" && gridView.GetRowCellValue(gridView.FocusedRowHandle, "No Invoice").ToString() == "")
            {
                gridView.FocusedColumn = gridView.Columns["No Invoice"];
                return;
            }

            setFooterPenjualan();
        }

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            GridView gridView = sender as GridView;

            if (gridView.GetRowCellValue(gridView.FocusedRowHandle, "No Invoice") == null)
            {
                gridView.FocusedColumn = gridView.Columns["No Invoice"];
                return;
            }

            if (gridView.FocusedColumn.FieldName != "No Invoice" && gridView.GetRowCellValue(gridView.FocusedRowHandle, "No Invoice").ToString() == "")
            {
                gridView.FocusedColumn = gridView.Columns["No Invoice"];
                return;
            }

            setFooterPenjualan();
        }

        private void gridView1_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            GridView gridView = sender as GridView;

            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["No Invoice"], "");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Tanggal"], "");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Jenis Invoice"], "");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Total Invoice"], "0");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Total Diterima"], "0");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Sisa"], "0");
        }

        private void gridView1_RowDeleted(object sender, DevExpress.Data.RowDeletedEventArgs e)
        {
            setFooterPenjualan();
        }

        private void gridView1_ValidateRow(object sender, DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs e)
        {
            GridView gridView = gridView1;

            if (gridView.GetRowCellValue(gridView.FocusedRowHandle, "No Invoice") == null)
            {
                gridView.FocusedColumn = gridView.Columns["No Invoice"];
                return;
            }

            if (gridView.GetRowCellValue(gridView.FocusedRowHandle, "No Invoice").ToString() == "")
            {
                gridView.FocusedColumn = gridView.Columns["No Invoice"];
                return;
            }

            setFooterPenjualan();
        }

        private void setFooterPenjualan()
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

                decimal dblTotalInvoice = 0;
                decimal dblTotalDiterima = 0;
                decimal dblTotalSisa = 0;
                for (int i = 0; i < gridView.DataRowCount; i++)
                {
                    if (gridView.GetRowCellValue(i, "No Invoice") == null)
                    {
                        continue;
                    }

                    if (gridView.GetRowCellValue(i, "No Invoice").ToString() == "")
                    {
                        continue;
                    }

                    decimal dblInvoice = Tools.getRoundMoney(decimal.Parse(gridView.GetRowCellValue(i, "Total Invoice").ToString()));
                    decimal dblDiterima = Tools.getRoundMoney(decimal.Parse(gridView.GetRowCellValue(i, "Total Diterima").ToString()));
                    decimal dblSisa = Tools.getRoundMoney(decimal.Parse(gridView.GetRowCellValue(i, "Sisa").ToString()));

                    dblTotalInvoice = Tools.getRoundMoney(dblTotalInvoice + dblInvoice);
                    dblTotalDiterima = Tools.getRoundMoney(dblTotalDiterima + dblDiterima);
                    dblTotalSisa = Tools.getRoundMoney(dblTotalSisa + dblSisa);
                }

                lblPenjualanTotalInvoice.Text = OswConvert.convertToRupiah(dblTotalInvoice);
                lblPenjualanTotalDiterima.Text = OswConvert.convertToRupiah(dblTotalDiterima);
                lblPenjualanSisa.Text = OswConvert.convertToRupiah(dblTotalSisa);

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

        private void setFooterPurchase()
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
                GridView gridView = gridView2;

                decimal dblTotalPurchase = 0;
                decimal dblTotalBayar = 0;
                decimal dblTotalSisa = 0;
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

                    decimal dblPurchase = Tools.getRoundMoney(decimal.Parse(gridView.GetRowCellValue(i, "Total Purchase").ToString()));
                    decimal dblBayar = Tools.getRoundMoney(decimal.Parse(gridView.GetRowCellValue(i, "Total Bayar").ToString()));
                    decimal dblSisa = Tools.getRoundMoney(decimal.Parse(gridView.GetRowCellValue(i, "Sisa").ToString()));

                    dblTotalPurchase = Tools.getRoundMoney(dblTotalPurchase + dblPurchase);
                    dblTotalBayar = Tools.getRoundMoney(dblTotalBayar + dblBayar);
                    dblTotalSisa = Tools.getRoundMoney(dblTotalSisa + dblSisa);
                }

                lblPurchaseTotalPurchase.Text = OswConvert.convertToRupiah(dblTotalPurchase);
                lblPurchaseTotalBayar.Text = OswConvert.convertToRupiah(dblTotalBayar);
                lblPurchaseSisa.Text = OswConvert.convertToRupiah(dblTotalSisa);

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
                                        B.kode AS 'Kode Klien',B.nama AS Klien, C.nama AS 'Tujuan Proyek', D.nama AS 'Jenis Proyek', E.nama AS PIC
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
                                                                new String[] { "Kode", "Nama", "Alamat", "Kota", "Provinsi", "Kode Pos", "Klien", "Tujuan Proyek", "Jenis Proyek", "PIC", "Kode Klien" },
                                                                new String[] { "Kode Klien", "Tujuan Proyek", "Jenis Proyek" },
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
                String strngKodeKlien = form.hasil["Kode Klien"];
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

                // KLIEN
                DataKlien dKlien = new DataKlien(command, strngKodeKlien);
                txtKodeKlien.EditValue = dKlien.kode;
                txtNama.EditValue = dKlien.nama;
                txtAlamat.Text = dKlien.alamat;
                txtProvinsi.Text = dKlien.provinsi;
                txtKota.Text = dKlien.kota;
                txtKodePos.Text = dKlien.kodepos;
                txtTelepon.Text = dKlien.telp;
                txtHandphone.Text = dKlien.handphone;
                txtEmail.Text = dKlien.email;

                setGridPenjualan(command, true);
                setGridPurchase(command, true);

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

        private void gridView2_FocusedColumnChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventArgs e)
        {
            GridView gridView = sender as GridView;

            if (gridView.GetRowCellValue(gridView.FocusedRowHandle, "No Purchase") == null)
            {
                gridView.FocusedColumn = gridView.Columns["No Purchase"];
                return;
            }

            if (gridView.FocusedColumn == null)
            {
                gridView.FocusedColumn = gridView.Columns["No Purchase"];
                return;
            }

            if (gridView.FocusedColumn.FieldName != "No Purchase" && gridView.GetRowCellValue(gridView.FocusedRowHandle, "No Purchase").ToString() == "")
            {
                gridView.FocusedColumn = gridView.Columns["No Purchase"];
                return;
            }

            setFooterPurchase();
        }

        private void gridView2_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
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

            setFooterPurchase();
        }

        private void gridView2_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            GridView gridView = sender as GridView;

            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["No Purchase"], "");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Tanggal"], "");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Outsource"], "");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Total Purchase"], "0");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Total Bayar"], "0");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Sisa"], "0");
        }

        private void gridView2_RowDeleted(object sender, DevExpress.Data.RowDeletedEventArgs e)
        {
            setFooterPurchase();
        }

        private void gridView2_ValidateRow(object sender, DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs e)
        {
            GridView gridView = gridView2;

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

            setFooterPurchase();
        }
    }
}