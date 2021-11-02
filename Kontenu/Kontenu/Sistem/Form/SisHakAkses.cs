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
using Kontenu.Umum;

namespace Kontenu.Sistem {
    public partial class SisHakAkses : DevExpress.XtraEditors.XtraForm {
        private String id = "HAKAKSES";
        private String dokumen = "Hak Akses";

        public SisHakAkses() {
            InitializeComponent();
        }

        private void cmbUsergroup_EditValueChanged(object sender, EventArgs e) {
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

        private void SisHakAkses_Load(object sender, EventArgs e) {
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
                OswControlDefaultProperties.setInputHakAkses(this, command);
                cmbUsergroup = ComboQueryUmum.getUserGroup(cmbUsergroup, command);
                cmbUsergroup.EditValue = OswCombo.getFirstEditValue(cmbUsergroup);

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
            String strngUsergroup = cmbUsergroup.EditValue.ToString();

            gridControl = OswGrid.getGridInputHakAkses(gridControl, command, strngUsergroup);
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
                for(int i = 0; i < gridView.DataRowCount; i++) {
                    string group = cmbUsergroup.EditValue.ToString();
                    string kode = gridView.GetRowCellValue(i, "Kode").ToString();
                    int lihat = Convert.ToInt32(gridView.GetRowCellValue(i, "Lihat"));
                    int tambah = Convert.ToInt32(gridView.GetRowCellValue(i, "Tambah"));
                    int ubah = Convert.ToInt32(gridView.GetRowCellValue(i, "Ubah"));
                    int hapus = Convert.ToInt32(gridView.GetRowCellValue(i, "Hapus"));
                    int cetak = Convert.ToInt32(gridView.GetRowCellValue(i, "Cetak"));

                    String query = @"SELECT COUNT(*) FROM oswhakakses WHERE menu = @kode AND usergroup = @group";
                    Dictionary<String, String> parameters = new Dictionary<String, String>();
                    parameters.Add("kode", kode);
                    parameters.Add("group", group);

                    int baris = int.Parse(OswDataAccess.executeScalarQuery(query, parameters, command));
                    if(baris > 0) {
                        query = @"UPDATE oswhakakses
                                SET lihat = @lihat, tambah = @tambah, ubah = @ubah, hapus = @hapus, cetak = @cetak
                                WHERE menu = @kode AND usergroup = @group";

                        parameters = new Dictionary<String, String>();
                        parameters.Add("lihat", lihat.ToString());
                        parameters.Add("tambah", tambah.ToString());
                        parameters.Add("ubah", ubah.ToString());
                        parameters.Add("hapus", hapus.ToString());
                        parameters.Add("cetak", cetak.ToString());
                        parameters.Add("kode", kode);
                        parameters.Add("group", group);
                    } else {
                        query = @"INSERT INTO oswhakakses(menu,usergroup,lihat,tambah,ubah,hapus,cetak)
                                                VALUES(@kode,@group,@lihat,@tambah,@ubah,@hapus,@cetak)";
                        parameters = new Dictionary<String, String>();
                        parameters.Add("lihat", lihat.ToString());
                        parameters.Add("tambah", tambah.ToString());
                        parameters.Add("ubah", ubah.ToString());
                        parameters.Add("hapus", hapus.ToString());
                        parameters.Add("cetak", cetak.ToString());
                        parameters.Add("kode", kode);
                        parameters.Add("group", group);
                    }

                    OswDataAccess.executeVoidQuery(query, parameters, command);
                }

                // OswLog.setTransaksi(command, this.dokumen, "Simpan Hak Akses");

                // Commit Transaction
                command.Transaction.Commit();

                OswPesan.pesanInfo("Proses penyimpanan hak akses Berhasil.");

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

        private void btnHapusSemua_Click(object sender, EventArgs e) {
            SplashScreenManager.ShowForm(typeof(SplashUtama));

            for(int i = 0; i < gridView.DataRowCount; i++) {
                gridView.SetRowCellValue(i, "Lihat", false);
                gridView.SetRowCellValue(i, "Tambah", false);
                gridView.SetRowCellValue(i, "Ubah", false);
                gridView.SetRowCellValue(i, "Hapus", false);
                gridView.SetRowCellValue(i, "Cetak", false);
            }

            try {
                SplashScreenManager.CloseForm();
            } catch(Exception ex) {
            }
        }

        private void btnPilihSemua_Click(object sender, EventArgs e) {
            SplashScreenManager.ShowForm(typeof(SplashUtama));

            for(int i = 0; i < gridView.DataRowCount; i++) {
                gridView.SetRowCellValue(i, "Lihat", true);
                gridView.SetRowCellValue(i, "Tambah", true);
                gridView.SetRowCellValue(i, "Ubah", true);
                gridView.SetRowCellValue(i, "Hapus", true);
                gridView.SetRowCellValue(i, "Cetak", true);
            }

            try {
                SplashScreenManager.CloseForm();
            } catch(Exception ex) {
            }
        }
    }
}