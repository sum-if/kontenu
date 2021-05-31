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

namespace OmahSoftware.Akuntansi {
    public partial class FrmAkun : DevExpress.XtraEditors.XtraForm {
        private String id = "AKUN";
        private String dokumen = "AKUN";

        public FrmAkun() {
            InitializeComponent();
        }

        private void FrmAkun_Load(object sender, EventArgs e) {
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
                cmbAkunGroup = ComboQueryUmum.getAkunGroup(cmbAkunGroup, command, cmbAkunKategori.EditValue == null ? "" : cmbAkunKategori.EditValue.ToString(), cmbAkunSubKategori.EditValue == null ? "" : cmbAkunSubKategori.EditValue.ToString(), true, true);
                cmbAkunGroup.EditValue = "%";
                cmbAkunSubGroup = ComboQueryUmum.getAkunSubGroup(cmbAkunSubGroup, command, cmbAkunKategori.EditValue == null ? "" : cmbAkunKategori.EditValue.ToString(), cmbAkunSubKategori.EditValue == null ? "" : cmbAkunSubKategori.EditValue.ToString(), cmbAkunGroup.EditValue == null ? "" : cmbAkunGroup.EditValue.ToString(), true, true);
                cmbAkunSubGroup.EditValue = "%";

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
            String strngAkunGroup = cmbAkunGroup.EditValue == null ? "" : cmbAkunGroup.EditValue.ToString();
            String strngAkunSubGroup = cmbAkunSubGroup.EditValue == null ? "" : cmbAkunSubGroup.EditValue.ToString();


            String query = @"SELECT A.kode AS Kode, A.nama AS Nama, A.saldonormal AS 'Saldo Normal', A.jurnalmanual AS 'Jurnal Manual',D.nama AS Kategori, C.nama AS 'Sub Kategori',COALESCE(B.nama,'[Tidak Ada]') AS 'Group' , 
                                    COALESCE(BB.nama,'[Tidak Ada]') AS 'Sub Group'
                            FROM akun A
                            LEFT JOIN akunsubgroup BB ON A.akunsubgroup = BB.kode
                            LEFT JOIN akungroup B ON A.akungroup = B.kode
                            LEFT JOIN akunsubkategori C ON A.akunsubkategori = C.kode
                            LEFT JOIN akunkategori D ON A.akunkategori = D.kode
                            WHERE A.kode LIKE @kode AND A.nama LIKE @nama AND A.akunsubgroup LIKE @akunsubgroup AND A.akungroup LIKE @akungroup AND 
                                  A.akunsubkategori LIKE @akunsubkategori AND A.akunkategori LIKE @akunkategori
                            ORDER BY A.kode";

            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("kode", "%" + strngKode + "%");
            parameters.Add("nama", "%" + strngNama + "%");
            parameters.Add("akunsubkategori", strngAkunSubKategori + "%");
            parameters.Add("akunkategori", strngAkunKategori + "%");
            parameters.Add("akungroup", strngAkunGroup + "%");
            parameters.Add("akunsubgroup", strngAkunSubGroup + "%");

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
            FrmAkunAdd form = new FrmAkunAdd(false);
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

                DataAkun dAkun = new DataAkun(command, strngKode);
                if(!dAkun.isExist) {
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
            FrmAkunAdd form = new FrmAkunAdd(true);
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

                    DataAkun dAkun = new DataAkun(command, strngKode);
                    if(!dAkun.isExist) {
                        throw new Exception(dokumen + " tidak ditemukan.");
                    }

                    dAkun.hapus();

                    // tulis log
                    OswLog.setTransaksi(command, "Hapus " + dokumen, dAkun.ToString());

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
                cmbAkunGroup = ComboQueryUmum.getAkunGroup(cmbAkunGroup, command, cmbAkunKategori.EditValue == null ? "" : cmbAkunKategori.EditValue.ToString(), cmbAkunSubKategori.EditValue == null ? "" : cmbAkunSubKategori.EditValue.ToString(), true, true);
                cmbAkunGroup.EditValue = "%";
                cmbAkunSubGroup = ComboQueryUmum.getAkunSubGroup(cmbAkunSubGroup, command, cmbAkunKategori.EditValue == null ? "" : cmbAkunKategori.EditValue.ToString(), cmbAkunSubKategori.EditValue == null ? "" : cmbAkunSubKategori.EditValue.ToString(), cmbAkunGroup.EditValue == null ? "" : cmbAkunGroup.EditValue.ToString(), true, true);
                cmbAkunSubGroup.EditValue = "%";

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

        private void cmbAkunSubKategori_EditValueChanged(object sender, EventArgs e) {
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
                cmbAkunGroup = ComboQueryUmum.getAkunGroup(cmbAkunGroup, command, cmbAkunKategori.EditValue == null ? "" : cmbAkunKategori.EditValue.ToString(), cmbAkunSubKategori.EditValue == null ? "" : cmbAkunSubKategori.EditValue.ToString(), true, true);
                cmbAkunGroup.EditValue = "%";
                cmbAkunSubGroup = ComboQueryUmum.getAkunSubGroup(cmbAkunSubGroup, command, cmbAkunKategori.EditValue == null ? "" : cmbAkunKategori.EditValue.ToString(), cmbAkunSubKategori.EditValue == null ? "" : cmbAkunSubKategori.EditValue.ToString(), cmbAkunGroup.EditValue == null ? "" : cmbAkunGroup.EditValue.ToString(), true, true);
                cmbAkunSubGroup.EditValue = "%";

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

        private void cmbAkunGroup_EditValueChanged(object sender, EventArgs e) {
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
                cmbAkunSubGroup = ComboQueryUmum.getAkunSubGroup(cmbAkunSubGroup, command, cmbAkunKategori.EditValue.ToString(), cmbAkunSubKategori.EditValue.ToString(), cmbAkunGroup.EditValue.ToString(), true, true);
                cmbAkunSubGroup.EditValue = "%";

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