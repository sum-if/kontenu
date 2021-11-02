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

namespace Kontenu.Akuntansi {
    public partial class FrmAkunGroup : DevExpress.XtraEditors.XtraForm {
        private String id = "AKUNGROUP";
        private String dokumen = "AKUNGROUP";

        public FrmAkunGroup() {
            InitializeComponent();
        }

        private void FrmAkunGroup_Load(object sender, EventArgs e) {
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

                OswControlDefaultProperties.setBrowse(this, id, command);
                cmbAkunKategori = ComboQueryUmum.getAkunKategori(cmbAkunKategori, command, true);
                cmbAkunKategori.EditValue = "%";
                cmbAkunSubKategori = ComboQueryUmum.getAkunSubKategori(cmbAkunSubKategori, command, cmbAkunKategori.EditValue == null ? "" : cmbAkunKategori.EditValue.ToString(), true);
                cmbAkunSubKategori.EditValue = "%";

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
            String strngAkunKategori = cmbAkunKategori.EditValue == null ? "" : cmbAkunKategori.EditValue.ToString();
            String strngAkunSubKategori = cmbAkunSubKategori.EditValue == null ? "" : cmbAkunSubKategori.EditValue.ToString();

            String query = @"SELECT A.kode AS Kode, A.nama AS Nama, C.nama AS Kategori, B.nama AS 'Sub Kategori'
                            FROM akungroup A
                            INNER JOIN akunsubkategori B ON A.akunsubkategori = B.kode
                            INNER JOIN akunkategori C ON B.akunkategori = C.kode
                            WHERE A.kode LIKE @kode AND A.nama LIKE @nama AND A.akunsubkategori LIKE @akunsubkategori AND B.akunkategori LIKE @akunkategori
                            ORDER BY A.kode";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("kode", "%" + strngKode + "%");
            parameters.Add("nama", "%" + strngNama + "%");
            parameters.Add("akunsubkategori", strngAkunSubKategori + "%");
            parameters.Add("akunkategori", strngAkunKategori + "%");

            gridControl = OswGrid.getGridBrowse(gridControl, command, query, parameters, 
                                                new String[] { }, 
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
            FrmAkunGroupAdd form = new FrmAkunGroupAdd(false);
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
                    OswPesan.pesanError("Silahkan pilih " + dokumen + " yang akan diubah.");
                    return;
                }

                String strngKode = gridView.GetRowCellValue(gridView.FocusedRowHandle, "Kode").ToString();

                DataAkunGroup dAkunGroup = new DataAkunGroup(command, strngKode);
                if(!dAkunGroup.isExist) {
                    throw new Exception(dokumen + " tidak ditemukan.");
                }

                this.AddOwnedForm(form);
                form.txtKode.Text = strngKode;

                // Commit Transaction
                command.Transaction.Commit();
            } catch(MySqlException ex) {
                OswPesan.pesanErrorCatch(ex, command, dokumen);
            } catch(Exception ex) {
                OswPesan.pesanErrorCatch(ex, command, dokumen);
            } finally {
                con.Close();
            }

            form.ShowDialog();
        }

        private void btnTambah_Click(object sender, EventArgs e) {
            FrmAkunGroupAdd form = new FrmAkunGroupAdd(true);
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
                    OswPesan.pesanError("Silahkan pilih " + dokumen + " yang akan dihapus.");
                    return;
                }

                if(OswPesan.pesanKonfirmasiHapus() == DialogResult.Yes) {
                    String strngKode = gridView.GetRowCellValue(gridView.FocusedRowHandle, "Kode").ToString();

                    DataAkunGroup dAkunGroup = new DataAkunGroup(command, strngKode);
                    if(!dAkunGroup.isExist) {
                        throw new Exception(dokumen + " tidak ditemukan.");
                    }

                    dAkunGroup.hapus();

                    // tulis log
                    // OswLog.setTransaksi(command, "Hapus " + dokumen, dAkunGroup.ToString());

                    // reload grid
                    this.setGrid(command);

                    // Commit Transaction
                    command.Transaction.Commit();

                    OswPesan.pesanInfo(dokumen + " berhasil dihapus.");
                }
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

        private void cmbAkunKategori_EditValueChanged(object sender, EventArgs e) {
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
                cmbAkunSubKategori = ComboQueryUmum.getAkunSubKategori(cmbAkunSubKategori, command, cmbAkunKategori.EditValue == null ? "" : cmbAkunKategori.EditValue.ToString(), true);
                cmbAkunSubKategori.EditValue = "%";

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
    }
}