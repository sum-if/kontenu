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
using Kontenu.OswLib;

namespace Kontenu.Sistem {
    public partial class SisUserGroup : DevExpress.XtraEditors.XtraForm {
        private String id = "USERGROUP";
        private String dokumen = "Daftar User Group";

        public SisUserGroup() {
            InitializeComponent();
        }

        private void MstUserGroup_Load(object sender, EventArgs e) {
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
                OswControlDefaultProperties.setBrowse(this, id, command);
                cmbStatus = ComboConstantUmum.getStatusAktif(cmbStatus, true);
                cmbStatus.ItemIndex = 0;

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
            String strngKode = txtKode.Text;
            String strngNama = txtNama.Text;
            String strngStatus = cmbStatus.EditValue.ToString();

            String query = @"SELECT kode AS Kode, nama AS Nama, keterangan AS Keterangan,status AS 'Kode Status', IF(status=1,'Aktif','Tidak Aktif') AS Status
                                 FROM oswusergroup
                                 WHERE kode LIKE @kode AND nama LIKE @nama AND status LIKE @status
                                 ORDER BY kode";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("kode", "%" + strngKode + "%");
            parameters.Add("nama", "%" + strngNama + "%");
            parameters.Add("status", strngStatus);

            gridControl = OswGrid.getGridBrowse(gridControl, command, query, parameters, 
                                                new String[] { "Kode Status" }, 
                                                new String[] { });
        }

        private void btnCari_Click(object sender, EventArgs e) {
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

        private void btnCetak_Click(object sender, EventArgs e) {
            Tools.cetakBrowse(gridControl, dokumen);
        }

        private void btnUbah_Click(object sender, EventArgs e) {
            SisUserGroupAdd form = new SisUserGroupAdd(false);

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
                if(gridView.GetSelectedRows().Length == 0) {
                    OswPesan.pesanError("Silahkan pilih data yang akan diubah.");
                    return;
                }

                String strngKode = gridView.GetRowCellValue(gridView.FocusedRowHandle, "Kode").ToString();

                DataOswUserGroup dOswUserGroup = new DataOswUserGroup(command, strngKode);
                if(!dOswUserGroup.isExist) {
                    throw new Exception("Data User Group tidak ditemukan.");
                }

                this.AddOwnedForm(form);
                form.txtKode.Text = strngKode;
                
                // Commit Transaction
                command.Transaction.Commit();
            } catch (MySqlException ex) {
                OswPesan.pesanErrorCatch(ex, command, dokumen);
            } catch (Exception ex) {
                OswPesan.pesanErrorCatch(ex, command, dokumen);
            } finally {
                con.Close();
            }

            form.ShowDialog();
        }

        private void btnTambah_Click(object sender, EventArgs e) {
            SisUserGroupAdd form = new SisUserGroupAdd(true);
            this.AddOwnedForm(form);
            form.ShowDialog();
        }

        private void btnHapus_Click(object sender, EventArgs e) {
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
                if(gridView.GetSelectedRows().Length == 0) {
                    OswPesan.pesanError("Silahkan pilih data yang akan dihapus.");
                    return;
                }

                if(OswPesan.pesanKonfirmasiHapus() == DialogResult.Yes) {

                    String strngKode = gridView.GetRowCellValue(gridView.FocusedRowHandle, "Kode").ToString();

                    DataOswUserGroup dOswUserGroup = new DataOswUserGroup(command, strngKode);
                    if(!dOswUserGroup.isExist) {
                        throw new Exception("Data User Group tidak ditemukan.");
                    }

                    dOswUserGroup.hapus();

                    // tulis log
                    OswLog.setTransaksi(command, dokumen, dOswUserGroup.ToString());

                    // reload grid
                    this.setGrid(command);

                    // Commit Transaction
                    command.Transaction.Commit();

                    OswPesan.pesanInfo("User Group berhasil dihapus.");
                }
            } catch (MySqlException ex) {
                OswPesan.pesanErrorCatch(ex, command, dokumen);
            } catch (Exception ex) {
                OswPesan.pesanErrorCatch(ex, command, dokumen);
            } finally {
                con.Close();
                try {
                    SplashScreenManager.CloseForm();
                } catch(Exception ex) {
                }
            }
        }
    }
}