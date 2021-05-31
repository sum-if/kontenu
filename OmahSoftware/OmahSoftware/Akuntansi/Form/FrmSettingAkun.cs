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
using OmahSoftware.OswLib;
using OmahSoftware.Umum;
using DevExpress.XtraEditors.Controls;

namespace OmahSoftware.Akuntansi {
    public partial class FrmSettingAkun : DevExpress.XtraEditors.XtraForm {
        private String id = "SETTINGAKUN";
        private String dokumen = "SETTINGAKUN";

        public FrmSettingAkun() {
            InitializeComponent();
        }

        private void FrmSettingAkun_Load(object sender, EventArgs e) {
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
                DataOswJenisDokumen dOswJenisDokumen = new DataOswJenisDokumen(command, id);
                this.dokumen = dOswJenisDokumen.nama;
                this.Text = this.dokumen;

                OswControlDefaultProperties.setInput(this, id, command, false);

                setGridAkunJurnalOtomatis(command);
                setGridAkunCombo(command);

                // Commit Transaction
                command.Transaction.Commit();
            } catch(MySqlException ex) {
                OswPesan.pesanErrorCatch(ex, command, id);
            } catch(Exception ex) {
                OswPesan.pesanErrorCatch(ex, command, id);
            } finally {
                con.Close();
                try {
                    SplashScreenManager.CloseForm();
                } catch(Exception ex) {
                }
            }
        }

        public void setGridAkunJurnalOtomatis(MySqlCommand command) {
            String query = @"SELECT A.kode AS Kode, A.nama AS Keterangan, COALESCE(C.kode,'') AS 'Kode Akun',COALESCE(C.nama,'') AS 'Nama Akun'
                             FROM konstantaakun A
                             LEFT JOIN oswsetting B ON A.kode = B.kode
                             LEFT JOIN akun C ON B.isi = C.kode
                             ORDER BY A.nama";

            Dictionary<String, String> parameters = new Dictionary<String, String>();

            Dictionary<String, int> widths = new Dictionary<String, int>();
            // 948 - 21 (kiri) - 17 (vertikal lines) = 722
            widths.Add("Keterangan", 372);
            widths.Add("Kode Akun", 138);
            widths.Add("Nama Akun", 400);

            Dictionary<String, String> inputType = new Dictionary<string, string>();

            OswGrid.getGridInput(gridControl1, command, query, parameters, widths, inputType, 
                new String[] { "Kode" },
                new String[] { "Keterangan", "Nama Akun" },
                false);

            // search produk di kolom kode
            RepositoryItemButtonEdit search = new RepositoryItemButtonEdit();
            search.Buttons[0].Kind = ButtonPredefines.Glyph;
            search.Buttons[0].Image = OswResources.getImage("cari-16.png", typeof(Program).Assembly);
            search.Buttons[0].Visible = true;
            search.ButtonClick += search_ButtonClick;

            GridView gridView = gridView1;
            gridView.Columns["Kode Akun"].ColumnEdit = search;
            gridView.Columns["Kode Akun"].ColumnEdit.ReadOnly = true;
        }

        public void setGridAkunCombo(MySqlCommand command) {
            String query = @"SELECT A.kode AS Kode, A.nama AS Nama
                            FROM kelompokakun A
                            ORDER BY A.kode";

            Dictionary<String, String> parameters = new Dictionary<String, String>();

            Dictionary<String, int> widths = new Dictionary<String, int>();
            // 946 - 21 (kiri) - 17 (vertikal lines) = 722
            widths.Add("Kode", 372);
            widths.Add("Nama", 536);

            Dictionary<String, String> inputType = new Dictionary<string, string>();

            OswGrid.getGridInput(gridControl2, command, query, parameters, widths, inputType,
                new String[] { },
                new String[] { "Kode", "Nama" },
                false);
        }

        void search_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e) {
            infoAkun();
        }

        private void infoAkun() {
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
                String query = @"SELECT A.kode AS Kode, A.nama AS Nama
                                FROM akun A
                                WHERE A.status = @status
                                ORDER BY A.kode";

                Dictionary<String, String> parameters = new Dictionary<String, String>();
                parameters.Add("status", Constants.STATUS_AKTIF);

                InfUtamaDictionary form = new InfUtamaDictionary("Info Akun", query, parameters, 
                                                                new String[] { "Kode", "Nama" }, 
                                                                new String[] { },
                                                                new String[] { });
                this.AddOwnedForm(form);
                form.ShowDialog();
                if(!form.hasil.ContainsKey("Kode")) {
                    return;
                }

                String strngKode = form.hasil["Kode"];
                String strngNama = form.hasil["Nama"];

                GridView gridView = gridView1;
                gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Kode Akun"], strngKode);
                gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Nama Akun"], strngNama);

                // Commit Transaction
                command.Transaction.Commit();
            } catch(MySqlException ex) {
                OswPesan.pesanErrorCatch(ex, command, id);
            } catch(Exception ex) {
                OswPesan.pesanErrorCatch(ex, command, id);
            } finally {
                con.Close();
                try {
                    SplashScreenManager.CloseForm();
                } catch(Exception ex) {
                }
            }
        }

        private void btnSimpan_Click(object sender, EventArgs e) {
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
                GridView gridView = gridView1;

                for(int i = 0; i < gridView.DataRowCount; i++) {
                    if(gridView.GetRowCellValue(i, "Kode").ToString() == "") {
                        continue;
                    }

                    if(gridView.GetRowCellValue(i, "Kode Akun").ToString() == "") {
                        continue;
                    }

                    String strngKode = gridView.GetRowCellValue(i, "Kode").ToString();
                    String strngKodeAkun = gridView.GetRowCellValue(i, "Kode Akun").ToString();

                    DataAkun dAkun = new DataAkun(command, strngKodeAkun);
                    if(!dAkun.isExist) {
                        throw new Exception("Akun ["+strngKodeAkun+"] tidak ditemukan");
                    }

                    DataOswSetting dOswSetting = new DataOswSetting(command, strngKode);
                    dOswSetting.isi = strngKodeAkun;
                    if(dOswSetting.isExist) {
                        dOswSetting.ubah();
                    } else {
                        dOswSetting.tambah();
                    }
                    
                    // tulis log
                    OswLog.setTransaksi(command, dokumen, dOswSetting.ToString());
                }

                // Commit Transaction
                command.Transaction.Commit();

                OswPesan.pesanInfo(dokumen + " berhasil disimpan.");

                setGridAkunJurnalOtomatis(command);

            } catch(MySqlException ex) {
                OswPesan.pesanErrorCatch(ex, command, id);
            } catch(Exception ex) {
                OswPesan.pesanErrorCatch(ex, command, id);
            } finally {
                con.Close();
                try {
                    SplashScreenManager.CloseForm();
                } catch(Exception ex) {
                }
            }
        }

        private void gridControl1_ProcessGridKey(object sender, KeyEventArgs e) {
            GridView gridView = gridView1;
            if(e.KeyCode == Keys.F1 && gridView.FocusedColumn.FieldName == "Kode Akun") {
                infoAkun();
            }
        }

        private void gridView1_FocusedColumnChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventArgs e) {
            GridView gridView = sender as GridView;

            if(gridView.GetRowCellValue(gridView.FocusedRowHandle, "Kode Akun") == null) {
                gridView.FocusedColumn = gridView.Columns["Kode Akun"];
                return;
            }

            if(gridView.FocusedColumn.FieldName != "Kode Akun" && gridView.GetRowCellValue(gridView.FocusedRowHandle, "Kode Akun").ToString() == "") {
                gridView.FocusedColumn = gridView.Columns["Kode Akun"];
                return;
            }
        }

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e) {
            GridView gridView = sender as GridView;

            if(gridView.GetRowCellValue(gridView.FocusedRowHandle, "Kode Akun") == null) {
                gridView.FocusedColumn = gridView.Columns["Kode Akun"];
                return;
            }

            if(gridView.FocusedColumn.FieldName != "Kode Akun" && gridView.GetRowCellValue(gridView.FocusedRowHandle, "Kode Akun").ToString() == "") {
                gridView.FocusedColumn = gridView.Columns["Kode Akun"];
                return;
            }
        }

        private void gridView2_DoubleClick(object sender, EventArgs e) {
            String strngKode = gridView2.GetRowCellValue(gridView2.FocusedRowHandle, "Kode").ToString();
            String strngNama = gridView2.GetRowCellValue(gridView2.FocusedRowHandle, "Nama").ToString();

            FrmSettingAkunTransaksi form = new FrmSettingAkunTransaksi(strngNama);
            this.AddOwnedForm(form);
            form.kode = strngKode;
            form.ShowDialog();
        }
    }
}