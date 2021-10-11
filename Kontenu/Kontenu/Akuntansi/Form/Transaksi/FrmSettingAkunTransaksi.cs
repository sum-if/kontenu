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
using Kontenu.Akuntansi;
using DevExpress.XtraEditors.DXErrorProvider;
using System.Net;
using Kontenu.Sistem;
using Kontenu.Umum;
using Kontenu.OswLib;
using DevExpress.XtraEditors.Controls;

namespace Kontenu.Akuntansi {
    public partial class FrmSettingAkunTransaksi : DevExpress.XtraEditors.XtraForm {
        private String id = "SETTINGAKUN";
        private String dokumen = "SETTINGAKUN";
        public String kode = "";

        public FrmSettingAkunTransaksi(String nama) {
            InitializeComponent();
            this.Text = nama;
        }

        private void FrmSettingAkunTransaksi_Load(object sender, EventArgs e) {
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
                OswControlDefaultProperties.setInput(this, id, command, true);

                this.setGrid(command);

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
                // hapus data lama
                DataKelompokAkunSetting dKelompokAkunSetting = new DataKelompokAkunSetting(command, kode, "0");
                dKelompokAkunSetting.hapus(true);

                GridView gridView = gridView1;

                for(int i = 0; i < gridView.DataRowCount; i++) {
                    if(gridView.GetRowCellValue(i, "Kategori").ToString() == null) {
                        continue;
                    }

                    String strngNo = gridView.GetRowCellValue(i, "No").ToString();
                    String strngKategori = gridView.GetRowCellValue(i, "Kode Kategori").ToString();
                    String strngSubKategori = gridView.GetRowCellValue(i, "Kode Sub Kategori").ToString();
                    String strngGroup = gridView.GetRowCellValue(i, "Kode Group").ToString();
                    String strngSubGroup = gridView.GetRowCellValue(i, "Kode Sub Group").ToString();
                    String strngAkun = gridView.GetRowCellValue(i, "Kode Akun").ToString();

                    dKelompokAkunSetting = new DataKelompokAkunSetting(command, kode, strngNo);
                    dKelompokAkunSetting.kategori = strngKategori;
                    dKelompokAkunSetting.subkategori = strngSubKategori;
                    dKelompokAkunSetting.group = strngGroup;
                    dKelompokAkunSetting.subgroup = strngSubGroup;
                    dKelompokAkunSetting.akun = strngAkun;
                    dKelompokAkunSetting.tambah();

                    // tulis log
                    OswLog.setTransaksi(command, dokumen, dKelompokAkunSetting.ToString());
                }

                OswPesan.pesanInfo("Proses Penyimpanan Setting Akun Transaksi berhasil.");

                // Commit Transaction
                command.Transaction.Commit();

                this.Close();
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

        public void setGrid(MySqlCommand command) {
            String query = @"SELECT A.no AS No, 
                                    COALESCE(E.kode, '%') AS 'Kode Kategori', COALESCE(E.nama, '[Semua]') AS 'Kategori', 
                                    COALESCE(D.kode, '%') AS 'Kode Sub Kategori', COALESCE(D.nama, '[Semua]') AS 'Sub Kategori', 
                                    COALESCE(C.kode, '%') AS 'Kode Group', COALESCE(C.nama, '[Semua]') AS 'Group', 
                                    COALESCE(B.kode, '%') AS 'Kode Sub Group', COALESCE(B.nama, '[Semua]') AS 'Sub Group', 
                                    COALESCE(F.kode, '%') AS 'Kode Akun', COALESCE(F.nama, '[Semua]') AS 'Akun'
                             FROM kelompokakunsetting A
                             LEFT JOIN akunsubgroup B ON A.subgroup = B.kode
                             LEFT JOIN akungroup C ON A.group = C.kode
                             LEFT JOIN akunsubkategori D ON A.subkategori = D.kode
                             LEFT JOIN akunkategori E ON A.kategori = E.kode
                             LEFT JOIN akun F ON A.akun = F.kode
                             WHERE A.kelompokakun = @kode";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("kode", kode);

            Dictionary<String, int> widths = new Dictionary<String, int>();
            // 960 - 21 (kiri) - 17 (vertikal lines) = 662
            widths.Add("No", 35);
            widths.Add("Kategori", 160);
            widths.Add("Sub Kategori", 160);
            widths.Add("Group", 160);
            widths.Add("Sub Group", 160);
            widths.Add("Akun", 247);

            Dictionary<String, String> tipeInput = new Dictionary<string, string>();

            OswGrid.getGridInput(gridControl1, command, query, parameters, widths, tipeInput, new String[] { "Kode Kategori", "Kode Sub Kategori", "Kode Group", "Kode Sub Group", "Kode Akun" }, new String[] { "No" });

            RepositoryItemButtonEdit searchKategori = new RepositoryItemButtonEdit();
            searchKategori.Buttons[0].Kind = ButtonPredefines.Glyph;
            searchKategori.Buttons[0].Image = OswResources.getImage("cari-16.png", typeof(Program).Assembly);
            searchKategori.Buttons[0].Visible = true;
            searchKategori.ButtonClick += searchKategori_ButtonClick;

            RepositoryItemButtonEdit searchSubKategori = new RepositoryItemButtonEdit();
            searchSubKategori.Buttons[0].Kind = ButtonPredefines.Glyph;
            searchSubKategori.Buttons[0].Image = OswResources.getImage("cari-16.png", typeof(Program).Assembly);
            searchSubKategori.Buttons[0].Visible = true;
            searchSubKategori.ButtonClick += searchSubKategori_ButtonClick;

            RepositoryItemButtonEdit searchGroup = new RepositoryItemButtonEdit();
            searchGroup.Buttons[0].Kind = ButtonPredefines.Glyph;
            searchGroup.Buttons[0].Image = OswResources.getImage("cari-16.png", typeof(Program).Assembly);
            searchGroup.Buttons[0].Visible = true;
            searchGroup.ButtonClick += searchGroup_ButtonClick;

            RepositoryItemButtonEdit searchSubGroup = new RepositoryItemButtonEdit();
            searchSubGroup.Buttons[0].Kind = ButtonPredefines.Glyph;
            searchSubGroup.Buttons[0].Image = OswResources.getImage("cari-16.png", typeof(Program).Assembly);
            searchSubGroup.Buttons[0].Visible = true;
            searchSubGroup.ButtonClick += searchSubGroup_ButtonClick;

            RepositoryItemButtonEdit searchAkun = new RepositoryItemButtonEdit();
            searchAkun.Buttons[0].Kind = ButtonPredefines.Glyph;
            searchAkun.Buttons[0].Image = OswResources.getImage("cari-16.png", typeof(Program).Assembly);
            searchAkun.Buttons[0].Visible = true;
            searchAkun.ButtonClick += searchAkun_ButtonClick;

            GridView gridView = gridView1;
            gridView.Columns["Kategori"].ColumnEdit = searchKategori;
            gridView.Columns["Sub Kategori"].ColumnEdit = searchSubKategori;
            gridView.Columns["Group"].ColumnEdit = searchGroup;
            gridView.Columns["Sub Group"].ColumnEdit = searchSubGroup;
            gridView.Columns["Akun"].ColumnEdit = searchAkun;

        }

        void searchKategori_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e) {
            infoKategori();
        }

        private void infoKategori() {
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
                String query = @"SELECT '%' AS Kode, '[Semua]' AS Nama UNION (SELECT A.kode AS Kode, A.nama AS Nama
                                FROM akunkategori A
                                ORDER BY A.kode)";

                Dictionary<String, String> parameters = new Dictionary<String, String>();

                InfUtamaDictionary form = new InfUtamaDictionary("Info Kategori Akun", query, parameters,
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
                gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Kode Kategori"], strngKode);
                gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Kategori"], strngNama);

                gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Kode Sub Kategori"], "%");
                gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Sub Kategori"], "[Semua]");
                gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Kode Group"], "%");
                gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Group"], "[Semua]");
                gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Kode Sub Group"], "%");
                gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Sub Group"], "[Semua]");
                gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Kode Akun"], "%");
                gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Akun"], "[Semua]");

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

        void searchSubKategori_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e) {
            infoSubKategori();
        }

        private void infoSubKategori() {
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

                String query = @"SELECT '%' AS Kode, '[Semua]' AS Nama UNION (SELECT A.kode AS Kode, A.nama AS Nama
                                FROM akunsubkategori A
                                ORDER BY A.kode)";

                Dictionary<String, String> parameters = new Dictionary<String, String>();

                InfUtamaDictionary form = new InfUtamaDictionary("Info Sub Kategori Akun", query, parameters,
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

                gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Kode Sub Kategori"], strngKode);
                gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Sub Kategori"], strngNama);

                gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Kode Kategori"], "%");
                gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Kategori"], "[Semua]");
                gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Kode Group"], "%");
                gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Group"], "[Semua]");
                gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Kode Sub Group"], "%");
                gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Sub Group"], "[Semua]");
                gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Kode Akun"], "%");
                gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Akun"], "[Semua]");
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

        void searchGroup_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e) {
            infoGroup();
        }

        private void infoGroup() {
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

                String query = @"SELECT '%' AS Kode, '[Semua]' AS Nama UNION (SELECT A.kode AS Kode, A.nama AS Nama
                                FROM akungroup A
                                ORDER BY A.kode)";

                Dictionary<String, String> parameters = new Dictionary<String, String>();

                InfUtamaDictionary form = new InfUtamaDictionary("Info Group Akun", query, parameters,
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

                gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Kode Group"], strngKode);
                gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Group"], strngNama);

                gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Kode Kategori"], "%");
                gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Kategori"], "[Semua]");
                gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Kode Sub Kategori"], "%");
                gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Sub Kategori"], "[Semua]");
                gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Kode Sub Group"], "%");
                gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Sub Group"], "[Semua]");
                gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Kode Akun"], "%");
                gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Akun"], "[Semua]");

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

        void searchSubGroup_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e) {
            infoSubGroup();
        }

        private void infoSubGroup() {
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

                String query = @"SELECT '%' AS Kode, '[Semua]' AS Nama UNION (SELECT A.kode AS Kode, A.nama AS Nama
                                FROM akunsubgroup A
                                ORDER BY A.kode)";

                Dictionary<String, String> parameters = new Dictionary<String, String>();

                InfUtamaDictionary form = new InfUtamaDictionary("Info Sub Group Akun", query, parameters,
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

                gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Kode Sub Group"], strngKode);
                gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Sub Group"], strngNama);

                gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Kode Kategori"], "%");
                gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Kategori"], "[Semua]");
                gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Kode Sub Kategori"], "%");
                gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Sub Kategori"], "[Semua]");
                gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Kode Group"], "%");
                gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Group"], "[Semua]");
                gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Kode Akun"], "%");
                gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Akun"], "[Semua]");

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

        void searchAkun_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e) {
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
                GridView gridView = gridView1;

                String query = @"SELECT '%' AS Kode, '[Semua]' AS Nama UNION (SELECT A.kode AS Kode, A.nama AS Nama
                                FROM akun A
                                WHERE A.status = @status
                                ORDER BY A.kode)";

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

                gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Kode Akun"], strngKode);
                gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Akun"], strngNama);

                gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Kode Kategori"], "%");
                gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Kategori"], "[Semua]");
                gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Kode Sub Kategori"], "%");
                gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Sub Kategori"], "[Semua]");
                gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Kode Group"], "%");
                gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Group"], "[Semua]");
                gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Kode Sub Group"], "%");
                gridView.SetRowCellValue(gridView.FocusedRowHandle, gridView.Columns["Sub Group"], "[Semua]");

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

        private void gridView1_InitNewRow(object sender, InitNewRowEventArgs e) {
            GridView gridView = sender as GridView;

            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["No"], gridView.DataRowCount + 1);
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Kode Kategori"], "%");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Kode Sub Kategori"], "%");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Kode Group"], "%");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Kode Sub Group"], "%");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Kode Akun"], "%");

            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Kategori"], "[Semua]");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Sub Kategori"], "[Semua]");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Group"], "[Semua]");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Sub Group"], "[Semua]");
            gridView.SetRowCellValue(e.RowHandle, gridView.Columns["Akun"], "[Semua]");
        }

        private void gridView1_RowDeleted(object sender, DevExpress.Data.RowDeletedEventArgs e) {
            GridView gridView = sender as GridView;
            for(int no = 1; no <= gridView.DataRowCount; no++) {
                gridView.SetRowCellValue(no - 1, "No", no);
            }
        }

        private void gridControl1_ProcessGridKey(object sender, KeyEventArgs e) {
            GridView gridView = gridView1;
            if(e.KeyCode == Keys.F1 && gridView.FocusedColumn.FieldName == "Kategori") {
                infoKategori();
            } else if(e.KeyCode == Keys.F1 && gridView.FocusedColumn.FieldName == "Sub Kategori") {
                infoSubKategori();
            } else if(e.KeyCode == Keys.F1 && gridView.FocusedColumn.FieldName == "Group") {
                infoGroup();
            } else if(e.KeyCode == Keys.F1 && gridView.FocusedColumn.FieldName == "Sub Group") {
                infoSubGroup();
            } else if(e.KeyCode == Keys.F1 && gridView.FocusedColumn.FieldName == "Akun") {
                infoAkun();
            }
        }
    }
}