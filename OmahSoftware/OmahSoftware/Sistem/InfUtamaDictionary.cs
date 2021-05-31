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
    public partial class InfUtamaDictionary : DevExpress.XtraEditors.XtraForm {
        private String dokumen = "";
        private String query = "";
        private Boolean isStoreProcedure;
        private Boolean isAutoWidthColumn;
        public Dictionary<String, String> parameters = new Dictionary<String, String>();
        private String[] hiddenColumns = new String[] { };
        private String[] numberColumns = new String[] { };
        private String[] returnColumns = new String[] { };

        public Dictionary<String, String> hasil = new Dictionary<String, String>();

        public InfUtamaDictionary(String dokumen, String query, Dictionary<String, String> parameters, String[] returnColumns, String[] hiddenColumns, String[] numberColumns, Boolean isStoreProcedure = false, Boolean isAutoWidthColumn = true) {
            this.dokumen = dokumen;
            this.query = query;
            this.parameters = parameters;
            this.hiddenColumns = hiddenColumns;
            this.returnColumns = returnColumns;
            this.numberColumns = numberColumns;
            this.isStoreProcedure = isStoreProcedure;
            this.isAutoWidthColumn = isAutoWidthColumn;

            InitializeComponent();
        }

        private void InfUtamaDictionary_Load(object sender, EventArgs e) {
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

                OswControlDefaultProperties.setInfo(this, command);
                
                this.setGrid(command);

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
        }

        private void btnPilih_Click(object sender, EventArgs e) {
            if(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, this.returnColumns[0]) != null) {
                foreach(String kolom in returnColumns){
                    String strngKolom = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, kolom).ToString();

                    hasil.Add(kolom, strngKolom);
                }
            }
            this.Close();
        }
    }
}