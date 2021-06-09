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

namespace Kontenu.Design {
    public partial class FrmQuotationAddDetail : DevExpress.XtraEditors.XtraForm {
        private String id = "QUOTATION";
        private String dokumen = "Perincian Quotation";
        private String jcs = "";
        private String idBarang = "";
        private String namaBarang = "";
        private bool isEdit = true;
        private bool isDashboard;

        public FrmQuotationAddDetail(String cabang, String jcs, String idBarang, String namaBarang, bool isEdit, bool pIsDashboard) {
            InitializeComponent();

            this.jcs = jcs;
            this.idBarang = idBarang;
            this.namaBarang = namaBarang;
            this.isEdit = isEdit;
            this.isDashboard = pIsDashboard;

            this.dokumen += " [" + namaBarang + "]";
        }

        private void FrmQuotationAddDetail_Load(object sender, EventArgs e) {
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
                this.Text = this.dokumen;

                OswControlDefaultProperties.setInput(this, id, command);

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
            //DataQuotationDetail dQuotationDetail = new DataQuotationDetail(command, this.jcs, this.idBarang);

            //this.setGrid(command);

            //if(!isEdit) {
            //    btnSimpan.Enabled = false;
            //}

            //if(isDashboard) {
            //    btnSimpan.Enabled = false;
            //}
        }

        public void setGridBarang(MySqlCommand command) {
//            String query = @"SELECT A.no AS No, A.nama AS Nama, A.jumlah AS Qty, A.satuan AS Satuan, 
//                                    A.hargaterakhir AS 'HB Terakhir', A.timeschedule AS 'Time Schedule', A.subtotal AS Subtotal
//                            FROM jcsdetailbarang A
//                            WHERE A.cabang = @cabang AND A.jcs = @jcs AND A.id = @id
//                            ORDER BY A.no";

//            Dictionary<String, String> parameters = new Dictionary<String, String>();
//            parameters.Add("cabang", cabang);
//            parameters.Add("jcs", jcs);
//            parameters.Add("id", idBarang);

//            Dictionary<String, int> widths = new Dictionary<String, int>();
//            // 790 - 21 (kiri) - 17 (vertikal lines) - 35 (No) = 927
//            widths.Add("No", 35);
//            widths.Add("Nama", 297);
//            widths.Add("Qty", 70);
//            widths.Add("Satuan", 70);
//            widths.Add("HB Terakhir", 80);
//            widths.Add("Time Schedule", 90);
//            widths.Add("Subtotal", 110);

//            Dictionary<String, String> inputType = new Dictionary<string, string>();
//            inputType.Add("Qty", OswInputType.NUMBER);
//            inputType.Add("HB Terakhir", OswInputType.NUMBER);
//            inputType.Add("Time Schedule", OswInputType.NUMBER);
//            inputType.Add("Subtotal", OswInputType.NUMBER);

//            OswGrid.getGridInput(gridControl1, command, query, parameters, widths, inputType,
//                                 new String[] {  },
//                                 new String[] { "No", "Subtotal" });

//            setFooter();
        }

        private void btnSimpan_Click(object sender, EventArgs e) {
            //SplashScreenManager.ShowForm(typeof(SplashUtama));
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
            //    setFooter();

            //    DataQuotationDetail dQuotationDetail = new DataQuotationDetail(command, this.cabang, this.jcs, this.idBarang);
            //    dQuotationDetail.hapusDetail();

            //    double dblTotalBarang = 0;
            //    double dblTotalBiaya = 0;

            //    GridView gridView = gridView1;
            //    for(int i = 0; i < gridView.DataRowCount; i++) {
            //        if(gridView.GetRowCellValue(i, "Nama").ToString() == "") {
            //            continue;
            //        }

            //        String strngNo = gridView.GetRowCellValue(i, "No").ToString();
            //        String strngNama = gridView.GetRowCellValue(i, "Nama").ToString();
            //        String strngQty = gridView.GetRowCellValue(i, "Qty").ToString();
            //        String strngSatuan = gridView.GetRowCellValue(i, "Satuan").ToString();
            //        String strngHBTerakhir = gridView.GetRowCellValue(i, "HB Terakhir").ToString();
            //        String strngTimeSchedule = gridView.GetRowCellValue(i, "Time Schedule").ToString();
            //        String strngSubtotal = gridView.GetRowCellValue(i, "Subtotal").ToString();

            //        dblTotalBarang = dblTotalBarang + double.Parse(strngSubtotal);

            //        DataQuotationDetailBarang dQuotationDetailBarang = new DataQuotationDetailBarang(command, this.cabang, this.jcs, this.idBarang, strngNo);
            //        dQuotationDetailBarang.nama = strngNama;
            //        dQuotationDetailBarang.jumlah = strngQty;
            //        dQuotationDetailBarang.satuan = strngSatuan;
            //        dQuotationDetailBarang.hargaterakhir = strngHBTerakhir;
            //        dQuotationDetailBarang.timeschedule = strngTimeSchedule;
            //        dQuotationDetailBarang.subtotal = strngSubtotal;
            //        dQuotationDetailBarang.tambah();

            //        // tulis log detail
            //        OswLog.setTransaksi(command, dokumen, dQuotationDetailBarang.ToString());
            //    }

            //    double dblTotal = dblTotalBarang + dblTotalBiaya;
            //    double dblProfit = double.Parse(txtProfit.EditValue.ToString());
            //    double dblTotalProfit = (dblTotal * dblProfit) / 100;

            //    dQuotationDetail = new DataQuotationDetail(command, this.cabang, this.jcs, this.idBarang);
            //    dQuotationDetail.profitpersen = dblProfit.ToString();
            //    dQuotationDetail.profit = dblTotalProfit.ToString();
            //    dQuotationDetail.ubah();

            //    // hitung
            //    DataQuotation dQuotation = new DataQuotation(command, this.cabang, this.jcs);
            //    dQuotation.prosesHitung();

            //    // tulis log
            //    OswLog.setTransaksi(command, dokumen, dQuotationDetail.ToString());

            //    // reload grid di form header
            //    FrmQuotationAdd frmQuotationAdd = (FrmQuotationAdd)this.Owner;
            //    frmQuotationAdd.setGrid(command);

            //    // Commit Transaction
            //    command.Transaction.Commit();

            //    OswPesan.pesanInfo("Proses simpan perincian berhasil.");

            //    this.Close();
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

        private void gridView1_FocusedColumnChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventArgs e) {
            GridView gridView = sender as GridView;

            if(gridView.GetRowCellValue(gridView.FocusedRowHandle, "Nama") == null) {
                gridView.FocusedColumn = gridView.Columns["Nama"];
                return;
            }

            if(gridView.FocusedColumn.FieldName != "Nama" && gridView.GetRowCellValue(gridView.FocusedRowHandle, "Nama").ToString() == "") {
                gridView.FocusedColumn = gridView.Columns["Nama"];
                return;
            }

            setCurrentDataRowBarang();
        }

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e) {
            GridView gridView = sender as GridView;

            if(gridView.GetRowCellValue(gridView.FocusedRowHandle, "Nama") == null) {
                gridView.FocusedColumn = gridView.Columns["Nama"];
                return;
            }

            if(gridView.FocusedColumn.FieldName != "Nama" && gridView.GetRowCellValue(gridView.FocusedRowHandle, "Nama").ToString() == "") {
                gridView.FocusedColumn = gridView.Columns["Nama"];
                return;
            }

            setCurrentDataRowBarang();
        }

        private void gridView1_InitNewRow(object sender, InitNewRowEventArgs e) {
            GridView gridView = sender as GridView;

            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["No"], gridView.DataRowCount + 1);
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Nama"], "");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Qty"], "0");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Satuan"], "");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["HB Terakhir"], "0");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Time Schedule"], "0");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Subtotal"], "0");
        }

        private void gridView1_RowDeleted(object sender, DevExpress.Data.RowDeletedEventArgs e) {
            GridView gridView = sender as GridView;
            for(int no = 1; no <= gridView.DataRowCount; no++) {
                gridView.SetRowCellValue(no - 1, "No", no);
            }
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

                String strngNama = gridView.GetRowCellValue(gridView.FocusedRowHandle, "Nama").ToString();

                if(strngNama == "") {
                    return;
                }

                setCurrentDataRowBarang();

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

        private void gridView1_ValidatingEditor(object sender, BaseContainerValidateEditorEventArgs e) {
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

                // e.Valid = true;

                // validasi
                if(gridView.GetRowCellValue(gridView.FocusedRowHandle, "Nama") == null) {
                    return;
                }

                if(gridView.FocusedColumn.FieldName != "Nama" && gridView.GetRowCellValue(gridView.FocusedRowHandle, "Nama").ToString() == "") {
                    return;
                }

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

        private void setCurrentDataRowBarang() {
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

                if(gridView.GetRowCellValue(gridView.FocusedRowHandle, "Nama") == null) {
                    gridView.FocusedColumn = gridView.Columns["Nama"];
                    return;
                }

                if(gridView.GetRowCellValue(gridView.FocusedRowHandle, "Nama").ToString() == "") {
                    gridView.FocusedColumn = gridView.Columns["Nama"];
                    return;
                }

                double dblQty = double.Parse(gridView.GetRowCellValue(gridView.FocusedRowHandle, "Qty").ToString());
                double dblHBTerakhir = double.Parse(gridView.GetRowCellValue(gridView.FocusedRowHandle, "HB Terakhir").ToString());

                double dblSubtotal = dblQty * dblHBTerakhir;

                gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Subtotal"], dblSubtotal);

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

        private void gridView2_FocusedColumnChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventArgs e) {
            GridView gridView = sender as GridView;

            if(gridView.GetRowCellValue(gridView.FocusedRowHandle, "Nama") == null) {
                gridView.FocusedColumn = gridView.Columns["Nama"];
                return;
            }

            if(gridView.FocusedColumn.FieldName != "Nama" && gridView.GetRowCellValue(gridView.FocusedRowHandle, "Nama").ToString() == "") {
                gridView.FocusedColumn = gridView.Columns["Nama"];
                return;
            }
        }

        private void gridView2_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e) {
            GridView gridView = sender as GridView;

            if(gridView.GetRowCellValue(gridView.FocusedRowHandle, "Nama") == null) {
                gridView.FocusedColumn = gridView.Columns["Nama"];
                return;
            }

            if(gridView.FocusedColumn.FieldName != "Nama" && gridView.GetRowCellValue(gridView.FocusedRowHandle, "Nama").ToString() == "") {
                gridView.FocusedColumn = gridView.Columns["Nama"];
                return;
            }
        }

        private void gridView2_InitNewRow(object sender, InitNewRowEventArgs e) {
            GridView gridView = sender as GridView;

            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["No"], gridView.DataRowCount + 1);
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Nama"], "");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Keterangan"], "");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Subtotal"], "0");
        }

        private void gridView2_RowDeleted(object sender, DevExpress.Data.RowDeletedEventArgs e) {
            GridView gridView = sender as GridView;
            for(int no = 1; no <= gridView.DataRowCount; no++) {
                gridView.SetRowCellValue(no - 1, "No", no);
            }
        }

        private void gridView2_ValidateRow(object sender, DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs e) {
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

                String strngNama = gridView.GetRowCellValue(gridView.FocusedRowHandle, "Nama").ToString();

                if(strngNama == "") {
                    return;
                }

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

        private void gridView2_ValidatingEditor(object sender, BaseContainerValidateEditorEventArgs e) {
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

                // e.Valid = true;

                // validasi
                if(gridView.GetRowCellValue(gridView.FocusedRowHandle, "Nama") == null) {
                    return;
                }

                if(gridView.FocusedColumn.FieldName != "Nama" && gridView.GetRowCellValue(gridView.FocusedRowHandle, "Nama").ToString() == "") {
                    return;
                }

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
        
    }
}