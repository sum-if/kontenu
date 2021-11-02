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
using Kontenu.Umum;
using Kontenu.Umum.Laporan;
using DevExpress.XtraEditors.Controls;
using DevExpress.Utils;
using DevExpress.XtraGrid.Columns;
using Kontenu.Master;

namespace Kontenu.Design
{
    public partial class FrmQuotationAddJasaDetail : DevExpress.XtraEditors.XtraForm
    {
        private String id = "QUOTATION";
        private String dokumen = "Form Detail Keterangan Jasa";
        private String quotation = "";
        private String no = "";

        public FrmQuotationAddJasaDetail(String quotation, String no)
        {
            InitializeComponent();

            this.quotation = quotation;
            this.no = no;
        }

        private void FrmQuotationAddJasaDetail_Load(object sender, EventArgs e)
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
                this.Text = this.dokumen;

                OswControlDefaultProperties.setInput(this, id, command);

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
            DataQuotationDetail dQuotationDetail = new DataQuotationDetail(command, this.quotation, this.no);
            DataJasa dJasa = new DataJasa(command, dQuotationDetail.jasa);
            lblJasa.Text = dJasa.nama;

            DataQuotation dQuotation = new DataQuotation(command, this.quotation);
            if (dQuotation.status == Constants.STATUS_QUOTATION_TUTUP)
            {
                btnSimpan.Enabled = false;
            }

            
            this.setGrid(command);
        }

        public void setGrid(MySqlCommand command)
        {
            String query = @"SELECT A.no AS No, A.keterangan AS Keterangan
                            FROM quotationdetailjasa A
                            WHERE A.quotation = @quotation AND A.quotationdetailno = @quotationdetailno
                            ORDER BY A.no";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("quotation", this.quotation);
            parameters.Add("quotationdetailno", this.no);

            Dictionary<String, int> widths = new Dictionary<String, int>();
            // 386 - 21 (kiri) - 17 (vertikal lines) - 35 (No) = 927
            widths.Add("Keterangan", 348);

            Dictionary<String, String> inputType = new Dictionary<string, string>();

            OswGrid.getGridInput(gridControl1, command, query, parameters, widths, inputType,
                                 new String[] { "No" },
                                 new String[] { });
        }

        private void btnSimpan_Click(object sender, EventArgs e)
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
                DataQuotationDetail dQuotationDetail = new DataQuotationDetail(command, this.quotation, this.no);
                dQuotationDetail.hapusDetail();

                GridView gridView = gridView1;
                for (int i = 0; i < gridView.DataRowCount; i++)
                {
                    if (gridView.GetRowCellValue(i, "Keterangan").ToString() == "")
                    {
                        continue;
                    }

                    String strngNo = gridView.GetRowCellValue(i, "No").ToString();
                    String strngKeterangan = gridView.GetRowCellValue(i, "Keterangan").ToString();

                    DataQuotationDetailJasa dQuotationDetailJasa = new DataQuotationDetailJasa(command, this.quotation, this.no, strngNo);
                    dQuotationDetailJasa.keterangan = strngKeterangan;
                    dQuotationDetailJasa.tambah();

                    // tulis log detail
                    // OswLog.setTransaksi(command, dokumen, dQuotationDetailJasa.ToString());
                }

                // Commit Transaction
                command.Transaction.Commit();

                OswPesan.pesanInfo("Proses simpan perincian berhasil.");

                this.Close();
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

        private void gridView1_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            GridView gridView = sender as GridView;

            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["No"], gridView.DataRowCount + 1);
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Keterangan"], "");
        }

        private void gridView1_RowDeleted(object sender, DevExpress.Data.RowDeletedEventArgs e)
        {
            GridView gridView = sender as GridView;
            for (int no = 1; no <= gridView.DataRowCount; no++)
            {
                gridView.SetRowCellValue(no - 1, "No", no);
            }
        }
    }
}