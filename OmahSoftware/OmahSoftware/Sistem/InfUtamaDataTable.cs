using DevExpress.XtraGrid.Columns;
using OmahSoftware.OswLib;
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

namespace OmahSoftware.Sistem {
    public partial class InfUtamaDataTable : DevExpress.XtraEditors.XtraForm {
        private String dokumen = "";
        private String query = "";
        private Boolean isStoreProcedure;
        private Boolean isAutoWidthColumn;
        public Dictionary<String, String> parameters = new Dictionary<String, String>();
        private String[] hiddenColumns = new String[] { };
        private String[] numberColumns = new String[] { };
        private DataTable defaultChecked = new DataTable();

        public DataTable hasil = new DataTable();
        public bool pilih = false;

        public InfUtamaDataTable(String dokumen, String query, Dictionary<String, String> parameters, String[] hiddenColumns, String[] numberColumns, DataTable defaultChecked, Boolean isStoreProcedure = false, Boolean isAutoWidthColumn = true) {
            this.dokumen = dokumen;
            this.query = query;
            this.parameters = parameters;
            this.hiddenColumns = hiddenColumns;
            this.numberColumns = numberColumns;
            this.isStoreProcedure = isStoreProcedure;
            this.isAutoWidthColumn = isAutoWidthColumn;
            this.defaultChecked = defaultChecked;

            InitializeComponent();
        }

        private void InfUtamaDataTable_Load(object sender, EventArgs e) {
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

                OswControlDefaultProperties.setInfo(this, command, false);

                this.setGrid(command);

                // untuk default checked
                //if(this.defaultChecked.Rows.Count > 0) {
                //    DataTable data = new DataTable();
                //    foreach(GridColumn kolom in gridView1.Columns) {
                //        data.Columns.Add(kolom.FieldName);
                //    }

                //    for(int i = 0; i < gridView1.RowCount; i++) {
                //        bool ketemu = false;

                //        for(int j = 0; j < defaultChecked.Rows.Count; j++) {
                //            // cari judul kolomnya

                //        }

                //        if(ketemu) {
                //            gridView1.SelectRow(i);
                //        }
                //    }
                //}


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



        public void setGrid(MySqlCommand command) {
            gridControl = OswGrid.getGridBrowse(gridControl, command, this.query, this.parameters, this.hiddenColumns, this.numberColumns, this.isStoreProcedure, this.isAutoWidthColumn);

            // tambahkan kolom di paling kiri
            gridView1.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CheckBoxRowSelect;
            gridView1.OptionsSelection.MultiSelect = true;

            if(this.defaultChecked.Rows.Count > 0) {
                this.setDefaultChecked();
            }


        }

        private void setDefaultChecked() {
            List<GridColumn> cols = new List<GridColumn>();
            foreach(DataColumn kolom in this.defaultChecked.Columns) {
                cols.Add(gridView1.Columns[kolom.Caption]);
            }

            for(int j = 0; j < this.defaultChecked.Rows.Count; j++) {
                object[] values = this.defaultChecked.Rows[j].ItemArray;

                int rowHandle = OswGrid.LocateRowByMultipleValues(gridView1, cols.ToArray(), values);

                if(rowHandle != DevExpress.XtraGrid.GridControl.InvalidRowHandle) {
                    gridView1.SelectRow(rowHandle);
                }
            }
        }

        private void btnPilih_Click(object sender, EventArgs e) {
            pilih = true;
            // buatkan judul kolom
            hasil = new DataTable();
            foreach(GridColumn kolom in gridView1.Columns) {
                hasil.Columns.Add(kolom.FieldName);
            }

            // ambil isi yang dicentang
            if(gridView1.SelectedRowsCount > 0) {
                foreach(int rowHandle in gridView1.GetSelectedRows()) {
                    DataRow tamp = gridView1.GetDataRow(rowHandle);
                    hasil.Rows.Add(tamp.ItemArray);
                }

            }

            this.Close();
        }

        private void gridView1_RowClick(object sender, RowClickEventArgs e) {
            if(gridView1.FocusedColumn.FieldName != "DX$CheckboxSelectorColumn") {
                String strngPilih = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "DX$CheckboxSelectorColumn").ToString();
                if(strngPilih == "True") {
                    gridView1.UnselectRow(gridView1.FocusedRowHandle);
                } else {
                    gridView1.SelectRow(gridView1.FocusedRowHandle);
                }
            }
        }
    }
}